using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RuleEngine.Domain.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProductCategory
    {
        Unknown = 0,
        Book = 1,
        Video = 2,
        Membership = 3,
    }
}
