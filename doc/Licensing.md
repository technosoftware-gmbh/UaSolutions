# Licensing

[TOC]

## Licensing the Solution

After purchasing one of the OPC UA Solutions you will receive a license file with the information on how to use it.

To activate the license in your application, you need to take the following steps which differes slightly depending if you use Client or Server Solutuion:

### Client Solution

Add the following to your application code, the best place would be the start of your application:

```
  #region License validation
  var licenseData =
          @"";
            LicenseHandler.Validate(licenseData);
  #endregion
```

You have to change the value of licenseData with the information you received in the license file.

### Server Solution based on UaBaseServer

Change the value of serialNumber with the information you received in the license file.

```
        public void OnGetLicenseInformation(out string serialNumber)
        {
            serialNumber = 	
				@"";
        }
```

### Server Solution based on UaStandardServer

This is similar as for the Client Solution. Add the following to your application code, the best place would be the start of your application:

```
  #region License validation
  var licenseData =
          @"";
            LicenseHandler.Validate(licenseData);
  #endregion
```

You have to change the value of licenseData with the information you received in the license file.


## Checking the license state

You can use the following code as an example on how to check if the provided license is valid:

```
            if (!Technosoftware.UaUtilities.Licensing.LicenseHandler.IsLicensed && !Technosoftware.UaUtilities.Licensing.LicenseHandler.IsEvaluation)
            {
                await output.WriteLineAsync("ERROR: No valid license applied.").ConfigureAwait(false);
                return;
            }
```

