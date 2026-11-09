Build a signed and notarized pkg for GitHub releases.
Requires the "Developer ID Application" and "Developer ID Installer" certificates for team TC77MF5B37
(backup: Walltech folder in iCloud Drive).

Steg0 (once per Mac): store notarization credentials (app-specific password from account.apple.com) in the keychain:
xcrun notarytool store-credentials walltec-notary --apple-id joacim.wall@gmail.com --team-id TC77MF5B37

Steg1 (run in src/MultilingualClient; bump ApplicationDisplayVersion/ApplicationVersion in the csproj first):
dotnet publish MultilingualClient.csproj -f net11.0-maccatalyst -c Release -p:MtouchLink=SdkOnly -p:CreatePackage=true -p:EnableCodeSigning=true -p:EnablePackageSigning=true -p:CodesignKey="Developer ID Application: MAX JOACIM WALL (TC77MF5B37)" -p:PackageSigningKey="Developer ID Installer: MAX JOACIM WALL (TC77MF5B37)" -p:UseHardenedRuntime=true -o ./artifacts

Notarizing macOS software before distribution
https://support.apple.com/en-us/102654
steg1.1:
cd artifacts
steg2:
xcrun notarytool submit MultilingualClient-1.0.6.pkg --keychain-profile walltec-notary --wait
steg3:
xcrun stapler staple MultilingualClient-1.0.6.pkg
steg4:
xcrun stapler validate MultilingualClient-1.0.6.pkg
spctl -a -vv -t install MultilingualClient-1.0.6.pkg
