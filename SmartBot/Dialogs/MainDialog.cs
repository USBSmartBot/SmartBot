// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.16.0

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System;
using System.Text;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.IO;

namespace SmartBot.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly ILogger _logger;

        private AzureContent azureContent;

        // Dependency injection uses this constructor to instantiate MainDialog
        public MainDialog(ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            _logger = logger;

            azureContent = new AzureContent();

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            var waterfallSteps = new WaterfallStep[]
            {
                    ConfirmAzureLevelAsync,
                    AzureLearningOptionAsyn,
                    AzureTopicOptionAsyn,
                    FinalStepAsync
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);            
        }

        #region Waterfall Dailogs

        private async Task<DialogTurnResult> ConfirmAzureLevelAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            azureContent.ExpertLevel = "";
            var azureList = new PromptOptions
            {
                Choices = new List<Choice> { new Choice("Beginner"), new Choice("Intermediate"), new Choice("Expert") },
                Prompt = MessageFactory.Text("What is your expertise level in azure techonology?")
            };
            
            var turnContext = stepContext.Context;
            var activity = turnContext.Activity;
            var welcomeMsg = MessageFactory.Attachment(CreateAdaptiveCardAttachment());

            await turnContext.SendActivityAsync(welcomeMsg);

            return await stepContext.PromptAsync(nameof(ChoicePrompt), azureList, cancellationToken);
        }

        private async Task<DialogTurnResult> AzureLearningOptionAsyn(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (azureContent.ExpertLevel == "")
                azureContent.ExpertLevel = stepContext.Context.Activity.Text;

            var promptMessage = MessageFactory.Text("What you want to learn for azure '" + azureContent.ExpertLevel + "' level?");

            azureContent.DailogId = nameof(WaterfallDialog);

            return await stepContext.BeginDialogAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> AzureTopicOptionAsyn(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            LanguageReponse langRes = null;
            switch (azureContent.ExpertLevel)
            {
                case "Beginner":
                    string langUrl = "https://smartbot-qna.cognitiveservices.azure.com/language/:query-knowledgebases?projectName=smartbot-beginner&api-version=2021-10-01&deploymentName=production";
                    string key = "339ce12228f34b1a840913e430bf8a91";
                    langRes = getLanguageResource(langUrl, key, stepContext.Context.Activity.Text);
                    break;
                case "Intermediate":

                    string langUrl1 = "https://smartbot-qna.cognitiveservices.azure.com/language/:query-knowledgebases?projectName=smartbot-Intermediate&api-version=2021-10-01&deploymentName=test";
                    string key1 = "339ce12228f34b1a840913e430bf8a91";
                    langRes = getLanguageResource(langUrl1, key1, stepContext.Context.Activity.Text);
                    break;
                case "Expert":
                    string langUrl2 = "https://smartbot-qna.cognitiveservices.azure.com/language/:query-knowledgebases?projectName=smartbot-expert&api-version=2021-10-01&deploymentName=test";
                    string key2 = "339ce12228f34b1a840913e430bf8a91";
                    langRes = getLanguageResource(langUrl2, key2, stepContext.Context.Activity.Text);
                    break;
            }
                        
            var langMessage = MessageFactory.Text(langRes.answers[0].answer);
            var langMessage1 = MessageFactory.Text(langRes.answers[0].source);

            var turnContext = stepContext.Context;
            var activity = turnContext.Activity;

            await turnContext.SendActivityAsync(langMessage);
            await turnContext.SendActivityAsync(langMessage1);

            var confirmOption = new PromptOptions
            {
                Choices = new List<Choice> { new Choice("Continue"), new Choice("Main Menu"), new Choice("Exit") }
            };

            return await stepContext.BeginDialogAsync(nameof(ChoicePrompt), confirmOption, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //Restart the main dialog
            var confirmOption = stepContext.Context.Activity.Text;
                      
            if (confirmOption == "Main Menu")
            {
                var promptMessage = "Thank you for choosing SmartBot to learn azure!!";

                return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
            }
            else if (confirmOption == "Exit")
            {
                var message = MessageFactory.Text("");
                var turnContext = stepContext.Context;
                var activity = turnContext.Activity;
                var reply = activity.CreateReply();
                reply.Text = "Thank You for choosing SmartBot to learn azure!!";

                var imageJsonString = "{\"$schema\": \"http://adaptivecards.io/schemas/adaptive-card.json\",\"type\": \"AdaptiveCard\",\"version\": \"1.0\", \"body\":[{\"type\": \"Image\",\"width\": \"82px\",\"height\": \"100px\",\"url\": \"https://i.pinimg.com/736x/bf/31/a3/bf31a3f95b984510958a887f7e513020.jpg\",\"size\": \"stretch\"}]}";

                var urlAttachment = new Attachment
                {
                    ContentType = "application/vnd.microsoft.card.adaptive",
                    Content = JsonConvert.DeserializeObject(imageJsonString)
                };

                reply.Attachments = new List<Attachment>() { urlAttachment };

                await turnContext.SendActivityAsync(reply);

                return await stepContext.EndDialogAsync(message, cancellationToken);
            }
            else
            {
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"] - 2;

                return await AzureLearningOptionAsyn(stepContext, cancellationToken);
            }
        }

        #endregion

        #region Language Resource Service

        private LanguageReponse getLanguageResource(string url, string key, string question)
        {
            LanguageReponse languageReponse = null;
            try
            {
                LanguageRequest languageRequest = new LanguageRequest()
                {
                    top = 2,
                    answerSpanRequest = new answerSpanRequest
                    {
                        confidenceScoreThreshold = "0",
                        enable = true,
                        topAnswersWithSpan = 1
                    },
                    confidenceScoreThreshold = "0",
                    filters = "",
                    includeUnstructuredSources = true,
                    question = question
                };

                string resultContent = string.Empty;

                var requestHandler = new HttpClientHandler();
                HttpClient httpClient = new HttpClient(requestHandler);
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("Ocp-Apim-Subscription-Key", key);

                var jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ContractResolver = new CamelCasePropertyNamesContractResolver() };
                request.Content = new StringContent(JsonConvert.SerializeObject(languageRequest, jsonSettings), Encoding.UTF8, "application/json");

                Task<String> res = PostMethod(request);

                languageReponse = JsonConvert.DeserializeObject<LanguageReponse>(res.Result);
            }
            catch (Exception ex)
            {
                throw;
            }
            return languageReponse;
        }
        private static async Task<String> PostMethod(HttpRequestMessage request)
        {
            var requestHandler = new HttpClientHandler();
            HttpClient httpClient = new HttpClient(requestHandler);

            var result = httpClient.Send(request);

            var resultContent = await result.Content.ReadAsStringAsync().ConfigureAwait(true);

            return resultContent;
        }

        #endregion

        #region Private Methods
        private Attachment CreateAdaptiveCardAttachment()
        {
            var cardResourcePath = GetType().Assembly.GetManifestResourceNames().First(name => name.EndsWith("welcomeCard.json"));

            using (var stream = GetType().Assembly.GetManifestResourceStream(cardResourcePath))
            {
                using (var reader = new StreamReader(stream))
                {
                    var adaptiveCard = reader.ReadToEnd();
                    return new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(adaptiveCard, new JsonSerializerSettings { MaxDepth = null }),
                    };
                }
            }
        }

        #endregion
    }
}
