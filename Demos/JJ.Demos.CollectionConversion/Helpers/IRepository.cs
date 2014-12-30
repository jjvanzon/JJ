using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Demos.CollectionConversion.Helpers
{
    internal interface IRepository
    {
        void Delete(Entity entity);

        Entity Get(int idToDelete);

        Entity TryGet(int p);

        Entity Create();
    }
}
