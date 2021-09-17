using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using HamTestWasmHosted.Server.Domain;
using HamTestWasmHosted.Shared.Form;
using Microsoft.AspNetCore.Hosting;

namespace HamTestWasmHosted.Server.Services
{
    public sealed class ExamService
    {
        public class TopicJsonDto
        {
            [JsonPropertyName("name")] public string Name { get; set; }

            [JsonPropertyName("questions")] public QuestionJsonDto[] Questions { get; set; }
        }

        public class QuestionJsonDto
        {
            [JsonPropertyName("cat")] public int[] Cat { get; set; }

            [JsonPropertyName("q")] public string Q { get; set; }

            [JsonPropertyName("a")] public string[] A { get; set; }

            public int RightAnswerIndex => A.Select((s, i) => new {s, i}).Single(x => x.s.First() == '@').i;
        }

        private readonly List<Question> _questions = new();

        public int GetTotalCount(int category) => category switch
        {
            1 => 45,
            2 => 30,
            3 => 25,
            4 => 20,
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };

        public int GetEnoughCount(int category) => category switch
        {
            1 => 40,
            2 => 25,
            3 => 20,
            4 => 15,
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };

        public ExamService(IWebHostEnvironment webHostEnvironment)
        {
            var http = new HttpClient();
            var _topicDtos = JsonSerializer.Deserialize<TopicJsonDto[]>(File.ReadAllText("questions.json"));

            int num = 1;
            foreach (var topicDto in _topicDtos)
            {
                var t = new Topic(topicDto.Name);
                foreach (var questionDto in topicDto.Questions)
                {
                    var info = webHostEnvironment.WebRootFileProvider.GetFileInfo($"i/{num}.png");
                    var question = new Question(t, num, questionDto.Q, questionDto.RightAnswerIndex, questionDto.Cat,
                        info.Exists, questionDto.A);
                    
                    _questions.Add(question);
                    num++;
                }
            }
        }

        public Exam GetExam(Random random, int cat)
        {
            var randomQuestions = new List<Question>(); 

            foreach (var g in _questions.Where(q=>q.Categories.Contains(cat)).GroupBy(q=>q.Topic))
            {
                randomQuestions.Add(g.OrderBy(_ => random.Next()).First());
            }

            var rest = _questions
                .Where(q=>q.Categories.Contains(cat))
                .Except(randomQuestions)
                .OrderBy(_ => random.Next())
                .Take(GetTotalCount(cat) - randomQuestions.Count);

            randomQuestions = randomQuestions.Union(rest).ToList();
         
            return new Exam()
            {
                Category = cat,
                EnoughCount = GetEnoughCount(cat),
                TotalCount = GetTotalCount(cat),
                Questions = randomQuestions
            };
        }
        public ExamCheckResult Check(Exam exam, Random random, ExamResultRequest request)
        {
            var wrongAnswers = new List<WrongAnswer>();

            var grouped = exam.Questions.GroupBy(q => q.Topic).ToList();
            int i = 0;
            foreach (var g in grouped)
            {
                foreach (var question in g)
                {
                    var (_, rightAnswerNewIndex) = question.GetShuffledAnswers(random);
                 
                    if (rightAnswerNewIndex != request.AnswerIndices[i])
                    {
                        wrongAnswers.Add(new WrongAnswer()
                        {
                            QuestionIndex = i,
                            RightAnswerIndex = rightAnswerNewIndex,
                            WrongAnswerIndex = (int)request.AnswerIndices[i]
                        });
                    }

                    i++;
                }
            }

            return new ExamCheckResult()
            {
                WrongAnswers = wrongAnswers
            };
        }
    }
}