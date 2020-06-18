using System;

namespace Ging.CoverageUtility
{
    public interface IInstanceFactory
    {

        bool CanHandle(Type type);
        object InstantiateInstanceOf(Type type, bool goodValue);
        void ClearInstances();

    }
}
