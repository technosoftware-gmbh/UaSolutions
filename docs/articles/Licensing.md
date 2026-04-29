# Licensing

## Licensing the Solution

After purchasing one of the OPC UA Solutions you will receive a license file with the information on how to use it.

To activate the license in your application, you need to take the following steps which differes slightly depending if you use Client or Server Solutuion:

### Client Solution

Add the following to your application code, the best place would be the start of your application:

```
  #region License validation
  const string licenseData =
          @"";
  bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);
  if (!licensed)
  {
      Console.WriteLine("WARNING: No valid license applied.");
  }
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
  const string licenseData =
          @"";
  bool licensed = LicenseHandler.Instance.Validate(ProductType.Server, licenseData);
  #endregion
```

You have to change the value of licenseData with the information you received in the license file.


## Checking the license state

You can use the following code as an example on how to get information about the license and check if the provided license is valid:

```
            string licensedString = $"   Licensed Product     : {LicenseHandler.Instance.LicensedProduct}";
            Console.WriteLine(licensedString);
                   licensedString = $"   Licensed Product Type: {LicenseHandler.Instance.LicensedProductType}";
            Console.WriteLine(licensedString);
            licensedString = $"   Licensed Features    : {LicenseHandler.Instance.LicensedFeatures}";
            Console.WriteLine(licensedString);
            if (LicenseHandler.Instance.IsEvaluation)
            {
                licensedString = $"   Evaluation expires at: {LicenseHandler.Instance.LicenseExpirationDate}";
                Console.WriteLine(licensedString);
                licensedString = $"   Days until Expiration: {LicenseHandler.Instance.LicenseExpirationDays}";
                Console.WriteLine(licensedString);
            }
            licensedString = $"   Support Included     : {LicenseHandler.Instance.Support}";
            Console.WriteLine(licensedString);
            if (LicenseHandler.Instance.Support != SupportLevel.None)
            {
                licensedString = $"   Support expire at    : {LicenseHandler.Instance.SupportExpirationDate}";
                Console.WriteLine(licensedString);
                licensedString = $"   Days until Expiration: {LicenseHandler.Instance.SupportExpirationDays}";
                Console.WriteLine(licensedString);
            }
            if (LicenseHandler.Instance.IsEvaluation)
            {
                licensedString = $"   Evaluation Period    : {LicenseHandler.Instance.EvaluationPeriod} minutes.";
                Console.WriteLine(licensedString);
            }

            if (!LicenseHandler.Instance.IsLicensed && !LicenseHandler.Instance.IsEvaluation)
            {
                Console.WriteLine("ERROR: No valid license applied.");
            }
```

