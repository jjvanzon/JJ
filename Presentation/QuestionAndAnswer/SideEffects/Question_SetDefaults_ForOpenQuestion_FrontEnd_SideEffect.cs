using JJ.Business.QuestionAndAnswer.Helpers;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Business;
using JJ.Framework.Exceptions.Basic;

namespace JJ.Presentation.QuestionAndAnswer.SideEffects
{
	/// <summary>
	/// These defaults are specific to the UI, not the business,
	/// so keep it in the front-end.
	/// </summary>
	internal class Question_SetDefaults_ForOpenQuestion_FrontEnd_SideEffect : ISideEffect
	{
		private const string DEFAULT_SOURCE_IDENTIFIER = "Manual";

		private readonly Question _entity;
		private readonly ISourceRepository _sourceRepository;
		private readonly EntityStatusManager _statusManager;

		public Question_SetDefaults_ForOpenQuestion_FrontEnd_SideEffect(
			Question entity,
			ISourceRepository sourceRepository,
			EntityStatusManager statusManager)
		{
			_entity = entity ?? throw new NullException(() => entity);
			_sourceRepository = sourceRepository ?? throw new NullException(() => sourceRepository);
			_statusManager = statusManager ?? throw new NullException(() => statusManager);
		}

		public void Execute()
		{
			if (_statusManager.IsNew(_entity))
			{
				_entity.IsManual = true;
				_entity.Source = _sourceRepository.GetByIdentifier(DEFAULT_SOURCE_IDENTIFIER);
			}
		}
	}
}
