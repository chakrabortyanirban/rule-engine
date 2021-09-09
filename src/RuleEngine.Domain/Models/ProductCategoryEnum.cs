using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace RuleEngine.Domain.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProductCategoryEnum
    {
        Unknown = 0,
        Book = 1,
        Video = 2,
        Membership = 3,
    }
}
