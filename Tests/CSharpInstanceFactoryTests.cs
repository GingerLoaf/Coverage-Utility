using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Ging.CoverageUtility.Tests
{
    public class CSharpInstanceFactoryTests
    {

        [Test]
        public void TestBasicUseGoodValues()
        {
            var instanceFactory = new CSharpInstanceFactory();

            // ZAS: Good Value type defaults are just the default() value
            Assert.AreEqual(default(bool), instanceFactory.InstantiateInstanceOf<bool>(goodValue: true));
            Assert.AreEqual(default(byte), instanceFactory.InstantiateInstanceOf<byte>(goodValue: true));
            Assert.AreEqual(default(sbyte), instanceFactory.InstantiateInstanceOf<sbyte>(goodValue: true));
            Assert.AreEqual(default(char), instanceFactory.InstantiateInstanceOf<char>(goodValue: true));
            Assert.AreEqual(default(decimal), instanceFactory.InstantiateInstanceOf<decimal>(goodValue: true));
            Assert.AreEqual(default(double), instanceFactory.InstantiateInstanceOf<double>(goodValue: true));
            Assert.AreEqual(default(float), instanceFactory.InstantiateInstanceOf<float>(goodValue: true));
            Assert.AreEqual(default(int), instanceFactory.InstantiateInstanceOf<int>(goodValue: true));
            Assert.AreEqual(default(uint), instanceFactory.InstantiateInstanceOf<uint>(goodValue: true));
            Assert.AreEqual(default(long), instanceFactory.InstantiateInstanceOf<long>(goodValue: true));
            Assert.AreEqual(default(ulong), instanceFactory.InstantiateInstanceOf<ulong>(goodValue: true));
            Assert.AreEqual(default(short), instanceFactory.InstantiateInstanceOf<short>(goodValue: true));
            Assert.AreEqual(default(ushort), instanceFactory.InstantiateInstanceOf<ushort>(goodValue: true));

            // ZAS: Good reference types are a pre-determined value instead of being null
            Assert.AreEqual(CSharpInstanceFactory.GOOD_OBJECT_VALUE, instanceFactory.InstantiateInstanceOf<object>(goodValue: true));
            Assert.AreEqual(CSharpInstanceFactory.GOOD_STRING_VALUE, instanceFactory.InstantiateInstanceOf<string>(goodValue: true));

            // ZAS: Default values can be customized
            instanceFactory.AssignDefaultValue<bool>(true, goodValue: true);
            instanceFactory.AssignDefaultValue<byte>(3, goodValue: true);
            instanceFactory.AssignDefaultValue<sbyte>(3, goodValue: true);
            instanceFactory.AssignDefaultValue<char>('3', goodValue: true);
            instanceFactory.AssignDefaultValue<decimal>(3, goodValue: true);
            instanceFactory.AssignDefaultValue<double>(3, goodValue: true);
            instanceFactory.AssignDefaultValue<float>(3, goodValue: true);
            instanceFactory.AssignDefaultValue<int>(3, goodValue: true);
            instanceFactory.AssignDefaultValue<uint>(3, goodValue: true);
            instanceFactory.AssignDefaultValue<long>(3, goodValue: true);
            instanceFactory.AssignDefaultValue<ulong>(3, goodValue: true);
            instanceFactory.AssignDefaultValue<short>(3, goodValue: true);
            instanceFactory.AssignDefaultValue<ushort>(3, goodValue: true);

            var newGoodDefaultObject = new { TheValue = 3 };
            instanceFactory.AssignDefaultValue<object>(newGoodDefaultObject, goodValue: true);

            var newGoodDefaultString = "Three";
            instanceFactory.AssignDefaultValue<string>(newGoodDefaultString, goodValue: true);

            // ZAS: Make sure our new values have been applied
            Assert.AreEqual(true, instanceFactory.InstantiateInstanceOf<bool>(goodValue: true));
            Assert.AreEqual(3, instanceFactory.InstantiateInstanceOf<byte>(goodValue: true));
            Assert.AreEqual(3, instanceFactory.InstantiateInstanceOf<sbyte>(goodValue: true));
            Assert.AreEqual('3', instanceFactory.InstantiateInstanceOf<char>(goodValue: true));
            Assert.AreEqual(3, instanceFactory.InstantiateInstanceOf<decimal>(goodValue: true));
            Assert.AreEqual(3, instanceFactory.InstantiateInstanceOf<double>(goodValue: true));
            Assert.AreEqual(3, instanceFactory.InstantiateInstanceOf<float>(goodValue: true));
            Assert.AreEqual(3, instanceFactory.InstantiateInstanceOf<int>(goodValue: true));
            Assert.AreEqual(3, instanceFactory.InstantiateInstanceOf<uint>(goodValue: true));
            Assert.AreEqual(3, instanceFactory.InstantiateInstanceOf<long>(goodValue: true));
            Assert.AreEqual(3, instanceFactory.InstantiateInstanceOf<ulong>(goodValue: true));
            Assert.AreEqual(3, instanceFactory.InstantiateInstanceOf<short>(goodValue: true));
            Assert.AreEqual(3, instanceFactory.InstantiateInstanceOf<ushort>(goodValue: true));
            Assert.AreEqual(newGoodDefaultObject, instanceFactory.InstantiateInstanceOf<object>(goodValue: true));
            Assert.AreEqual(newGoodDefaultString, instanceFactory.InstantiateInstanceOf<string>(goodValue: true));

            // ZAS: Clear back to defaults
            instanceFactory.ClearDefaultValues(goodValues: true);
        }

        [Test]
        public void TestBasicUseBadValues()
        {
            var instanceFactory = new CSharpInstanceFactory();

            // ZAS: Bad Value type defaults are values that are likely to not be considered
            Assert.AreEqual(false, instanceFactory.InstantiateInstanceOf<bool>(goodValue: false));
            Assert.AreEqual(byte.MaxValue, instanceFactory.InstantiateInstanceOf<byte>(goodValue: false));
            Assert.AreEqual(sbyte.MinValue, instanceFactory.InstantiateInstanceOf<sbyte>(goodValue: false));
            Assert.AreEqual(char.MinValue, instanceFactory.InstantiateInstanceOf<char>(goodValue: false));
            Assert.AreEqual(decimal.MinValue, instanceFactory.InstantiateInstanceOf<decimal>(goodValue: false));
            Assert.AreEqual(double.MinValue, instanceFactory.InstantiateInstanceOf<double>(goodValue: false));
            Assert.AreEqual(float.MinValue, instanceFactory.InstantiateInstanceOf<float>(goodValue: false));
            Assert.AreEqual(int.MinValue, instanceFactory.InstantiateInstanceOf<int>(goodValue: false));
            Assert.AreEqual(uint.MaxValue, instanceFactory.InstantiateInstanceOf<uint>(goodValue: false));
            Assert.AreEqual(long.MinValue, instanceFactory.InstantiateInstanceOf<long>(goodValue: false));
            Assert.AreEqual(ulong.MaxValue, instanceFactory.InstantiateInstanceOf<ulong>(goodValue: false));
            Assert.AreEqual(short.MinValue, instanceFactory.InstantiateInstanceOf<short>(goodValue: false));
            Assert.AreEqual(ushort.MaxValue, instanceFactory.InstantiateInstanceOf<ushort>(goodValue: false));

            // ZAS: Bad reference types are just null
            Assert.AreEqual(null, instanceFactory.InstantiateInstanceOf<object>(goodValue: false));
            Assert.AreEqual(null, instanceFactory.InstantiateInstanceOf<string>(goodValue: false));

            // ZAS: Default values can be customized
            instanceFactory.AssignDefaultValue<bool>(false, goodValue: false);
            instanceFactory.AssignDefaultValue<byte>(3, goodValue: false);
            instanceFactory.AssignDefaultValue<sbyte>(-3, goodValue: false);
            instanceFactory.AssignDefaultValue<char>('3', goodValue: false);
            instanceFactory.AssignDefaultValue<decimal>(-3, goodValue: false);
            instanceFactory.AssignDefaultValue<double>(-3, goodValue: false);
            instanceFactory.AssignDefaultValue<float>(-3, goodValue: false);
            instanceFactory.AssignDefaultValue<int>(-3, goodValue: false);
            instanceFactory.AssignDefaultValue<uint>(3, goodValue: false);
            instanceFactory.AssignDefaultValue<long>(-3, goodValue: false);
            instanceFactory.AssignDefaultValue<ulong>(3, goodValue: false);
            instanceFactory.AssignDefaultValue<short>(-3, goodValue: false);
            instanceFactory.AssignDefaultValue<ushort>(3, goodValue: false);

            var newBadDefaultObject = new { BrokenObject = true };
            instanceFactory.AssignDefaultValue<object>(newBadDefaultObject, goodValue: false);

            var newBadDefaultString = "!@#$%^&*()";
            instanceFactory.AssignDefaultValue<string>(newBadDefaultString, goodValue: false);

            // ZAS: Make sure our new values have been applied
            Assert.AreEqual(false, instanceFactory.InstantiateInstanceOf<bool>(goodValue: false));
            Assert.AreEqual(3, instanceFactory.InstantiateInstanceOf<byte>(goodValue: false));
            Assert.AreEqual(-3, instanceFactory.InstantiateInstanceOf<sbyte>(goodValue: false));
            Assert.AreEqual('3', instanceFactory.InstantiateInstanceOf<char>(goodValue: false));
            Assert.AreEqual(-3, instanceFactory.InstantiateInstanceOf<decimal>(goodValue: false));
            Assert.AreEqual(-3, instanceFactory.InstantiateInstanceOf<double>(goodValue: false));
            Assert.AreEqual(-3, instanceFactory.InstantiateInstanceOf<float>(goodValue: false));
            Assert.AreEqual(-3, instanceFactory.InstantiateInstanceOf<int>(goodValue: false));
            Assert.AreEqual(3, instanceFactory.InstantiateInstanceOf<uint>(goodValue: false));
            Assert.AreEqual(-3, instanceFactory.InstantiateInstanceOf<long>(goodValue: false));
            Assert.AreEqual(3, instanceFactory.InstantiateInstanceOf<ulong>(goodValue: false));
            Assert.AreEqual(-3, instanceFactory.InstantiateInstanceOf<short>(goodValue: false));
            Assert.AreEqual(3, instanceFactory.InstantiateInstanceOf<ushort>(goodValue: false));
            Assert.AreEqual(newBadDefaultObject, instanceFactory.InstantiateInstanceOf<object>(goodValue: false));
            Assert.AreEqual(newBadDefaultString, instanceFactory.InstantiateInstanceOf<string>(goodValue: false));

            // ZAS: Clear back to defaults
            instanceFactory.ClearDefaultValues(goodValues: false);
        }

        [Test]
        public void TestAdvancedObjects()
        {
            var instanceFactory = new CSharpInstanceFactory();

            // ZAS: Is able to navigate generic types
            var list = instanceFactory.InstantiateInstanceOf<List<string>>(goodValue: true);
            Assert.IsNotNull(list);

            // ZAS: Is able to navigate complicated objects and dependencies
            var complexObject = instanceFactory.InstantiateInstanceOf<AdvancedObjectA>(goodValue: true);
            Assert.IsNotNull(complexObject);

            // ZAS: Is able to navigate interface objects
            var interfaceObject = instanceFactory.InstantiateInstanceOf<IAdvancedObject>(goodValue: true);
            Assert.IsNotNull(interfaceObject);
            Assert.IsTrue(typeof(IAdvancedObject).IsAssignableFrom(interfaceObject.GetType()));

            // ZAS: Is able to create dynamic Actions
            var emptyAction = instanceFactory.InstantiateInstanceOf<Action>(goodValue: true);
            Assert.IsNotNull(emptyAction);
            Assert.DoesNotThrow(new TestDelegate(emptyAction));

            var boolAction = instanceFactory.InstantiateInstanceOf<Action<bool>>(goodValue: true);
            Assert.IsNotNull(boolAction);
            Assert.DoesNotThrow(() => boolAction(false));

            // ZAS: Is able to create dynamic functions
            var emptyFunction = instanceFactory.InstantiateInstanceOf<Func<bool>>(goodValue: true);
            Assert.IsNotNull(emptyFunction);
            Assert.IsFalse(emptyFunction());

            var boolFunction = instanceFactory.InstantiateInstanceOf<Func<bool, bool>>(goodValue: true);
            Assert.IsNotNull(boolFunction);
            Assert.IsFalse(boolFunction(false));
        }

        public interface IAdvancedObject
        {

        }

        private class AdvancedObjectA
        {
            public AdvancedObjectA(AdvancedObjectB objectB)
            {
                m_objectB = objectB;
            }

            private readonly AdvancedObjectB m_objectB = null;
        }

        private class AdvancedObjectB
        {
            public AdvancedObjectB(AdvancedObjectC objectC)
            {
                m_objectC = objectC;
            }

            private readonly AdvancedObjectC m_objectC = null;
        }

        private class AdvancedObjectC
        {

        }

    }
}
