using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ging.CoverageUtility
{
    public static class CoverageUtility
    {

        #region Public Static Methods

        public static void TestAllTypesInAssembly(Assembly assembly)
        {
            _ = assembly ?? throw new ArgumentNullException(nameof(assembly));

            TestAllTypesInAssembly(assembly, new CoverageUtilityConfig());
        }

        public static void TestAllTypesInAssembly(Assembly assembly, CoverageUtilityConfig config)
        {
            _ = assembly ?? throw new ArgumentNullException(nameof(assembly));
            _ = config ?? throw new ArgumentNullException(nameof(config));

            foreach (var type in assembly.GetTypes())
            {
                // ZAS: We aren't interested in testing these types of objects
                if (type.IsAbstract || type.IsInterface || type.IsEnum)
                {
                    continue;
                }

                TestMethods(type, config);
            }
        }

        public static T TestMethods<T>()
        {
            return (T)TestMethods(typeof(T), new CoverageUtilityConfig());
        }

        public static T TestMethods<T>(CoverageUtilityConfig config)
        {
            _ = config ?? throw new ArgumentNullException(nameof(config));

            return (T)TestMethods(typeof(T), config);
        }

        public static void TestMethods<T>(CoverageUtilityConfig config, T optionalInstance)
        {
            _ = config ?? throw new ArgumentNullException(nameof(config));

            TestMethods(typeof(T), config, optionalInstance);
        }

        public static object TestMethods(Type type)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));

            return TestMethods(type, new CoverageUtilityConfig());
        }

        public static object TestMethods(Type type, CoverageUtilityConfig config)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));
            _ = config ?? throw new ArgumentNullException(nameof(config));

            // ZAS: Ensure we have value concretes
            config.InstanceFactory = config.InstanceFactory ?? new CSharpInstanceFactory();
            config.Asserter = config.Asserter ?? new NullAsserter();

            object optionalInstance = null;

            var isStatic = type.IsAbstract && type.IsSealed;
            if (!isStatic)
            {
                optionalInstance =
                    config.InstanceFactory.InstantiateInstanceOf(type, goodValue: true) ??
                    throw new InvalidOperationException($"Instance factory failed to instantiate type of {type.Name}")
                ;

                // ZAS: The type used to create the instance might be different than "type"
                //      example: generic types
                type = optionalInstance.GetType();
            }

            TestMethods(type, config, optionalInstance);

            return optionalInstance;
        }

        public static void TestMethods(Type type, CoverageUtilityConfig config, object optionalInstance)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));
            _ = config ?? throw new ArgumentNullException(nameof(config));

            if (type.IsGenericTypeDefinition || type.IsInterface || type.IsEnum)
            {
                throw new System.InvalidOperationException($"Type {type.Name} can not be tested");
            }

            // ZAS: Ensure we have value concretes
            config.InstanceFactory = config.InstanceFactory ?? new CSharpInstanceFactory();
            config.Asserter = config.Asserter ?? new NullAsserter();

            var isStatic = type.IsAbstract && type.IsSealed;
            if (isStatic)
            {
                optionalInstance = null;
            }

            try
            {
                BindingFlags flags = default;
                if (!isStatic)
                {
                    flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
                }
                else
                {
                    flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;
                }

                var methods = type.GetMethods(flags);
                for (int i = 0; i < methods.Length; i++)
                {
                    var method = methods[i];

                    if (method.IsGenericMethodDefinition)
                    {
                        var length = method.GetGenericArguments().Length;
                        var parameters = new Type[length];
                        for (int ii = 0; ii < length; ii++)
                        {
                            parameters[ii] = typeof(object);
                        }

                        method = method.MakeGenericMethod(parameters);
                    }

                    var methodParamInfos = method.GetParameters();
                    var methodParams = new object[methodParamInfos.Length];

                    RunWithGoodParameters(config, type, optionalInstance, method, methodParamInfos, methodParams);
                    RunWithBadParameters(config, type, optionalInstance, method, methodParamInfos, methodParams);
                    RunWithSuggestions(config, type, optionalInstance, method, methodParamInfos, methodParams);
                }
            }
            catch (TargetInvocationException targetInvocationException)
            {
                throw targetInvocationException.InnerException;
            }
            finally
            {
                config.InstanceFactory.ClearInstances();
            }
        }

        #endregion

        #region Private Static Methods

        private static void RunWithGoodParameters(CoverageUtilityConfig config, Type type, object optionalInstance, MethodInfo method, ParameterInfo[] methodParamInfos, object[] methodParams)
        {
            var factory = config.InstanceFactory;
            var asserter = config.Asserter;

            // ZAS: Run a version with all good values
            for (int j = 0; j < methodParams.Length; j++)
            {
                methodParams[j] = factory.InstantiateInstanceOf(methodParamInfos[j].ParameterType, goodValue: true);
            }

            InvokeMethod(asserter, method, type, optionalInstance, methodParams);
        }

        private static void RunWithBadParameters(CoverageUtilityConfig config, Type type, object optionalInstance, MethodInfo method, ParameterInfo[] methodParamInfos, object[] methodParams)
        {
            var factory = config.InstanceFactory;
            var asserter = config.Asserter;

            // ZAS: Now run with one value at a time as "bad"
            for (int i = 0; i < methodParams.Length; i++)
            {
                for (int j = 0; j < methodParams.Length; j++)
                {
                    methodParams[j] = factory.InstantiateInstanceOf(methodParamInfos[j].ParameterType, i != j);
                }

                InvokeMethod(asserter, method, type, optionalInstance, methodParams);
            }
        }

        private static void RunWithSuggestions(CoverageUtilityConfig config, Type type, object optionalInstance, MethodInfo method, ParameterInfo[] methodParamInfos, object[] methodParams)
        {
            var factory = config.InstanceFactory;
            var asserter = config.Asserter;

            // ZAS: If there are any suggestions on this method, let's test them out
            var testSuggestions = method.GetCustomAttributes<TestSuggestionAttribute>().ToArray();
            for (int i = 0; i < testSuggestions.Length; i++)
            {
                var parameter = testSuggestions[i].Parameter;
                if (parameter == null)
                {
                    //$$ report issue?
                    continue;
                }

                for (int j = 0; j < methodParams.Length; j++)
                {
                    if (methodParamInfos[j].Name == parameter)
                    {
                        methodParams[j] = testSuggestions[i].Value;
                    }
                    else
                    {
                        methodParams[j] = factory.InstantiateInstanceOf(methodParamInfos[j].ParameterType, goodValue: true);
                    }
                }

                InvokeMethod(asserter, method, type, optionalInstance, methodParams);
            }
        }

        private static void InvokeMethod(IAsserter asserter, MethodInfo method, Type type, object optionalInstance, object[] args)
        {
            var thrownExceptions = new List<Exception>();
            try
            {
                var taskResult = method.Invoke(optionalInstance, args) as Task;
                if (taskResult != null)
                {
                    taskResult.Wait();
                }
            }
            catch (AggregateException aggregateException)
            {
                thrownExceptions.AddRange(aggregateException.InnerExceptions);
            }
            catch (TargetInvocationException targetInvocationException)
            {
                thrownExceptions.Add(targetInvocationException.InnerException);
            }
            catch (Exception genericException)
            {
                thrownExceptions.Add(genericException);
            }

            for (int i = 0; i < thrownExceptions.Count; i++)
            {
                if (thrownExceptions[i] == null)
                {
                    continue;
                }

                asserter.AssertMethodException(thrownExceptions[i], type, method);
            }
        }

        #endregion

    }
}