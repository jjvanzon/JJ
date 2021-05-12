QuestionAndAnswer Release 1.1
=============================

*2021-05-10*

Changes
-------

The Import Tool was added, which may now be fixed not to crash for import source W3CSpecCss21_UserInterface_LooseDefinitions. Then an attempt was made to add that import to the accompanying database. Some imperfections in the import tool might be commented on in this document.

Database
--------

- The database backup may be in the file "QuestionAndAnswerDB.1.1.zip"
- The file might be unzipped and restored in SQL Server with name PROD_QuestionAndAnswerDB.
- The database user name "prod" may be expected with password "prod".
- If needed, different database name / user name / password might be configured.
- The web.config and app.config in the web site files and/or import tool might be changed for that.

Web Site Files
--------------

- "JJ.Presentation.QuestionAndAnswer.Mvc.1.1.zip" may contain the web site files.
- It is suggested to create an IIS web site whose that connects to (a copy of) that folder (using the 'Physical path' setting).

Import Tool
-----------

- "JJ.Presentation.QuestionAndAnswer.Import.WinForms.1.1.zip" might contain an import tool.
- It has no installer.
- It might be unzipped and put somewhere on a computer and then perhaps a shortcut made to it.
- It may once have filled the database with relatively static content.
- Questions with answers and other content.
- This import tool might not need to run again.
- Running it again, may not be entirely without consequence.
- Unintended impact of rerunning it, might be subtle.
- *Manual edits* to questions might be overwritten by an import.
- *Content flaggings* might be removed for re-imported questions.
- Using its config, imports may be selected to run or not.
- The config may also allow you to select the file path of a possible file to import.

IO Files
--------

- These were the files that may originally have been imported into the database using the Import Tool.
- If reimporting is desired, configuration of the import tool may be used to select which imports might run and let them point to the desired file location of an IO file.