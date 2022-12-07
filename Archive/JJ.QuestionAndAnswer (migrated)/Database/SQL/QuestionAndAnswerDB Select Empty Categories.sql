select * 
from Category c 
where not exists (select * from QuestionCategory qc where qc.CategoryID = c.ID)
and not exists (select * from Category c2 where c2.ParentCategoryID = c.ID)