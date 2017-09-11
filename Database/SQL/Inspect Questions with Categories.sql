-- Kentico questions
select qc.ID as QuestionCategory_ID, q.*, c.Description as Category
from Question q
inner join QuestionCategory qc on qc.QuestionID = q.ID
inner join Category c on c.ID = qc.CategoryID
left join Category parentCategory on parentCategory.ID = c.ParentCategoryID
left join Category grandParentCategory on grandParentCategory.ID = parentCategory.ParentCategoryID
where c.Identifier = 'Kentico' or parentCategory.Identifier = 'Kentico' or grandParentCategory.Identifier = 'Kentico' 

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

-- For inserting categories
select ID, ParentCategoryID, Identifier, Description, IsActive
from Category
where ParentCategoryID = 6622 /*Kentico*/

-- For Inserting QuestionCategories
/*
declare @categoryID int = 6628;
insert into QuestionCategory (QuestionID, CategoryID) values 
(78009, @categoryID),
(78010, @categoryID),
(78011, @categoryID),
(78012, @categoryID)
*/