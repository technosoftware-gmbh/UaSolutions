/* ========================================================================
 * Copyright (c) 2005-2024 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/

using System;
using System.Text;
using System.IO;
using Opc.Ua;

#pragma warning disable 1591

namespace SampleCompany.NodeManagers.SampleDataTypes
{
    #region _ClassName_ Declarations
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public static partial class PredefinedNodes
    {
        #region PredefinedNodes Declarations
        // <summary/>
        public static NodeStateCollection Load(ISystemContext context)
        {
            byte[] initializationBuffer = Convert.FromBase64String(
               "AQAAAEIAAABodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2FtcGxlU2VydmVyL05vZGVNYW5hZ2Vycy9T" +
               "YW1wbGVEYXRhVHlwZXP/////DAAAAARgAFBAAAAAAQAUAAAATWFjaGluZVN0YXRlRGF0YVR5cGUBAQEA" +
               "AB0AewHWAQAADAAAAAAAAAAAAAAAAggAAABJbmFjdGl2ZQAIAAAASW5hY3RpdmUBAAAAAAAAAAIHAAAA" +
               "Q3V0dGluZwAHAAAAQ3V0dGluZwIAAAAAAAAAAgsAAABQcmVwYXJlTG9hZAALAAAAUHJlcGFyZUxvYWQD" +
               "AAAAAAAAAAILAAAARXhlY3V0ZUxvYWQACwAAAEV4ZWN1dGVMb2FkBAAAAAAAAAACDQAAAFByZXBhcmVV" +
               "bmxvYWQADQAAAFByZXBhcmVVbmxvYWQFAAAAAAAAAAINAAAARXhlY3V0ZVVubG9hZAANAAAARXhlY3V0" +
               "ZVVubG9hZAYAAAAAAAAAAg0AAABQcmVwYXJlUmVtb3ZlAA0AAABQcmVwYXJlUmVtb3ZlBwAAAAAAAAAC" +
               "DQAAAEV4ZWN1dGVSZW1vdmUADQAAAEV4ZWN1dGVSZW1vdmUIAAAAAAAAAAILAAAAUHJlcGFyZVNvcnQA" +
               "CwAAAFByZXBhcmVTb3J0CQAAAAAAAAACCwAAAEV4ZWN1dGVTb3J0AAsAAABFeGVjdXRlU29ydAoAAAAA" +
               "AAAAAggAAABGaW5pc2hlZAAIAAAARmluaXNoZWQLAAAAAAAAAAIGAAAARmFpbGVkAAYAAABGYWlsZWT/" +
               "////AQAAABdgqQoCAAAAAAALAAAARW51bVN0cmluZ3MBAQIAAC4ARAIAAACVDAAAAAMAAAAACAAAAElu" +
               "YWN0aXZlAwAAAAAHAAAAQ3V0dGluZwMAAAAACwAAAFByZXBhcmVMb2FkAwAAAAALAAAARXhlY3V0ZUxv" +
               "YWQDAAAAAA0AAABQcmVwYXJlVW5sb2FkAwAAAAANAAAARXhlY3V0ZVVubG9hZAMAAAAADQAAAFByZXBh" +
               "cmVSZW1vdmUDAAAAAA0AAABFeGVjdXRlUmVtb3ZlAwAAAAALAAAAUHJlcGFyZVNvcnQDAAAAAAsAAABF" +
               "eGVjdXRlU29ydAMAAAAACAAAAEZpbmlzaGVkAwAAAAAGAAAARmFpbGVkABUBAAAAAQAAAAAAAAABAf//" +
               "//8AAAAABGAAUEAAAAABAA8AAABNYWNoaW5lRGF0YVR5cGUBAQMAABYAegGNAAAAAAAAFgAAAAAEAAAA" +
               "CwAAAE1hY2hpbmVOYW1lAAAM/////wAAAAAAAAAAAAwAAABNYW51ZmFjdHVyZXIAAAz/////AAAAAAAA" +
               "AAAADAAAAFNlcmlhbE51bWJlcgAADP////8AAAAAAAAAAAAMAAAATWFjaGluZVN0YXRlAAEBAQD/////" +
               "AAAAAAAAAAAA/////wAAAAAEYAAQCAAAAAEAFQAAAEdlbmVyaWNDb250cm9sbGVyVHlwZQEBBwAAOv//" +
               "//8CAAAAFWCJCwIAAAABAAgAAABTZXRQb2ludAEBCAAALwEAQAkATggAAAAAC/////8DA/////8BAAAA" +
               "FWCJCwIAAAAAAAcAAABFVVJhbmdlAQEMAAAuAEQATgwAAAABAHQD/////wEB/////wAAAAAVYIkLAgAA" +
               "AAEACwAAAE1lYXN1cmVtZW50AQEOAAAvAQBACQBODgAAAAAL/////wEB/////wEAAAAVYIkLAgAAAAAA" +
               "BwAAAEVVUmFuZ2UBARIAAC4ARABOEgAAAAEAdAP/////AQH/////AAAAAARgABAIAAAAAQASAAAARmxv" +
               "d0NvbnRyb2xsZXJUeXBlAQEUAAEBBwD/////AAAAAARgABAIAAAAAQATAAAATGV2ZWxDb250cm9sbGVy" +
               "VHlwZQEBIQABAQcA/////wAAAAAEYAAQCAAAAAEAGQAAAFRlbXBlcmF0dXJlQ29udHJvbGxlclR5cGUB" +
               "AS4AAQEHAP////8AAAAABGAAEAgAAAABAAsAAABNYWNoaW5lVHlwZQEBOwAAOv////8EAAAABGCACwEA" +
               "AAABAAsAAABUZW1wZXJhdHVyZQEBPAAALwEBLgAATjwAAAD/////AgAAABVgiQsCAAAAAQAIAAAAU2V0" +
               "UG9pbnQBAT0AAC8BAEAJAE49AAAAAAv/////AwP/////AQAAABVgiQsCAAAAAAAHAAAARVVSYW5nZQEB" +
               "QQAALgBEAE5BAAAAAQB0A/////8BAf////8AAAAAFWCJCwIAAAABAAsAAABNZWFzdXJlbWVudAEBQwAA" +
               "LwEAQAkATkMAAAAAC/////8BAf////8BAAAAFWCJCwIAAAAAAAcAAABFVVJhbmdlAQFHAAAuAEQATkcA" +
               "AAABAHQD/////wEB/////wAAAAAEYIALAQAAAAEABAAAAEZsb3cBAUkAAC8BARQAAE5JAAAA/////wIA" +
               "AAAVYIkLAgAAAAEACAAAAFNldFBvaW50AQFKAAAvAQBACQBOSgAAAAAL/////wMD/////wEAAAAVYIkL" +
               "AgAAAAAABwAAAEVVUmFuZ2UBAU4AAC4ARABOTgAAAAEAdAP/////AQH/////AAAAABVgiQsCAAAAAQAL" +
               "AAAATWVhc3VyZW1lbnQBAVAAAC8BAEAJAE5QAAAAAAv/////AQH/////AQAAABVgiQsCAAAAAAAHAAAA" +
               "RVVSYW5nZQEBVAAALgBEAE5UAAAAAQB0A/////8BAf////8AAAAABGCACwEAAAABAAUAAABMZXZlbAEB" +
               "VgAALwEBIQAATlYAAAD/////AgAAABVgiQsCAAAAAQAIAAAAU2V0UG9pbnQBAVcAAC8BAEAJAE5XAAAA" +
               "AAv/////AwP/////AQAAABVgiQsCAAAAAAAHAAAARVVSYW5nZQEBWwAALgBEAE5bAAAAAQB0A/////8B" +
               "Af////8AAAAAFWCJCwIAAAABAAsAAABNZWFzdXJlbWVudAEBXQAALwEAQAkATl0AAAAAC/////8BAf//" +
               "//8BAAAAFWCJCwIAAAAAAAcAAABFVVJhbmdlAQFhAAAuAEQATmEAAAABAHQD/////wEB/////wAAAAAV" +
               "YIkLAgAAAAEACwAAAE1hY2hpbmVEYXRhAQFjAAAuAEQATmMAAAABAQMA/////wMD/////wAAAAAEYMAC" +
               "AQAAAA0AAABEZWZhdWx0QmluYXJ5AAAOAAAARGVmYXVsdCBCaW5hcnkBAWQAAExkAAAAAgAAAAAmAQEB" +
               "AwAAJwABAXkAAAAAABVg6QICAAAAHAAAAFNhbXBsZURhdGFUeXBlc19CaW5hcnlTY2hlbWEBACoAAABT" +
               "YW1wbGVDb21wYW55Lk5vZGVNYW5hZ2Vycy5TYW1wbGVEYXRhVHlwZXMBAXUAAEh1AAAAD3AGAAA8b3Bj" +
               "OlR5cGVEaWN0aW9uYXJ5DQogIHhtbG5zOm9wYz0iaHR0cDovL29wY2ZvdW5kYXRpb24ub3JnL0JpbmFy" +
               "eVNjaGVtYS8iDQogIHhtbG5zOnhzaT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEtaW5z" +
               "dGFuY2UiDQogIHhtbG5zOnVhPSJodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvIg0KICB4bWxuczp0" +
               "bnM9Imh0dHA6Ly9zYW1wbGVjb21wYW55LmNvbS9TYW1wbGVTZXJ2ZXIvTm9kZU1hbmFnZXJzL1NhbXBs" +
               "ZURhdGFUeXBlcyINCiAgRGVmYXVsdEJ5dGVPcmRlcj0iTGl0dGxlRW5kaWFuIg0KICBUYXJnZXROYW1l" +
               "c3BhY2U9Imh0dHA6Ly9zYW1wbGVjb21wYW55LmNvbS9TYW1wbGVTZXJ2ZXIvTm9kZU1hbmFnZXJzL1Nh" +
               "bXBsZURhdGFUeXBlcyINCj4NCiAgPG9wYzpJbXBvcnQgTmFtZXNwYWNlPSJodHRwOi8vb3BjZm91bmRh" +
               "dGlvbi5vcmcvVUEvIiBMb2NhdGlvbj0iT3BjLlVhLkJpbmFyeVNjaGVtYS5ic2QiLz4NCg0KICA8b3Bj" +
               "OkVudW1lcmF0ZWRUeXBlIE5hbWU9Ik1hY2hpbmVTdGF0ZURhdGFUeXBlIiBMZW5ndGhJbkJpdHM9IjMy" +
               "Ij4NCiAgICA8b3BjOkVudW1lcmF0ZWRWYWx1ZSBOYW1lPSJJbmFjdGl2ZSIgVmFsdWU9IjAiIC8+DQog" +
               "ICAgPG9wYzpFbnVtZXJhdGVkVmFsdWUgTmFtZT0iQ3V0dGluZyIgVmFsdWU9IjEiIC8+DQogICAgPG9w" +
               "YzpFbnVtZXJhdGVkVmFsdWUgTmFtZT0iUHJlcGFyZUxvYWQiIFZhbHVlPSIyIiAvPg0KICAgIDxvcGM6" +
               "RW51bWVyYXRlZFZhbHVlIE5hbWU9IkV4ZWN1dGVMb2FkIiBWYWx1ZT0iMyIgLz4NCiAgICA8b3BjOkVu" +
               "dW1lcmF0ZWRWYWx1ZSBOYW1lPSJQcmVwYXJlVW5sb2FkIiBWYWx1ZT0iNCIgLz4NCiAgICA8b3BjOkVu" +
               "dW1lcmF0ZWRWYWx1ZSBOYW1lPSJFeGVjdXRlVW5sb2FkIiBWYWx1ZT0iNSIgLz4NCiAgICA8b3BjOkVu" +
               "dW1lcmF0ZWRWYWx1ZSBOYW1lPSJQcmVwYXJlUmVtb3ZlIiBWYWx1ZT0iNiIgLz4NCiAgICA8b3BjOkVu" +
               "dW1lcmF0ZWRWYWx1ZSBOYW1lPSJFeGVjdXRlUmVtb3ZlIiBWYWx1ZT0iNyIgLz4NCiAgICA8b3BjOkVu" +
               "dW1lcmF0ZWRWYWx1ZSBOYW1lPSJQcmVwYXJlU29ydCIgVmFsdWU9IjgiIC8+DQogICAgPG9wYzpFbnVt" +
               "ZXJhdGVkVmFsdWUgTmFtZT0iRXhlY3V0ZVNvcnQiIFZhbHVlPSI5IiAvPg0KICAgIDxvcGM6RW51bWVy" +
               "YXRlZFZhbHVlIE5hbWU9IkZpbmlzaGVkIiBWYWx1ZT0iMTAiIC8+DQogICAgPG9wYzpFbnVtZXJhdGVk" +
               "VmFsdWUgTmFtZT0iRmFpbGVkIiBWYWx1ZT0iMTEiIC8+DQogIDwvb3BjOkVudW1lcmF0ZWRUeXBlPg0K" +
               "DQogIDxvcGM6U3RydWN0dXJlZFR5cGUgTmFtZT0iTWFjaGluZURhdGFUeXBlIiBCYXNlVHlwZT0idWE6" +
               "RXh0ZW5zaW9uT2JqZWN0Ij4NCiAgICA8b3BjOkZpZWxkIE5hbWU9Ik1hY2hpbmVOYW1lIiBUeXBlTmFt" +
               "ZT0ib3BjOlN0cmluZyIgLz4NCiAgICA8b3BjOkZpZWxkIE5hbWU9Ik1hbnVmYWN0dXJlciIgVHlwZU5h" +
               "bWU9Im9wYzpTdHJpbmciIC8+DQogICAgPG9wYzpGaWVsZCBOYW1lPSJTZXJpYWxOdW1iZXIiIFR5cGVO" +
               "YW1lPSJvcGM6U3RyaW5nIiAvPg0KICAgIDxvcGM6RmllbGQgTmFtZT0iTWFjaGluZVN0YXRlIiBUeXBl" +
               "TmFtZT0idG5zOk1hY2hpbmVTdGF0ZURhdGFUeXBlIiAvPg0KICA8L29wYzpTdHJ1Y3R1cmVkVHlwZT4N" +
               "Cg0KPC9vcGM6VHlwZURpY3Rpb25hcnk+AA//////AQEBAAAAAC8BAF0DAAAAFWCpCgIAAAAAAAwAAABO" +
               "YW1lc3BhY2VVcmkBAXcAAC4ARHcAAAAMQgAAAGh0dHA6Ly9zYW1wbGVjb21wYW55LmNvbS9TYW1wbGVT" +
               "ZXJ2ZXIvTm9kZU1hbmFnZXJzL1NhbXBsZURhdGFUeXBlcwAM/////wEB/////wAAAAAVYKkKAgAAAAAA" +
               "CgAAAERlcHJlY2F0ZWQBAXgAAC4ARHgAAAABAQAB/////wEB/////wAAAAAVYKkKAgAAAAEADwAAAE1h" +
               "Y2hpbmVEYXRhVHlwZQEBeQAALwBFeQAAAAwPAAAATWFjaGluZURhdGFUeXBlAAz/////AQH/////AAAA" +
               "AARgwAIBAAAACgAAAERlZmF1bHRYbWwAAAsAAABEZWZhdWx0IFhNTAEBbAAATGwAAAACAAAAACYBAQED" +
               "AAAnAAEBgAAAAAAAFWDpAgIAAAAZAAAAU2FtcGxlRGF0YVR5cGVzX1htbFNjaGVtYQEAKgAAAFNhbXBs" +
               "ZUNvbXBhbnkuTm9kZU1hbmFnZXJzLlNhbXBsZURhdGFUeXBlcwEBfAAASHwAAAAPYQoAADx4czpzY2hl" +
               "bWENCiAgeG1sbnM6eHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvWE1MU2NoZW1hIg0KICB4bWxuczp1" +
               "YT0iaHR0cDovL29wY2ZvdW5kYXRpb24ub3JnL1VBLzIwMDgvMDIvVHlwZXMueHNkIg0KICB4bWxuczp0" +
               "bnM9Imh0dHA6Ly9zYW1wbGVjb21wYW55LmNvbS9TYW1wbGVTZXJ2ZXIvTm9kZU1hbmFnZXJzL1NhbXBs" +
               "ZURhdGFUeXBlcyINCiAgdGFyZ2V0TmFtZXNwYWNlPSJodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2Ft" +
               "cGxlU2VydmVyL05vZGVNYW5hZ2Vycy9TYW1wbGVEYXRhVHlwZXMiDQogIGVsZW1lbnRGb3JtRGVmYXVs" +
               "dD0icXVhbGlmaWVkIg0KPg0KICA8eHM6YW5ub3RhdGlvbj4NCiAgICA8eHM6YXBwaW5mbz4NCiAgICAg" +
               "IDx1YTpNb2RlbCBNb2RlbFVyaT0iaHR0cDovL3NhbXBsZWNvbXBhbnkuY29tL1NhbXBsZVNlcnZlci9O" +
               "b2RlTWFuYWdlcnMvU2FtcGxlRGF0YVR5cGVzIiBWZXJzaW9uPSIxLjAuMCIgUHVibGljYXRpb25EYXRl" +
               "PSIyMDI1LTExLTE3VDA4OjA0OjI2LjAxMTM0MDVaIiAvPg0KICAgIDwveHM6YXBwaW5mbz4NCiAgPC94" +
               "czphbm5vdGF0aW9uPg0KICANCiAgPHhzOmltcG9ydCBuYW1lc3BhY2U9Imh0dHA6Ly9vcGNmb3VuZGF0" +
               "aW9uLm9yZy9VQS8yMDA4LzAyL1R5cGVzLnhzZCIgLz4NCg0KICA8eHM6c2ltcGxlVHlwZSAgbmFtZT0i" +
               "TWFjaGluZVN0YXRlRGF0YVR5cGUiPg0KICAgIDx4czpyZXN0cmljdGlvbiBiYXNlPSJ4czpzdHJpbmci" +
               "Pg0KICAgICAgPHhzOmVudW1lcmF0aW9uIHZhbHVlPSJJbmFjdGl2ZV8wIiAvPg0KICAgICAgPHhzOmVu" +
               "dW1lcmF0aW9uIHZhbHVlPSJDdXR0aW5nXzEiIC8+DQogICAgICA8eHM6ZW51bWVyYXRpb24gdmFsdWU9" +
               "IlByZXBhcmVMb2FkXzIiIC8+DQogICAgICA8eHM6ZW51bWVyYXRpb24gdmFsdWU9IkV4ZWN1dGVMb2Fk" +
               "XzMiIC8+DQogICAgICA8eHM6ZW51bWVyYXRpb24gdmFsdWU9IlByZXBhcmVVbmxvYWRfNCIgLz4NCiAg" +
               "ICAgIDx4czplbnVtZXJhdGlvbiB2YWx1ZT0iRXhlY3V0ZVVubG9hZF81IiAvPg0KICAgICAgPHhzOmVu" +
               "dW1lcmF0aW9uIHZhbHVlPSJQcmVwYXJlUmVtb3ZlXzYiIC8+DQogICAgICA8eHM6ZW51bWVyYXRpb24g" +
               "dmFsdWU9IkV4ZWN1dGVSZW1vdmVfNyIgLz4NCiAgICAgIDx4czplbnVtZXJhdGlvbiB2YWx1ZT0iUHJl" +
               "cGFyZVNvcnRfOCIgLz4NCiAgICAgIDx4czplbnVtZXJhdGlvbiB2YWx1ZT0iRXhlY3V0ZVNvcnRfOSIg" +
               "Lz4NCiAgICAgIDx4czplbnVtZXJhdGlvbiB2YWx1ZT0iRmluaXNoZWRfMTAiIC8+DQogICAgICA8eHM6" +
               "ZW51bWVyYXRpb24gdmFsdWU9IkZhaWxlZF8xMSIgLz4NCiAgICA8L3hzOnJlc3RyaWN0aW9uPg0KICA8" +
               "L3hzOnNpbXBsZVR5cGU+DQogIDx4czplbGVtZW50IG5hbWU9Ik1hY2hpbmVTdGF0ZURhdGFUeXBlIiB0" +
               "eXBlPSJ0bnM6TWFjaGluZVN0YXRlRGF0YVR5cGUiIC8+DQoNCiAgPHhzOmNvbXBsZXhUeXBlIG5hbWU9" +
               "Ikxpc3RPZk1hY2hpbmVTdGF0ZURhdGFUeXBlIj4NCiAgICA8eHM6c2VxdWVuY2U+DQogICAgICA8eHM6" +
               "ZWxlbWVudCBuYW1lPSJNYWNoaW5lU3RhdGVEYXRhVHlwZSIgdHlwZT0idG5zOk1hY2hpbmVTdGF0ZURh" +
               "dGFUeXBlIiBtaW5PY2N1cnM9IjAiIG1heE9jY3Vycz0idW5ib3VuZGVkIiAvPg0KICAgIDwveHM6c2Vx" +
               "dWVuY2U+DQogIDwveHM6Y29tcGxleFR5cGU+DQogIDx4czplbGVtZW50IG5hbWU9Ikxpc3RPZk1hY2hp" +
               "bmVTdGF0ZURhdGFUeXBlIiB0eXBlPSJ0bnM6TGlzdE9mTWFjaGluZVN0YXRlRGF0YVR5cGUiIG5pbGxh" +
               "YmxlPSJ0cnVlIj48L3hzOmVsZW1lbnQ+DQoNCiAgPHhzOmNvbXBsZXhUeXBlIG5hbWU9Ik1hY2hpbmVE" +
               "YXRhVHlwZSI+DQogICAgPHhzOnNlcXVlbmNlPg0KICAgICAgPHhzOmVsZW1lbnQgbmFtZT0iTWFjaGlu" +
               "ZU5hbWUiIHR5cGU9InhzOnN0cmluZyIgbWluT2NjdXJzPSIwIiBuaWxsYWJsZT0idHJ1ZSIgLz4NCiAg" +
               "ICAgIDx4czplbGVtZW50IG5hbWU9Ik1hbnVmYWN0dXJlciIgdHlwZT0ieHM6c3RyaW5nIiBtaW5PY2N1" +
               "cnM9IjAiIG5pbGxhYmxlPSJ0cnVlIiAvPg0KICAgICAgPHhzOmVsZW1lbnQgbmFtZT0iU2VyaWFsTnVt" +
               "YmVyIiB0eXBlPSJ4czpzdHJpbmciIG1pbk9jY3Vycz0iMCIgbmlsbGFibGU9InRydWUiIC8+DQogICAg" +
               "ICA8eHM6ZWxlbWVudCBuYW1lPSJNYWNoaW5lU3RhdGUiIHR5cGU9InRuczpNYWNoaW5lU3RhdGVEYXRh" +
               "VHlwZSIgbWluT2NjdXJzPSIwIiAvPg0KICAgIDwveHM6c2VxdWVuY2U+DQogIDwveHM6Y29tcGxleFR5" +
               "cGU+DQogIDx4czplbGVtZW50IG5hbWU9Ik1hY2hpbmVEYXRhVHlwZSIgdHlwZT0idG5zOk1hY2hpbmVE" +
               "YXRhVHlwZSIgLz4NCg0KICA8eHM6Y29tcGxleFR5cGUgbmFtZT0iTGlzdE9mTWFjaGluZURhdGFUeXBl" +
               "Ij4NCiAgICA8eHM6c2VxdWVuY2U+DQogICAgICA8eHM6ZWxlbWVudCBuYW1lPSJNYWNoaW5lRGF0YVR5" +
               "cGUiIHR5cGU9InRuczpNYWNoaW5lRGF0YVR5cGUiIG1pbk9jY3Vycz0iMCIgbWF4T2NjdXJzPSJ1bmJv" +
               "dW5kZWQiIG5pbGxhYmxlPSJ0cnVlIiAvPg0KICAgIDwveHM6c2VxdWVuY2U+DQogIDwveHM6Y29tcGxl" +
               "eFR5cGU+DQogIDx4czplbGVtZW50IG5hbWU9Ikxpc3RPZk1hY2hpbmVEYXRhVHlwZSIgdHlwZT0idG5z" +
               "Okxpc3RPZk1hY2hpbmVEYXRhVHlwZSIgbmlsbGFibGU9InRydWUiPjwveHM6ZWxlbWVudD4NCg0KPC94" +
               "czpzY2hlbWE+AA//////AQEBAAAAAC8BAFwDAAAAFWCpCgIAAAAAAAwAAABOYW1lc3BhY2VVcmkBAX4A" +
               "AC4ARH4AAAAMQgAAAGh0dHA6Ly9zYW1wbGVjb21wYW55LmNvbS9TYW1wbGVTZXJ2ZXIvTm9kZU1hbmFn" +
               "ZXJzL1NhbXBsZURhdGFUeXBlcwAM/////wEB/////wAAAAAVYKkKAgAAAAAACgAAAERlcHJlY2F0ZWQB" +
               "AX8AAC4ARH8AAAABAQAB/////wEB/////wAAAAAVYKkKAgAAAAEADwAAAE1hY2hpbmVEYXRhVHlwZQEB" +
               "gAAALwBFgAAAAAwlAAAALy94czplbGVtZW50W0BuYW1lPSdNYWNoaW5lRGF0YVR5cGUnXQAM/////wEB" +
               "/////wAAAAAEYMACAQAAAAsAAABEZWZhdWx0SnNvbgAADAAAAERlZmF1bHQgSlNPTgEBdAAATHQAAAAB" +
               "AAAAACYBAQEDAAAAAAA="
            );
            using (MemoryStream stream = new MemoryStream(initializationBuffer))
            {
                NodeStateCollection predefinedNodes = new NodeStateCollection();
                predefinedNodes.LoadFromBinary(context, stream, true);
                return predefinedNodes;
            }
        }
        #endregion
    }
    #endregion
}