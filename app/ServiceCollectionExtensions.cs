using Microsoft.Agents.Authentication;
using Microsoft.Agents.Builder;
using Microsoft.Agents.Hosting.AspNetCore;
using Microsoft.Agents.Storage;

namespace microsoft_agent_sk
{
    public static class ServiceCollectionExtensions
    {
        public static IHostApplicationBuilder AddBot<TAgent, THandler>(this IHostApplicationBuilder builder) where TAgent : IAgent where THandler : class, TAgent
        {
            builder.Services.AddAgentAspNetAuthentication(builder.Configuration);

            builder.AddAgentApplicationOptions();

            builder.AddAgent<THandler>();

            builder.Services.AddSingleton<IStorage, MemoryStorage>();

            return builder;
        }

    }
}
