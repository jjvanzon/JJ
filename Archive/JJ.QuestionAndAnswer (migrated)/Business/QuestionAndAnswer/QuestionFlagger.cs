using System;
using JJ.Business.QuestionAndAnswer.Cascading;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Exceptions.Basic;

namespace JJ.Business.QuestionAndAnswer
{
    public class QuestionFlagger
    {
        private readonly User _user;

        private readonly IQuestionFlagRepository _questionFlagRepository;
        private readonly IFlagStatusRepository _flagStatusRepository;

        public QuestionFlagger(User user, IQuestionFlagRepository questionFlagRepository, IFlagStatusRepository flagStatusRepository)
        {
            _questionFlagRepository = questionFlagRepository ?? throw new NullException(() => questionFlagRepository);
            _flagStatusRepository = flagStatusRepository ?? throw new NullException(() => flagStatusRepository);
            _user = user ?? throw new NullException(() => user);
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

        public QuestionFlag TryGetFlag(Question question) => _questionFlagRepository.TryGetByCriteria(question.ID, _user.ID);
    }
}
