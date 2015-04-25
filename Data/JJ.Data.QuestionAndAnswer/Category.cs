using System;
using System.Collections.Generic;

namespace JJ.Data.QuestionAndAnswer
{
    public class Category
    {
        private int _iD;
        private string _identifier;
        private string _description;
        private bool _isActive;
        private Category _parentCategory;
        private IList<Category> _subCategories = new List<Category>();
        private IList<QuestionCategory> _categoryQuestions = new List<QuestionCategory>();
        private IList<RunCategory> _categoryRuns = new List<RunCategory>();

        public virtual int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public virtual string Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        public virtual bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }
        
        public virtual Category ParentCategory
        {
            get { return _parentCategory; }
            set { _parentCategory = value; }
        }

        public virtual IList<Category> SubCategories
        {
            get { return _subCategories; }
            set { _subCategories = value; }
        }

        public virtual IList<QuestionCategory> CategoryQuestions
        {
            get { return _categoryQuestions; }
            set { _categoryQuestions = value; }
        }

        public virtual IList<RunCategory> CategoryRuns
        {
            get { return _categoryRuns; }
            set { _categoryRuns = value; }
        }
    }
}
