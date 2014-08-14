using JJ.Apps.QuestionAndAnswer.Import.Configuration;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Import;
using JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3;
using JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Converters;
using JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Models;
using JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Selectors;
using JJ.Framework.Configuration;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.Repositories;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.Import.WinForms
{
    internal static class ImportProcess
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
                        throw new Exception( String.Format("modelType '{0}' not found.", importerConfig.ModelType));
                    }

                    Type converterType = Type.GetType(importerConfig.ConverterType);
                    if (converterType == null)
                    {
                        throw new Exception(String.Format("converterType '{0}' not found.", importerConfig.ConverterType));
                    }

                    Type selectorType = Type.GetType(importerConfig.SelectorType);
                    if (selectorType == null)
                    {
                        throw new Exception(String.Format("selectorType '{0}' not found.", importerConfig.SelectorType));
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
