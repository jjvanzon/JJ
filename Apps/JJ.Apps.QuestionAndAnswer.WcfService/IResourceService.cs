using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace JJ.Apps.QuestionAndAnswer.WcfService
{
    [ServiceContract]
    public interface IResourceService
    {
        [OperationContract]
        List<Resource> GetAllLabels(string cultureName);

        [OperationContract]
        List<Resource> GetAllTitles(string cultureName);

        [OperationContract]
        List<Resource> GetLabels(string cultureName, string[] resourceNames);

        [OperationContract]
        List<Resource> GetTitles(string cultureName, string[] resourceNames);
    }
}
