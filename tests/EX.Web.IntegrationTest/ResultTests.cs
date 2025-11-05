using System.Threading.Tasks;
using EX.Data.Entities;
using EX.Data.Enums;
using EX.Web.Areas.Client.Controllers;
using EX.Web.Jobs;
using EX.Web.Models;
using Xunit;

namespace EX.Web.IntegrationTest
{
    public class ResultTests
    {
        private ExamMode _examMode;
        private int _notAnswered;
        private ExamProgressReport _report;
        private readonly ExamResult _examResult;
        private readonly ExamController _examController;
        private readonly ExamJob _job;

        public ResultTests()
        {
            _examResult = new ExamResult();
            _examController = new ExamController(null, null, null, null, null, null);
            _job  = new ExamJob(null, null, null);
            
        }

        [Theory]
        [InlineData(ExamMode.Standard, 8, 9, 7, null, ResultStatus.Failed)]
        [InlineData(ExamMode.Standard, 8, 8, 8, null, ResultStatus.Pass)]
        [InlineData(ExamMode.Standard, 10, 4, 4, "meessage", ResultStatus.Failed)]
        [InlineData(ExamMode.Standard, 10, 10, 4, "meessage", ResultStatus.Error)]
        [InlineData(ExamMode.Standard, 10, 0, 4, "meessage", ResultStatus.Failed)]
        [InlineData(ExamMode.Standard, 10, 0, 11, "meessage", ResultStatus.Pass)]
        public void Can_Process_Report(ExamMode mode, int testScore, int notAnswered, int correctsCount, string errorMessage, ResultStatus status)
        {
            _examMode = mode;
            _examResult.ErrorMessage = errorMessage;
            _notAnswered = notAnswered;

            _report = new ExamProgressReport()
            {
                CorrectSelectedAnswersCount = correctsCount,
                TestScore = testScore
            };

            _examController.CalculateResults(_examResult, _examMode, _notAnswered, _report);

            Assert.Equal(status, _examResult.Status);
        }


        [Theory]
        [InlineData(ExamMode.Standard, 10, 4, 4, null, ResultStatus.Failed)]
        [InlineData(ExamMode.Standard, 8, 8, 8, null, ResultStatus.Pass)]
        [InlineData(ExamMode.Standard, 8, 8, 8, "meessage", ResultStatus.Pass)]
        [InlineData(ExamMode.Standard, 10, 4, 4, "meessage", ResultStatus.Failed)]
        [InlineData(ExamMode.Standard, 10, 10, 4, "meessage", ResultStatus.Error)]
        [InlineData(ExamMode.Standard, 10, 0, 4, "meessage", ResultStatus.Failed)]
        [InlineData(ExamMode.Standard, 10, 0, 11, "meessage", ResultStatus.Pass)]
        public void Can_Process_Report_Not_Finished(ExamMode mode, int testScore, int notAnswered, int correctsCount, string errorMessage, ResultStatus status)
        {
            _examMode = mode;
            _examResult.ErrorMessage = errorMessage;
            _notAnswered = notAnswered;

            _report = new ExamProgressReport()
            {
                CorrectSelectedAnswersCount = correctsCount,
                TestScore = testScore
            };

            _job.CalculateResults(_examResult, _examMode, _notAnswered, _report);

            Assert.Equal(status, _examResult.Status);
        }

    }
}