
This certificate was created using the instructions in "Certificate Expiration in ClickOnce Deployment" under the section "Test Certificates, MakeCert, RenewCert, and The Big Workaround," which can be found at http://msdn.microsoft.com/en-us/library/ff369721.aspx.  It is used to sign the ExcelTemplate's ClickOnce manifest.  It does not expire until 2039.

The commands used to create the certificate were as follows:

  1. makecert.exe -sv NodeXLExcelTemplate.pvk -n "CN=Social Media Research Foundation" NodeXLExcelTemplate.cer

  2. pvk2pfx -pvk NodeXLExcelTemplate.pvk -spc NodeXLExcelTemplate.cer -pfx NodeXLExcelTemplate.pfx -po MyPassword

If the certificate is ever changed, the corresponding public key stored in TrustInstaller.cs in the ExcelTemplateTrustInstaller project must be updated.  Follow these steps:

  1. Open ExcelTemplate\bin\Debug\Microsoft.NodeXL.ExcelTemplate.vsto.

  2. Copy the <RSAKeyValue><Modulus> node.

  3. Paste the value to the RSA_PublicKey assignment in TrustInstaller.cs.
