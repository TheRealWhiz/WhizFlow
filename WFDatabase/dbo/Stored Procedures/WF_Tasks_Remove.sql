CREATE PROCEDURE [dbo].[WF_Tasks_Remove]
	@Queue as varchar(50),
	@RowCount as int
AS
BEGIN
	SET NOCOUNT ON;

	set rowcount @RowCount

	delete from [dbo].[WF_Tasks] with (rowlock) where [TaskQueue] = @Queue and [Status] = 1

	set rowcount 0

END

GO
GRANT VIEW DEFINITION
    ON OBJECT::[dbo].[WF_Tasks_Remove] TO [WF_SPViewDefinition]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[WF_Tasks_Remove] TO [WF_SPExecute]
    AS [dbo];

