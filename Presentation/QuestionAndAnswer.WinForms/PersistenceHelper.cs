﻿using JJ.Framework.Data;

namespace JJ.Presentation.QuestionAndAnswer.WinForms
{
	internal static class PersistenceHelper
	{
		public static IContext CreateContext()
		{
			return ContextFactory.CreateContextFromConfiguration();
		}

		public static TRepositoryInterface CreateRepository<TRepositoryInterface>(IContext context)
		{
			return RepositoryFactory.CreateRepositoryFromConfiguration<TRepositoryInterface>(context);
		}
	}
}