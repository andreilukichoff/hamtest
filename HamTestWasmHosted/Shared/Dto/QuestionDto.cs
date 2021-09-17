namespace HamTestWasmHosted.Shared.Dto
{
    public class QuestionDto
    {
        public string Text { get; init; }

        public string[] Answers { get; init; }

        public bool? HasImage { get; init; }
        public int Num { get; init; }
    }
}