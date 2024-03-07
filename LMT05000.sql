﻿EXEC RSP_GS_GET_PROPERTY_LIST 'rcd','ghc'
SELECT CCODE, CDESCRIPTION FROM RFT_GET_GSB_CODE_INFO ('BIMASAKTI', 'rcd', '_BS_UNIT_CHARGES_TYPE', '', 'en')
EXEC RSP_GS_GET_PERIOD_DT_LIST 'rcd', '2024'

EXEC RSP_LM_GET_DISCOUNT_LIST 
'Login Company Id'
, 'Selected Property Id'
, 'Selected Charge Type'
, 'Selected Charge'
, 'Selected Discount'
, 'Selected "Discount Type'
, 'Selected "Invoice Period'
, 'All Building Flag'
, 'Selected Building'
, 'Selected Agreement Type'
,'Login User Id'



CREATE TABLE #LEASE_PRICING
(	
	CCOMPANY_ID		VARCHAR(8)
,	CPROPERTY_ID		VARCHAR(20)
,	CREF_NO			VARCHAR(30)
,	CSEQ_NO			VARCHAR(30)
,	CFLOOR_ID		VARCHAR(30)
,	CUNIT_ID		VARCHAR(30)
,	CTENANT_ID		VARCHAR(30)
,	NCHARGES_AMOUNT		NUMERIC(18,2)
,	NCHARGE_DISCOUNT		NUMERIC(18,2)
,	NNET_CHARGE		VARCHAR(30)
,	CAGREEMENT_NO		VARCHAR(30)
,	CSTART_DATE		VARCHAR(30)
,	CEND_DATE		VARCHAR(30)
,	CEXISTING_DISCOUNT_CODE	VARCHAR(30)
,	NEXISTING_CHARGE_DISCOUNT	VARCHAR(30)
)

EXEC RSP_LM_PROCESS_AGREEMENT_CHARGE_DISCOUNT 
 'CCOMPANY_ID'			
,'CPROPERTY_ID'
, 'CREF_NO		'
, 'CREF_DATE			'
, 'CCHARGES_TYPE		'
, 'CCHARGES_ID			'
, 'CDISCOUNT_CODE		'
, 'CINV_PERIOD_YEAR		'
, 'CINV_PERIOD_MONTH	'
, true
, 'CBUILDING_ID			'
, 'CAGREEMENT_TYPE		'
, 'PROCESS'		
, 'CUSER_ID'
DROP TABLE #LEASE_PRICING