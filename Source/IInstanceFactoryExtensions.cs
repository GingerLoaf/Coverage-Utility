namespace Ging.CoverageUtility
{
    public static class IInstanceFactoryExtensions
    {
        public static T InstantiateInstanceOf<T>(this IInstanceFactory objectReference, bool goodValue)
        {
            return (T)objectReference.InstantiateInstanceOf(typeof(T), goodValue);
        }
    }
}
