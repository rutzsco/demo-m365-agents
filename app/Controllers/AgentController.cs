using Microsoft.Agents.Hosting.AspNetCore;
using Microsoft.Agents.Protocols.Primitives;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using microsoft_agent_sk.Agents;
using MinimalApi.Services.Skills;

namespace microsoft_agent_sk.Controllers
{
    [Route("api/agent")]
    [ApiController]
    public class AgentController(WeatherAgent weatherAgent) : ControllerBase
    {
        [HttpPost]
        public Task PostAsync(CancellationToken cancellationToken)
           => weatherAgent.InvokeAgentAsync("MankatoMN");
    }
}
