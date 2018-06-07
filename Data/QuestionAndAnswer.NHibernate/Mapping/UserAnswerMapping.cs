using FluentNHibernate.Mapping;
using JetBrains.Annotations;
using JJ.Data.QuestionAndAnswer.NHibernate.Names;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Mapping
{
    [UsedImplicitly]
	public class UserAnswerMapping : ClassMap<UserAnswer>
	{
		public UserAnswerMapping()
		{
			Id(x => x.ID);

			Map(x => x.DateTime);

			References(x => x.Question, ColumnNames.QuestionID);
			References(x => x.AnswerStatus, ColumnNames.AnswerStatusID);
			References(x => x.User, ColumnNames.UserID);
			References(x => x.Run, ColumnNames.RunID);
		}
	}
}
