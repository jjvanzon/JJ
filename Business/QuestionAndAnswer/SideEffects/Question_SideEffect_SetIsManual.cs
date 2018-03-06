using System.Linq;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Business;
using JJ.Framework.Exceptions;
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
