using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using HamTestWasmHosted.Server.Domain;
using HamTestWasmHosted.Server.Services;
using HamTestWasmHosted.Shared.Dto;
using HamTestWasmHosted.Shared.Form;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HamTestWasmHosted.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly CipherService _cipherService;
        private readonly ExamService _examService;

        public TestController(ILogger<TestController> logger, CipherService cipherService)
        {
            _logger = logger;
            _cipherService = cipherService;
            _examService = ExamService.Instance;
        }

        [HttpPost("{category}/new")]
        public ActionResult<ExamDto> Get(int category)
        {
            var seed = new Random().Next();
            var random = new Random(seed);

            var exam = _examService.GetExam(random, category);

            var topicList = new List<TopicDto>();
            var grouped = exam.Questions.GroupBy(q => q.Topic).ToList();
            foreach (var g in grouped)
            {
                var t = new TopicDto()
                {
                    Name = g.Key.Name,
                    Questions = new List<QuestionDto>()
                };

                foreach (var question in g)
                {
                    var (answers, rightAnswerNewIndex) = question.GetShuffledAnswers(random);

                    t.Questions.Add(new QuestionDto()
                    {
                        Text = question.Text,
                        Answers = answers,
                    });
                }

                topicList.Add(t);
            }

            var token = _cipherService.Encrypt(JsonSerializer.Serialize(new Token()
            {
                ExpiresAt = DateTime.UtcNow.AddHours(category == 1 ? 1.5 : 1),
                Seed = seed
            }));
            
            var examDto = new ExamDto()
            {
                Category = category,
                EnoughCount = exam.EnoughCount,
                TotalCount = exam.TotalCount,
                Topics = topicList,
                Token = token
            };

            return examDto;
        }

        [HttpPost("{category}/check")]
        public ActionResult<ExamCheckResultDto> Check(int category, [FromBody] ExamResultRequest request)
        {
            var token = JsonSerializer.Deserialize<Token>(_cipherService.Decrypt(request.Token));

            var random = new Random(token.Seed);

            var exam = _examService.GetExam(random, category);

            if (request.AnswerIndices.Length != exam.TotalCount)
                return BadRequest(
                    $"Invalid answers count: {request.AnswerIndices.Length}, expected: {exam.TotalCount}");

            if (request.AnswerIndices.Any(x => x == null))
                return BadRequest(
                    $"Invalid answers count: {request.AnswerIndices.Count(x => x != null)}, expected: {exam.TotalCount}");

            if (category < 1 || category > 4)
                return BadRequest($"Invalid category, expected: 1..4");

            var examCheckResult = _examService.Check(exam, random, request);

            var resultDto = new ExamCheckResultDto()
            {
                Data = new List<int[]>()
            };

            foreach (var x in examCheckResult.WrongAnswers)
            {
                resultDto.Data.Add(new[] {x.QuestionIndex, x.RightAnswerIndex, x.WrongAnswerIndex});
            }

            return resultDto;
        }
    }
}