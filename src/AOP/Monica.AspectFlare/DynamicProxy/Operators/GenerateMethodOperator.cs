using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Monica.AspectFlare.DynamicProxy.Extensions;
using Monica.AspectFlare.Utilities;

namespace Monica.AspectFlare.DynamicProxy
{
    internal abstract class GenerateMethodOperator : IGenerateTypeOperator
    {
        public abstract void Generate(GeneratorTypeContext context);

        protected void GenerateMethod(
                MethodBase methodBuilder,
                MethodBase classMethod,
                MethodInfo interfaceMethod,
                ILGenerator generator,
                GeneratorTypeContext typeContext,
                ParameterInfo[] parameters
            )
        {
            var context = GetMethodContext(
                methodBuilder,
                classMethod,
                interfaceMethod,
                generator,
                typeContext,
                parameters
            );

            GenerateDynamicMethod(context);
        }

        private static GeneratorContext GetMethodContext(
                MethodBase builder,
                MethodBase proxyMethod,
                MethodInfo interfaceMethod,
                ILGenerator generator,
                GeneratorTypeContext typeContext,
                ParameterInfo[] parameters
            )
        {
            var context = new GeneratorContext(typeContext)
            {
                Generator = generator,
                InterfaceMethod = interfaceMethod,
                Parameters = proxyMethod.GetParameters().Select(x => new ParamInfo(x)).ToArray()
            };

            Type fieldType;
            if (proxyMethod is MethodInfo meth)
            {
                context.Method = meth;
                var returnType = meth.ReturnType;
                if (returnType.IsByRef)
                {
                    context.ReturnType = returnType;
                }
                else
                {
                    if (returnType == typeof(void))
                    {
                        context.ReturnType = returnType;
                        context.CallerType = CallerType.Void;
                        fieldType = typeof(VoidCaller);
                    }
                    else if (!meth.IsDefined(typeof(StateMachineAttribute)))
                    {
                        context.ReturnType = returnType;
                        context.CallerType = CallerType.Return;
                        fieldType = typeof(ReturnCaller<>).MakeGenericType(returnType);
                    }
                    else if (returnType == typeof(Task))
                    {
                        context.ReturnType = null;
                        context.CallerType = CallerType.Task;
                        fieldType = typeof(TaskCaller);
                    }
                    else if (returnType.IsGenericType)
                    {
                        var type = returnType.GetGenericTypeDefinition();
                        context.ReturnType = returnType.GetGenericArguments()[0];
                        if (type == typeof(Task<>))
                        {
                            context.CallerType = CallerType.TaskOfT;
                            fieldType = typeof(TaskCaller<>).MakeGenericType(context.ReturnType);
                        }
                        else if (type == typeof(ValueTask<>))
                        {
                            context.CallerType = CallerType.ValueTaskOfT;
                            fieldType = typeof(ValueTaskCaller<>).MakeGenericType(context.ReturnType);
                        }
                        else
                        {
                            throw new InvalidOperationException("function return value error!");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("function return value error!");
                    }

                    context.Caller = context.TypeBuilder.DefineField($"<>_caller{context.Token}", fieldType, FieldAttributes.Private);
                    context.MethodBuilder = (MethodBuilder)builder;
                }
            }
            else if (proxyMethod is ConstructorInfo ctor)
            {
                context.Constructor = ctor;
                context.ReturnType = null;
                context.CallerType = CallerType.Ctor;
                context.Caller = context.TypeBuilder.DefineField($"<>_caller{context.Token}", typeof(VoidCaller), FieldAttributes.Private);
                context.ConstructorBuilder = (ConstructorBuilder)builder;
            }

            return context;
        }

        public void GenerateDynamicMethod(GeneratorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Interface != null && context.ReturnType.IsByRef)
            {
                GenerateRefMethod(context);
            }
            else
            {
                GenerateCallerMethod(context);
                GenerateDisplay(context);
                GenerateMethodBody(context);
            }
        }

        private static void GenerateRefMethod(GeneratorContext context)
        {
            var generator = context.Generator;
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, context.Interface);
            var parameters = context.Parameters;
            for (var i = 1; i <= parameters.Length; i++)
            {
                generator.Emit(OpCodes.Ldarg_S, i);
            }

            generator.Emit(OpCodes.Callvirt, context.InterfaceMethod);
            generator.Emit(OpCodes.Ret);
        }

