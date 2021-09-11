using System.Collections.Generic;

namespace HamTestWasmHosted.Shared.Dto
{
    public class ExamCheckResultDto
    {
        // question index, right answer index, wrong answer index
        public List<int[]> Data { get; set; }
    }
}