using System.Collections.Generic;
using EX.Data.Enums;

namespace EX.Web.Models
{
    public class ExamProgressReport
    {
        public int[] QuestionIds { get; set; } = new int[0];

        public int ScheduleId { get; set; }

        public string CultureId { get; set; }

        public Dictionary<int, QuestionAnswerViewModel> Answered { get; set; } = new Dictionary<int, QuestionAnswerViewModel>();

        public ResultStatus Status { get; set; } = ResultStatus.NotStarted;

        public int TestScore { get; set; }

        public int CorrectSelectedAnswersCount { get; set; }

        public int IncorrectSelectedAnswersCount { get; set; }

        public int CurrentPosition { get; set; }

        public string ResultMessage { get; set; }

        public int? ResultId { get; set; }
    }
}