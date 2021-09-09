using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;
namespace RuleEngine.Domain.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AllowedProductEnum
    {
        Unknown = 0,
        Book = 1,
        Membership = 2,
        UpgradeMembership = 3,
        Video = 4
    }
}
