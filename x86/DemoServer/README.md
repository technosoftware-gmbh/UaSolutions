# Classic OPC DA/AE demo server

`OpcDaAeServer.exe` is a Classic OPC (COM) DA/AE server. It is used as the **example
server** for the OPC UA Client Gateway — the gateway connects to it to demonstrate
and exercise the Classic DA/AE/HDA path.

**This binary is tracked on purpose. Do not delete it.**

It lives under `x86/`, which `.gitignore` otherwise treats as a build-output
directory; the ignore rules carry an explicit exception for this folder. If you move
or rename it, update `.gitignore` to match, or the file will silently stop being
tracked.

Being COM-based, it is Windows-x64 only, like the `ClientGateway` project itself.
