REM collect a trace using the EventSource provider Technosoftware.UaCore,Technosoftware.UaClient
dotnet tool install --global dotnet-trace
dotnet-trace collect --name SampleCompany.ReferenceClient --providers Technosoftware.UaCore,Technosoftware.UaClient
