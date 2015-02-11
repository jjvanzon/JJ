using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Business;
using JJ.Framework.Reflection;
using JJ.Persistence.QuestionAndAnswer;
using JJ.Persistence.QuestionAndAnswer.DefaultRepositories.Interfaces;

namespace JJ.Presentation.QuestionAndAnswer.SideEffects
{
    /// <summary>
    /// These defaults are specific to the UI, not the business,
    /// so keep it in the front-end.
    /// </summary>
    internal class Question_SetDefaults_ForOpenQuestion_FrontEnd_SideEffect : ISideEffect
    {
        private const string DEFAULT_SOURCE_IDENTIFIER = "Manual";

        private Question _entity;
        private ISourceRepository _sourceRepository;
        private EntityStatusManager _statusManager;

        public Question_SetDefaults_ForOpenQuestion_FrontEnd_SideEffect(
            Question entity,
            ISourceRepository sourceRepository,
            EntityStatusManager statusManager)
        {
            if (entity == null) throw new NullException(() => entity);
            if (sourceRepository == null) throw new NullException(() => sourceRepository);
            if (statusManager == null) throw new NullException(() => statusManager);

            _entity = entity;
            _sourceRepository = sourceRepository;
            _statusManager = statusManager;
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
