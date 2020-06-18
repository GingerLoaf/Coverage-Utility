using System;

namespace Ging.CoverageUtility
{
    public static class ReflectionUtility
    {

        #region Public Static Methods

        public static Type[] GetGenericArguments(Type type)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));

            Type[] genericArgs = null;
            if (type.IsGenericType)
            {
                genericArgs = type.GetGenericArguments();
            }
            else
            {
                var currentType = type.BaseType;
                while (genericArgs == null && currentType != null)
                {
                    if (currentType.IsGenericType)
                    {
                        genericArgs = currentType.GetGenericArguments();
                        break;
                    }

                    currentType = currentType.BaseType;
                }
            }

            return genericArgs;
        }

        #endregion

    }
}
