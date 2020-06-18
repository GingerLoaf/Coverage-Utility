using Ging.CoverageUtility;
using NUnit.Framework;
using System;
using System.Reflection;

namespace Tests
{
    public class NUnitTestAsserter : IAsserter
    {
        public void AssertMethodException(Exception exception, Type type, MethodInfo method)
        {
            if (!(exception is ArgumentNullException))
            {
                Assert.Fail($"{type.Name}.{method.Name} encountered exception: {exception}");
            }
        }
    }
}
