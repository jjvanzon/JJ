﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Canonical = JJ.Data.Canonical;
using JJ.Presentation.SaveText.Interface.ViewModels;
using JJ.Framework.Data;
using JJ.Presentation.SaveText.Presenters;
using JJ.Data.SaveText.DefaultRepositories.RepositoryInterfaces;
using JJ.Presentation.SaveText.Interface.PresenterInterfaces;
using JJ.Presentation.SaveText.AppService.Interface;
using System.Threading;
using System.Globalization;
using JJ.Presentation.SaveText.AppService.Helpers;

namespace JJ.Presentation.SaveText.AppService
{
    public class SaveTextAppService : ISaveTextAppService
    {
        public SaveTextViewModel Show(string cultureName)
        {
            ServiceHelper.SetCulture(cultureName);

            using (IContext context = PersistenceHelper.CreateContext())
            {
                IEntityRepository entityRepository = PersistenceHelper.CreateRepository<IEntityRepository>(context);
                var presenter = new SaveTextPresenter(entityRepository);
                var viewModel = presenter.Show();
                return viewModel;
            }
        }

        public SaveTextViewModel Save(SaveTextViewModel viewModel, string cultureName)
        {
            ServiceHelper.SetCulture(cultureName);

            using (IContext context = PersistenceHelper.CreateContext())
            {
                IEntityRepository entityRepository = PersistenceHelper.CreateRepository<IEntityRepository>(context);
                var presenter = new SaveTextPresenter(entityRepository);
                var viewModel2 = presenter.Save(viewModel);
                return viewModel2;
            }
        }
    }
}