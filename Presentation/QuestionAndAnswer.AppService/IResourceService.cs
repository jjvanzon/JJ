using System.ServiceModel;

namespace JJ.Presentation.QuestionAndAnswer.AppService
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
        CommonResources GetCommonResources(string cultureName);
    }
}
