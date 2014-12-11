using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace JJ.Apps.QuestionAndAnswer.AppService
{
    [ServiceContract]
    public interface IResourceService
    {
        [OperationContract]
        Messages GetMessages(string cultureName);

        [OperationContract]
        Titles GetTitles(string cultureName);

        [OperationContract]
        PropertyDisplayNames GetPropertyDisplayNames(string cultureName);

        [OperationContract]
        CommonTitles GetCommonTitles(string cultureName);
    }
}
