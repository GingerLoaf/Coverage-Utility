using NUnit.Framework;
using System;
using System.Reflection;
using Tests;

namespace Ging.CoverageUtility.Tests
{
    public class CoverageUtilityTests
    {

        [Test]
        public void TestBasicUse()
        {
            var config = new CoverageUtilityConfig();
            config.Asserter = new NUnitTestAsserter();

            var assembly = Assembly.Load("Ging.CoverageUtility");
            CoverageUtility.TestAllTypesInAssembly(assembly, config);
        }

    }

    public class TestClass
    {

        public bool Constructor0_Value = false;
        public string Constructor1_Value = null;
        public (string, string) Constructor2_Value = default;
        public bool Test0ArgsVoidReturn_Value = false;
        public bool Test1ArgsStringReturn_Value = false;
        public string Test2ArgStringReturn_Value = null;
        public bool Test3ArgMatchedSuggestion = false;

        public TestClass()
        {
            Constructor0_Value = true;
        }

        public TestClass(string arg1)
            : this()
        {
            _ = arg1 ?? throw new ArgumentNullException(nameof(arg1));

            Constructor1_Value = arg1;
        }

        public TestClass(string arg1, string arg2)
            : this(arg1)
        {
            _ = arg2 ?? throw new ArgumentNullException(nameof(arg2));

            Constructor2_Value = (arg1, arg2);
        }

        public T Test1ArgWithGenerics<T>(T input)
        {
            return input;
        }

        public bool Test1ArgOutBoolReturn(out string result)
        {
            result = "out";
            return false;
        }

        public void Test0ArgsVoidReturn()
        {
            Test0ArgsVoidReturn_Value = true;
        }

        public string Test0ArgsStringReturn()
        {
            Test1ArgsStringReturn_Value = true;
            return Test1ArgsStringReturn_Value.ToString();
        }

        public string Test1ArgStringReturn(string @string)
        {
            if (string.IsNullOrEmpty(@string)) throw new ArgumentNullException(nameof(@string));

            Test2ArgStringReturn_Value = @string;
            return Test2ArgStringReturn_Value;
        }

        [TestSuggestion(Parameter = "string", Value = "TESTSTRING")]
        public void Test2ArgStringVoidReturnSuggestion(string @string)
        {
            if (string.IsNullOrEmpty(@string)) throw new ArgumentNullException(nameof(@string));

            if (@string == "TESTSTRING")
            {
                Test3ArgMatchedSuggestion = true;
            }
        }

    }
}
