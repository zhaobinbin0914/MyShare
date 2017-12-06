using Newtonsoft.Json.Converters;
using System;
using Newtonsoft.Json;

namespace MyShare.Kernel.Json
{
    public class DateTimeConverter : IsoDateTimeConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var date = base.ReadJson(reader, objectType, existingValue, serializer) as DateTime?;

            return date.HasValue ? UniversalTime(date.Value) : null;
        }

        private static object UniversalTime(DateTime date)
        {
            switch (date.Kind)
            {
                case DateTimeKind.Unspecified:
                    return DateTime.SpecifyKind(date, DateTimeKind.Utc);
                case DateTimeKind.Local:
                    return date.ToUniversalTime();
            }

            return date;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var date = value as DateTime?;
            base.WriteJson(writer, date.HasValue ? UniversalTime(date.Value) : value, serializer);
        }
    }
}
