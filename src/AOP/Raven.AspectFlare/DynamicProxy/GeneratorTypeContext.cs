using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Raven.AspectFlare.DynamicProxy
{
    public class GeneratorTypeContext
    {
        public int Token { get; set; } = 0;
        public ModuleBuilder ModuleBuilder { get; set; }
        public Type ClassType { get; set; }
        public Type InterfaceType { get; set; }
        public TypeBuilder TypeBuilder { get; set; }
        public FieldBuilder Wrappers { get; set; }
        public FieldBuilder Interface { get; set; }
        public MethodBuilder InitMethod { get; set; }
        public List<RuntimeMethodHandle> MethodHandles { get; set; } = new List<RuntimeMethodHandle>();
    }
}
