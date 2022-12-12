CREATE TABLE [dbo].[ArticleContent]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[ArticleId] UNIQUEIDENTIFIER UNIQUE NOT NULL,
	[Content] [nvarchar](MAX) NULL,
	CONSTRAINT [Pk_ArticleContent_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ArticleContent]  ADD  CONSTRAINT [Fk_ArticleContent_ArticleId] FOREIGN KEY([ArticleId])
REFERENCES [dbo].[Article] ([Id])
GO

ALTER TABLE [dbo].[ArticleContent] CHECK CONSTRAINT [Fk_ArticleContent_ArticleId]
GO

/*Start adding description*/
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ArticleId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArticleContent', @level2type=N'COLUMN',@level2name=N'ArticleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary Key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArticleContent', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Article Content' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArticleContent', @level2type=N'COLUMN',@level2name='Content'
GO
/*End adding description*/