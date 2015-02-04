using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Business;
using JJ.Framework.Reflection;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Extensions;

namespace JJ.Business.QuestionAndAnswer.SideEffects
{
    public class Question_SetOpenQuestionDefaults_SideEffect : ISideEffect
    {
        private const QuestionTypeEnum DEFAULT_QUESTION_TYPE_ENUM = QuestionTypeEnum.OpenQuestion;

        private Question _entity;
        private IQuestionTypeRepository _questionTypeRepository;
        private EntityStatusManager _statusManager;

        public Question_SetOpenQuestionDefaults_SideEffect(
            Question entity,
            IQuestionTypeRepository questionTypeRepository,
            EntityStatusManager statusManager)
        {
            if (entity == null) throw new NullException(() => entity);
            if (questionTypeRepository == null) throw new NullException(() => questionTypeRepository);
            if (statusManager == null) throw new NullException(() => statusManager);

            _entity = entity;
            _questionTypeRepository = questionTypeRepository;
            _statusManager = statusManager;
        }

        public void Execute()
        {
            if (_statusManager.IsNew(_entity))
            {
                _entity.IsActive = true;
                _entity.QuestionType = _questionTypeRepository.Get((int)DEFAULT_QUESTION_TYPE_ENUM);
            }
        }
    }
}
