QuestionAndAnswer Release 1.1
=============================

*2021-05-10 JJ van Zon*

`[Preliminary documentation]`

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

- This import tool might not need to run again.
- It may once have filled the database with relatively static content.
- Running it again, may not be entirely without consequence.
- The impact of rerunning it, might not be that high though.
- *Manual edits* to questions might be overwritten by an import.
- *Content flaggings* might be removed for questions, that might be reimported.
- Using its config, imports may be selected to run or not by commenting ones out.
- It may also allow you to select the file path of a possible file to import.