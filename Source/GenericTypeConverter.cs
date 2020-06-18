using System;

namespace Ging.CoverageUtility
{
    internal class GenericTypeConverter : ITypeConverter
    {

        #region Public Methods

        public bool CanHandle(Type type)
        {
            if (type == null)
            {
                return false;
            }

            return type.IsGenericTypeDefinition;
        }

        public Type ConvertType(Type type)
        {
            if (type == null)
            {
                return null;
            }

            var arguments = type.GetGenericArguments();
            var types = new Type[arguments.Length];
            for (int i = 0; i < types.Length; i++)
            {
                types[i] = arguments[i].BaseType;
            }

            return type.MakeGenericType(types);
        }

        #endregion

    }
}
