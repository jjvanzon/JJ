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
        List<Resource> GetAllTitles(string cultureName);

        [OperationContract]
        List<Resource> GetTitles(string cultureName, string[] resourceNames);

        [OperationContract]
        List<Resource> GetAllLabels(string cultureName);

        [OperationContract]
        List<Resource> GetLabels(string cultureName, string[] resourceNames);

        [OperationContract]
        List<Resource> GetAllMessages(string cultureName);

        [OperationContract]
        List<Resource> GetMessages(string cultureName, string[] resourceNames);

        /*[OperationContract]
        Messages GetMessages_New(string cultureName);

        [OperationContract]
        Labels GetLabels_New(string cultureName);

        [OperationContract]
        Titles GetTitles_New(string cultureName);*/
    }
}
