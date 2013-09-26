using System;
using System.Collections.Generic;

namespace JJ.Models.QuestionAndAnswer
{
    public class Category
    {
        private int _iD;
        private string _name;
        private Category _parentCategory;
        private IList<Category> _subCategories;
        private IList<QuestionCategory> _categoryQuestions;

        public virtual int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual Category ParentCategory
        {
            get { return _parentCategory; }
            set { _parentCategory = value; }
        }

        public virtual IList<Category> SubCategories
        {
            get
            {
                return _subCategories;
            }
            set
            {
                if (_subCategories == null)
                {
                    _subCategories = new List<Category>();
                }

                _subCategories = value;
            }
        }

        public virtual IList<QuestionCategory> CategoryQuestions
        {
            get
            {
                return _categoryQuestions;
            }
            set
            {
                _categoryQuestions = value;
            }
        }
    }
}
