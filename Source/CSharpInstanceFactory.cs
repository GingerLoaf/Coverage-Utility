using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ging.CoverageUtility
{
    public class CSharpInstanceFactory : IInstanceFactory
    {

        #region Public Constants

        public const string GOOD_STRING_VALUE = "GoodString";
        public static readonly object GOOD_OBJECT_VALUE = new object();

        #endregion

        #region Constructors

        public CSharpInstanceFactory()
        {
            m_instanceFactories = new List<IInstanceFactory>()
            {
                new CSharpActionInstanceFactory(),
                new CSharpFunctionInstanceFactory(this),
                new CSharpInterfaceInstanceFactory()
            };

            m_typeConverters = new List<ITypeConverter>()
            {
                new GenericTypeConverter()
            };
        }

        #endregion

        #region Private Fields

        private readonly List<IInstanceFactory> m_instanceFactories = null;
        private readonly List<ITypeConverter> m_typeConverters = null;
        private readonly ConcurrentDictionary<Type, object> m_goodValueMap = new ConcurrentDictionary<Type, object>();
        private readonly ConcurrentDictionary<Type, object> m_badValueMap = new ConcurrentDictionary<Type, object>();

        #endregion

        #region Public Methods

        public void ClearDefaultValues(bool goodValues)
        {
            if (goodValues)
            {
                m_goodValueMap.Clear();
            }
            else
            {
                m_badValueMap.Clear();
            }
        }

        public void AssignDefaultValue<T>(T value, bool goodValue)
        {
            AssignDefaultValue(typeof(T), value, goodValue);
        }

        public void AssignDefaultValue(Type type, object value, bool goodValue)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));

            if (goodValue)
            {
                m_goodValueMap[type] = value;
            }
            else
            {
                m_badValueMap[type] = value;
            }
        }

        public bool CanHandle(Type type)
        {
            return type != null;
        }

        public object InstantiateInstanceOf(Type type, bool goodValue)
        {
            if (type == null)
            {
                return null;
            }

            // ZAS: Convert generic type templates to generic types
            var convertedType =
                ConvertType(type) ??
                throw new InvalidOperationException($"Type {type.Name} was converted to null")
            ;

            if (goodValue)
            {
                if (m_goodValueMap.TryGetValue(convertedType, out object value))
                {
                    return value;
                }
            }
            else
            {
                if (m_badValueMap.TryGetValue(convertedType, out object value))
                {
                    return value;
                }
            }

            if (convertedType.IsValueType)
            {
                if (!goodValue)
                {
                    if (convertedType == typeof(int)) return int.MinValue;
                    if (convertedType == typeof(uint)) return uint.MaxValue;
                    if (convertedType == typeof(short)) return short.MinValue;
                    if (convertedType == typeof(ushort)) return ushort.MaxValue;
                    if (convertedType == typeof(decimal)) return decimal.MinValue;
                    if (convertedType == typeof(byte)) return byte.MaxValue;
                    if (convertedType == typeof(sbyte)) return sbyte.MinValue;
                    if (convertedType == typeof(long)) return long.MinValue;
                    if (convertedType == typeof(ulong)) return ulong.MaxValue;
                    if (convertedType == typeof(float)) return float.MinValue;
                    if (convertedType == typeof(double)) return double.MinValue;
                }
            }
            else
            {
                if (!goodValue)
                {
                    return null;
                }

                if (convertedType == typeof(string)) return GOOD_STRING_VALUE;
                if (convertedType == typeof(object)) return GOOD_OBJECT_VALUE;
            }

            var subHandler = m_instanceFactories.FirstOrDefault(f => f.CanHandle(convertedType));
            if (subHandler != null)
            {
                return subHandler.InstantiateInstanceOf(convertedType, goodValue);
            }

            var constructors = convertedType.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            // ZAS: Select constructor with the least amount of reference types to end the cycle of constructing stuff
            var ctors = constructors.OrderBy(c => c.GetParameters().Count(p => !p.ParameterType.IsValueType)).ToArray();
            var bestConstructor = ctors.FirstOrDefault();
            if (bestConstructor == null)
            {
                if (convertedType.IsAbstract)
                {
                    return null;
                }

                return Activator.CreateInstance(convertedType);
            }

            var args = bestConstructor.GetParameters().Select(p => InstantiateInstanceOf(p.ParameterType, goodValue)).ToArray();
            return bestConstructor.Invoke(args);
        }

        public void ClearInstances()
        {
            // Do nothing
        }

        #endregion

        #region Private Methods

        private Type ConvertType(Type type)
        {
            var resultingType = type;
            for (int i = 0; i < m_typeConverters.Count; i++)
            {
                if (m_typeConverters[i].CanHandle(type))
                {
                    resultingType = m_typeConverters[i].ConvertType(resultingType);
                }
            }

            return resultingType;
        }

        #endregion

    }
}
