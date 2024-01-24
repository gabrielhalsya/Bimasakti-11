--get property 
EXEC RSP_GS_GET_PROPERTY_LIST 'rcd','ghc'

--get unit type ctg
EXEC RSP_GS_GET_UNIT_TYPE_CTG_LIST 'rcd','ASHMD','ghc'

--get pricing list
EXEC RSP_LM_GET_PRICING_LIST 'rcd', 'ASHMD', '1ROOM', '02', 0, '01', '', '', 'ghc'

--get pricing date list
EXEC RSP_LM_GET_PRICING_DATE_LIST 'rcd','ASHMD','1ROOM','02','02','ghc'

--get next pricing list
EXEC RSP_LM_GET_PRICING_LIST 'rcd', 'ASHMD', '1ROOM', '02', 0, '02', '20240123', '20240123001', 'ghc'

-- save pricing 
EXEC RSP_LM_MAINTAIN_PRICING 'Login Company Id', 'Property Id', '02', '1ROOM', 'Overwrite', 'ADD', 'ghc'

--get pricing rate date list
EXEC RSP_LM_GET_PRICNG_RATE_DATE_LIST 'rcd','ASHMD','02','ghc'

--get pricing rate list 
EXEC RSP_LM_GET_PRICING_RATE_LIST 'rcd','ASHMD','02','','ghc'

--save pricing rate
EXEC RSP_LM_MAINTAIN_PRICING_RATE 'rcd','ASHMD','02','ADD','ghc'


