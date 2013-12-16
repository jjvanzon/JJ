using System;
namespace JJ.Models.QuestionAndAnswer
{

    public class sysdiagram
    {

        private System.Int32 _id;
        private System.Byte[] _definition;
        private System.String _name;
        private System.Int32 _principalid;
        private System.Int32 _version;

        public virtual System.Int32 Id
        {
            get
            {
                return _id;
            }
        }

        public virtual System.Byte[] definition
        {
            get
            {
                return _definition;
            }
            set
            {
                _definition = value;
            }
        }

        public virtual System.String name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public virtual System.Int32 principalid
        {
            get
            {
                return _principalid;
            }
            set
            {
                _principalid = value;
            }
        }

        public virtual System.Int32 version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
            }
        }







    }
}
