using System.Text.Json;
using System.Text.Json.Serialization;

public class IgnoreUnsupportedTypesConverter : JsonConverter<object>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(System.Reflection.MethodBase).IsAssignableFrom(typeToConvert);
    }

    public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException($"Deserialization of {typeToConvert} is not supported.");
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        // Desteklenmeyen türleri serileştirme
        writer.WriteNullValue();
    }
}
