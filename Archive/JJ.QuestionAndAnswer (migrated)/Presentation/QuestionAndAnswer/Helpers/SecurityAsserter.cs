using System;
using System.Security.Authentication;
using JetBrains.Annotations;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;

// ReSharper disable MemberCanBeMadeStatic.Global

namespace JJ.Presentation.QuestionAndAnswer.Helpers
{
    internal class SecurityAsserter
    {
        private readonly IUserRepository _userRepository;

        public SecurityAsserter(IUserRepository userRepository)
            => _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

        [AssertionMethod]
        public void Assert(string authenticatedUserName)
        {
            if (string.IsNullOrEmpty(authenticatedUserName))
            {
                throw new AuthenticationException();
            }

            User user = _userRepository.TryGetByUserName(authenticatedUserName);

            if (user == null)
            {
                throw new AuthenticationException();
            }
        }
    }
}