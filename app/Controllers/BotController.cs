using Microsoft.Agents.Builder;
using Microsoft.Agents.Hosting.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace microsoft_agent_sk.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class BotController(IAgentHttpAdapter adapter, IAgent bot) : ControllerBase
    {
        [HttpPost]
        public Task PostAsync(CancellationToken cancellationToken)
           => adapter.ProcessAsync(Request, Response, bot, cancellationToken);
    }
}
