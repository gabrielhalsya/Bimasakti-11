IF (OBJECT_ID('tempdb..#__SP_ERR_Table') is null) BEGIN
	select SP_Name=cast('' as varchar(50)), Err_Code=cast('' as varchar(20)), Err_Detail=cast('' as nvarchar(max)) into #__SP_ERR_Table where 0=1
end else begin
	truncate table #__SP_ERR_TABLE
end

begin try
		--DROP TABLE #GLM00200_JOURNAL_DETAIL
		CREATE TABLE #GLM00200_JOURNAL_DETAIL 
		(
			CGLACCOUNT_NO   VARCHAR(20),
			CCENTER_CODE    VARCHAR(10),
			CDBCR           CHAR(1),
			NAMOUNT         NUMERIC(19,2),
			CDETAIL_DESC    NVARCHAR(200),
			CDOCUMENT_NO    VARCHAR(20),
			CDOCUMENT_DATE  VARCHAR(8)
		)

		INSERT INTO #GLM00200_JOURNAL_DETAIL (CGLACCOUNT_NO, CCENTER_CODE, CDBCR, NAMOUNT, CDETAIL_DESC, CDOCUMENT_NO, CDOCUMENT_DATE)
		VALUES ('0A102', 'C', 'I', 100.00, 'Tes Dev', 'JRNDEVTEST', '20240318'),
				('0A109', 'D', 'I', 100.00, 'Tes Dev', 'JRNDEVTEST', '20240318');

		EXEC RSP_GL_SAVE_RECURRING_JRN 
		'ghc', --CUSER_ID
		'', --CJRN_ID
		'NEW', --CACTION
		'rcd', --CCOMPANY_ID
		'ACC', --CDEPT_CODE
		'000030', --CTRANS_CODE
		'JRNDEVTEST2', --CREF_NO

		'JRNDEVTEST2', --CDOC_NO
		'20240318', --CDOC_DATE
		0, --IFREQUENCY
		0, --IPERIOD

		'20240318', --CSTART_DATE
		'Tes Dev', --CTRANS_DESC
		'IDR', --CCURRENCY_CODE
		0, --LFIX_RATE	
		1, --NLBASE_RATE
		1, --NLCURRENCY_RATE
		1, --NBBASE_RATE
		1, --NBCURRENCY_RATE
		200 --NPRELIST_AMOUNT
end try
begin catch
	select * from #__SP_ERR_TABLE --untuk tahu error code yg di raise
end catch
