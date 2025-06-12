
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
            await turnContext.StreamingResponse.QueueInformativeUpdateAsync("Thinking...", cancellationToken);
            await Task.Yield();
            // Invoke the WeatherForecastAgent to process the message
            var forecastResponse = await _agent.InvokeAgentAsync(turnContext.Activity.Text);

            //if (forecastResponse == null)
            //{
            //    await turnContext.SendActivityAsync(MessageFactory.Text("Sorry, I couldn't get the weather forecast at the moment."), cancellationToken);
            //    return;
            //}
            turnContext.StreamingResponse.QueueTextChunk("HI");
            turnContext.StreamingResponse.QueueTextChunk("HI");
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
            await Task.Yield();
            turnContext.StreamingResponse.FinalMessage = response;
            await turnContext.StreamingResponse.EndStreamAsync(cancellationToken); // End the streaming response
        }

        protected async Task WelcomeMessageAsync(ITurnContext turnContext, ITurnState turnState, CancellationToken cancellationToken)
        {
            foreach (ChannelAccount member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text("Hello and Welcome! I am here to answer questions on the weather!"), cancellationToken);
                }
            }
        }
    }
}
