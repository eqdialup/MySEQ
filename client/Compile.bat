@echo off
if "%1"=="" Goto Error
del %1.exe
echo Compiling %1
SET PATH=d:\programme\microsoft.net\sdk\v1.1\bin\;%PATH%
del *.resources
resgen /compile AboutDlg.resx          frmMain.resx           frmOptions.resx ListBoxPanel.resx      ListViewPanel.resx     MacroQuestPanel.resx MapCon.resx            MapPane.resx
csc /nologo /out:%1.exe /target:winexe /r:Speechlib.dll,folderbrowser.dll,Magiclibrary.dll /win32icon:app.ico /recurse:*.cs  /res:AboutDlg.resources,myseq.AboutDlg.resources /res:frmMain.resources,myseq.frmMain.resources /res:frmOptions.resources,myseq:frmOptions.resources /res:ListBoxPanel.resources,myseq:ListBoxPanel.resources /res:ListViewPanel.resources,myseq:ListViewPanel.resources /res:MacroQuestPanel.resources,myseq:MacroQuestPanel.resources /res:MapCon.resources,myseq:MapCon.resources /res:MapPane.resources,myseq:MapPane.resources
echo Completed.
echo Launching %1
start /abovenormal %1
Goto Done
:Error
echo.
echo Usage: %0 {filenmae}
echo filename - This is the name of the .Exe you want to use. No spaces allowed.
It can be anything you want!
echo.
echo Example: %0 MySEQ
echo.
echo.
:done