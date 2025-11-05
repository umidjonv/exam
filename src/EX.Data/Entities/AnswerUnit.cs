using EX.Common.Core;

namespace EX.Data.Entities
{
    public class AnswerUnit : BaseEntity
    {

        public int ResultId { get; set; }
        
        public virtual ExamResult Result { get; set; }

        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
         
        public int? AnswerId { get; set; }

        public virtual QuestionInAnswer Answer { get; set; }

    }
}
