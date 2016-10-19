using System;
namespace JJ.Models.QuestionAndAnswer
{

    public class Category
    {

        private System.Int32 _id;
        private System.Collections.IList _subCategories;
        private Category _parentCategory;
        private System.Collections.IList _categoryQuestions;
        private System.String _description;
        private System.String _identifier;
        private System.Boolean _isActive;
        private System.Collections.IList _categoryRuns;

        public virtual System.Int32 Id
        {
            get
            {
                return _id;
            }
        }

        public virtual System.Collections.IList SubCategories
        {
            get
            {
                return _subCategories;
            }
            set
            {
                _subCategories = value;
            }
        }

        public virtual Category ParentCategory
        {
            get
            {
                return _parentCategory;
            }
            set
            {
                _parentCategory = value;
            }
        }

        public virtual System.Collections.IList CategoryQuestions
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

        public virtual System.String Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public virtual System.String Identifier
        {
            get
            {
                return _identifier;
            }
            set
            {
                _identifier = value;
            }
        }

        public virtual System.Boolean IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
            }
        }

        public virtual System.Collections.IList CategoryRuns
        {
            get
            {
                return _categoryRuns;
            }
            set
            {
                _categoryRuns = value;
            }
        }







    }
}
