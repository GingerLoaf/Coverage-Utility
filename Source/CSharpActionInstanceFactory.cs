using System;
using System.Linq;
using System.Linq.Expressions;

namespace Ging.CoverageUtility
{
    internal class CSharpActionInstanceFactory : IInstanceFactory
    {

        #region Public Methods

        public bool CanHandle(Type type)
        {
            if (type == null)
            {
                return false;
            }

            if (type.FullName == null)
            {
                return false;
            }

            return type.FullName.StartsWith("System.Action", StringComparison.InvariantCulture);
        }

        public void ClearInstances()
        {
            // Do Nothing
        }

        public object InstantiateInstanceOf(Type type, bool goodValue)
        {
            if (!goodValue)
            {
                return null;
            }

            Type[] genericArgs = ReflectionUtility.GetGenericArguments(type);
            if (genericArgs == null)
            {
                return new Action(() => { });
            }

            var parameters = genericArgs.Select(a => Expression.Parameter(a, a.Name)).ToArray();
            var lambdaFunction = Expression.Lambda(type, Expression.Empty(), parameters).Compile();
            return lambdaFunction;
        }

        #endregion

    }
}
