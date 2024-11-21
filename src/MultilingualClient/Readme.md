Steg1:
dotnet publish -f net8.0-maccatalyst -c Release -p:MtouchLink=SdkOnly -p:CreatePackage=true -p:EnableCodeSigning=true -p:EnablePackageSigning=true -p:CodesignKey="Developer ID Application: EVRY Sweden AB (EMV34W7ZW5)" -p:CodesignProvision="multilingualclient (Non-App Store)" -p:PackageSigningKey="Developer ID Installer: EVRY Sweden AB (EMV34W7ZW5)" -p:UseHardenedRuntime=true  -o ./artifacts

Notarizing macOS software before distribution
https://support.apple.com/en-us/102654
steg1.1:
cd artifacts
steg2:
xcrun notarytool submit MultilingualClient-1.0.5.pkg --wait --apple-id joacim.wall@tietoevry.com --password aldd-eesi-pyhc-zdkv --team-id EMV34W7ZW5
steg3:
xcrun stapler staple MultilingualClient-1.0.5.pkg
steg4:
xcrun stapler validate MultilingualClient-1.0.5.pkg