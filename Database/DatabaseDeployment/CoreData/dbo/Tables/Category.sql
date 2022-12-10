CREATE TABLE [dbo].[Category]
(
	[Id] [int] IDENTITY(10,1) NOT NULL ,
	[CategoryName] NVARCHAR(500) NOT NULL
	CONSTRAINT [Pk_Category_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
    [ParentId] INT NOT NULL 
) ON [PRIMARY]
GO

/*Start adding description*/
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary Key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Category Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name='CategoryName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Parent Category' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name='ParentId'
GO
/*End adding description*/