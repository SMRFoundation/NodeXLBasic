
REM  Create an empty folder.

rd /s "C:\Temp\NodeXLAutomationToolsRelease"
md "C:\Temp\NodeXLAutomationToolsRelease"


REM  Copy the executables.

xcopy "..\GraphMLFileProcessorStarter\bin\Debug\GraphMLFileProcessorStarter.exe" "C:\Temp\NodeXLAutomationToolsRelease"

xcopy "..\GraphMLFileProcessorStarter\bin\Debug\GraphMLFileProcessorStarter.pdb" "C:\Temp\NodeXLAutomationToolsRelease"

xcopy "..\GraphMLFileProcessorStarter\bin\Debug\Smrf.NodeXL.Util.dll" "C:\Temp\NodeXLAutomationToolsRelease"

xcopy "..\GraphMLFileProcessorStarter\bin\Debug\Smrf.NodeXL.Util.pdb" "C:\Temp\NodeXLAutomationToolsRelease"

xcopy "..\NetworkServerStarter\bin\Debug\NodeXLNetworkServerStarter.exe" "C:\Temp\NodeXLAutomationToolsRelease"

xcopy "..\NetworkServerStarter\bin\Debug\NodeXLNetworkServerStarter.pdb" "C:\Temp\NodeXLAutomationToolsRelease"


REM  Copy the sample configuration files.

xcopy "E:\NodeXL\NetworkServer\SampleNetworkConfigurationFiles" "C:\Temp\NodeXLAutomationToolsRelease\SampleNetworkConfigurationFiles" /S /I


REM  Create a documentation file.

echo See "How to use the NodeXL Automation Tools" at http://nodexl.codeplex.com/discussions/545884. > "C:\Temp\NodeXLAutomationToolsRelease\HowToUseTheseTools.txt"


start C:\Temp\NodeXLAutomationToolsRelease
