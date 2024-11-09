CREATE PROCEDURE [dbo].[WF_TaskContent_Read]
	@Id int
AS
BEGIN
	SET NOCOUNT ON;

	select
		[Id],
		[Timestamp],
		[Content],
		[Serialized],
		[Disk]
	from
		[dbo].[WF_TaskContents] with (nolock)
	where
		[Id] = @Id

END

GO
