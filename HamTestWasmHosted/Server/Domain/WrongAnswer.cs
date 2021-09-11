namespace HamTestWasmHosted.Server.Domain
{
    public class WrongAnswer
    {
        public int QuestionIndex { get; init; }
        
        public int RightAnswerIndex { get; init; }
        
        public int WrongAnswerIndex { get; init; }
    }
}