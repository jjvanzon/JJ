using JJ.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3SelectorIndex
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
                    throw new InvalidEnumValueException(importType);
            }
        }
    }
}