        private static void GenerateMethodBody(GeneratorContext context)
        {
            var generator = context.MethodBuilder?.GetILGenerator() ??
                                context.ConstructorBuilder?.GetILGenerator();

            DefineLocals(context);
            MethodBodSetp1(context, generator);
            MethodBodSetp2(context, generator);
            MethodBodSetp3(context, generator);
            MethodBodSetp4(context, generator);
        }

        private static void DefineLocals(GeneratorContext context)
        {
            var generator = context.Generator;
            var localCase = context.Parameters.Length > 0;
            var locals = new LocalBuilder[localCase ? 4 : 2];
            context.CallType = context.CallerType == CallerType.Ctor || context.CallerType == CallerType.Void ?
                    typeof(Action) :
                    typeof(Func<>).MakeGenericType(context.Method.ReturnType
                );
            if (localCase)
            {
                // 4
                locals[0] = generator.DeclareLocal(context.DisplayTypeBuilder);
                locals[1] = generator.DeclareLocal(typeof(object[]));
                locals[2] = generator.DeclareLocal(context.CallType);
            }
            else
            {
                // 2                
                locals[0] = generator.DeclareLocal(context.CallType);
            }

            locals[locals.Length - 1] = generator.DeclareLocal(typeof(InterceptorWrapper));
            context.Locals = locals;
        }

