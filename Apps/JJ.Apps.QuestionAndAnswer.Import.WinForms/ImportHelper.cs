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
    internal static class ImportHelper
    {
        public static void RunAllImportsFromConfiguration(Action<string> progressCallback = null, Func<bool> isCancelledCallback = null)
        {
            ImportConfiguration configSection = ConfigurationManager.GetSection<ImportConfiguration>();

            foreach (ImportConfigurationImporter importerConfiguration in configSection.Importers)
            {
                using (IContext context = ContextHelper.CreateContext())
                {
                    string inputFilePath = importerConfiguration.InputFilePath;

                    string sourceIdentifier = importerConfiguration.SourceIdentifier;
                    string sourceUrl = importerConfiguration.SourceUrl;
                    string sourceDescription = importerConfiguration.SourceDescription;

                    Type modelType = Type.GetType(importerConfiguration.ModelType);
                    Type converterType = Type.GetType(importerConfiguration.ConverterType);
                    Type selectorType = Type.GetType(importerConfiguration.SelectorType);

                    IQuestionRepository questionRepository = new QuestionRepository(context, context.Location);
                    IAnswerRepository answerRepository = new AnswerRepository(context);
                    ICategoryRepository categoryRepository = new CategoryRepository(context);
                    IQuestionCategoryRepository questionCategoryRepository = new QuestionCategoryRepository(context);
                    IQuestionLinkRepository questionLinkRepository = new QuestionLinkRepository(context);
                    IQuestionTypeRepository questionTypeRepository = new QuestionTypeRepository(context);
                    ISourceRepository sourceRepository = new SourceRepository(context);

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
                        source);

                    importer.Execute(inputFilePath, progressCallback, isCancelledCallback);

                    context.Commit();
                }
            }

            // Correct category descriptions
            using (IContext context = ContextHelper.CreateContext())
            {
                ICategoryRepository categoryRepository = new CategoryRepository(context);

                var categoryDescriptionCorrector = new CategoryDescriptionCorrector(categoryRepository);
                categoryDescriptionCorrector.Execute();

                context.Commit();
            }
        }
    }
}
