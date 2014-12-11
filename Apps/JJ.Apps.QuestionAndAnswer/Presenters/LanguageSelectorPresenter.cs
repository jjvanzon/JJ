﻿using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class LanguageSelectorPresenter
    {
        private readonly string[] _availableCultureNames = new string[] { "en-US", "nl-NL" };
        private const string DEFAULT_CULTURE_NAME = "en-US";

        public LanguageSelectorViewModel Show()
        {
            return CreateLanguageSelectionViewModel();
        }

        public LanguageSelectorViewModel SetLanguage(string cultureName)
        {
            SetCulture(cultureName);

            return CreateLanguageSelectionViewModel();
        }

        private void SetCulture(string cultureName)
        {
            CultureInfo culture = CultureInfo.GetCultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        private LanguageSelectorViewModel CreateLanguageSelectionViewModel()
        {
            var viewModel = new LanguageSelectorViewModel();
            
            // Fill culture list
            viewModel.Languages = new List<LanguageViewModel>();

            foreach (string cultureName in _availableCultureNames)
            {
                CultureInfo cultureInfo = CultureInfo.GetCultureInfo(cultureName);

                var language = new LanguageViewModel
                {
                    CultureName = cultureName,
                    Name = cultureInfo.NativeName
                };

                viewModel.Languages.Add(language);
            }

            // Set selected culture
            string currentCultureName = CultureInfo.CurrentUICulture.Name;
            if (_availableCultureNames.Contains(currentCultureName))
            {
                viewModel.SelectedLanguageCultureName = currentCultureName;
            }
            else
            {
                viewModel.SelectedLanguageCultureName = DEFAULT_CULTURE_NAME;
            }

            return viewModel;
        }
    }
}
