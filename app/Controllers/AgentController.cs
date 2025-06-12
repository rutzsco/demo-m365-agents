using Microsoft.AspNetCore.Mvc;
using microsoft_agent_sk.Agents;

namespace microsoft_agent_sk.Controllers
{
    [Route("api/agent")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly WeatherAgent _weatherAgent;

        public AgentController(WeatherAgent weatherAgent)
        {
            _weatherAgent = weatherAgent;
        }


        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ChatRequest request,CancellationToken cancellationToken)
        {
            var result = await _weatherAgent.InvokeAgentAsync(request.GetLastMessage().Content);     
            return Ok(result);
        }
    }
}
