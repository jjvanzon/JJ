﻿using JJ.Apps.SetText.AppService.Interface;
using JJ.Apps.SetText.Interface.PresenterInterfaces;
using JJ.Apps.SetText.Interface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace JJ.Apps.SetText.AppService.Client.Wcf
{
    // If ISetTextAppService differs from ISetTextPresenter it will probably only be additional parameters for the service methods.
    // You can pass those to the constructor of the client, expose the presenter interface, and pass the extra parameters to the Channel.

    public class SetTextWithSyncAppServiceClient : ClientBase<ISetTextWithSyncAppService>, ISetTextWithSyncPresenter
    {
        private string _cultureName;

        public SetTextWithSyncAppServiceClient(string url, string cultureName)
            : base(new BasicHttpBinding(), new EndpointAddress(url))
        {
            _cultureName = cultureName;
        }

        public SetTextViewModel Show()
        {
            return Channel.Show(_cultureName);
        }

        public SetTextViewModel Save(SetTextViewModel viewModel)
        {
            return Channel.Save(viewModel, _cultureName);
        }

        public SetTextViewModel Synchronize(SetTextViewModel viewModel)
        {
            return Channel.Synchronize(viewModel, _cultureName);
        }
    }
}
