CREATE TABLE [dbo].[WF_Configurations] (
    [Id]            INT           IDENTITY (-2147483648, 1) NOT NULL,
    [Hostname]      VARCHAR (255) NOT NULL,
    [Service]       VARCHAR (255) NOT NULL,
    [Domain]        VARCHAR (255) NOT NULL,
    [Configuration] VARCHAR (MAX) NOT NULL,
    [Active]        BIT           NOT NULL,
    CONSTRAINT [PK_WF_Configurations] PRIMARY KEY CLUSTERED ([Id] ASC)
);

