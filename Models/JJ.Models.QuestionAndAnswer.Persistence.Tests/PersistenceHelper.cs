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
            // TODO: Make [i] work...
            //var modelAssemblyNames = new List<string>();
            //for (int i = 0; i <= Configuration<IPersistenceSettings>.GetValue(x => x.ModelAssemblies.Length); i++)
            //{
            //    string modelAssemblyName = Configuration<IPersistenceSettings>.GetValue(x => x.ModelAssemblies[i]);
            //    modelAssemblyNames.Add(modelAssemblyName);
            //}

            //return ContextFactory.CreateContext(
            //    Configuration<IPersistenceSettings>.GetValue(x => x.ContextType),
            //    Configuration<IPersistenceSettings>.GetValue(x => x.Location),
            //    modelAssemblyNames.ToArray());


            return ContextFactory.CreateContext(
                Configuration<IPersistenceSettings>.GetValue(x => x.ContextType),
                Configuration<IPersistenceSettings>.GetValue(x => x.Location),
                Configuration<IPersistenceSettings>.GetValue(x => x.ModelAssemblies[0]),
                Configuration<IPersistenceSettings>.GetValue(x => x.ModelAssemblies[1]));
        }
    }
}
