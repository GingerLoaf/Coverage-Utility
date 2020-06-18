namespace Ging.CoverageUtility
{
    public class CoverageUtilityConfig
    {
        public IInstanceFactory InstanceFactory = new CSharpInstanceFactory();
        public IAsserter Asserter = new NullAsserter();
    }
}
