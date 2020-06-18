using System;
using System.Reflection;

namespace Ging.CoverageUtility
{
    public interface IAsserter
    {
        void AssertMethodException(Exception exception, Type type, MethodInfo method);
    }
}
