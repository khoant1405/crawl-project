CREATE TABLE [dbo].[Article]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[ArticleName] [nvarchar](500) NOT NULL,
	[Status] [nvarchar](255) NOT NULL,
	[CreationDate] [datetime2](7) NULL,
	[CreationBy] UNIQUEIDENTIFIER NULL, 
    [LastSaveDate] DATETIME2 NULL, 
    [RefURL] NVARCHAR(255) NOT NULL, 
    [ImageThumb] NVARCHAR(255) NULL, 
    [Description] NVARCHAR(500) NULL, 
    [CategoryId] INT NOT NULL, 
    [IdDisplay] INT NOT NULL, 
    CONSTRAINT [Pk_Article_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Article]  ADD  CONSTRAINT [Fk_Article_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
GO

ALTER TABLE [dbo].[Article] CHECK CONSTRAINT [Fk_Article_CategoryId]
GO

/****** Object:  Index [Nidx_Article_Status] ******/
CREATE NONCLUSTERED INDEX [Nidx_Article_ArticleName] ON [dbo].[Article]
(
	[ArticleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/*Start adding description*/
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User create article' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Article', @level2type=N'COLUMN',@level2name=N'CreationBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The datetime when article record is inserted' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Article', @level2type=N'COLUMN',@level2name=N'CreationDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Article Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Article', @level2type=N'COLUMN',@level2name=N'ArticleName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary Key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Article', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Article Status : EDIT, PUBLISH, DELETE...' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Article', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The datetime when article is save' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Article', @level2type=N'COLUMN',@level2name=N'LastSaveDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'RefURL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Article', @level2type=N'COLUMN',@level2name=N'RefURL'
GO

GO
/*End adding description*/