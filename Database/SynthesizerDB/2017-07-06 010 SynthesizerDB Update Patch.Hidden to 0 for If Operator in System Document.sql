update Patch
set Hidden = 0
where Name = 'If'
and DocumentID = (select ID from Document where Name = 'System');