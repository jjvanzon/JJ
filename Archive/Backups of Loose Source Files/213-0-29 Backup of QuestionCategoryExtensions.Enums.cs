//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using JJ.Models.QuestionAndAnswer;
//using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
//using JJ.Business.QuestionAndAnswer.Enums;

//namespace JJ.Business.QuestionAndAnswer.Extensions
//{
//    public static partial class QuestionCategoryExtensions
//    {
//        public static void SetCategoryEnum(this QuestionCategory entity, ICategoryRepository categoryRepository, CategoryEnum value)
//        {
//            if (entity == null) throw new ArgumentNullException("entity");
//            if (categoryRepository == null) throw new ArgumentNullException("categoryRepository");

//            Category category = categoryRepository.Get((int)value);
//            category.LinkTo(entity);
//        }
//    }
//}
