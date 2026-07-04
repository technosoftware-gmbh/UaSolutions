REM collect a trace using the EventSource provider Technosoftware.UaCore,Technosoftware.UaServer
dotnet tool install --global dotnet-trace
dotnet-trace collect --name SampleCompany.ReferenceServer --providers Technosoftware.UaCore,Technosoftware.UaServer
