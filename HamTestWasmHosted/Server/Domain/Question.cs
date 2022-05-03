using System;
using System.Linq;
using HamTestWasmHosted.Server.Extensions;

namespace HamTestWasmHosted.Server.Domain
{
    public class Question
    {
        public int Num { get; }
        public string Text { get; }
        public string[] Answers { get; }
        public int RightAnswerIndex { get; }
        public int[] Categories { get; }

        public Topic Topic { get; }
        
        public bool HasImage { get; }

        public Question(Topic topic, int num, string text, int rightAnswerIndex, int[] cat, bool hasImage, params string[] answers)
        {
            if (cat == null)
                throw new ArgumentNullException(nameof(cat));

            if (cat.Length < 1)
                throw new ArgumentException();

            if (answers == null)
                throw new ArgumentNullException(nameof(answers));

            if (answers.Length < 2)
                throw new ArgumentException();

            if (rightAnswerIndex >= answers.Length ||
                rightAnswerIndex < 0)
                throw new IndexOutOfRangeException();

            Topic = topic;
            Num = num;
            Text = text ?? throw new ArgumentNullException(nameof(text));
            RightAnswerIndex = rightAnswerIndex;
            Answers = answers.Select(a => a.Trim('@')).ToArray();
            Categories = cat;
            HasImage = hasImage;
        }

        public (string[] answers, int rightAnswerNewIndex) GetShuffledAnswers(Random random)
        {
            int[] indices = Enumerable.Range(0, Answers.Length).ToArray();
            indices.Shuffle(random);

            var shuffledAnswers = (string[]) Answers.Clone();
            Array.Sort(indices.ToArray(), shuffledAnswers);

            return (shuffledAnswers, indices[RightAnswerIndex]);
        }
    }
}