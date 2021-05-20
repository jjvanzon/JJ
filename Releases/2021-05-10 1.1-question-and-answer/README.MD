QuestionAndAnswer Release 1.1
=============================

*2021-05-10*


Changes
-------

The Import Tool was added, which may now be fixed not to crash for import source W3CSpecCss21_UserInterface_LooseDefinitions. Then an attempt was made to add that import to the accompanying database. Installation instructions might be found in the README as wel a description of some imperfections of the import tool.


Prerequisites
-------------

- *Microsoft SQL Server* may be required, which may be downloaded elsewhere.
- IIS (Internet Information Services) might be needed, which may be installed by activating it as a *Windows component* on some operating systems.


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

### General Installation Steps

- "JJ.Presentation.QuestionAndAnswer.Import.WinForms.1.1.zip" might contain an import tool.
- It has no installer.
- It might be unzipped and put somewhere on a computer and then perhaps a shortcut made to it.

### Description of Import Tool

- The import tool may once have filled the database with relatively static content.
- Questions with answers and other content.
- This import tool might not need to run again.
- Using its config, imports may be selected to run or not.
- The config may also allow you to select the file path of a possible file to import.

### Instabilities

- There may be instabilities in the Import tool.
- When something might be wrong with its config file, the program may disappear from screen but a process might be still hanging in the backgrond which might then be killed.
- The following error may have slipped into the config file of the import tool.
    - An excessive "-->" may be present in the config file.
    - This may be resolved as follows:
    - Opening "JJ.Presentation.QuestionAndAnswer.Import.WinForms.exe.config".
    - Finding "categoryPath="Css3\Properties\UserInterface" />-->"
    - Replacing it with: "categoryPath="Css3\Properties\UserInterface" />"
- The program may crash if the import file paths would be incorrectly entered in the config file.
- Running it again, may not be entirely without consequence.
    - Unintended impact of rerunning it, might be subtle.
    - *Manual edits* to questions might be overwritten by an import.
    - *Content flaggings* might be removed for re-imported questions.


IO Files
--------

- These were the files that may originally have been imported into the database using the Import Tool.
- These files might be copied somewhere for reference.
- Perhaps in a sub-folder "IO Files" in the installation folder of the Import Tool.
- If reimporting is desired, configuration of the import tool may be used to select which imports might run.
- And let the config entries point to the desired file location of an IO file.