﻿--get property 
EXEC RSP_GS_GET_PROPERTY_LIST 'rcd','ghc'

--get unit type ctg
EXEC RSP_GS_GET_UNIT_TYPE_CTG_LIST  'rcd','ASHMD','ghc'

--get all pricing list
EXEC RSP_LM_GET_PRICING_LIST 'rcd', 'ASHMD', '1ROOM', '02', 0, '01', '', '', 'ghc'

--get pricing date list
EXEC RSP_LM_GET_PRICING_DATE_LIST 'RCD','ASHMD','STUDIO','02','02','ghc'

--get next pricing list
EXEC RSP_LM_GET_PRICING_LIST 'rcd', 'ASHMD', '1ROOM', '02', 0, '02', '20240122', '20240122001', 'ghc'

-- save pricing 
BEGIN TRANSACTION
CREATE TABLE #LEASE_PRICING(
	  ISEQ INT
	, CVALID_INTERNAL_ID	VARCHAR(36)
	, CCHARGES_TYPE			VARCHAR(2)
	, CCHARGES_ID			VARCHAR(20)
	, CPRICE_MODE			VARCHAR(2)
	, NNORMAL_PRICE			NUMERIC(18,2)
	, NBOTTOM_PRICE			NUMERIC(18,2)
	, LOVERWRITE			BIT	
	)
INSERT INTO #LEASE_PRICING
VALUES 
	(1, '', '02', '02', '01', 500000, 450000, 1)
,	(2, '', '02', 'c000','01', 1500000, 1200000, 1)
EXEC RSP_LM_MAINTAIN_PRICING 'RCD', 'ASHMD', '02', '1ROOM', '20240123', 'ADD', 'GHC'
EXEC RSP_LM_GET_PRICING_LIST 'rcd', 'ASHMD', '1ROOM', '02', 0, '02', '20240123', '20240123001', 'ghc'
ROLLBACK

--get unit type ctg
EXEC RSP_GS_GET_UNIT_TYPE_CTG_LIST 'rcd','ASHMD','ghc'

--using list category type above
EXEC RSP_LM_GET_PRICING_DATE_LIST 'rcd','ASHMD','1ROOM','02','03','ghc'

--get history pricing list
EXEC RSP_LM_GET_PRICING_LIST 'rcd', 'ASHMD', '1ROOM', '02', 0, '03', '20240122', '20240122001', 'ghc'

--get pricing rate date list
EXEC RSP_LM_GET_PRICNG_RATE_DATE_LIST 'rcd','ASHMD','02','ghc'

--get pricing rate list 
EXEC RSP_LM_GET_PRICING_RATE_LIST 'rcd','ASHMD','1ROOM','02','20240122','ghc'

--save pricing rate
BEGIN TRANSACTION
CREATE TABLE #LEASE_PRICING_RATE(CCURRENCY_CODE	CHAR(3)
				, NBASE_RATE_AMOUNT	NUMERIC(18,2)
				, NCURRENCY_RATE_AMOUNT	NUMERIC(18,2)
				)
INSERT INTO #LEASE_PRICING_RATE 
VALUES ('SGD',11745,11745)
EXEC RSP_LM_MAINTAIN_PRICING_RATE 'rcd','ASHMD','02','20240122','ADD','ghc'
ROLLBACK

EXEC RSP_LM_GET_PRICING_RATE_LIST 'rcd','ASHMD','1ROOM','02','20240122','ghc'




