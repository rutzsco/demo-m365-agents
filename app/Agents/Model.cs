using System.Text.Json.Serialization;

namespace microsoft_agent_sk.Agents
{
    public record ChatMessage( [property: JsonPropertyName("role")] string Role, [property: JsonPropertyName("content")] string Content);


    public record ChatRequest([property: JsonPropertyName("messages")] IEnumerable<ChatMessage> Messages);
}
