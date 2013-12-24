﻿using JJ.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces
{
    public class UserRepository : IUserRepository
    {
        private IContext _context;

        public UserRepository(IContext context)
        {
            if (context == null) { throw new ArgumentNullException("context"); }

            _context = context;
        }

        public User TryGetByUserName(string userName)
        {
            return _context.Query<User>().Where(x => x.UserName == userName).SingleOrDefault();
        }

        public User GetByUserName(string userName)
        {
            User user = TryGetByUserName(userName);

            if (user == null)
            {
                throw new Exception(String.Format("User with UserName '{0}' not found.", userName));
            }

            return user;
        }
    }
}
