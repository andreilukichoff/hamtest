using System;
using System.Collections.Generic;

namespace HamTestWasmHosted.Server.Domain
{
    public class Exam
    {
        public int Category { get; init; }
        public int TotalCount { get; init; }
        public int EnoughCount { get; init; }
        public IReadOnlyList<Question> Questions { get; init; }

       
    }
}