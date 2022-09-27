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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartBot.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly ILogger _logger;
        
        // Dependency injection uses this constructor to instantiate MainDialog
        public MainDialog(ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            _logger = logger;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            
            var waterfallSteps = new WaterfallStep[]
            {
                    ConfirmAzureLevelAsync,
                    ConfirmLearningOptionsAsync,
                    AzureLearningOptionAsyn,
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
                Prompt=MessageFactory.Text("What is your expertize level in azure techonology")
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt),azureList, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmLearningOptionsAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var azureLearnList = new PromptOptions
            {
                Choices = new List<Choice> { new Choice("Read Up"), new Choice("Certification"), new Choice("Practice Test") },
                Prompt = MessageFactory.Text("What do like to do?")
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), azureLearnList, cancellationToken);
        }

        private async Task<DialogTurnResult> AzureLearningOptionAsyn(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {  
            SmartBot_Content.ModelInput content = new SmartBot_Content.ModelInput()
            {
                Col0 = stepContext.Context.Activity.Text
            };

            var predict = SmartBot_Content.Predict(content);

            var promptMessage = MessageFactory.Text(predict.PredictedLabel);

            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //Restart the main dialog
            var promptMessage = "what other information required on Azure?";
            return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage,cancellationToken);
        }

    }
}
