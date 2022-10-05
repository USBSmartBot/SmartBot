using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBot
{
    public class LanguageRequest
    {
        public Int16 top { get; set; }
        public string question { get; set; }
        public Boolean includeUnstructuredSources { get; set; }
        public string confidenceScoreThreshold { get; set; }
        public answerSpanRequest answerSpanRequest { get; set; }
        public string filters { get; set; }
    }

    public class answerSpanRequest
    {
        public Boolean enable { get; set; }
        public Int16 topAnswersWithSpan { get; set; }
        public string confidenceScoreThreshold { get; set; }
    }
}