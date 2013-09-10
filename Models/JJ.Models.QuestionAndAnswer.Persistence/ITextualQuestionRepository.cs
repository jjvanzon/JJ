using JJ.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Persistence
{
    public interface ITextualQuestionRepository
    {
        IEnumerable<EntityWrapper<TextualQuestion>> GetAll();
        EntityWrapper<TextualQuestion> Get(int id);
    }
}
