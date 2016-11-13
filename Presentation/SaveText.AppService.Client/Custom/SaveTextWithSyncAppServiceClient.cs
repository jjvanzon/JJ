﻿using JJ.Presentation.SaveText.AppService.Interface;
using JJ.Presentation.SaveText.Interface.PresenterInterfaces;
using JJ.Presentation.SaveText.Interface.ViewModels;
using JJ.Framework.Soap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace JJ.Presentation.SaveText.AppService.Client.Custom
{
    public class SaveTextWithSyncAppServiceClient : CustomWcfSoapClient<ISaveTextWithSyncAppService>, ISaveTextWithSyncPresenter
    {
        private string _cultureName;

        public SaveTextWithSyncAppServiceClient(string url, string cultureName)
            : base(url)
        {
            _cultureName = cultureName;
        }

        /// <param name="sendMessageDelegate">
        /// You can handle the sending of the SOAP message and the receiving of the response yourself
        /// by passing this sendMessageDelegate. This is for environments that do not support HttpWebRequest.
        /// First parameter of the delegate is SOAP action, second parameter is SOAP message as an XML string,
        /// return value should be text received.
        /// </param>
        public SaveTextWithSyncAppServiceClient(string url, string cultureName, Func<string, string, string> sendMessageDelegate)
            : base(url, sendMessageDelegate)
        {
            _cultureName = cultureName;
        }

        public SaveTextViewModel Show()
        {
            return Invoke(x => x.Show(_cultureName));
        }

        public SaveTextViewModel Save(SaveTextViewModel viewModel)
        {
            return Invoke(x => x.Save(viewModel, _cultureName));
        }

        public SaveTextViewModel Synchronize(SaveTextViewModel viewModel)
        {
            return Invoke(x => x.Synchronize(viewModel, _cultureName));
        }
    }
}