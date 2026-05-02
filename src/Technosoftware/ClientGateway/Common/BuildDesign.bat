@echo off
setlocal

SET PATH=..\..\scripts;..\..\bin\net471;..\..\..\bin\net471;%PATH%;

echo Building ModelDesign
Technosoftware.UaModelCompiler.exe -d2 ".\ModelDesign.xml" -c ".\ModelDesign.csv" -o ".\"
echo Success!
