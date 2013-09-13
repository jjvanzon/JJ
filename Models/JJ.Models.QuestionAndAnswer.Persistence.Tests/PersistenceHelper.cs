using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Configuration;
using JJ.Framework.Persistence;

namespace JJ.Models.QuestionAndAnswer.Persistence.Tests
{
    public static class PersistenceHelper
    {
        public static IContext CreatePersistenceContext()
        {
            PersistenceConfiguration persistenceConfiguration = ConfigurationManager.GetSection<PersistenceConfiguration>();

            return ContextFactory.CreateContext(
                persistenceConfiguration.ContextType,
                persistenceConfiguration.Location,
                persistenceConfiguration.ModelAssemblies);
        }
    }
}
