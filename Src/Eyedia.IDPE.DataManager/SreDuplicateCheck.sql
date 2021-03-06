SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:	Deb'jyoti Das
-- Create date: 09-Nov-2010
-- Description:	Checks duplicate against consumer database.
--		-IDPE sends incoming data to the store procedure with two additional columns; 'Position' and 'IsDuplicate'
--		-Position store record position and should not be modified.
--		-Boolean type 'IsDuplicate' should be marked as 'true' if the record is duplicate.
--		-One can loop through the incoming data and set 'IsDuplicate' to 'true'
--		-Alternatively, a query with exact same columns can be fired against database and returned back to IDPE
--		-In any case, the returned data should be always 
--			-same column name as incoming
--			-order by position
-- =============================================
CREATE PROCEDURE [SreDuplicateCheck]	
	@dataSourceId int, 
	@inputData xml
AS
BEGIN
	
	SET NOCOUNT ON;

	{0}
END
