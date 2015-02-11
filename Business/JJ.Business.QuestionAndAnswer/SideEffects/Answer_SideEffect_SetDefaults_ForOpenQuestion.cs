using JJ.Framework.Business;
using JJ.Framework.Reflection;
using JJ.Persistence.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.SideEffects
{
    public class Answer_SideEffect_SetDefaults_ForOpenQuestion : ISideEffect
    {
        private Answer _entity;
        private EntityStatusManager _statusManager;

        public Answer_SideEffect_SetDefaults_ForOpenQuestion(Answer entity, EntityStatusManager statusManager)
        {
            if (entity == null) throw new NullException(() => entity);
            if (statusManager == null) throw new NullException(() => statusManager);
            _entity = entity;
            _statusManager = statusManager;
        }

        public void Execute()
        {
            if (_statusManager.IsNew(_entity))
            {
                _entity.IsCorrectAnswer = true;
            }
        }
    }
}
