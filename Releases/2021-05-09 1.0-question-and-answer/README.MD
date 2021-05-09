QuestionAndAnswer 1.0 Release
=============================

*2021-05-09 JJ van Zon*

Database
--------

- The database backup may be in the file "QuestionAndAnswerDB.zip" 
- The file might be unzipped and restored in SQL Server with name PROD_QuestionAndAnswerDB.
- The database user name "prod" may be expected with password "prod".
- If needed, different database name / user name / password might be configured.
- The web.config in the web site files might be changed for that.

Web Site Files
--------------

- The folder "JJ.Presentation.QuestionAndAnswer.Mvc" may contain the web site files.
- It is suggested to create an IIS web site that connects to (a copy of) that folder.

Import Tool
-----------

- This import tool might not need to run.
- It may have filled the database with relatively static content once.
- It is not entirely sure, whether it is safe to run it again, just like that.