﻿using JJ.Presentation.SaveText.AppService.Interface;
using JJ.Presentation.SaveText.AppService.Interface.Models;
using JJ.Framework.Soap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace JJ.Presentation.SaveText.AppService.Client.Wcf
{
    public class ResourceAppServiceClient : ClientBase<IResourceAppService>, IResourceAppService
    {
        public ResourceAppServiceClient(string url)
            : base(new BasicHttpBinding(), new EndpointAddress(url))
        { }

        public Messages GetMessages(string cultureName)
        {
            Messages messages = Channel.GetMessages(cultureName);
            return messages;
        }

        public Labels GetLabels(string cultureName)
        {
            Labels labels = Channel.GetLabels(cultureName);
            return labels;
        }

        public Titles GetTitles(string cultureName)
        {
            Titles titles = Channel.GetTitles(cultureName);
            return titles;
        }
    }
}