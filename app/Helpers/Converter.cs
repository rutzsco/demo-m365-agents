using System.Text.Json;
using System.Text.Json.Serialization;

namespace microsoft_agent_sk.Helpers
{
    public class JsonObjectToStringConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // If the token is a string, return it directly.
            if (reader.TokenType == JsonTokenType.String)
            {
                return reader.GetString();
            }
            else
            {
                // Otherwise, parse the current JSON object and return its raw text.
                using (JsonDocument document = JsonDocument.ParseValue(ref reader))
                {
                    return document.RootElement.GetRawText();
                }
            }
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            // Write the value as a JSON string.
            writer.WriteStringValue(value);
        }
    }
}
