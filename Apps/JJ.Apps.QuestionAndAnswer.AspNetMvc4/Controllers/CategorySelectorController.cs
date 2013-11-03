using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JJ.Framework.Presentation.AspNetMvc4;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers.Helpers;
using JJ.Apps.QuestionAndAnswer.AspNetMvc4.Views;

namespace JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers
{
    public class CategorySelectorController : Controller
    {
        // GET: /CategorySelector/

        public ViewResult Index()
        {
            using (var presenter = new CategorySelectorPresenter())
            {
                CategorySelectorViewModel viewModel = presenter.Show();
                return View(viewModel);
            }
        }

        // POST: /CategorySelector/Add/5

        [HttpPost]
        public ViewResult Add(int categoryID, CategorySelectorViewModel viewModel)
        {
            using (var presenter = new CategorySelectorPresenter())
            {
                CategorySelectorViewModel viewModel2 = presenter.Add(viewModel, categoryID);
                return View(ViewNames.Index, viewModel2);
            }
        }

        // POST: /CategorySelector/AddCategory/5

        [HttpPost]
        public ViewResult Remove(int categoryID, CategorySelectorViewModel viewModel)
        {
            using (var presenter = new CategorySelectorPresenter())
            {
                CategorySelectorViewModel viewModel2 = presenter.Remove(viewModel, categoryID);
                return View(ViewNames.Index, viewModel2);
            }
        }
    }
}
