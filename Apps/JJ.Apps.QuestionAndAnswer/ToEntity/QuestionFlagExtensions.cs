using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ToEntity
{
    internal static class QuestionFlagExtensions
    {
        public static QuestionFlag ToEntity(this QuestionFlagViewModel viewModel, IQuestionFlagRepository questionFlagRepository, IFlagStatusRepository flagStatusRepository)
        {
            QuestionFlag questionFlag = questionFlagRepository.TryGet(viewModel.ID);
            // TODO: This is not the TryGet-Insert-Update pattern. It just happens to work for the QuestionEditViewModel.
            if (questionFlag != null)
            {
                questionFlag.FlagStatus = flagStatusRepository.Get(viewModel.Status.ID);
                questionFlag.Comment = viewModel.Comment;
            }

            return questionFlag;
        }
    }
}
