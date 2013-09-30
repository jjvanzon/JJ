using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces
{
    public interface ICategoryRepository
    {
        Category Get(int id);
        Category Create();
        Category TryGetByIdentifier(string identifier);
        Category TryGetCategoryByParentAndIdentifier(Category parentCategory, string identifier);
    }
}
