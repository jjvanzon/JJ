QuestionAndAnswer 1.0 Release
=============================

*2021-05-09 JJ van Zon*

Database
--------

- The database backup may be in the file "PROD_QuestionAndAnswerDB.zip" 
- The file might be unzipped and restored with name PROD_QuestionAndAnswerDB.
- The database user name "prod" with password "prod" may be expected.
- If needed, different database name / user name / password might be configured. The web.config in the web site files might be used for that.

Web Site Files
--------------

- The folder "PROD_JJ.Presentation.QuestionAndAnswer.Mvc" may contain the web site files.
- It is suggested to create an IIS web site that connects to (a copy of) that folder.
