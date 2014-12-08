using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.EntityFramework5
{
    public class QuestionAndAnswerContext : DbContext
    {
        public QuestionAndAnswerContext(string specialConnectionString)
            : base(specialConnectionString)
        { }
    }
}
