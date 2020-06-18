using System;
using System.Reflection;

namespace Ging.CoverageUtility
{
    public class NullAsserter : IAsserter
    {

        #region Public Methods

        public void AssertMethodException(Exception exception, Type type, MethodInfo method)
        {
            // Do nothing
        }

        #endregion

    }
}
