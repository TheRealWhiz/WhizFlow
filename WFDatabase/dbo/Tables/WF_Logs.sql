CREATE TABLE [dbo].[WF_Logs] (
    [Id]                    BIGINT        IDENTITY (-2147483648, 1) NOT NULL,
    [HostName]              VARCHAR (255) NOT NULL,
    [Service]               VARCHAR (255) NOT NULL,
    [Domain]                VARCHAR (255) NOT NULL,
    [ModuleId]              INT           NOT NULL,
    [LogTypeId]             INT           NOT NULL,
    [Object]                VARCHAR (MAX) NOT NULL,
    [fk_TaskContentId]      INT           NULL,
    [Message]               VARCHAR (MAX) NOT NULL,
    [AdditionalInformation] VARCHAR (MAX) NOT NULL,
    [Time]                  DATETIME      CONSTRAINT [DF_WF_Logs_Time] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_WF_Logs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

