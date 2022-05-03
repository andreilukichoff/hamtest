namespace HamTestWasmHosted.Shared.Dto
{
    public class WrongAnswerDto
    {
        // question index, right answer index, wrong answer index
        public int[,] Data { get; set; }
    }
}