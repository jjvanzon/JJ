QuestionAndAnswer Release 1.0
=============================

*2021-05-09*

Prerequisites
-------------

- *Microsoft SQL Server* may be required which may be downloaded elsewhere.
- IIS (Internet Information Services) might be needed, which may be installed by activating it as a *Windows component* on some operating systems.

Database
--------

- A database backup may be in the file "QuestionAndAnswerDB.1.0.zip" 
- The file might be unzipped and restored in SQL Server with name PROD_QuestionAndAnswerDB.
- The database user name "prod" may be expected with password "prod".
- If needed, different database name / user name / password might be configured.
- The web.config in the web site files might be changed for that.

Web Site Files
--------------

- "JJ.Presentation.QuestionAndAnswer.Mvc.1.0.zip" may contain the web site files.
- It is suggested to create an IIS web site that connects to (a copy of) that folder.