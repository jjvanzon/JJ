using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers.Helpers
{
    public static class RepositoryFactory
    {
        public static TInterface CreateRepository<TInterface>()
        {
            IContext context = ContextHelper.CreateContextFromConfiguration();

            if (typeof(TInterface) == typeof(IUserRepository))
            {
                return (TInterface)(object)new UserRepository(context);
            }

            throw new NotSupportedException(String.Format("Repository of the following type is not supported: '{0}'", typeof(TInterface).FullName));
        }
    }
}