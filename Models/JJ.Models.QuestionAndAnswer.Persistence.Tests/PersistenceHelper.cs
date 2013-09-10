using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Persistence;
using JJ.Framework.Persistence.NHibernate;
using JJ.Models.QuestionAndAnswer;
using JJ.Framework.Configuration;
using JJ.Models.QuestionAndAnswer.Persistence.NHibernate;

namespace JJ.Models.QuestionAndAnswer.Persistence.Tests
{
    public static class PersistenceHelper
    {
        public static IContext CreatePersistenceContext()
        {
            return ContextFactory.CreateContext(
                AppSettings<IPersistenceSettings>.Get(x => x.PersistenceContextType),
                AppSettings<IPersistenceSettings>.Get(x => x.PersistenceLocation),
                AppSettings<IPersistenceSettings>.Get(x => x.PersistenceModelAssembly),
                typeof(TextualQuestionMapping).Assembly.FullName);
        }
    }
}

