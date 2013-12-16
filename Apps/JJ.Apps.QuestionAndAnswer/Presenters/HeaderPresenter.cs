using JJ.Apps.QuestionAndAnswer.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class HeaderPresenter
    {
        public HeaderViewModel Show()
        {
            return CreateHeaderViewModel();
        }

        public HeaderViewModel SetLanguage(string cultureName)
        {
            CultureInfo culture = CultureInfo.GetCultureInfo(cultureName);
            SetCulture(culture);

            HeaderViewModel viewModel = CreateHeaderViewModel();
            viewModel.Language.SelectedLanguageCultureName = culture.Name;
            return viewModel;
        }

        private void SetCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        private HeaderViewModel CreateHeaderViewModel()
        {
            return new HeaderViewModel
            {
                Language = CreateSelectLanguageViewModel(),
                Login = new SmallLoginViewModel { IsLoggedIn = false }
            };
        }

        private SelectLanguageViewModel CreateSelectLanguageViewModel()
        {
            return new SelectLanguageViewModel
            {
                Languages = CreateLanguagesViewModel()
            };
        }

        private IList<LanguageViewModel> CreateLanguagesViewModel()
        {
            var list = new List<LanguageViewModel>();

            var languageNotSpecified = new LanguageViewModel
            {
                Name = null,
                CultureName = null
            };
            list.Add(languageNotSpecified);

            foreach (string cultureName in _availableCultureNames)
            {
                CultureInfo cultureInfo = CultureInfo.GetCultureInfo(cultureName);

                var language = new LanguageViewModel
                {
                    CultureName = cultureName,
                    Name = cultureInfo.NativeName
                };
                list.Add(language);
            }

            return list;
        }

        private readonly string[] _availableCultureNames = new string[] { "en-US", "nl-NL" };
    }
}
