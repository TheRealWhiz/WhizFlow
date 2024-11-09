CREATE TABLE [dbo].[WF_Tasks] (
    [fk_TaskContentId] INT          NOT NULL,
    [TaskQueue]        VARCHAR (50) NOT NULL,
    [Signature]        VARCHAR (50) NOT NULL,
    [Status]           INT          NOT NULL,
    CONSTRAINT [PK_WF_Tasks] PRIMARY KEY CLUSTERED ([fk_TaskContentId] ASC, [TaskQueue] ASC)
);

