using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace JJ.Models.QuestionAndAnswer.Persistence.NHibernate
{
    internal static class ColumnNames
    {
        public const string CategoryID = "CategoryID";
        public const string SourceID = "SourceID";
        public const string ParentCategoryID = "ParentCategoryID";
        public const string QuestionCategoryID = "QuestionCategoryID";
        public const string QuestionID = "QuestionID";
        public const string QuestionTypeID = "QuestionTypeID";
    }
}
