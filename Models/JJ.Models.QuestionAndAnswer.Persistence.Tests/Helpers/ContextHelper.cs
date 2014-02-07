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
    public static class ContextHelper
    {
        public static IContext CreateContext(string contextType)
        {
            PersistenceConfiguration configuration = CustomConfigurationManager.GetSection<PersistenceConfiguration>();
            
            return ContextFactory.CreateContext(
                contextType,
                configuration.Location,
                configuration.ModelAssembly,
                configuration.MappingAssembly);
        }
    }
}
