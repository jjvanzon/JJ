using JJ.Framework.Business;
using JJ.Framework.Reflection;
using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.SideEffects
{
    public class Question_SideEffect_SetIsManual : ISideEffect
    {
        private Question _entity;
        private EntityStatusManager _statusManager;

        public Question_SideEffect_SetIsManual(Question entity, EntityStatusManager statusManager)
        {
            if (entity == null) throw new NullException(() => entity);
            if (statusManager == null) throw new NullException(() => statusManager);

            _entity = entity;
            _statusManager = statusManager;
        }

        public void Execute()
        {
            if (MustSetIsManual(_entity, _statusManager))
            {
                _entity.IsManual = true;
            }
        }

        private bool MustSetIsManual(Question entity, EntityStatusManager statusManager)
        {
            // MustSetIsManual is almost determined by 'anything is dirty' except for question flag status changes.

            return statusManager.IsDirty(entity) ||
                   statusManager.IsNew(entity) ||
                   statusManager.IsDirty(() => entity.QuestionType) ||
                   statusManager.IsDirty(() => entity.Source) ||
                   statusManager.IsDirty(() => entity.QuestionCategories) ||
                   entity.QuestionCategories.Any(x => statusManager.IsDirty(x)) ||
                   entity.QuestionCategories.Any(x => statusManager.IsNew(x)) ||
                   statusManager.IsDirty(() => entity.QuestionLinks) ||
                   entity.QuestionLinks.Any(x => statusManager.IsDirty(x)) ||
                   entity.QuestionLinks.Any(x => statusManager.IsNew(x));
        }
    }
}
