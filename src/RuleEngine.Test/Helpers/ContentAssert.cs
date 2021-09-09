using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace RuleEngine.Test.Helpers
{
    internal static class ContentAssert
    {
        public static void JsonAreEquivalents(string expectedJson, string actualJson)
        {
            var one = JToken.Parse(expectedJson);
            var two = JToken.Parse(actualJson);

            if (JToken.DeepEquals(one, two)) return;
            var expected = one.ToString();
            var actual = two.ToString();

            StringAssert.AreEqualIgnoringCase(expected, actual);
        }
    }
}
