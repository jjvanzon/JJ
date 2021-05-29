using System.Collections.Generic;

namespace JJ.Data.QuestionAndAnswer
{
    public class User
    {
        public virtual int ID { get; set; }

        public virtual string DisplayName { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string SecuritySalt { get; set; }

        public virtual IList<Run> Runs { get; set; } = new List<Run>();
        public virtual IList<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
        public virtual IList<QuestionFlag> AsLastModifiedByInQuestionFlags { get; set; } = new List<QuestionFlag>();
        public virtual IList<QuestionFlag> AsFlaggedByInQuestionFlags { get; set; } = new List<QuestionFlag>();
        public virtual IList<Question> AsLastModifiedByInQuestions { get; set; } = new List<Question>();
    }
}