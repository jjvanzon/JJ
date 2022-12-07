select top (@count) x.ID
from 
(
	select 
		q.ID, 
		ROW_NUMBER() over (order by q.ID) as RowNumber 
	from Question q
) as x
where x.RowNumber >= @firstRowNumberOneBased
order by x.RowNumber;
