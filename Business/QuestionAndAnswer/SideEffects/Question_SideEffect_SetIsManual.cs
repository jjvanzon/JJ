using System.Linq;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Business;
using JJ.Framework.Exceptions.Basic;

namespace JJ.Business.QuestionAndAnswer.SideEffects
{
    public class Question_SideEffect_SetIsManual : ISideEffect
    {
        private readonly Question _entity;
        private readonly EntityStatusManager _statusManager;

        public Question_SideEffect_SetIsManual(Question entity, EntityStatusManager statusManager)
        {
            _entity = entity ?? throw new NullException(() => entity);
            _statusManager = statusManager ?? throw new NullException(() => statusManager);
        }

        public void Execute()
        {
            if (MustSetIsManual(_entity, _statusManager))
            {
                _entity.IsManual = true;
            }
        }

        private bool MustSetIsManual(Question entity, EntityStatusManager statusManager)
            => statusManager.IsDirty(entity) ||
               statusManager.IsNew(entity) ||
               statusManager.IsDirty(() => entity.QuestionType) ||
               statusManager.IsDirty(() => entity.Source) ||
               statusManager.IsDirty(() => entity.QuestionCategories) ||
               entity.QuestionCategories.Any(statusManager.IsDirty) ||
               entity.QuestionCategories.Any(statusManager.IsNew) ||
               statusManager.IsDirty(() => entity.QuestionLinks) ||
               entity.QuestionLinks.Any(statusManager.IsDirty) ||
               entity.QuestionLinks.Any(statusManager.IsNew);
    }
}