using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.ViewModels;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Helpers
{
    internal static class QuestionExtensions
    {
        public static QuestionDetailViewModel ToDetailViewModel(this Question entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            
            return new QuestionDetailViewModel
            {
                Question = entity.ToViewModelWithRelatedEntities(),
                SelectedCategories = new List<CategoryViewModel>(),
                Login = new LoginViewModel(),
            };
        }

        public static QuestionViewModel ToViewModelWithRelatedEntities(this Question entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            QuestionViewModel viewModel = entity.ToViewModel();

            // Links
            foreach (QuestionLink questionLink in entity.QuestionLinks)
            {
                var linkModel = new LinkViewModel(questionLink.Description, questionLink.Url);
                viewModel.Links.Add(linkModel);
            }

            // Categories
            foreach (Category category in entity.QuestionCategories.Select(x => x.Category))
            {
                CategoryViewModel categoryModel = category.ToViewModel();
                viewModel.Categories.Add(categoryModel);
            }

            return viewModel;
        }

        public static QuestionViewModel ToViewModel(this Question entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            return new QuestionViewModel
            {
                ID = entity.ID,
                Text = entity.Text,
                Answer = entity.Answers[0].Text, // TODO: Refactor
                Links = new List<LinkViewModel>(),
                Categories = new List<CategoryViewModel>(),
                Flag = new QuestionFlagViewModel()
            };
        }
    }
}
