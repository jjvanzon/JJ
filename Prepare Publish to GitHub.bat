prompt @
echo off
cls

chdir "D:\Source\JJs Software"

rem /s: Removes all directories and files in the specified directory in addition to the directory itself.  Used to remove a directory tree.
rem /q: Quiet mode, do not ask if ok to remove a directory tree with /s

echo DELETE CODE FOLDERS
rd /s /q "JJ.SaveText on GitHub\Business"
rd /s /q "JJ.SaveText on GitHub\Data"
rd /s /q "JJ.SaveText on GitHub\Database"
rd /s /q "JJ.SaveText on GitHub\Demos"
rd /s /q "JJ.SaveText on GitHub\Presentation"

rem /q: Quiet mode, do not ask if ok to delete on global wildcard
rem /f: Force deleting of read-only files.

echo DELETE CODE FILES IN ROOT
del /q /f "JJ.SaveText on GitHub\*.sln"
del /q /f "JJ.SaveText on GitHub\*.txt"
del /q /f "JJ.SaveText on GitHub\*.md"

rem /s: Copies directories and subdirectories except empty ones.
rem /v: Verifies the size of each new file.
rem /r: Overwrites read-only files.
rem /q: Does not display file names while copying
rem Slash at the end of dest is needed, otherwise it asks if it is a directory or file.
rem Slash should not be put at the end of source, because then it gives an error message.

