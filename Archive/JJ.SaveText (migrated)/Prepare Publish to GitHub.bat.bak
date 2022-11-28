prompt @
echo off
cls

chdir "C:\Repositories"

rem /s: Removes all directories and files in the specified directory in addition to the directory itself.  Used to remove a directory tree.
rem /q: Quiet mode, do not ask if ok to remove a directory tree with /s

echo DELETE FOLDERS
rd /s /q "JJ.SaveText Public\Business"
rd /s /q "JJ.SaveText Public\Data"
rd /s /q "JJ.SaveText Public\Database"
rd /s /q "JJ.SaveText Public\Demos"
rd /s /q "JJ.SaveText Public\Presentation"
rd /s /q "JJ.SaveText Public\Releases"
rd /s /q "JJ.SaveText Public\images"

rem /q: Quiet mode, do not ask if ok to delete on global wildcard
rem /f: Force deleting of read-only files.

echo DELETE FILES IN ROOT
del /q /f "JJ.SaveText Public\*.sln"
del /q /f "JJ.SaveText Public\*.txt"
del /q /f "JJ.SaveText Public\*.md"

rem /s: Copies directories and subdirectories except empty ones.
rem /v: Verifies the size of each new file.
rem /r: Overwrites read-only files.
rem /q: Does not display file names while copying
rem Slash at the end of dest is needed, otherwise it asks if it is a directory or file.
rem Slash should not be put at the end of source, because then it gives an error message.

echo COPY FOLDERS
xcopy /s /v /r /q "JJ\Demos\CollectionConversion" "JJ.SaveText Public\Demos\CollectionConversion\"
xcopy /s /v /r /q "JJ\Demos\IndexersProposal" "JJ.SaveText Public\Demos\IndexersProposal\"
xcopy /s /v /r /q "JJ\Demos\JavaScript" "JJ.SaveText Public\Demos\JavaScript\"
xcopy /s /v /r /q "JJ\Demos\Misc" "JJ.SaveText Public\Demos\Misc\"
xcopy /s /v /r /q "JJ\Demos\TryUnityWww" "JJ.SaveText Public\Demos\TryUnityWww\"
xcopy /v /r /q "JJ\Demos\2013-01-11 Recursion.cs" "JJ.SaveText Public\Demos"
mkdir "JJ.SaveText Public\Business\"
xcopy /s /v /r /q "JJ\Business\Canonical" "JJ.SaveText Public\Business\Canonical\"
xcopy /s /v /r /q "JJ\Business\SaveText" "JJ.SaveText Public\Business\SaveText\"
mkdir "JJ.SaveText Public\Data\"
xcopy /s /v /r /q "JJ\Data\Canonical" "JJ.SaveText Public\Data\Canonical\"
xcopy /s /v /r /q "JJ\Data\SaveText" "JJ.SaveText Public\Data\SaveText\"
xcopy /s /v /r /q "JJ\Data\SaveText.DefaultRepositories" "JJ.SaveText Public\Data\SaveText.DefaultRepositories\"
xcopy /s /v /r /q "JJ\Data\SaveText.EntityFramework" "JJ.SaveText Public\Data\SaveText.EntityFramework\"
xcopy /s /v /r /q "JJ\Data\SaveText.Memory" "JJ.SaveText Public\Data\SaveText.Memory\"
xcopy /s /v /r /q "JJ\Data\SaveText.NHibernate" "JJ.SaveText Public\Data\SaveText.NHibernate\"
xcopy /s /v /r /q "JJ\Data\SaveText.Xml" "JJ.SaveText Public\Data\SaveText.Xml\"
xcopy /s /v /r /q "JJ\Data\SaveText.Xml.Linq" "JJ.SaveText Public\Data\SaveText.Xml.Linq\"
mkdir "JJ.SaveText Public\Database\"
xcopy /s /v /r /q "JJ\Database\SaveTextDB" "JJ.SaveText Public\Database\SaveTextDB\"
mkdir "JJ.SaveText Public\Presentation\"
xcopy /s /v /r /q "JJ\Presentation\SaveText" "JJ.SaveText Public\Presentation\SaveText\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Android" "JJ.SaveText Public\Presentation\SaveText.Android\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.AppService" "JJ.SaveText Public\Presentation\SaveText.AppService\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.AppService.Client" "JJ.SaveText Public\Presentation\SaveText.AppService.Client\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.AppService.Interface" "JJ.SaveText Public\Presentation\SaveText.AppService.Interface\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.AppService.Tests" "JJ.SaveText Public\Presentation\SaveText.AppService.Tests\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.AppService.Tests.SoapUI" "JJ.SaveText Public\Presentation\SaveText.AppService.Tests.SoapUI\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Html" "JJ.SaveText Public\Presentation\SaveText.Html\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Interface" "JJ.SaveText Public\Presentation\SaveText.Interface\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Mvc" "JJ.SaveText Public\Presentation\SaveText.Mvc\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Unity.DotNet.Offline" "JJ.SaveText Public\Presentation\SaveText.Unity.DotNet.Offline\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Unity.DotNet.OfflineWithSync" "JJ.SaveText Public\Presentation\SaveText.Unity.DotNet.OfflineWithSync\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Unity.DotNet.Online" "JJ.SaveText Public\Presentation\SaveText.Unity.DotNet.Online\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Unity.DotNet.Online.WwwObject" "JJ.SaveText Public\Presentation\SaveText.Unity.DotNet.Online.WwwObject\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Unity.Offline" "JJ.SaveText Public\Presentation\SaveText.Unity.Offline\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Unity.OfflineWithSync" "JJ.SaveText Public\Presentation\SaveText.Unity.OfflineWithSync\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Unity.Online" "JJ.SaveText Public\Presentation\SaveText.Unity.Online\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.Unity.Online.WwwObject" "JJ.SaveText Public\Presentation\SaveText.Unity.Online.WwwObject\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.WinForms.Offline" "JJ.SaveText Public\Presentation\SaveText.WinForms.Offline\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.WinForms.OfflineWithSync" "JJ.SaveText Public\Presentation\SaveText.WinForms.OfflineWithSync\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.WinForms.Online" "JJ.SaveText Public\Presentation\SaveText.WinForms.Online\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.WinForms.Online.CustomSoapClient" "JJ.SaveText Public\Presentation\SaveText.WinForms.Online.CustomSoapClient\"
xcopy /s /v /r /q "JJ\Presentation\SaveText.WinForms.OnlineOfflineSwitched" "JJ.SaveText Public\Presentation\SaveText.WinForms.OnlineOfflineSwitched\"
xcopy /s /v /r /q "JJ\Releases\2021-05-18 0.9-save-text" "JJ.SaveText Public\Releases\2021-05-18 0.9-save-text\"

echo COPY LOOSE FILES
xcopy /v /r /q "JJ\JJ.Demos.sln" "JJ.SaveText Public"
xcopy /v /r /q "JJ\JJ.SaveText.sln" "JJ.SaveText Public"
xcopy /v /r /q "JJ\LICENSE.TXT" "JJ.SaveText Public"
copy "JJ\README for JJ.SaveText.MD" "JJ.SaveText Public\README.MD"
mkdir "JJ.SaveText Public\images\"
xcopy /v /r /q "JJ\images\save-text-1.jpg" "JJ.SaveText Public\images"
xcopy /v /r /q "JJ\images\save-text-2.jpg" "JJ.SaveText Public\images"

echo WARNING: CANONICAL DATA / BUSINESS IS PUBLISHED.
echo CONSIDER IF IT CONTAINS DATA MODELING YOU DO NOT WANT TO OPEN SOURCE.

pause
prompt