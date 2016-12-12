using JJ.Framework.Business;
using JJ.Framework.Exceptions;
using JJ.Data.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.SideEffects
{
    public class Question_SideEffect_SetLastModifiedByUser : ISideEffect
    {
        private Question _question;
        private User _user;
        private EntityStatusManager _statusManager;

        public Question_SideEffect_SetLastModifiedByUser(Question question, User user, EntityStatusManager statusManager)
        {
            if (question == null) throw new NullException(() => question);
            if (user == null) throw new NullException(() => user);
            if (statusManager == null) throw new NullException(() => statusManager);

            _question = question;
            _user = user;
            _statusManager = statusManager;
        }

        public void Execute()
        {
            if (MustSetLastModifiedByUser(_question, _statusManager))
            {
                _question.LastModifiedByUser = _user;
            }
        }

        private bool MustSetLastModifiedByUser(Question entity, EntityStatusManager statusManager)
        {
            return statusManager.IsDirty(entity) ||
                   statusManager.IsNew(entity) ||
                   statusManager.IsDirty(() => entity.QuestionType) ||
                   statusManager.IsDirty(() => entity.Source) ||
                   statusManager.IsDirty(() => entity.QuestionCategories) ||
                   entity.QuestionCategories.Any(x => statusManager.IsDirty(x)) ||
                   statusManager.IsDirty(() => entity.QuestionLinks) ||
                   entity.QuestionLinks.Any(x => statusManager.IsDirty(x)) ||
                   entity.QuestionLinks.Any(x => statusManager.IsNew(x)) ||
                   statusManager.IsDirty(() => entity.QuestionFlags) ||
                   entity.QuestionFlags.Any(x => statusManager.IsDirty(x)) ||
                   entity.QuestionFlags.Any(x => statusManager.IsNew(x));

        }
    }
}
