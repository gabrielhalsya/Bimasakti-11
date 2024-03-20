EXEC RSP_GS_GET_TRANS_CODE_INFO 'RCD','000000' 

IF (OBJECT_ID('tempdb..#__SP_ERR_Table') is null) BEGIN
	select SP_Name=cast('' as varchar(50)), Err_Code=cast('' as varchar(20)), Err_Detail=cast('' as nvarchar(max)) into #__SP_ERR_Table where 0=1
end else begin
	truncate table #__SP_ERR_TABLE
end

begin try
	-- Jalankan script SP disini
	DROP TABLE IF EXISTS #GLT0100_JOURNAL_DETAIL;
	CREATE TABLE #GLT0100_JOURNAL_DETAIL (
    CGLACCOUNT_NO VARCHAR(20),
    CCENTER_CODE VARCHAR(10),
    CDBCR CHAR(1),
    NAMOUNT NUMERIC(19,2),
    CDETAIL_DESC NVARCHAR(200),
    CDOCUMENT_NO VARCHAR(20),
    CDOCUMENT_DATE VARCHAR(8)
	);
	INSERT INTO #GLT0100_JOURNAL_DETAIL VALUES
	('14.00.0000','12345678','D','100','bug devtest desc2','bug devtest2','20240320'),
	('14.10.1000','12345678','C','50','bug devtest desc2','bug devtest2','20240320'),
	('15.00.0000','12345678','C','50','bug devtest desc2','bug devtest2','20240320');

	EXEC RSP_GL_SAVE_JOURNAL 
	'ghc', --CUSER_ID
	'4F1913FE-0CE5-4D4A-A5A5-F565ADD43090',--CJRN_ID
	'EDIT',--CACTION
	'RCD',--CCOMPANY_ID
	'FIN',--CDEPT_CODE
	'000000',--CTRANS_CODE
	'GJV/FIN240010',--CREF_NO
	'bug devtest2',--CDOC_NO
	'20240320',--CDOC_DATE
	'20240317',--CREF_DATE
	'',--CREVERSE_DATE
	0,--LREVERSE_DATE
	'bug devtest desc2', --CTRANS_DESC
	'IDR',--CCURRENCY_CODE
	1.000000,--NLBASE_RATE
	1.000000,--NLCURRENCY_RATE
	1.000000,--NBBASERATE
	1.000000,--NBCURRENCY_RATE
	100.000000;--NPRELIST_AMOUNT
end try
begin catch
	select * from #__SP_ERR_TABLE --untuk tahu error code yg di raise
end catch

EXEC RSP_GL_GET_JOURNAL 'RCD','ghc','4F1913FE-0CE5-4D4A-A5A5-F565ADD43090','en'
EXEC RSP_GL_GET_JOURNAL_DETAIL_LIST '4F1913FE-0CE5-4D4A-A5A5-F565ADD43090','en'