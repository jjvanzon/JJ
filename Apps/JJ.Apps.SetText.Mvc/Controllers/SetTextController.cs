﻿using JJ.Apps.SetText.Mvc.Names;
using JJ.Apps.SetText.Mvc.Helpers;
using JJ.Apps.SetText.Presenters;
using JJ.Apps.SetText.Interface.ViewModels;
using JJ.Framework.Persistence;
using JJ.Models.Canonical;
using JJ.Models.SetText.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JJ.Apps.SetText.Mvc.Controllers
{
    public class SetTextController : Controller
    {
        public ActionResult Index()
        {
            SetTextViewModel viewModel;
            if (TempData.ContainsKey(TempDataKeys.ViewModel))
            {
                viewModel = (SetTextViewModel)TempData[TempDataKeys.ViewModel];
            }
            else
            {
                using (IContext ormContext = PersistenceHelper.CreateContext())
                {
                    SetTextPresenter presenter = CreatePresenter(ormContext);
                    viewModel = presenter.Show();
                }
            }

            foreach (ValidationMessage validationMessage in viewModel.ValidationMessages)
            {
                ModelState.AddModelError(validationMessage.PropertyKey, validationMessage.Text);
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(SetTextViewModel viewModel)
        {
            using (IContext ormContext = PersistenceHelper.CreateContext())
            {
                SetTextPresenter presenter = CreatePresenter(ormContext);
                SetTextViewModel viewModel2 = presenter.Save(viewModel);
                TempData[TempDataKeys.ViewModel] = viewModel2;
                return RedirectToAction(ActionNames.Index);
            }
        }

        private SetTextPresenter CreatePresenter(IContext context)
        {
            IEntityRepository entityRepository = PersistenceHelper.CreateRepository<IEntityRepository>(context);
            SetTextPresenter presenter = new SetTextPresenter(entityRepository);
            return presenter;
        }
    }
}