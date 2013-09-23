//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JJ.Models.QuestionAndAnswer
//{
//    public class MultipleChoiceQuestion : IQuestion
//    {
//        public virtual object ID { get; set; }

//        public virtual string Text { get; set; }
//        public virtual IList<MultipleChoiceAnswer> PossibleMultipleChoiceAnswers { get; set; }
//        public virtual MultipleChoiceAnswer MultipleChoiceAnswer { get; set; }

//        string IQuestion.Text
//        {
//            get { return Text; }
//            set { Text = value; }
//        }

//        IAnswer IQuestion.Answer
//        {
//            get { return MultipleChoiceAnswer; }
//            set { MultipleChoiceAnswer = (MultipleChoiceAnswer)value; }
//        }
//    }
//}
