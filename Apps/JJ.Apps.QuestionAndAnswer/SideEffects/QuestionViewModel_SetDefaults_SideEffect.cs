using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Business;
using JJ.Models.QuestionAndAnswer;
using JJ.Apps.QuestionAndAnswer.ToViewModel;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Framework.Reflection;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;

namespace JJ.Apps.QuestionAndAnswer.SideEffects
{
    internal class QuestionViewModel_SetDefaults_SideEffect : ISideEffect
    {
        private const string DEFAULT_SOURCE_IDENTIFIER = "Manual";
        private const QuestionTypeEnum DEFAULT_QUESTION_TYPE_ENUM = QuestionTypeEnum.OpenQuestion;

        private QuestionViewModel _viewModel;
        private ISourceRepository _sourceRepository;
        private IQuestionTypeRepository _questionTypeRepository;

        public QuestionViewModel_SetDefaults_SideEffect(
            QuestionViewModel viewModel,
            ISourceRepository sourceRepository,
            IQuestionTypeRepository questionTypeRepository)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            if (sourceRepository == null) throw new NullException(() => sourceRepository);
            if (questionTypeRepository == null) throw new NullException(() => questionTypeRepository);

            _viewModel = viewModel;
            _sourceRepository = sourceRepository;
            _questionTypeRepository = questionTypeRepository;
        }

        public void Execute()
        {
            _viewModel.IsManual = true;

            // These defaults are specific to the UI, not the business.
            Source source = _sourceRepository.GetByIdentifier(DEFAULT_SOURCE_IDENTIFIER);
            _viewModel.Source = source.ToViewModel();

            QuestionType questionType = _questionTypeRepository.Get((int)DEFAULT_QUESTION_TYPE_ENUM);
            _viewModel.Type = questionType.ToViewModel();
        }
    }
}
