using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3
{
    public class CategoryDescriptionCorrector : CategoryDescriptionCorrectorBase
    {
        public CategoryDescriptionCorrector(ICategoryRepository categoryRepository)
            : base(categoryRepository)
        { }

        public override void Execute()
        {
            CorrectCategoryDescription("Css3", "CSS3");
            CorrectCategoryDescription("BoxModel", "Box Model");

            CorrectCategoryDescription("PossibleValues", "Possible Values");
            CorrectCategoryDescription("InitialValue", "Initial Value");
            CorrectCategoryDescription("AppliesToElements", "Applies to Elements");
            CorrectCategoryDescription("IsInherited", "Inherit");
            CorrectCategoryDescription("ComputedValue", "Computed Value");

            CorrectCategoryDescription("TermToMeaning", "Term to Meaning");
            CorrectCategoryDescription("MeaningToTerm", "Meaning to Term");

            CorrectCategoryDescription("PatternToMeaning", "Pattern to Meaning");
            CorrectCategoryDescription("MeaningToPattern", "Meaning to Pattern");
            CorrectCategoryDescription("SelectorType", "Selector Type");
        }
    }
}
