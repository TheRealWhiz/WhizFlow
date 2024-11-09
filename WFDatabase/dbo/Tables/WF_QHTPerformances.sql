CREATE TABLE [dbo].[WF_QHTPerformances] (
    [Id]               INT           IDENTITY (-2147483648, 1) NOT NULL,
    [fk_TaskContentId] INT           NOT NULL,
    [HostName]         VARCHAR (255) NOT NULL,
    [Service]          VARCHAR (255) NOT NULL,
    [Domain]           VARCHAR (255) NOT NULL,
    [Milliseconds]     INT           NOT NULL,
    [Signature]        VARCHAR (50)  NOT NULL,
    [TaskQueue]        VARCHAR (50)  NOT NULL,
    [TimeStamp]        DATETIME      NOT NULL,
    CONSTRAINT [PK_WF_Performances] PRIMARY KEY CLUSTERED ([Id] ASC)
);

