using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Helpers
{
    internal static class ViewModelExtensions_IsDirty
    {
        /// <summary>
        /// Marks different pieces of the view model as dirty,
        /// by setting IsDirty properties in the view model,
        /// depending on the differences with the passed entity.
        /// </summary>
        public static void SetIsDirtyRecursive(this QuestionViewModel viewModel, Question question)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            viewModel.NullCoallesce();

            viewModel.SetIsDirty(question);

            if (question != null)
            {
                viewModel.Categories.SetListIsDirty(question.QuestionCategories);
                viewModel.Categories.SetItemsAreDirty(question.QuestionCategories);

                viewModel.Links.SetListIsDirty(question.QuestionLinks);
                viewModel.Links.SetItemsAreDirty(question.QuestionLinks);

                viewModel.Flags.SetListIsDirty(question.QuestionFlags);
                viewModel.Flags.SetItemsAreDirty(question.QuestionFlags);
            }
        }

        private static bool SetIsDirty(this QuestionCategoryViewModel viewModel, QuestionCategory entity)
        {
            return viewModel.IsDirty = GetIsDirty(viewModel, entity);
        }

        private static bool SetIsDirty(this QuestionLinkViewModel viewModel, QuestionLink entity)
        {
            return viewModel.IsDirty = GetIsDirty(viewModel, entity);
        }

        private static bool SetIsDirty(this QuestionViewModel viewModel, Question entity)
        {
            return viewModel.IsDirty = GetIsDirty(viewModel, entity);
        }

        private static bool SetIsDirty(this QuestionFlagViewModel viewModel, QuestionFlag entity)
        {
            return viewModel.IsDirty = GetIsDirty(viewModel, entity);
        }

        private static void SetListIsDirty(this ListViewModel<QuestionCategoryViewModel> listViewModel, IList<QuestionCategory> entities)
        {
            listViewModel.IsDirty = GetListIsDirty(listViewModel, entities);
        }

        private static void SetListIsDirty(this ListViewModel<QuestionLinkViewModel> listViewModel, IList<QuestionLink> entityList)
        {
            listViewModel.IsDirty = GetListIsDirty(listViewModel, entityList);
        }

        private static void SetListIsDirty(this ListViewModel<QuestionFlagViewModel> listViewModel, IList<QuestionFlag> entityList)
        {
            listViewModel.IsDirty = GetListIsDirty(listViewModel, entityList);
        }

        private static void SetItemsAreDirty(this ListViewModel<QuestionCategoryViewModel> listViewModel, IList<QuestionCategory> entities)
        {
            IEnumerable<int> ids1 = listViewModel.Select(x => x.QuestionCategoryID);
            IEnumerable<int> ids2 = entities.Select(x => x.ID);

            foreach (int id in Enumerable.Union(ids1, ids2))
            {
                QuestionCategoryViewModel viewModel = listViewModel.Where(x => x.QuestionCategoryID == id).SingleOrDefault();
                QuestionCategory entity = entities.Where(x => x.ID == id).SingleOrDefault();

                if (viewModel != null && entity != null)
                {
                    viewModel.SetIsDirty(entity);
                }
            }
        }

        private static void SetItemsAreDirty(this ListViewModel<QuestionLinkViewModel> listViewModel, IList<QuestionLink> entities)
        {
            IEnumerable<int> ids1 = listViewModel.Select(x => x.ID);
            IEnumerable<int> ids2 = entities.Select(x => x.ID);

            foreach (int id in Enumerable.Union(ids1, ids2))
            {
                QuestionLinkViewModel viewModel = listViewModel.Where(x => x.ID == id).SingleOrDefault();
                QuestionLink entity = entities.Where(x => x.ID == id).SingleOrDefault();

                if (viewModel != null && entity != null)
                {
                    viewModel.SetIsDirty(entity);
                }
            }
        }

        private static void SetItemsAreDirty(this ListViewModel<QuestionFlagViewModel> listViewModel, IList<QuestionFlag> entities)
        {
            IEnumerable<int> ids1 = listViewModel.Select(x => x.ID);
            IEnumerable<int> ids2 = entities.Select(x => x.ID);

            foreach (int id in Enumerable.Union(ids1, ids2))
            {
                QuestionFlagViewModel viewModel = listViewModel.Where(x => x.ID == id).SingleOrDefault();
                QuestionFlag entity = entities.Where(x => x.ID == id).SingleOrDefault();

                if (viewModel != null && entity != null)
                {
                    viewModel.SetIsDirty(entity);
                }
            }
        }

        // GetIsDirty

        private static bool GetIsDirty(this QuestionViewModel viewModel, Question entity)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            if (entity == null) return false;
            
            return viewModel.Answer != entity.Answers[0].Text || // TODO: Support multiple answers.
                   viewModel.IsActive != entity.IsActive ||
                   viewModel.Text != entity.Text ||
                   viewModel.Type.ID != entity.QuestionType.ID ||
                   viewModel.Source.ID != entity.Source.ID;
        }

        private static bool GetIsDirty(this QuestionCategoryViewModel viewModel, QuestionCategory entity)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            if (entity == null)
            {
                return false;
            }

            return viewModel.Category.ID != entity.Category.ID;
        }

        private static bool GetIsDirty(this QuestionLinkViewModel viewModel, QuestionLink entity)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            if (entity == null)
            {
                return false;
            }

            return viewModel.Url != entity.Url ||
                   viewModel.Description != entity.Description;
        }

        private static bool GetIsDirty(QuestionFlagViewModel viewModel, QuestionFlag entity)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            if (entity == null)
            {
                return false;
            }

            return entity.FlagStatus.ID != viewModel.Status.ID;
        }

        private static bool GetListIsDirty(this IList<QuestionCategoryViewModel> listViewModel, IList<QuestionCategory> entities)
        {
            return GetListIsDirty(listViewModel, x => x.QuestionCategoryID, entities, x => x.ID);
        }

        private static bool GetListIsDirty(this IList<QuestionLinkViewModel> listViewModel, IList<QuestionLink> entities)
        {
            return GetListIsDirty(listViewModel, x => x.ID, entities, x => x.ID);
        }

        private static bool GetListIsDirty(this IList<QuestionFlagViewModel> listViewModel, IList<QuestionFlag> entities)
        {
            return GetListIsDirty(listViewModel, x => x.ID, entities, x => x.ID);
        }

        // TODO: Move to a Framework or a if it proves useful for reuse.

        private static bool GetListIsDirty<TViewModel, TEntity>(
            IList<TViewModel> listViewModel, Func<TViewModel, object> getViewModelKey, 
            IList<TEntity> entities, Func<TEntity, object> getEntityKey, 
            bool ingoreOrder = false)
        {
            if (listViewModel == null) { throw new ArgumentNullException("listViewModel"); }
            if (getViewModelKey == null) { throw new ArgumentNullException("getViewModelKey"); }
            if (entities == null) { throw new ArgumentNullException("entities"); }
            if (getEntityKey == null) { throw new ArgumentNullException("getEntityKey"); }

            if (listViewModel.Count != entities.Count)
            {
                return true;
            }

            // If the order does not matter you have to sort the list and compare the sorted lists.
            if (ingoreOrder)
            {
                listViewModel = listViewModel.OrderBy(getViewModelKey).ToArray();
                entities = entities.OrderBy(getEntityKey).ToArray();
            }

            for (int i = 0; i < listViewModel.Count; i++)
            {
                object key1 = getViewModelKey(listViewModel[i]);
                object key2 = getEntityKey(entities[i]);

                if (!Object.Equals(key1, key2))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
