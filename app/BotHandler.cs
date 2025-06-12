
using AdaptiveCards;
using Microsoft.Agents.Builder;
using Microsoft.Agents.Builder.App;
using Microsoft.Agents.Builder.State;
using Microsoft.Agents.Core.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using microsoft_agent_sk.Agents;
using microsoft_agent_sk.Helpers;
using System.Text;

namespace microsoft_agent_sk
{
    public class BotHandler : AgentApplication
    {
        private readonly WeatherAgent _agent;

 
        public BotHandler(AgentApplicationOptions options, WeatherAgent agent) : base(options)
        {
            _agent = agent ?? throw new ArgumentNullException(nameof(agent));

            OnConversationUpdate(ConversationUpdateEvents.MembersAdded, WelcomeMessageAsync);
            OnActivity(ActivityTypes.Message, MessageActivityAsync, rank: RouteRank.Last);
        }

        protected async Task MessageActivityAsync(ITurnContext turnContext, ITurnState turnState, CancellationToken cancellationToken)
        {
            // Invoke the WeatherForecastAgent to process the message
            var forecastResponse = await _agent.InvokeAgentAsync(turnContext.Activity.Text);
            if (forecastResponse == null)
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("Sorry, I couldn't get the weather forecast at the moment."), cancellationToken);
                return;
            }

            // Create a response message based on the response content type from the WeatherForecastAgent
            IActivity response = forecastResponse.ContentType switch
            {
                FlightResponseContentType.AdaptiveCard => MessageFactory.Attachment(new Attachment()
                {
                    ContentType = "application/vnd.microsoft.card.adaptive",
                    Content = forecastResponse.Content,
                }),
                _ => MessageFactory.Text(forecastResponse.Content),
            };

            // Send the response message back to the user. 
            await turnContext.SendActivityAsync(response, cancellationToken);
        }

        protected async Task WelcomeMessageAsync(ITurnContext turnContext, ITurnState turnState, CancellationToken cancellationToken)
        {
            foreach (ChannelAccount member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text("Hello and Welcome! I'm here to help with all your SAP needs!"), cancellationToken);
                }
            }
        }
    }
}
