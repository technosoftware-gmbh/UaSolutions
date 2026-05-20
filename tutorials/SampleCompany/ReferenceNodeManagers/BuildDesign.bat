rem @echo off
setlocal

set MODELCOMPILER=Opc.Ua.ModelCompiler.exe
SET PATH=..\..\..\bin\modelcompiler;%PATH%;
set MODELROOT=.

echo Building DataTypes
%MODELCOMPILER% compile -version v104 -d2 "%MODELROOT%/SampleDataTypes/Generated/SampleDataTypesModelDesign.xml" -cg "%MODELROOT%/SampleDataTypes/Generated/SampleDataTypesModelDesign.csv" -o2 "%MODELROOT%/SampleDataTypes/Generated"
IF %ERRORLEVEL% EQU 0 echo Success!

echo Building TestData
%MODELCOMPILER% compile -version v104 -id 1000 -d2 "%MODELROOT%/TestData/Generated/TestDataDesign.xml" -cg "%MODELROOT%/TestData/Generated/TestDataDesign.csv" -o2 "%MODELROOT%/TestData/Generated"
IF %ERRORLEVEL% EQU 0 echo Success!

echo Building MemoryBuffer
%MODELCOMPILER% compile -version v104 -id 1000 -d2 "%MODELROOT%/MemoryBuffer/Generated/MemoryBufferDesign.xml" -cg "%MODELROOT%/MemoryBuffer/Generated/MemoryBufferDesign.csv" -o2 "%MODELROOT%/MemoryBuffer/Generated" 
IF %ERRORLEVEL% EQU 0 echo Success!

echo Building BoilerDesign
%MODELCOMPILER% compile -version v104 -id 1000 -d2 "%MODELROOT%/Boiler/Generated/BoilerDesign.xml" -cg "%MODELROOT%/Boiler/Generated/BoilerDesign.csv" -o2 "%MODELROOT%/Boiler/Generated"
IF %ERRORLEVEL% EQU 0 echo Success!

pause