        private static void MethodBodSetp1(GeneratorContext context, ILGenerator generator)
        {
            if (context.Parameters.Length == 0)
                return;

            generator.Emit(OpCodes.Newobj, context.DisplayConstructor);
            generator.Emit(OpCodes.Stloc_0);
            generator.Emit(OpCodes.Ldloc_0);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Stfld, context.DisplayFields[0]);
        }

        private static void MethodBodSetp2(GeneratorContext context, ILGenerator generator)
        {
            // if (this._caller == null)
            var label = generator.DefineLabel();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, context.Caller);
            generator.Emit(OpCodes.Brtrue_S, label);

            // InterceptorWrapper wrapper = this._wrappers.GetWrapper(int);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, context.Wrappers);
            generator.Emit(OpCodes.Ldc_I4, context.CallerType == CallerType.Ctor ?
                context.Constructor.MetadataToken :
                context.Method.MetadataToken
            );

            generator.Emit(OpCodes.Callvirt, ReflectionInfoProvider.GetWrapper);
            if (context.Parameters.Length == 0)
            {
                generator.Emit(OpCodes.Stloc_1);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldloc_1);
            }
            else
            {
                generator.Emit(OpCodes.Stloc_3);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldloc_3);
            }

            generator.Emit(OpCodes.Newobj, context.Caller.FieldType.GetConstructor(
                    BindingFlags.Public | BindingFlags.Instance,
                    null,
                    new Type[] { typeof(InterceptorWrapper) },
                    null
                ));

            generator.Emit(OpCodes.Stfld, context.Caller);
            generator.MarkLabel(label);
        }

        private static void MethodBodSetp3(GeneratorContext context, ILGenerator generator)
        {
            if (context.Parameters.Length == 0)
            {
                return;
            }

            var parameters = context.Parameters;
            var displayFields = context.DisplayFields;

            generator.Emit(OpCodes.Ldc_I4, parameters.Length);
            generator.Emit(OpCodes.Newarr, typeof(object));
            generator.Emit(OpCodes.Stloc_1);

            for (var i = 1; i <= parameters.Length; i++)
            {
                var parameter = parameters[i - 1];
                var displayField = displayFields[i];

                generator.Emit(OpCodes.Ldloc_0);
                if (parameter.IsOut)
                {
                    if (displayField.FieldType.IsValueType)
                    {
                        generator.Emit(OpCodes.Ldflda, displayField);
                        generator.Emit(OpCodes.Initobj, displayField.FieldType);
                    }
                    else
                    {
                        generator.Emit(OpCodes.Ldnull);
                        generator.Emit(OpCodes.Stfld, displayField);
                    }
                }
                else if (parameter.IsRef)
                {
                    generator.Emit(OpCodes.Ldarg_S, i);
                    if (displayField.FieldType.IsValueType)
                    {
                        generator.Emit(OpCodes.Ldobj, displayField.FieldType);
                    }
                    else
                    {
                        generator.Emit(OpCodes.Ldind_Ref);
                    }

                    generator.Emit(OpCodes.Stfld, displayField);
                }
                else
                {
                    generator.Emit(OpCodes.Ldarg_S, i);
                    generator.Emit(OpCodes.Stfld, displayField);
                }

                generator.Emit(OpCodes.Ldloc_1);
                generator.Emit(OpCodes.Ldc_I4, i - 1);
                if (parameter.IsOut)
                {
                    generator.Emit(OpCodes.Ldloc_0);
                    generator.Emit(OpCodes.Ldfld, displayField);
                    if (displayField.FieldType.IsValueType)
                    {
                        generator.Emit(OpCodes.Box, displayField.FieldType);
                    }
                }
                else if (parameter.IsRef)
                {
                    generator.Emit(OpCodes.Ldarg_S, i);
                    if (displayField.FieldType.IsValueType)
                    {
                        generator.Emit(OpCodes.Ldobj, displayField.FieldType);
                        generator.Emit(OpCodes.Box, displayField.FieldType);
                    }
                    else
                    {
                        generator.Emit(OpCodes.Ldind_Ref);
                    }
                }
                else
                {
                    generator.Emit(OpCodes.Ldloc_0);
                    generator.Emit(OpCodes.Ldfld, displayField);
                    if (displayField.FieldType.IsValueType)
                    {
                        generator.Emit(OpCodes.Box, displayField.FieldType);
                    }
                }

                generator.Emit(OpCodes.Stelem_Ref);
            }
        }

        private static void MethodBodSetp4(GeneratorContext context, ILGenerator generator)
        {
            var callerType = context.CallerType;
            var parameters = context.Parameters;
            if (parameters.Length > 0)
            {
                // Action or Func<T> call = () => base.Method(xx);
                generator.Emit(OpCodes.Ldloc_0);
                generator.Emit(OpCodes.Ldftn, context.DisplayMethod);
                generator.Emit(OpCodes.Newobj, context.CallType.GetConstructors()[0]);
                generator.Emit(OpCodes.Stloc_2);

                // T result = this._caller.Call(this, call, parameters);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, context.Caller);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldloc_2);
                generator.Emit(OpCodes.Ldloc_1);
                generator.Emit(OpCodes.Ldstr, context.Method.Name);
                generator.Emit(OpCodes.Callvirt, context.Caller.FieldType.GetMethod(
                    "Call",
                    BindingFlags.Public | BindingFlags.Instance
                ));

                for (var i = 1; i <= parameters.Length; i++)
                {
                    var parameter = parameters[i - 1];
                    if (parameter.IsOut || parameter.IsRef)
                    {
                        var field = context.DisplayFields[i];
                        generator.Emit(OpCodes.Ldarg_S, i);
                        generator.Emit(OpCodes.Ldloc_0);
                        generator.Emit(OpCodes.Ldfld, field);
                        if (field.FieldType.IsValueType)
                        {
                            generator.Emit(OpCodes.Stobj, field.FieldType);
                        }
                        else
                        {
                            generator.Emit(OpCodes.Stind_Ref);
                        }
                    }
                }
            }
            else
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldftn, context.CallerMethod);
                generator.Emit(OpCodes.Newobj, context.CallType.GetConstructors()[0]);
                generator.Emit(OpCodes.Stloc_0);

                // this._caller.Call(this, call, null);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, context.Caller);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldloc_0);
                generator.Emit(OpCodes.Ldnull);
                generator.Emit(OpCodes.Ldstr, context.Method.Name);
                generator.Emit(OpCodes.Callvirt, context.Caller.FieldType.GetMethod(
                    "Call",
                    BindingFlags.Public | BindingFlags.Instance
                ));
            }

            generator.Emit(OpCodes.Ret);
        }

        private static void GenerateCallerMethod(GeneratorContext context)
        {
            if (context.Parameters.Length != 0)
            {
                return;
            }

            MethodBase baseMethod;
            Type returnType;
            if (context.CallerType == CallerType.Ctor)
            {
                baseMethod = context.Constructor;
                returnType = null;
            }
            else
            {
                baseMethod = context.Method;
                returnType = context.Method.ReturnType;
            }

            var method = context.TypeBuilder.DefineMethod(
                    $"<{baseMethod.Name}>_{context.Token}",
                    MethodAttributes.Private | MethodAttributes.HideBySig,
                    CallingConventions.HasThis | CallingConventions.Standard,
                    returnType,
                    context.Parameters.Select(x => x.Type).ToArray()
                );

            method.SetMethodParameters(baseMethod, baseMethod.GetParameters());

            var generator = method.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);

            if (context.Parameters.Length > 0)
            {
                for (var i = 1; i <= context.Parameters.Length; i++)
                {
                    generator.Emit(OpCodes.Ldarg_S, i);
                }
            }

            if (context.InterfaceMethod == null)
            {
                if (context.CallerType == CallerType.Ctor)
                {
                    generator.Emit(OpCodes.Call, context.Constructor);
                }
                else
                {
                    generator.Emit(OpCodes.Call, context.Method);
                }
            }
            else
            {
                generator.Emit(OpCodes.Ldfld, context.Interface);
                generator.Emit(OpCodes.Callvirt, context.InterfaceMethod);
            }

            generator.Emit(OpCodes.Ret);
            context.CallerMethod = method;
        }

        private static void GenerateDisplay(GeneratorContext context)
        {
            if (context.Parameters.Length == 0)
            {
                return;
            }

            context.DisplayTypeBuilder = context.TypeBuilder.DefineNestedType(
                $"<display>_proxy{context.Token}",
                TypeAttributes.NestedPrivate |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.Sealed |
                TypeAttributes.BeforeFieldInit
            );

            DefineDisplayFields(context);
            GenerateDisplayCtor(context);
            GenerateDisplayMethod(context);
            context.DisplayTypeBuilder.CreateTypeInfo();
        }

        private static void DefineDisplayFields(GeneratorContext context)
        {
            var parameters = context.Parameters;
            var fields = new FieldInfo[parameters.Length + 1];
            var displayBuilder = context.DisplayTypeBuilder;

            fields[0] = displayBuilder.DefineField(
                "<>__this",
                context.TypeBuilder,
                FieldAttributes.Public
            );

            for (var i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].IsOut || parameters[i].IsRef)
                {
                    var name = parameters[i].Type.FullName;
                    fields[i + 1] = displayBuilder.DefineField(
                        parameters[i].Name,
                        Type.GetType(name.Substring(0, name.Length - 1)),
                        FieldAttributes.Public
                    );
                }
                else
                {
                    fields[i + 1] = displayBuilder.DefineField(
                        parameters[i].Name,
                        parameters[i].Type,
                        FieldAttributes.Public
                    );
                }
            }

            context.DisplayFields = fields;
        }

        private static void GenerateDisplayCtor(GeneratorContext context)
        {
            var ctor = context.DisplayTypeBuilder.DefineConstructor(
                MethodAttributes.Public |
                MethodAttributes.HideBySig |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName,
                CallingConventions.HasThis | CallingConventions.Standard,
                null
            );

            var generator = ctor.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Call, ReflectionInfoProvider.ObjectConstructor);
            generator.Emit(OpCodes.Ret);

            context.DisplayConstructor = ctor;
        }

        private static void GenerateDisplayMethod(GeneratorContext context)
        {
            var method = context.DisplayTypeBuilder.DefineMethod(
                $"<{context.TypeBuilder.Name}>__{context.Token}",
                MethodAttributes.Assembly | MethodAttributes.HideBySig,
                CallingConventions.HasThis | CallingConventions.Standard,
                context.Method?.ReturnType,
                null
            );

            var fields = context.DisplayFields;
            var parameters = context.Parameters;
            var generator = method.GetILGenerator();
            var hasInterface = context.Interface != null;

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, fields[0]);
            if (hasInterface)
            {
                generator.Emit(OpCodes.Ldfld, context.Interface);
            }

            ParamInfo parameter;
            for (var i = 0; i < parameters.Length; i++)
            {
                parameter = parameters[i];
                generator.Emit(OpCodes.Ldarg_0);
                if (parameter.IsOut || parameter.IsRef)
                {
                    generator.Emit(OpCodes.Ldflda, fields[i + 1]);
                }
                else
                {
                    generator.Emit(OpCodes.Ldfld, fields[i + 1]);
                }
            }

            if (hasInterface)
            {
                generator.Emit(OpCodes.Callvirt, context.InterfaceMethod);
            }
            else
            {
                generator.Emit(OpCodes.Call, context.CallerMethod);
            }

            generator.Emit(OpCodes.Ret);
            context.DisplayMethod = method;
        }
    }
}
