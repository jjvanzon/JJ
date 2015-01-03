using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NerdDinner.Repositories;
using NerdDinner.Models;
using NerdDinner.Views;
using NerdDinner.Resources;
using NerdDinner.Validation;
using NerdDinner.Helpers;
using NerdDinner.ViewModels;
using NerdDinner.ViewModels.Extensions;

namespace NerdDinner.Controllers
{
    public class DinnersController : Controller
    {
        private NerdDinnerDataContext _dataContext;
        private DinnerRepository _dinnerRepository;
        private CountryRepository _countryRepository;

        public DinnersController()
        {
            _dataContext = new NerdDinnerDataContext();
            _dinnerRepository = new DinnerRepository(_dataContext);
            _countryRepository = new CountryRepository(_dataContext);
        }

        // GET: /Dinners/

        public ActionResult Index()
        {
            List<Dinner> dinners = _dinnerRepository.GetUpcomingDinners().ToList();

            return View(dinners.ToViewModel());
        }

        // GET: /Dinners/Details/2

        public ActionResult Details(int id)
        {
            Dinner dinner = _dinnerRepository.TryGetDinner(id);

            if (dinner == null)
            {
                return View(ViewNames.NotFound);
            }
            else
            {
                return View(dinner.ToViewModel());
            }
        }

        // GET: /Dinners/Create

        public ActionResult Create()
        {
            IEnumerable<Country> countries = _countryRepository.GetAllCountries();

            var dinnerFormViewModel = new DinnerFormViewModel(countries);

            dinnerFormViewModel.Dinner.EventDate = DateTime.Now.AddDays(7);

            return View(dinnerFormViewModel);
        }

        // POST: /Dinners/Create

        private const string DEFAULT_HOSTED_BY = "SomeUser";

        [HttpPost]
        public ActionResult Create(DinnerFormViewModel dinnerFormViewModel)
        {
            return Save(dinnerFormViewModel);
        }

        // GET: /Dinners/Edit/2
        // POST: /Dinners/Edit/2

        public ActionResult Edit(int id)
        {
            Dinner dinner = _dinnerRepository.GetDinner(id);
            IEnumerable<Country> countries = _countryRepository.GetAllCountries();

            var viewModel = new DinnerFormViewModel(dinner, countries);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(DinnerFormViewModel dinnerFormViewModel)
        {
            return Save(dinnerFormViewModel);
        }

        private ActionResult Save(DinnerFormViewModel dinnerFormViewModel)
        {
            // Prevent validation errors.
            // TODO: Properly implement filling in HostedBy.
            dinnerFormViewModel.Dinner.HostedBy = DEFAULT_HOSTED_BY;

            Dinner dinner = dinnerFormViewModel.Dinner.ToModel(_dataContext);

            if (ModelState.IsValid && dinner.IsValid)
            {
                _dinnerRepository.Save();

                return RedirectToAction(ActionNames.Details, new { id = dinner.DinnerID });
            }
            else
            {
                // Model not valid and not updated.
                ModelState.AddRuleViolations(dinner.GetRuleViolations());

                // This is missing the countries.
                //return View(dinnerFormViewModel);
                // This is horrible.
                var viewModel = new DinnerFormViewModel(dinner, _countryRepository.GetAllCountries());
                return View(viewModel);
            }
        }

        // GET: /Dinners/Delete/1
        // POST: /Dinners/Delete/1

        public ActionResult Delete(int id)
        {
            switch (Request.HttpMethod)
            {
                case HttpHelper.HttpMethodGet:
                    return DeleteGet(id);

                case HttpHelper.HttpMethodPost:
                    return DeletePost(id);
            }

            throw new NotSupportedException(String.Format("HttpMethod '{0}' not supported.", Request.HttpMethod));
        }

        private ActionResult DeleteGet(int id)
        {
            Dinner dinner = _dinnerRepository.GetDinner(id);

            if (dinner == null)
            {
                return View(ViewNames.NotFound);
            }
            else
            {
                return View(dinner);
            }
        }

        private ActionResult DeletePost(int id)
        {
            Dinner dinner = _dinnerRepository.TryGetDinner(id);

            if (dinner == null)
            {
                return View(ViewNames.NotFound);
            }

            _dinnerRepository.Delete(dinner);
            _dinnerRepository.Save();

            return View(ViewNames.Deleted);
        }
    }
}