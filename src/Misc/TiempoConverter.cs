using Newtonsoft.Json;
using System;

namespace aplicacion_musica
{
    public class TiempoConverter: JsonConverter<TimeSpan>
    {
        public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
        {
            writer.WriteValue((int)value.TotalMilliseconds);
        }
        public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            int msTotales = Convert.ToInt32(reader.Value);
            return new TimeSpan(0, 0, 0, 0, msTotales);
        }
    }
}
