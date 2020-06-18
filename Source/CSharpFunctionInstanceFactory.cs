using System;
using System.Linq;
using System.Linq.Expressions;

namespace Ging.CoverageUtility
{
    internal class CSharpFunctionInstanceFactory : IInstanceFactory
    {

        #region Constructors

        public CSharpFunctionInstanceFactory(IInstanceFactory rootFactory)
        {
            m_rootFactory = rootFactory ?? throw new ArgumentNullException(nameof(rootFactory));
        }

        #endregion

        #region Private Fields

        private readonly IInstanceFactory m_rootFactory = null;

        #endregion

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

            return type.FullName.StartsWith("System.Func", StringComparison.InvariantCulture);
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
                return null;
            }

            var parameters = genericArgs.Select(a => Expression.Parameter(a, a.Name)).ToArray();
            var allButLast = parameters.Take(parameters.Length - 1);
            var last = parameters.Last();
            var returnValue = m_rootFactory.InstantiateInstanceOf(last.Type, goodValue);
            var lambdaFunction = Expression.Lambda(type, Expression.Constant(returnValue, last.Type), allButLast).Compile();
            return lambdaFunction;
        }

        #endregion

    }
}
