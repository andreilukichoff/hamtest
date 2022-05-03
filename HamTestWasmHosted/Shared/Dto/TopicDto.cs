using System.Collections.Generic;

namespace HamTestWasmHosted.Shared.Dto
{
    public class TopicDto
    {
        public string Name { get; set; }
        
        public List<QuestionDto> Questions { get; set; }
    }
}