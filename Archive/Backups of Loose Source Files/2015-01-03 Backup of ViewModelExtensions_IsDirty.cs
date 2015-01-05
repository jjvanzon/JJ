//using JJ.Apps.QuestionAndAnswer.Helpers;
//using JJ.Apps.QuestionAndAnswer.ViewModels;
//using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
//using JJ.Framework.Business;
//using JJ.Framework.Reflection;
//using JJ.Models.QuestionAndAnswer;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JJ.Apps.QuestionAndAnswer.Extensions
//{
//    internal static class ViewModelExtensions_IsDirty
//    {
//        /// <summary>
//        /// Marks different pieces of the view model as dirty,
//        /// by setting IsDirty properties in the view model,
//        /// depending on the differences with the passed entity.
//        /// </summary>
//        /// <param name="entity"> nullable </param>
//        public static void SetIsDirtyWithRelatedEntities(this QuestionViewModel viewModel, Question entity)
//        {
//            if (viewModel == null) throw new NullException(() => viewModel);

//            viewModel.NullCoallesce();

//            viewModel.IsDirty = GetIsDirty(viewModel, entity);

//            if (entity != null)
//            {
//                viewModel.Categories.IsDirty = EntityStatusHelper.GetListIsDirty(viewModel.Categories, x => x.QuestionCategoryID, entity.QuestionCategories, x => x.ID);
//                viewModel.Categories.SetItemsAreDirty(entity.QuestionCategories);

//                viewModel.Links.IsDirty = EntityStatusHelper.GetListIsDirty(viewModel.Links, x => x.ID, entity.QuestionLinks, x => x.ID);
//                viewModel.Links.SetItemsAreDirty(entity.QuestionLinks);

//                viewModel.Flags.IsDirty = EntityStatusHelper.GetListIsDirty(viewModel.Flags, x => x.ID, entity.QuestionFlags, x => x.ID);
//                viewModel.Flags.SetItemsAreDirty(entity.QuestionFlags);
//            }
//        }

//        // SetItemsAreDirty

//        private static void SetItemsAreDirty(this IList<QuestionCategoryViewModel> listViewModel, IList<QuestionCategory> entities)
//        {
//            var tuples = from viewModel in listViewModel
//                         join entity in entities on viewModel.QuestionCategoryID equals entity.ID
//                         select new 
//                         { 
//                             ViewModel = viewModel, 
//                             Entity = entity 
//                         };

//            foreach (var tuple in tuples)
//            {
//                tuple.ViewModel.IsDirty = GetIsDirty(tuple.ViewModel, tuple.Entity);
//            }
//        }

//        private static void SetItemsAreDirty(this IList<QuestionLinkViewModel> listViewModel, IList<QuestionLink> entities)
//        {
//            var tuples = from viewModel in listViewModel
//                         join entity in entities on viewModel.ID equals entity.ID
//                         select new
//                         {
//                             ViewModel = viewModel,
//                             Entity = entity
//                         };

//            foreach (var tuple in tuples)
//            {
//                tuple.ViewModel.IsDirty = GetIsDirty(tuple.ViewModel, tuple.Entity);
//            }
//        }

//        private static void SetItemsAreDirty(this IList<QuestionFlagViewModel> listViewModel, IList<QuestionFlag> entities)
//        {
//            var tuples = from viewModel in listViewModel
//                         join entity in entities on viewModel.ID equals entity.ID
//                         select new
//                         {
//                             ViewModel = viewModel,
//                             Entity = entity
//                         };

//            foreach (var tuple in tuples)
//            {
//                tuple.ViewModel.IsDirty = GetIsDirty(tuple.ViewModel, tuple.Entity);
//            }
//        }

//        // GetIsDirty

//        private static bool GetIsDirty(this QuestionViewModel viewModel, Question entity)
//        {
//            if (entity == null) return false;
            
//            return !String.Equals(viewModel.Answer, entity.Answers[0].Text) || 
//                   viewModel.IsActive != entity.IsActive ||
//                   !String.Equals(viewModel.Text, entity.Text) ||
//                   viewModel.Type.ID != entity.QuestionType.ID ||
//                   viewModel.Source.ID != entity.Source.ID;
//        }

//        private static bool GetIsDirty(this QuestionCategoryViewModel viewModel, QuestionCategory entity)
//        {
//            if (entity == null) return false;

//            return viewModel.Category.ID != entity.Category.ID;
//        }

//        private static bool GetIsDirty(this QuestionLinkViewModel viewModel, QuestionLink entity)
//        {
//            if (entity == null) return false;

//            return !String.Equals(viewModel.Url, entity.Url) ||
//                   !String.Equals(viewModel.Description, entity.Description);
//        }

//        private static bool GetIsDirty(QuestionFlagViewModel viewModel, QuestionFlag entity)
//        {
//            if (entity == null) return false;

//            return entity.FlagStatus.ID != viewModel.Status.ID;
//        }
//    }
//}
