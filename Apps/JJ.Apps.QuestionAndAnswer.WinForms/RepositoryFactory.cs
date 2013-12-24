using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.Repositories;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJ.Apps.QuestionAndAnswer.WinForms
{
    internal static class RepositoryFactory
    {
        public static IUserRepository CreateUserRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            return new UserRepository(context);
        }

        public static IQuestionRepository CreateQuestionRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            return new QuestionRepository(context, context.Location);
        }

        public static IQuestionFlagRepository CreateQuestionFlagRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            return new QuestionFlagRepository(context);
        }

        internal static ICategoryRepository CreateCategoryRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            return new CategoryRepository(context);
        }

        internal static IFlagStatusRepository CreateFlagStatusRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            return new FlagStatusRepository(context);
        }
    }
}