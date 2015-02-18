using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Common;
using JJ.Framework.Validation;
using JJ.Persistence.QuestionAndAnswer;
using JJ.Persistence.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Validation;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;
using JJ.Presentation.QuestionAndAnswer.Extensions;
using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Presentation.QuestionAndAnswer.Resources;
using JJ.Framework.Reflection;

namespace JJ.Presentation.QuestionAndAnswer.Presenters
{
    public class QuestionDetailsPresenter
    {
        private Repositories _repositories;
        private string _authenticatedUserName;

        /// <param name="authenticatedUserName">nullable</param>
        public QuestionDetailsPresenter(
            Repositories repositories, 
            string authenticatedUserName)
        {
            if (repositories == null) throw new NullException(() => repositories);

            _repositories = repositories;
            _authenticatedUserName = authenticatedUserName;
        }

        public object Show(int id)
        {
            Question question = _repositories.QuestionRepository.TryGet(id);
            if (question == null)
            {
                var presenter2 = new QuestionNotFoundPresenter(_repositories.UserRepository, _authenticatedUserName);
                return presenter2.Show();
            }

            QuestionDetailsViewModel viewModel = question.ToDetailsViewModel(_repositories.UserRepository, _authenticatedUserName);
            return viewModel;
        }

        public object Edit(int id)
        {
            var editPresenter = new QuestionEditPresenter(_repositories, _authenticatedUserName);
            return editPresenter.Edit(id);
        }

        public object Delete(int id)
        {
            var deletePresenter = new QuestionConfirmDeletePresenter(_repositories, _authenticatedUserName);
            return deletePresenter.Show(id);
        }

        public QuestionListViewModel BackToList()
        {
            var listPresenter = new QuestionListPresenter(_repositories, _authenticatedUserName);
            return listPresenter.Show();
        }
    }
}
