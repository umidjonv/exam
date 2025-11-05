using System;
using EX.Data.Entities;
using EX.Data.Enums;
using EX.Web.Areas.Client.Controllers;
using EX.Web.Models;
using NUnit.Framework;

namespace EX.Web.Test
{
    public class ExamResultTest
    {
        private ExamMode _examMode;
        private int _notAnswered;
        private ExamProgressReport _report;
        private ExamResult _examResult;
        private ExamController _examController;

        [SetUp]
        public void Setup()
        {
            _examResult = new ExamResult();
            _examController = new ExamController(null, null, null, null, null, null);
        }

        [TestCase(ExamMode.Standard, 10, 4, 4, null, ResultStatus.Failed)]
        [TestCase(ExamMode.Standard, 10, 4, 12, null,ResultStatus.Pass)]
        [TestCase(ExamMode.Standard, 10, 4, 4,"meessage", ResultStatus.Failed)]
        [TestCase(ExamMode.Standard, 10, 10, 4, "meessage", ResultStatus.Error)]
        [TestCase(ExamMode.Standard, 10, 0, 4, "meessage", ResultStatus.Failed)]
        [TestCase(ExamMode.Standard, 10, 0, 11, "meessage", ResultStatus.Pass)]
        [Test]
        public void Test(ExamMode mode, int testScore, int notAnswered, int correctsCount,string errorMessage, ResultStatus status)
        {
            _examMode = mode;
            _examResult.ErrorMessage = errorMessage;
            _notAnswered = notAnswered;

            _report = new ExamProgressReport()
            {
                CorrectSelectedAnswersCount = correctsCount,
                TestScore = testScore,

            };

            TestRun(status);;

        }

        private void TestRun(ResultStatus status)
        {
            _examController.CalculateResults(_examResult, _examMode, _notAnswered, _report);

            Assert.AreEqual(status, _examResult.Status);
        }

    }
}