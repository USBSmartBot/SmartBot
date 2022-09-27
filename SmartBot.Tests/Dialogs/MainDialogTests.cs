// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Testing;
using Microsoft.Bot.Builder.Testing.XUnit;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using SmartBot.Dialogs;
using SmartBot.Tests.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SmartBot.Tests.Dialogs
{
    public class MainDialogTests : BotTestBase
    {        
        private readonly Mock<ILogger<MainDialog>> _mockLogger;

        public MainDialogTests(ITestOutputHelper output)
            : base(output)
        {
            _mockLogger = new Mock<ILogger<MainDialog>>();
         }

        /// <summary>
        /// Loads the embedded json resource with the LUIS as a string.
        /// </summary>
        private string GetEmbeddedTestData(string resourceName)
        {
            using (var stream = GetType().Assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
