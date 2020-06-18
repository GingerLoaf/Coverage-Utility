using System;

namespace Ging.CoverageUtility
{
    public class TestSuggestionAttribute : Attribute
    {
        public string Parameter;
        public object Value;
    }
}
