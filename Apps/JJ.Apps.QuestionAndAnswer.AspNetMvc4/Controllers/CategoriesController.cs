using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.Presenters;

namespace JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers
{
    public class CategoriesController : Controller
    {
        // GET: /Categories/Select

        public ViewResult Select()
        {
            using (var presenter = new CategorySelectorPresenter())
            {
                CategorySelectorViewModel viewModel = presenter.Show();
                return View(viewModel);
            }
        }

        // JSON

        public JsonResult AddCategory(int categoryID)
        {
            throw new NotImplementedException();
        }
    }
}