using System;
using System.Collections.Generic;
using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Data.Canonical;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Exceptions.Basic;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;

namespace JJ.Presentation.QuestionAndAnswer.ToViewModel
{
    internal static class ToEntityViewModelExtensions
    {
        public static QuestionCategoryViewModel ToViewModel(this QuestionCategory questionCategory)
        {
            var viewModel = new QuestionCategoryViewModel
            {
                QuestionCategoryID = questionCategory.ID,
                TemporaryID = Guid.NewGuid(),
                CategoryLookup = new List<CategoryViewModel>()
            };

            if (questionCategory.Category != null)
            {
                viewModel.Category = questionCategory.Category.ToViewModel();
            }
            else
            {
                viewModel.Category = ViewModelHelper.CreateEmptyCategoryViewModel();
            }


            return viewModel;
        }

        /// <summary>
        /// Converts the entity to a view model, but does not convert the related entities.
        /// (objects and lists will be created, though: no nulls.)
        /// </summary>
        public static QuestionViewModel ToViewModel(this Question entity)
        {
            if (entity == null) throw new NullException(() => entity);

            var viewModel = new QuestionViewModel
            {
                ID = entity.ID,
                Text = entity.Text,
                IsActive = entity.IsActive,
                LastModifiedBy = entity.LastModifiedByUser != null ? entity.LastModifiedByUser.DisplayName : "",
                IsManual = entity.IsManual,
                Source = new SourceViewModel(),
                Type = new QuestionTypeViewModel(),
                Categories = new List<QuestionCategoryViewModel>(),
                Links = new List<QuestionLinkViewModel>(),
                Flags = new List<QuestionFlagViewModel>()
            };

            // TODO: Refactor to support multiple answers.
            if (entity.Answers.Count > 0)
            {
                viewModel.Answer = entity.Answers[0].Text;
            }

            return viewModel;
        }

        public static QuestionFlagViewModel ToViewModel(this QuestionFlag entity)
        {
            if (entity == null) throw new NullException(() => entity);

            return new QuestionFlagViewModel
            {
                ID = entity.ID,
                Comment = entity.Comment,
                DateAndTime = entity.DateTime,
                FlaggedBy = entity.FlaggedByUser.DisplayName,
                LastModifiedBy = entity.LastModifiedByUser.DisplayName,
                Status = entity.FlagStatus.ToIDAndName()
            };
        }

        public static QuestionLinkViewModel ToViewModel(this QuestionLink entity)
        {
            if (entity == null) throw new NullException(() => entity);

            return new QuestionLinkViewModel
            {
                ID = entity.ID,
                TemporaryID = Guid.NewGuid(),
                Description = entity.Description,
                Url = entity.Url
            };
        }

        public static QuestionTypeViewModel ToViewModel(this QuestionType entity)
        {
            if (entity == null) throw new NullException(() => entity);

            return new QuestionTypeViewModel
            {
                ID = entity.ID,
                Name = entity.Name
            };
        }

        public static SourceViewModel ToViewModel(this Source entity)
        {
            if (entity == null) throw new NullException(() => entity);

            return new SourceViewModel
            {
                ID = entity.ID,
                Description = entity.Description,
                Url = entity.Url
            };
        }

        public static IDAndName ToIDAndName(this FlagStatus entity)
        {
            if (entity == null) throw new NullException(() => entity);

            var viewModel = new IDAndName
            {
                ID = entity.ID,
                Name = ResourceFormatter.GetDisplayName(entity.Description)
            };

            return viewModel;
        }
    }
}