using System.Collections.Generic;
using JJ.Data.QuestionAndAnswer;

namespace JJ.Business.QuestionAndAnswer.Helpers
{
    public class EntityStatusManager
    {
        // TODO: Maybe the internal state can be simplified.
        private readonly HashSet<object> _newEntities = new HashSet<object>();
        private readonly HashSet<Question> _questionsWithDirtyQuestionCategoriesList = new HashSet<Question>();
        private readonly HashSet<Question> _questionsWithDirtyQuestionLinksList = new HashSet<Question>();
        private readonly HashSet<Question> _questionsWithDirtyQuestionFlagsList = new HashSet<Question>();
        private readonly HashSet<Question> _questionsWithDirtyIsActive = new HashSet<Question>();
        private readonly HashSet<Question> _questionsWithDirtyText = new HashSet<Question>();
        private readonly HashSet<Question> _questionsWithDirtyQuestionType = new HashSet<Question>();
        private readonly HashSet<Question> _questionsWithDirtySource = new HashSet<Question>();
        private readonly HashSet<Answer> _answerWithDirtyText = new HashSet<Answer>();
        private readonly HashSet<QuestionCategory> _questionCategoriesWithDirtyCategory = new HashSet<QuestionCategory>();
        private readonly HashSet<QuestionLink> _questionLinksWithDirtyUrl = new HashSet<QuestionLink>();
        private readonly HashSet<QuestionLink> _questionLinksWithDirtyDescription = new HashSet<QuestionLink>();
        private readonly HashSet<QuestionFlag> _questionFlagsWithDirtyFlagStatus = new HashSet<QuestionFlag>();

        public void SetIsNew(Question entity) => _newEntities.Add(entity);
        public void SetIsNew(Answer entity) => _newEntities.Add(entity);
        public void SetIsNew(QuestionCategory entity) => _newEntities.Add(entity);
        public void SetIsNew(QuestionLink entity) => _newEntities.Add(entity);
        public void SetIsNew(QuestionFlag entity) => _newEntities.Add(entity);
        public void SetQuestionCategoriesListIsDirty(Question entity) => _questionsWithDirtyQuestionCategoriesList.Add(entity);
        public void SetQuestionLinksListIsDirty(Question entity) => _questionsWithDirtyQuestionLinksList.Add(entity);
        public void SetQuestionFlagsListIsDirty(Question entity) => _questionsWithDirtyQuestionFlagsList.Add(entity);
        public void SetIsActiveIsDirty(Question entity) => _questionsWithDirtyIsActive.Add(entity);
        public void SetTextIsDirty(Question entity) => _questionsWithDirtyText.Add(entity);
        public void SetQuestionTypeIsDirty(Question entity) => _questionsWithDirtyQuestionType.Add(entity);
        public void SetSourceIsDirty(Question entity) => _questionsWithDirtySource.Add(entity);
        public void SetTextIsDirty(Answer entity) => _answerWithDirtyText.Add(entity);
        public void SetCategoryIsDirty(QuestionCategory entity) => _questionCategoriesWithDirtyCategory.Add(entity);
        public void SetUrlIsDirty(QuestionLink entity) => _questionLinksWithDirtyUrl.Add(entity);
        public void SetDescriptionIsDirty(QuestionLink entity) => _questionLinksWithDirtyDescription.Add(entity);
        public void SetFlagStatusIsDirty(QuestionFlag entity) => _questionFlagsWithDirtyFlagStatus.Add(entity);

        public bool IsNew(Answer entity) => _newEntities.Contains(entity);
        public bool IsNew(Question entity) => _newEntities.Contains(entity);
        public bool IsNew(QuestionCategory entity) => _newEntities.Contains(entity);
        public bool IsNew(QuestionLink entity) => _newEntities.Contains(entity);
        public bool IsNew(QuestionFlag entity) => _newEntities.Contains(entity);
        public bool QuestionCategoriesListIsDirty(Question entity) => _questionsWithDirtyQuestionCategoriesList.Contains(entity);
        public bool QuestionLinksListIsDirty(Question entity) => _questionsWithDirtyQuestionLinksList.Contains(entity);
        public bool QuestionFlagsListIsDirty(Question entity) => _questionsWithDirtyQuestionFlagsList.Contains(entity);
        public bool IsActiveIsDirty(Question entity) => _questionsWithDirtyIsActive.Contains(entity);
        public bool TextIsDirty(Question entity) => _questionsWithDirtyText.Contains(entity);
        public bool QuestionTypeIsDirty(Question entity) => _questionsWithDirtyQuestionType.Contains(entity);
        public bool SourceIsDirty(Question entity) => _questionsWithDirtySource.Contains(entity);
        public bool TextIsDirty(Answer entity) => _answerWithDirtyText.Contains(entity);
        public bool CategoryIsDirty(QuestionCategory entity) => _questionCategoriesWithDirtyCategory.Contains(entity);
        public bool UrlIsDirty(QuestionLink entity) => _questionLinksWithDirtyUrl.Contains(entity);
        public bool DescriptionIsDirty(QuestionLink entity) => _questionLinksWithDirtyDescription.Contains(entity);
        public bool FlagStatusIsDirty(QuestionFlag entity) => _questionFlagsWithDirtyFlagStatus.Contains(entity);
    }
}