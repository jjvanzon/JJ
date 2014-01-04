using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces
{
    public interface IRepository<TEntity, TID>
    {
        TEntity TryGet(TID id);
        TEntity Get(TID id);
        IEnumerable<TEntity> GetAll();
        TEntity Create();
        void Delete(TEntity entity);
        void Update(TEntity entity);
        void Commit();
    }
}
