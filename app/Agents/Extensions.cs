namespace microsoft_agent_sk.Agents
{
    public static class Extensions
    {
        /// <summary>
        /// Returns the last <see cref="ChatMessage"/> from the conversation.
        /// </summary>
        /// <param name="request">The chat completion request.</param>
        /// <returns>The last message if one exists; otherwise, <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="request"/> is null.</exception>
        public static ChatMessage? GetLastMessage(this ChatRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return request.Messages?.LastOrDefault();
        }
    }
}
