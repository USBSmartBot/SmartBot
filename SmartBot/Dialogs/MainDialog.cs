// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.16.0

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
                    ConfirmLearningOptionsAsync,
                    AzureLearningOptionAsyn,
                    AzureTopicOptionAsyn,
                    FinalStepAsync
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> ConfirmAzureLevelAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var azureList = new PromptOptions
            {
                Choices=new List<Choice> { new Choice("Beginner"), new Choice("Intermediate") , new Choice("Expert") },
                Prompt=MessageFactory.Text("What is your expertize level in azure techonology?")
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt),azureList, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmLearningOptionsAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            azureContent.ExpertLevel = stepContext.Context.Activity.Text;
            var azureLearnList = new PromptOptions
            {
                Choices = new List<Choice> { new Choice("Read Up"), new Choice("Certification"), new Choice("Practice Test") },
                Prompt = MessageFactory.Text("Which option would you like to go with?")
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), azureLearnList, cancellationToken);
        }

        private async Task<DialogTurnResult> AzureLearningOptionAsyn(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            azureContent.AzureChoice = stepContext.Context.Activity.Text;
                      
            var promptMessage= MessageFactory.Text(AzureQuestionText(azureContent.ExpertLevel));

           //return await stepContext.BeginDialogAsync(promptMessage, cancellationToken);

            return await stepContext.BeginDialogAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private string AzureQuestionText(string ExpertLevel)
        {
            string questionText = "";
            switch (azureContent.AzureChoice)
            {
                case "Read Up":
                    questionText = "What you want to read for azure " + ExpertLevel + " level?";
                    break;
                case "Certification":
                    questionText = "Which certification you would like to do for azure " + ExpertLevel + " level?";
                    break;
                case "Practice Test":
                    questionText = "Which azure certification Practivie test would you like to do azure" + ExpertLevel + " level?";
                    break;
            }
            return questionText;
        }

        private async Task<DialogTurnResult> AzureTopicOptionAsyn(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            var turnContext = stepContext.Context;
            var activity = turnContext.Activity;
            var reply = activity.CreateReply();
            reply.Text = "sample images for azure certification.";

            //read from local
            var att = new Attachment
            {
                Name = @"Resources\architecture-resize.png",
                ContentType = "image/png",
                ContentUrl = @"C:\Users\kiran\Downloads\azure1.jfif",
            };
                                  
            //an Internet url attachment
            var urlAttachment = new Attachment
            {
                Name = @"Resources\architecture-resize.png",
                ContentType = "image/png",
                ContentUrl = "https://docs.microsoft.com/en-us/bot-framework/media/how-it-works/architecture-resize.png",
            };
            reply.Attachments = new List<Attachment>() { urlAttachment };

            string predictdetails = "";
            switch (azureContent.ExpertLevel)
            {
                case "Beginner":
                    if(azureContent.AzureChoice=="Read Up")
                    {
                        Beginner_read.ModelInput read_content = new Beginner_read.ModelInput()
                        {
                            Col0 = stepContext.Context.Activity.Text
                        };

                        var predictRead = Beginner_read.Predict(read_content);
                        predictdetails = predictRead.PredictedLabel;
                    }
                    else if(azureContent.AzureChoice == "Practice Test")
                    {
                        Beginner_test.ModelInput test_content = new Beginner_test.ModelInput()
                        {
                            Col0 = stepContext.Context.Activity.Text
                        };

                        var predictTest = Beginner_test.Predict(test_content);
                        predictdetails = predictTest.PredictedLabel;
                    }
                    else if(azureContent.AzureChoice == "Certification")
                    {
                        Beginner_cert.ModelInput cert_content = new Beginner_cert.ModelInput()
                        {
                            Col0 = stepContext.Context.Activity.Text
                        };

                        var predictCert = Beginner_cert.Predict(cert_content);
                        predictdetails = predictCert.PredictedLabel;
                    }
                    break;
                case "Intermediate":              
                    if (azureContent.AzureChoice == "Read Up")
                    {
                        Intermediate_read.ModelInput read_content = new Intermediate_read.ModelInput()
                        {
                            Col0 = stepContext.Context.Activity.Text
                        };

                        var predictRead = Intermediate_read.Predict(read_content);
                        predictdetails = predictRead.PredictedLabel;
                    }
                    else if (azureContent.AzureChoice == "Practice Test")
                    {
                        Intermediate_test.ModelInput test_content = new Intermediate_test.ModelInput()
                        {
                            Col0 = stepContext.Context.Activity.Text
                        };

                        var predictTest = Intermediate_test.Predict(test_content);
                        predictdetails = predictTest.PredictedLabel;
                    }
                    else if (azureContent.AzureChoice == "Certification")
                    {
                        Intermediate_cert.ModelInput cert_content = new Intermediate_cert.ModelInput()
                        {
                            Col0 = stepContext.Context.Activity.Text
                        };

                        var predictCert = Intermediate_cert.Predict(cert_content);
                        predictdetails = predictCert.PredictedLabel;
                    }
                    break;                    
                case "Expert":                
                    if (azureContent.AzureChoice == "Read Up")
                    {
                        Expert_read.ModelInput read_content = new Expert_read.ModelInput()
                        {
                            Col0 = stepContext.Context.Activity.Text
                        };

                        var predictRead = Expert_read.Predict(read_content);
                        predictdetails = predictRead.PredictedLabel;
                    }
                    else if (azureContent.AzureChoice == "Practice Test")
                    {
                        Expert_test.ModelInput test_content = new Expert_test.ModelInput()
                        {
                            Col0 = stepContext.Context.Activity.Text
                        };

                        var predictTest = Expert_test.Predict(test_content);
                        predictdetails = predictTest.PredictedLabel;
                    }
                    else if (azureContent.AzureChoice == "Certification")
                    {
                        Expert_cert.ModelInput cert_content = new Expert_cert.ModelInput()
                        {
                            Col0 = stepContext.Context.Activity.Text
                        };

                        var predictCert = Expert_cert.Predict(cert_content);
                        predictdetails = predictCert.PredictedLabel;
                    }
                    break;                    
            }
                        
            var message = MessageFactory.Text(predictdetails);

            await turnContext.SendActivityAsync(message);
            await turnContext.SendActivityAsync(reply);
                        
            var confirmOption = new PromptOptions
            {
                Choices = new List<Choice> { new Choice("Yes"), new Choice("No") },
                Prompt = MessageFactory.Text("Do you want to continue to learn more?")
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), confirmOption, cancellationToken);            
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //Restart the main dialog
            var confirmOption = stepContext.Context.Activity.Text;

            var promptMessage = "Thank you for choosing SmartBot to learn azure!!";

            if (confirmOption == "Yes")
                return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
            else
            {                
                var message = MessageFactory.Text(promptMessage);
                var turnContext = stepContext.Context;
                var activity = turnContext.Activity;
                /*var reply = activity.CreateReply();
                reply.Text = "Thank You for choosing SmartBot to learn azure!!";

                //read from local
                var att = new Attachment
                {
                    Name = @"Resources\architecture-resize.png",
                    ContentType = "image/png",
                    ContentUrl = @"C:\Users\kiran\Downloads\azure1.jfif",
                };

                //an Internet url attachment
                var urlAttachment = new Attachment
                {
                    Name = @"Resources\architecture-resize.png",
                    ContentType = "image/png",
                    ContentUrl = "https://i.pinimg.com/736x/bf/31/a3/bf31a3f95b984510958a887f7e513020.jpg"                   
                };
                reply.Attachments = new List<Attachment>() { urlAttachment };*/

                await turnContext.SendActivityAsync(promptMessage);
                return await stepContext.EndDialogAsync(promptMessage, cancellationToken);
            }            
        }

    }
}
