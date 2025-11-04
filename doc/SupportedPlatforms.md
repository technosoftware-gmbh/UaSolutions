# Supported Platforms

[TOC]

## Supported OS versions

### Apple

| OS                            | Versions                    | Architectures         |
| ----------------------------- | --------------------------- | --------------------- |
| [macOS][1]                    | 15, 14, 13                  | Arm64, x64            |

[1]: https://developer.apple.com/macos/

### Linux

| OS                            | Versions                    | Architectures         |
| ----------------------------- | --------------------------- | --------------------- |
| [Alpine][2]                   | 3.21, 3.20, 3.19            | Arm32, Arm64, x64     |
| [Azure Linux][3]              | 3.0                         | Arm64, x64            |
| [CentOS Stream][4]            | 10, 9                       | Arm64, ppc64le, s390x, x64 |
| [Debian][5]                   | 12                          | Arm32, Arm64, x64     |
| [Fedora][6]                   | 42, 41, 40                  | Arm32, Arm64, x64     |
| [openSUSE Leap][7]            | 15.6                        | Arm64, x64            |
| [Red Hat Enterprise Linux][8] | 10, 9, 8                   | Arm64, ppc64le, s390x, x64 |
| [SUSE Enterprise Linux][9]    | 15.6                        | Arm64, x64            |
| [Ubuntu][10]                  | 25.04, 24.10, 24.04, 22.04  | Arm32, Arm64, x64     |

[2]: https://alpinelinux.org/
[3]: https://github.com/microsoft/azurelinux
[4]: https://centos.org/
[5]: https://www.debian.org/
[6]: https://fedoraproject.org/
[7]: https://www.opensuse.org/
[8]: https://www.suse.com/
[9]: https://ubuntu.com/


### Windows

| OS                            | Versions                    | Architectures         |
| ----------------------------- | --------------------------- | --------------------- |
| [Nano Server][10]             | 2025, 2022, 2019            | x64                   |
| [Windows][11]                 | 11 24H2 (IoT), 11 24H2 (E), 11 24H2, 11 23H2, 11 22H2 (E), 10 22H2, 10 21H2 (E), 10 21H2 (IoT), 10 1809 (E), 10 1607 (E) | Arm64, x64, x86 |
| [Windows Server][12]          | 2025, 23H2, 2022, 2019, 2016, 2012-R2, 2012 | x64, x86 |
| [Windows Server Core][13]     | 2025, 2022, 2019, 2016, 2012-R2, 2012 | x64, x86    |

[10]: https://learn.microsoft.com/virtualization/windowscontainers/manage-containers/container-base-images
[11]: https://learn.microsoft.com/windows-server/get-started/windows-server-release-info
[12]: https://www.microsoft.com/windows/
[13]: https://support.microsoft.com/help/13853/windows-lifecycle-fact-sheet
[14]: https://www.microsoft.com/windows-server

## Supported .NET framworks

| OS Platform                   | Versions                    |
| ----------------------------- | --------------------------- |
| Apple                         | [.NET 9.0][15], [.NET 8.0][16]          |
| Linux                         | [.NET 9.0][15], [.NET 8.0][16]          |
| Windows                       | [.NET 9.0][15], [.NET 8.0][16], [.NET 4.8][17], [.NET 4.7.2][18]          |

[15]: https://dotnet.microsoft.com/en-us/download/dotnet/9.0
[16]: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
[17]: https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48
[18]: https://dotnet.microsoft.com/en-us/download/dotnet-framework/net472
