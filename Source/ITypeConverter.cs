using System;

namespace Ging.CoverageUtility
{
    public interface ITypeConverter
    {
        bool CanHandle(Type type);
        Type ConvertType(Type type);
    }
}
