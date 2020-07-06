using System;
using System.Reflection;

namespace Raven.AspectFlare.Utilities
{
    internal class ParamInfo
    {
        public ParamInfo(ParameterInfo parameter)
        {
            Name = parameter.Name;
            Type = parameter.ParameterType;
            IsOut = parameter.IsOut;
            IsRef = IsOut == true ? false : Type.IsByRef;
        }

        public string Name { get; set; }
        public Type Type { get; set; }
        public bool IsRef { get; set; }
        public bool IsOut { get; set; }
    }
}
