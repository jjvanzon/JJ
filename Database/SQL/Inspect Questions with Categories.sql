-- Kentico questions
select q.*, c.Description as Category
from Question q
inner join QuestionCategory qc on qc.QuestionID = q.ID
inner join Category c on c.ID = qc.CategoryID
left join Category parentCategory on parentCategory.ID = c.ParentCategoryID
where c.Identifier = 'Kentico' or parentCategory.Identifier = 'Kentico'

-- Kentico Categories
select c.*
from Category c
left join Category parentCategory on parentCategory.ID = c.ParentCategoryID
where c.Identifier = 'Kentico' or parentCategory.Identifier = 'Kentico'

-- QuestionCategories
select
	qc.ID as QuestionCategory_ID,
	q.Text as Question_Text,
	c.Identifier as Category_Identifier,
	c.ID as Category_ID
from QuestionCategory qc
inner join Question q on q.ID = qc.QuestionID
inner join Category c on c.ID = qc.CategoryID
left join Category parentCategory on parentCategory.ID = c.ParentCategoryID
--where c.Identifier = 'Kentico' or parentCategory.Identifier = 'Kentico'
order by q.ID desc

-- No category
select *
from Question q
where not exists (select * from QuestionCategory qc where qc.QuestionID = q.ID)

-- For Updating QuestionCategories
select *
from QuestionCategory 
where ID in (
101309,
101307,
101313,
101306,
101311,
101310,
101305,
101304
)
