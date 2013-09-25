using JJ.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces
{
    public interface ITextualQuestionRepository
    {
        IEnumerable<TextualQuestion> GetAll();
        TextualQuestion Get(int id);
        TextualQuestion TryGet(int id);
        TextualQuestion TryGetRandomTextualQuestion();
        TextualQuestion Create();
        //TextualQuestion CreateWithRelatedEntities();

        IEnumerable<TextualQuestion> GetBySource(int sourceID);

        void Delete(TextualQuestion textualQuestion);
        //void DeleteWithRelatedEntities(TextualQuestion textualQuestion);

        void Commit();

        
    }
}
