CREATE TABLE [dbo].[TextualQuestionCategory](
	[ID] [int] NOT NULL,
	[CategoryID] [int] NOT NULL,
	[TextualQuestionID] [int] NOT NULL,
 CONSTRAINT [PK_TextualQuestionCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[TextualQuestionCategory]  WITH CHECK ADD  CONSTRAINT [FK_TextualQuestionCategory_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([ID])

ALTER TABLE [dbo].[TextualQuestionCategory] CHECK CONSTRAINT [FK_TextualQuestionCategory_Category]

ALTER TABLE [dbo].[TextualQuestionCategory]  WITH CHECK ADD  CONSTRAINT [FK_TextualQuestionCategory_TextualQuestion] FOREIGN KEY([TextualQuestionID])
REFERENCES [dbo].[TextualQuestion] ([ID])

ALTER TABLE [dbo].[TextualQuestionCategory] CHECK CONSTRAINT [FK_TextualQuestionCategory_TextualQuestion]

CREATE NONCLUSTERED INDEX [IX_TextualQuestionCategory_CategoryID] ON [dbo].[TextualQuestionCategory] 
(
	[CategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_TextualQuestionCategory_TextualQuestionID] ON [dbo].[TextualQuestionCategory] 
(
	[TextualQuestionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
