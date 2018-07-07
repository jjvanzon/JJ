using System.Linq;
using JJ.Business.QuestionAndAnswer.Helpers;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Business;
using JJ.Framework.Exceptions.Basic;

namespace JJ.Business.QuestionAndAnswer.SideEffects
{
    public class Question_SideEffect_SetLastModifiedByUser : ISideEffect
    {
        private readonly Question _question;
        private readonly User _user;
        private readonly EntityStatusManager _statusManager;

        public Question_SideEffect_SetLastModifiedByUser(Question question, User user, EntityStatusManager statusManager)
        {
            _question = question ?? throw new NullException(() => question);
            _user = user ?? throw new NullException(() => user);
            _statusManager = statusManager ?? throw new NullException(() => statusManager);
        }

        public void Execute()
        {
            if (MustSetLastModifiedByUser(_question, _statusManager))
            {
                _question.LastModifiedByUser = _user;
            }
        }

        private bool MustSetLastModifiedByUser(Question entity, EntityStatusManager statusManager)
            => statusManager.IsNew(entity) ||
               statusManager.QuestionTypeIsDirty(entity) ||
               statusManager.SourceIsDirty(entity) ||
               statusManager.QuestionCategoriesListIsDirty(entity) ||
               statusManager.QuestionLinksListIsDirty(entity) ||
               entity.QuestionLinks.Any(statusManager.IsNew) ||
               statusManager.QuestionFlagsListIsDirty(entity) ||
               entity.QuestionFlags.Any(statusManager.IsNew);
    }
}