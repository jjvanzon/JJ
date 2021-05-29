using JJ.Presentation.QuestionAndAnswer.ViewModels.Partials;

namespace JJ.Presentation.QuestionAndAnswer.ViewModels
{
    public sealed class LoginViewModel
    {
        public LanguageSelectorPartialViewModel LanguageSelector { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string SecurityToken { get; set; }

        public bool IsAuthenticated { get; set; }
    }
}
