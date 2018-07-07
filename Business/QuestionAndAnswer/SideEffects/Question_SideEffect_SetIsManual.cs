using System.Linq;
using JJ.Business.QuestionAndAnswer.Helpers;
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
            => statusManager.IsNew(entity) ||
               statusManager.QuestionTypeIsDirty(entity) ||
               statusManager.SourceIsDirty(entity) ||
               statusManager.QuestionCategoriesListIsDirty(entity) ||
               entity.QuestionCategories.Any(statusManager.IsNew) ||
               statusManager.QuestionLinksListIsDirty(entity) ||
               entity.QuestionLinks.Any(statusManager.IsNew);
    }
}