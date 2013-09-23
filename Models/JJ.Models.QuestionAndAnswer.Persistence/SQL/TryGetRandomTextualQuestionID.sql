-- This is rather horrible, and maybe not completely random, but let's go with it for now.
-- TODO: Test of randomization even works a little bit.
select top 1 ID
from TextualQuestion
tablesample (1000 rows)
order by NEWID();