echo COPY CODE FOLDERS
xcopy /s /v /r /q "JJ\Demos\CollectionConversion" "JJ.SaveText on GitHub\Demos\CollectionConversion\"
xcopy /s /v /r /q "JJ\Demos\IndexersProposal" "JJ.SaveText on GitHub\Demos\IndexersProposal\"
xcopy /s /v /r /q "JJ\Demos\JavaScript" "JJ.SaveText on GitHub\Demos\JavaScript\"
xcopy /s /v /r /q "JJ\Demos\Misc" "JJ.SaveText on GitHub\Demos\Misc\"
xcopy /s /v /r /q "JJ\Demos\SvnAndTfsToGitMigrationScripts" "JJ.SaveText on GitHub\Demos\SvnAndTfsToGitMigrationScripts\"
xcopy /s /v /r /q "JJ\Demos\Git Scripts to Isolate Project from Larger Repository" "JJ.SaveText on GitHub\Demos\Git Scripts to Isolate Project from Larger Repository\"
xcopy /s /v /r /q "JJ\Demos\TryUnityWww" "JJ.SaveText on GitHub\Demos\TryUnityWww\"
xcopy /v /r /q "JJ\Demos\2013-01-11 Recursion.cs" "JJ.SaveText on GitHub\Demos"
mkdir "JJ.SaveText on GitHub\Business\"
xcopy /s /v /r /q "JJ\Business\Canonical" "JJ.SaveText on GitHub\Business\Canonical\"
xcopy /s /v /r /q "JJ\Business\SaveText" "JJ.SaveText on GitHub\Business\SaveText\"
mkdir "JJ.SaveText on GitHub\Data\"
xcopy /s /v /r /q "JJ\Data\Canonical" "JJ.SaveText on GitHub\Data\Canonical\"
xcopy /s /v /r /q "JJ\Data\SaveText" "JJ.SaveText on GitHub\Data\SaveText\"
xcopy /s /v /r /q "JJ\Data\SaveText.DefaultRepositories" "JJ.SaveText on GitHub\Data\SaveText.DefaultRepositories\"
xcopy /s /v /r /q "JJ\Data\SaveText.EntityFramework" "JJ.SaveText on GitHub\Data\SaveText.EntityFramework\"
xcopy /s /v /r /q "JJ\Data\SaveText.Memory" "JJ.SaveText on GitHub\Data\SaveText.Memory\"
xcopy /s /v /r /q "JJ\Data\SaveText.NHibernate" "JJ.SaveText on GitHub\Data\SaveText.NHibernate\"
xcopy /s /v /r /q "JJ\Data\SaveText.Xml" "JJ.SaveText on GitHub\Data\SaveText.Xml\"
xcopy /s /v /r /q "JJ\Data\SaveText.Xml.Linq" "JJ.SaveText on GitHub\Data\SaveText.Xml.Linq\"
mkdir "JJ.SaveText on GitHub\Database\"
xcopy /s /v /r /q "JJ\Database\SaveTextDB" "JJ.SaveText on GitHub\Database\SaveTextDB\"
mkdir "JJ.SaveText on GitHub\Presentation\"
xcopy /s /v /r /q "JJ\Presentation\SaveText" "JJ.SaveText on GitHub\Presentation\SaveText\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Android" "JJ.SaveText on GitHub\Presentation\SaveText.Android\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.AppService" "JJ.SaveText on GitHub\Presentation\SaveText.AppService\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.AppService.Client" "JJ.SaveText on GitHub\Presentation\SaveText.AppService.Client\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.AppService.Interface" "JJ.SaveText on GitHub\Presentation\SaveText.AppService.Interface\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.AppService.Tests" "JJ.SaveText on GitHub\Presentation\SaveText.AppService.Tests\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.AppService.Tests.SoapUI" "JJ.SaveText on GitHub\Presentation\SaveText.AppService.Tests.SoapUI\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Html" "JJ.SaveText on GitHub\Presentation\SaveText.Html\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Interface" "JJ.SaveText on GitHub\Presentation\SaveText.Interface\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Mvc" "JJ.SaveText on GitHub\Presentation\SaveText.Mvc\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Unity.DotNet.Offline" "JJ.SaveText on GitHub\Presentation\SaveText.Unity.DotNet.Offline\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Unity.DotNet.OfflineWithSync" "JJ.SaveText on GitHub\Presentation\SaveText.Unity.DotNet.OfflineWithSync\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Unity.DotNet.Online" "JJ.SaveText on GitHub\Presentation\SaveText.Unity.DotNet.Online\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Unity.DotNet.Online.WwwObject" "JJ.SaveText on GitHub\Presentation\SaveText.Unity.DotNet.Online.WwwObject\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Unity.Offline" "JJ.SaveText on GitHub\Presentation\SaveText.Unity.Offline\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Unity.OfflineWithSync" "JJ.SaveText on GitHub\Presentation\SaveText.Unity.OfflineWithSync\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Unity.Online" "JJ.SaveText on GitHub\Presentation\SaveText.Unity.Online\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Unity.Online.WwwObject" "JJ.SaveText on GitHub\Presentation\SaveText.Unity.Online.WwwObject\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.WinForms.Offline" "JJ.SaveText on GitHub\Presentation\SaveText.WinForms.Offline\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.WinForms.OfflineWithSync" "JJ.SaveText on GitHub\Presentation\SaveText.WinForms.OfflineWithSync\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.WinForms.Online" "JJ.SaveText on GitHub\Presentation\SaveText.WinForms.Online\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.WinForms.Online.CustomSoapClient" "JJ.SaveText on GitHub\Presentation\SaveText.WinForms.Online.CustomSoapClient\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.WinForms.OnlineOfflineSwitched" "JJ.SaveText on GitHub\Presentation\SaveText.WinForms.OnlineOfflineSwitched\"

echo COPY CODE FILES IN ROOT
xcopy /v /r /q "JJ\JJ.Demos.sln" "JJ.SaveText on GitHub"
xcopy /v /r /q "JJ\JJ.SaveText.sln" "JJ.SaveText on GitHub"
xcopy /v /r /q "JJ\LICENSE.TXT" "JJ.SaveText on GitHub"
copy "JJ\README for JJ.SaveText.MD" "JJ.SaveText on GitHub\README.MD"

echo WARNING: CANONICAL DATA / BUSINESS IS PUBLISHED.
echo CONSIDER IF IT CONTAINS DATA MODELING YOU DO NOT WANT TO OPEN SOURCE.

pause
prompt