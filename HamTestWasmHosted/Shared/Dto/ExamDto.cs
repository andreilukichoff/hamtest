using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace HamTestWasmHosted.Shared.Dto
{
    public class ExamDto
    {
        public int Category { get; set; }
        
        public int TotalCount { get; set; }

        public int EnoughCount { get; set; }

        [JsonIgnore]
        public int MaxWrongAnswers => TotalCount - EnoughCount;

        public IEnumerable<TopicDto> Topics { get; set; }
        
        public string Token { get; set; }

        [JsonIgnore]
        public IEnumerable<QuestionDto> AllQuestions => Topics.SelectMany(t => t.Questions);
    }
}