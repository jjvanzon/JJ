using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Business.QuestionAndAnswer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.QuestionAndAnswer
{
    public class QuestionFlagger
    {
        private User _user;

        private IQuestionFlagRepository _questionFlagRepository;
        private IFlagStatusRepository _flagStatusRepository;

        public QuestionFlagger(User user, IQuestionFlagRepository questionFlagRepository, IFlagStatusRepository flagStatusRepository)
        {
            if (questionFlagRepository == null) throw new NullException(() => questionFlagRepository);
            if (flagStatusRepository == null) throw new NullException(() => flagStatusRepository);
            if (user == null) throw new NullException(() => user);

            _questionFlagRepository = questionFlagRepository;
            _flagStatusRepository = flagStatusRepository;
            _user = user;
        }

        /// <summary> Flags a question. If an existing flag already exists for this user, then it is updated. </summary>
        public QuestionFlag FlagQuestion(Question question, string comment)
        {
            if (question == null) throw new NullException(() => question);

            QuestionFlag questionFlag = TryGetFlag(question);
            if (questionFlag == null)
            {
                questionFlag = _questionFlagRepository.Create();
                questionFlag.LinkTo(question);
                questionFlag.LinkToFlaggedByUser(_user);
            }

            questionFlag.LinkToLastModifiedByUser(_user);
            questionFlag.DateTime = DateTime.Now;
            questionFlag.Comment = comment;
            questionFlag.SetFlagStatusEnum(FlagStatusEnum.Flagged, _flagStatusRepository);

            return questionFlag;
        }

        /// <summary> Removes a possibly existing question flag, that the user might have placed. </summary>
        public void UnflagQuestion(Question question)
        {
            QuestionFlag questionFlag = TryGetFlag(question);

            if (questionFlag != null)
            {
                questionFlag.UnlinkRelatedEntities();

                _questionFlagRepository.Delete(questionFlag);
            }
        }

        public QuestionFlag TryGetFlag(Question question)
        {
            return _questionFlagRepository.TryGetByCriteria(question.ID, _user.ID);
        }
    }
}
