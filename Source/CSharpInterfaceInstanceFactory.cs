using System;

namespace Ging.CoverageUtility
{
    internal class CSharpInterfaceInstanceFactory : IInstanceFactory
    {

        #region Public Methods

        public bool CanHandle(Type type)
        {
            if (type == null)
            {
                return false;
            }

            return type.IsInterface;
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

            //$$
            // TODO-ZAS: replace moq with my own framework
            var mockedType = typeof(Moq.Mock<>).MakeGenericType(type);
            var mockInterface = Activator.CreateInstance(mockedType) as Moq.Mock;
            return mockInterface.Object;
        }

        #endregion

    }
}
