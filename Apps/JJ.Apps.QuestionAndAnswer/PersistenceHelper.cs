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
using JJ.Framework.Reflection;

namespace JJ.Apps.QuestionAndAnswer
{
    public static class PersistenceHelper
    {
        public static IContext CreatePersistenceContext()
        {
            //return ContextFactory.CreateContext(
            //    AppSettings<IPersistenceSettings>.Get(x => x.PersistenceLocation),
            //    AppSettings<IPersistenceSettings>.Get(x => x.PersistenceModelAssembly),
            //    AppSettings<IPersistenceSettings>.Get(x => x.PersistenceContextType));
            throw new NotImplementedException();
        }
    }
}
