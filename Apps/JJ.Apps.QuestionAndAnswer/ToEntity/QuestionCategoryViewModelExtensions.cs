using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using JJ.Business.QuestionAndAnswer.LinkTo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Apps.QuestionAndAnswer.ToEntity
{
    internal static class QuestionCategoryViewModelExtensions
    {
        public static QuestionCategory ToEntity(this QuestionCategoryViewModel viewModel, IQuestionCategoryRepository repository, ICategoryRepository categoryRepository)
        {
            QuestionCategory entity = repository.TryGet(viewModel.QuestionCategoryID);
            if (entity == null)
            {
                entity = repository.Create();
            }

            Category category = categoryRepository.TryGet(viewModel.Category.ID);
            entity.LinkTo(category);

            return entity;
        }
    }
}
