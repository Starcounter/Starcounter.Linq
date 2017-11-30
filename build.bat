@ECHO OFF

SETLOCAL EnableDelayedExpansion

:: Set up the env to use Msbuild
PUSHD %~dp0
IF EXIST "%programfiles(x86)%\Microsoft Visual Studio\2017\Community\Common7\Tools\VsDevCmd.bat" (
    CALL "%programfiles(x86)%\Microsoft Visual Studio\2017\Community\Common7\Tools\VsDevCmd.bat"
) ELSE IF EXIST "%programfiles(x86)%\Microsoft Visual Studio\2017\Enterprise\Common7\Tools\VsDevCmd.bat" (
    CALL "%programfiles(x86)%\Microsoft Visual Studio\2017\Enterprise\Common7\Tools\VsDevCmd.bat"
) ELSE IF EXIST "%programfiles(x86)%\Microsoft Visual Studio\2017\Professional\Common7\Tools\VsDevCmd.bat" (
    CALL "%programfiles(x86)%\Microsoft Visual Studio\2017\Professional\Common7\Tools\VsDevCmd.bat"
) ELSE IF EXIST "%programfiles(x86)%\Microsoft Visual Studio\2017\BuildTools\Common7\Tools\VsDevCmd.bat" (
    CALL "%programfiles(x86)%\Microsoft Visual Studio\2017\BuildTools\Common7\Tools\VsDevCmd.bat"
) ELSE IF EXIST "%VS140COMNTOOLS%\vsvars32.bat" (
    CALL "%VS140COMNTOOLS%\vsvars32.bat"
) ELSE (
    ECHO Error: You don't seem to have Visual Studio 2015 or 2017 installed
)
POPD

:: Try to restore packages. This is only needed when the solution depends on NuGet packages
PUSHD %~dp0\tools
WHERE nuget.exe >nul 2>nul
IF %ERRORLEVEL% EQU 0 nuget.exe restore ..
IF NOT EXIST "..\packages\" (ECHO Error: Get nuget.exe or build the sln in VS to restore the packages && EXIT /B 1)
POPD

PUSHD %~dp0
msbuild /m
POPD

ENDLOCAL