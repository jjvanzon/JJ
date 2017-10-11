using JJ.Presentation.QuestionAndAnswer.Import.Configuration;
using JJ.Business.QuestionAndAnswer.Import;
using JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3;
using JJ.Framework.Configuration;
using JJ.Framework.Data;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System;

namespace JJ.Presentation.QuestionAndAnswer.Import.WinForms
{
    internal static class ImportExecutor
    {
        public static void RunAllImportsFromConfiguration(Action<string> progressCallback = null, Func<bool> isCancelledCallback = null)
        {
            ImportConfiguration configSection = CustomConfigurationManager.GetSection<ImportConfiguration>();

            foreach (ImportConfigurationImporter importerConfig in configSection.Importers)
            {
                using (IContext context = PersistenceHelper.CreateContext())
                {
                    string inputFilePath = importerConfig.InputFilePath;

                    string sourceIdentifier = importerConfig.SourceIdentifier;
                    string sourceUrl = importerConfig.SourceUrl;
                    string sourceDescription = importerConfig.SourceDescription;
                    string categoryIdentifier = importerConfig.CategoryIdentifier;

                    Type modelType = Type.GetType(importerConfig.ModelType);
                    if (modelType == null)
                    {
                        throw new Exception($"modelType '{importerConfig.ModelType}' not found.");
                    }

                    Type converterType = Type.GetType(importerConfig.ConverterType);
                    if (converterType == null)
                    {
                        throw new Exception($"converterType '{importerConfig.ConverterType}' not found.");
                    }

                    Type selectorType = Type.GetType(importerConfig.SelectorType);
                    if (selectorType == null)
                    {
                        throw new Exception($"selectorType '{importerConfig.SelectorType}' not found.");
                    }

                    IQuestionRepository questionRepository = PersistenceHelper.CreateRepository<IQuestionRepository>(context);
                    IAnswerRepository answerRepository = PersistenceHelper.CreateRepository<IAnswerRepository>(context);
                    ICategoryRepository categoryRepository = PersistenceHelper.CreateRepository<ICategoryRepository>(context);
                    IQuestionCategoryRepository questionCategoryRepository = PersistenceHelper.CreateRepository<IQuestionCategoryRepository>(context);
                    IQuestionLinkRepository questionLinkRepository = PersistenceHelper.CreateRepository<IQuestionLinkRepository>(context);
                    IQuestionTypeRepository questionTypeRepository = PersistenceHelper.CreateRepository<IQuestionTypeRepository>(context);
                    ISourceRepository sourceRepository = PersistenceHelper.CreateRepository<ISourceRepository>(context);
                    IQuestionFlagRepository questionFlagRepository = PersistenceHelper.CreateRepository<IQuestionFlagRepository>(context);

                    Source source = sourceRepository.TryGetByIdentifier(sourceIdentifier);
                    if (source == null)
                    {
                        source = sourceRepository.Create();
                        source.Identifier = sourceIdentifier;
                    }
                    source.Description = sourceDescription;
                    source.Url = sourceUrl;

                    Type importerType = typeof(Importer<,,>);
                    Type importerType2 = importerType.MakeGenericType(modelType, selectorType, converterType);

                    IImporter importer = (IImporter)Activator.CreateInstance(
                        importerType2,
                        questionRepository,
                        answerRepository,
                        categoryRepository,
                        questionCategoryRepository,
                        questionLinkRepository,
                        questionTypeRepository,
                        sourceRepository,
                        questionFlagRepository,
                        source,
                        categoryIdentifier);

                    importer.Execute(inputFilePath, progressCallback, isCancelledCallback);

                    if (isCancelledCallback != null)
                    {
                        if (isCancelledCallback())
                        {
                            return;
                        }
                    }

                    context.Commit();
                }
            }

            // Correct category descriptions
            using (IContext context = PersistenceHelper.CreateContext())
            {
                ICategoryRepository categoryRepository = PersistenceHelper.CreateRepository<ICategoryRepository>(context);

                var categoryDescriptionCorrector = new CategoryDescriptionCorrector(categoryRepository);
                categoryDescriptionCorrector.Execute();

                context.Commit();
            }
        }
    }
}
