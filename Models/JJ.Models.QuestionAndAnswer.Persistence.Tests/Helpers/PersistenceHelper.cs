using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Configuration;
using JJ.Framework.Persistence;

namespace JJ.Models.QuestionAndAnswer.Persistence.Tests.Helpers
{
    public static class PersistenceHelper
    {
        public static IContext CreatePersistenceContext(string contextType)
        {
            PersistenceConfiguration persistenceConfiguration = ConfigurationManager.GetSection<PersistenceConfiguration>();
            
            return ContextFactory.CreateContext(
                contextType,
                persistenceConfiguration.Location,
                persistenceConfiguration.ModelAssemblies);
        }
    }
}
