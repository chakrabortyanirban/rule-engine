using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace RuleEngine.Domain.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProductTypeEnum
    {
        Unknown = 0,
        PhysicalProduct = 1,
        MembershipProduct = 2,
        VideoProduct = 3,
    }
}
