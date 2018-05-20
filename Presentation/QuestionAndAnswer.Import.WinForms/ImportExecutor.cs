using System;
using JJ.Business.QuestionAndAnswer.Import;
using JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Configuration;
using JJ.Framework.Data;
using JJ.Framework.Exceptions.Aggregates;
using JJ.Presentation.QuestionAndAnswer.Import.Configuration;

namespace JJ.Presentation.QuestionAndAnswer.Import.WinForms
{
	internal static class ImportExecutor
	{
		public static void RunAllImportsFromConfiguration(Action<string> progressCallback = null, Func<bool> isCancelledCallback = null)
		{
			var configSection = CustomConfigurationManager.GetSection<ImportConfiguration>();

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
						throw new NotFoundException(() => importerConfig.ModelType);
					}

					Type converterType = Type.GetType(importerConfig.ConverterType);
					if (converterType == null)
					{
						throw new NotFoundException(() => importerConfig.ConverterType);
					}

					Type selectorType = Type.GetType(importerConfig.SelectorType);
					if (selectorType == null)
					{
						throw new NotFoundException(() => importerConfig.SelectorType);
					}

					var questionRepository = PersistenceHelper.CreateRepository<IQuestionRepository>(context);
					var answerRepository = PersistenceHelper.CreateRepository<IAnswerRepository>(context);
					var categoryRepository = PersistenceHelper.CreateRepository<ICategoryRepository>(context);
					var questionCategoryRepository = PersistenceHelper.CreateRepository<IQuestionCategoryRepository>(context);
					var questionLinkRepository = PersistenceHelper.CreateRepository<IQuestionLinkRepository>(context);
					var questionTypeRepository = PersistenceHelper.CreateRepository<IQuestionTypeRepository>(context);
					var sourceRepository = PersistenceHelper.CreateRepository<ISourceRepository>(context);
					var questionFlagRepository = PersistenceHelper.CreateRepository<IQuestionFlagRepository>(context);

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

					var importer = (IImporter)Activator.CreateInstance(
						importerType2,
						questionRepository,
						answerRepository,
						categoryRepository,
						questionCategoryRepository,
						questionLinkRepository,
						questionTypeRepository,
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
				var categoryRepository = PersistenceHelper.CreateRepository<ICategoryRepository>(context);

				var categoryDescriptionCorrector = new CategoryDescriptionCorrector(categoryRepository);
				categoryDescriptionCorrector.Execute();

				context.Commit();
			}
		}
	}
}