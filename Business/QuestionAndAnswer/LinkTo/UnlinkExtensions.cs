using JJ.Data.QuestionAndAnswer;

namespace JJ.Business.QuestionAndAnswer.LinkTo
{
	public static class UnlinkExtensions
	{
		public static void UnlinkSource(this Question question) => question.LinkTo((Source)null);

	    public static void UnlinkQuestionType(this Question question) => question.LinkTo((QuestionType)null);

	    public static void UnlinkQuestion(this QuestionCategory questionCategory) => questionCategory.LinkTo((Question)null);

	    public static void UnlinkCategory(this QuestionCategory questionCategory) => questionCategory.LinkTo((Category)null);

	    public static void UnlinkQuestion(this QuestionFlag questionFlag) => questionFlag.LinkTo((Question)null);

	    public static void UnlinkFlagStatus(this QuestionFlag questionFlag) => questionFlag.LinkTo((FlagStatus)null);

	    public static void UnlinkFlaggedByUser(this QuestionFlag questionFlag) => questionFlag.LinkToFlaggedByUser(null);

	    public static void UnlinkLastModifiedByUser(this QuestionFlag questionFlag) => questionFlag.LinkToLastModifiedByUser(null);

	    public static void UnlinkQuestion(this QuestionLink questionLink) => questionLink.LinkTo(null);

	    public static void UnlinkQuestion(this Answer answer) => answer.LinkTo(null);
	}
}