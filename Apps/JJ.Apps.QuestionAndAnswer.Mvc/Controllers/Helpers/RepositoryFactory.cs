using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.Repositories;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJ.Apps.QuestionAndAnswer.Mvc.Controllers.Helpers
{
    // TODO: Remove this class after refactoring creation of presenters with repository sets other than for QuestionDetail and stuff.
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

        public static ICategoryRepository CreateCategoryRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            return new CategoryRepository(context);
        }

        public static IFlagStatusRepository CreateFlagStatusRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            return new FlagStatusRepository(context);
        }

        public static IAnswerRepository CreateAnswerRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            return new AnswerRepository(context);
        }

        public static IQuestionCategoryRepository CreateQuestionCategoryRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            return new QuestionCategoryRepository(context);
        }

        public static IQuestionLinkRepository CreateQuestionLinkRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            return new QuestionLinkRepository(context);
        }

        public static ISourceRepository CreateSourceRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            return new SourceRepository(context);
        }

        internal static IQuestionTypeRepository CreateQuestionTypeRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            return new QuestionTypeRepository(context);
        }
    }
}