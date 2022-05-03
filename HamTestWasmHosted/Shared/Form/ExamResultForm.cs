using System.Text.Json.Serialization;

namespace HamTestWasmHosted.Shared.Form
{
    public class ExamResultRequest
    {
        public string Token { get; set; }
        public int?[] AnswerIndices { get; set; }
    }
    
    public class AnswerIndex
    {
        public int? Value { get; set; }
    }
    
    public class ExamResultForm
    {
        public AnswerIndex[] AnswerIndices { get; set; }

        public ExamResultForm()
        {
        }

        public ExamResultForm(int questionCount)
        {
            AnswerIndices = new AnswerIndex[questionCount];
            for (var index = 0; index < AnswerIndices.Length; index++)
            {
                AnswerIndices[index] = new AnswerIndex();
            }
        }
    }
}