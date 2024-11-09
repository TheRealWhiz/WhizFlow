CREATE PROCEDURE [dbo].[WF_Configurations_Read]
	@Hostname varchar(255),
	@Service varchar(255)
AS
BEGIN
	SET NOCOUNT ON;

	select * from [dbo].[WF_Configurations] where [Service] = @Service and Hostname = @Hostname-- and [Active] = 1

END
