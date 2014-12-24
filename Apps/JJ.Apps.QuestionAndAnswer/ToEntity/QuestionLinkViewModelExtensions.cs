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
    internal static class QuestionLinkViewModelExtensions
    {
        public static QuestionLink ToEntity(this QuestionLinkViewModel viewModel, IQuestionLinkRepository repository)
        {
            QuestionLink entity = repository.TryGet(viewModel.ID);
            if (entity == null)
            {
                entity = repository.Create();
            }

            entity.Url = viewModel.Url;
            entity.Description = viewModel.Description;

            return entity;
        }
    }
}
