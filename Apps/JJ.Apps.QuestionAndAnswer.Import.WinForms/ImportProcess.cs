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

            foreach (ImportConfigurationImporter importerConfiguration in configSection.Importers)
            {
                using (IContext context = ContextHelper.CreateContextFromConfiguration())
                {
                    string inputFilePath = importerConfiguration.InputFilePath;

                    string sourceIdentifier = importerConfiguration.SourceIdentifier;
                    string sourceUrl = importerConfiguration.SourceUrl;
                    string sourceDescription = importerConfiguration.SourceDescription;
                    string categoryIdentifier = importerConfiguration.CategoryIdentifier;

                    Type modelType = Type.GetType(importerConfiguration.ModelType);
                    if (modelType == null)
                    {
                        throw new Exception( String.Format("modelType '{0}' not found.", importerConfiguration.ModelType));
                    }

                    Type converterType = Type.GetType(importerConfiguration.ConverterType);
                    if (converterType == null)
                    {
                        throw new Exception(String.Format("converterType '{0}' not found.", importerConfiguration.ConverterType));
                    }

                    Type selectorType = Type.GetType(importerConfiguration.SelectorType);
                    if (selectorType == null)
                    {
                        throw new Exception(String.Format("selectorType '{0}' not found.", importerConfiguration.SelectorType));
                    }

                    IQuestionRepository questionRepository = new QuestionRepository(context, context.Location);
                    IAnswerRepository answerRepository = new AnswerRepository(context);
                    ICategoryRepository categoryRepository = new CategoryRepository(context);
                    IQuestionCategoryRepository questionCategoryRepository = new QuestionCategoryRepository(context);
                    IQuestionLinkRepository questionLinkRepository = new QuestionLinkRepository(context);
                    IQuestionTypeRepository questionTypeRepository = new QuestionTypeRepository(context);
                    ISourceRepository sourceRepository = new SourceRepository(context);
                    IQuestionFlagRepository questionFlagRepository = new QuestionFlagRepository(context);

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
            using (IContext context = ContextHelper.CreateContextFromConfiguration())
            {
                ICategoryRepository categoryRepository = new CategoryRepository(context);

                var categoryDescriptionCorrector = new CategoryDescriptionCorrector(categoryRepository);
                categoryDescriptionCorrector.Execute();

                context.Commit();
            }
        }
    }
}
