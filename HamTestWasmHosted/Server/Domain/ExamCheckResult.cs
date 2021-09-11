using System.Collections.Generic;

namespace HamTestWasmHosted.Server.Domain
{
    public class ExamCheckResult
    {
        public IReadOnlyList<WrongAnswer> WrongAnswers { get; init; }
    }
}