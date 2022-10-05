using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBot
{
    public class LanguageReponse
    {
        public List<answers> answers { get; set; }
    }

    public class answers
    {
        public List<string> questions { get; set; }
        public string answer { get; set; }
        public string confidenceScore { get; set; }
        public string id { get; set; }
        public string source { get; set; }
    }

}