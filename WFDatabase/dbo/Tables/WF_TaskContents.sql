CREATE TABLE [dbo].[WF_TaskContents] (
    [Id]         INT           IDENTITY (-2147483648, 1) NOT NULL,
    [Timestamp]  DATETIME      NOT NULL,
    [Content]    VARCHAR (MAX) NOT NULL,
    [Serialized] BIT           NOT NULL,
    [Disk]       BIT           NOT NULL,
    CONSTRAINT [PK_WF_TaskContents] PRIMARY KEY CLUSTERED ([Id] ASC)
);

