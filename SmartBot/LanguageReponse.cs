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
        public dialog dialog { get; set; }
    }

    public class dialog
    {
        public Boolean isContextOnly { get; set; }
        public List<prompts> prompts { get; set; }
    }

    public class prompts
    {
        public string displayOrder { get; set; }
        public string qnaId { get; set; }
        public string displayText { get; set; }
    }
}