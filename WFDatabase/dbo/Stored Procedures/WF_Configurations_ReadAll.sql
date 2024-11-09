CREATE PROCEDURE [dbo].[WF_Configurations_ReadAll]
AS
BEGIN
	SET NOCOUNT ON;

	select * from [dbo].[WF_Configurations]

END