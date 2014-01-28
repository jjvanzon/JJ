using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Helpers
{
    internal static class ViewModelExtensions_IsNew
    {
        /// <summary>
        /// Marks different pieces of the view model as new,
        /// by setting IsNew properties in the view model,
        /// depending on whether an entity or related entity does not exist in the entity model yet.
        /// </summary>
        public static void SetIsNewRecursive(this QuestionViewModel viewModel, Question question)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            viewModel.NullCoallesce();

            viewModel.SetIsNew(question);

            if (question != null)
            {
                viewModel.Categories.SetItemsAreNew(question.QuestionCategories);

                viewModel.Links.SetItemsAreNew(question.QuestionLinks);

                viewModel.Flags.SetItemsAreNew(question.QuestionFlags);
            }
        }

        private static bool SetIsNew(this QuestionViewModel viewModel, Question entity)
        {
            return viewModel.IsNew = GetIsNew(viewModel, entity);
        }

        private static void SetItemsAreNew(this ListViewModel<QuestionCategoryViewModel> listViewModel, IList<QuestionCategory> entities)
        {
            IEnumerable<int> ids1 = listViewModel.Select(x => x.QuestionCategoryID);
            IEnumerable<int> ids2 = entities.Select(x => x.ID);

            foreach (int id in Enumerable.Union(ids1, ids2))
            {
                QuestionCategoryViewModel viewModel = listViewModel.Where(x => x.QuestionCategoryID == id).SingleOrDefault();
                QuestionCategory entity = entities.Where(x => x.ID == id).SingleOrDefault();

                if (viewModel != null)
                {
                    viewModel.SetIsNew(entity);
                }
            }
        }

        private static bool SetIsNew(this QuestionCategoryViewModel viewModel, QuestionCategory entity)
        {
            return viewModel.IsNew = GetIsNew(viewModel, entity);
        }

        private static void SetItemsAreNew(this ListViewModel<QuestionLinkViewModel> listViewModel, IList<QuestionLink> entities)
        {
            IEnumerable<int> ids1 = listViewModel.Select(x => x.ID);
            IEnumerable<int> ids2 = entities.Select(x => x.ID);

            foreach (int id in Enumerable.Union(ids1, ids2))
            {
                QuestionLinkViewModel viewModel = listViewModel.Where(x => x.ID == id).SingleOrDefault();
                QuestionLink entity = entities.Where(x => x.ID == id).SingleOrDefault();

                if (viewModel != null)
                {
                    viewModel.SetIsNew(entity);
                }
            }
        }

        private static bool SetIsNew(this QuestionLinkViewModel viewModel, QuestionLink entity)
        {
            return viewModel.IsNew = GetIsNew(viewModel, entity);
        }

        private static void SetItemsAreNew(this ListViewModel<QuestionFlagViewModel> listViewModel, IList<QuestionFlag> entities)
        {
            IEnumerable<int> ids1 = listViewModel.Select(x => x.ID);
            IEnumerable<int> ids2 = entities.Select(x => x.ID);

            foreach (int id in Enumerable.Union(ids1, ids2))
            {
                QuestionFlagViewModel viewModel = listViewModel.Where(x => x.ID == id).SingleOrDefault();
                QuestionFlag entity = entities.Where(x => x.ID == id).SingleOrDefault();

                if (viewModel != null)
                {
                    viewModel.SetIsNew(entity);
                }
            }
        }

        private static bool SetIsNew(this QuestionFlagViewModel viewModel, QuestionFlag entity)
        {
            return viewModel.IsNew = GetIsNew(viewModel, entity);
        }

        private static bool GetIsNew(this object viewModel, object entity)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");
            return entity == null;
        }
    }
}
