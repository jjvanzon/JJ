using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace JJ.Apps.QuestionAndAnswer.WcfService
{
    public class ResourceService : IResourceService
    {
        public List<Resource> GetAllLabels(string cultureName)
        {
            return GetResources(Resources.Labels.ResourceManager, cultureName);
        }

        public List<Resource> GetLabels(string cultureName, string[] resourceNames)
        {
            return GetResources(Resources.Labels.ResourceManager, cultureName, resourceNames);
        }

        public List<Resource> GetAllTitles(string cultureName)
        {
            return GetResources(Resources.Titles.ResourceManager, cultureName);
        }

        public List<Resource> GetTitles(string cultureName, string[] resourceNames)
        {
            return GetResources(Resources.Titles.ResourceManager, cultureName, resourceNames);
        }

        public List<Resource> GetAllMessages(string cultureName)
        {
            return GetResources(Resources.Messages.ResourceManager, cultureName);
        }

        public List<Resource> GetMessages(string cultureName, string[] resourceNames)
        {
            return GetResources(Resources.Messages.ResourceManager, cultureName, resourceNames);
        }

        private List<Resource> GetResources(ResourceManager resourceManager, string cultureName, string[] resourceNames)
        {
            var list = new List<Resource>();

            foreach (string name in resourceNames)
            {
                string text = resourceManager.GetString(name, GetCulture(cultureName));
                var resource = new Resource { Name = name, Text = text };

                list.Add(resource);
            }

            return list;
        }

        private List<Resource> GetResources(ResourceManager resourceManager, string cultureName)
        {
            var list = new List<Resource>();

            ResourceSet resourceSet = resourceManager.GetResourceSet(GetCulture(cultureName), true, true);

            foreach (DictionaryEntry x in resourceSet)
            {
                var resource = new Resource { Name = (string)x.Key, Text = (string)x.Value };

                list.Add(resource);
            }

            return list;
        }

        private CultureInfo GetCulture(string cultureName)
        {
            if (String.IsNullOrEmpty(cultureName))
            {
                return CultureInfo.InvariantCulture;
            }

            return CultureInfo.GetCultureInfo(cultureName);
        }

        /*public Messages GetMessages_New(string cultureName)
        {
            SetCulture(cultureName);

            return new Messages
            {
                QuestionNotFound = Resources.Messages.QuestionNotFound
            };
        }

        public Labels GetLabels_New(string cultureName)
        {
            SetCulture(cultureName);
            return new Labels
            {
                Answer = Resources.Labels.Answer,
                TheCorrectAnswer = Resources.Labels.TheCorrectAnswer
            };
        }

        public Titles GetTitles_New(string cultureName)
        {
            SetCulture(cultureName);

            return new Titles
            {
                NextQuestion = Resources.Titles.NextQuestion,
                Question = Resources.Titles.Question,
                QuestionNotFound = Resources.Titles.QuestionNotFound,
                ShowAnswer = Resources.Titles.ShowAnswer
            };
        }

        private void SetCulture(string cultureName)
        {
            CultureInfo cultureInfo = GetCulture(cultureName);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }*/
    }
}
