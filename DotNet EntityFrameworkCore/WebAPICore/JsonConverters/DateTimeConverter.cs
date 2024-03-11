using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DotNet_EntityFrameworkCore.WebAPICore
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        internal static DateTimeStyles SupportedStyles = DateTimeStyles.AllowWhiteSpaces;
        public DateTimeConverter()
        {
            SupportedStyles = SupportedStyles | DateTimeStyles.AdjustToUniversal;
        }
        public DateTimeConverter(DateTimeStyles dateTimeStyles)
        {
            SupportedStyles = SupportedStyles | dateTimeStyles;
        }
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString(), CultureInfo.InvariantCulture, SupportedStyles);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToUniversalTime().ToString("o"));
        }
    }
}
