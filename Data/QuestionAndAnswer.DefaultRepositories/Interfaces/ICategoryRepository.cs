using System.Collections.Generic;
using JJ.Framework.Data;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category, int>
    {
        IList<Category> GetAll();
        IList<Category> TryGetManyByIdentifier(string identifier);
        Category TryGetByIdentifier(string identifier);
        IList<Category> GetRootCategories();
    }
}
