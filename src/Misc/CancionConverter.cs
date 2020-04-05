//using System;
//using Newtonsoft.Json;

//namespace aplicacion_musica
//{
//    class CancionConverter : JsonConverter<Cancion>
//    {
//        public override void WriteJson(JsonWriter writer, Cancion value, JsonSerializer serializer)
//        {
//            if (value is Cancion)
//            {
//                writer.WriteComment("Normal");
//                writer.WriteValue(value.titulo);
//                writer.WriteValue(value.duracion.TotalMilliseconds);
//                writer.WriteValue(value.Bonus);
//            }
//            else if (value is CancionLarga c)
//            {
//                writer.WriteComment("Partes");
//                writer.WriteValue(c.titulo);
//                writer.WriteValue(c.Partes);
//            }
//        }
//        public override TimeSpan ReadJson(JsonReader reader, Type objectType, Cancion existingValue, bool hasExistingValue, JsonSerializer serializer)
//        {

//        }
//    }
//}
