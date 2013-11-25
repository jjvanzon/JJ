using JJ.Framework.Common;
using JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3PropertyIndex.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3PropertyIndex.Selectors
{
    public static class SelectorFactory
    {
        public static ISelector CreateSelector(ImportTypeEnum importType)
        {
            switch (importType)
            {
                case ImportTypeEnum.Csv:
                    return new CsvSelector();

                case ImportTypeEnum.Html:
                    return new HtmlSelector();

                default:
                    throw new InvalidValueException(importType);
            }
        }
    }
}
