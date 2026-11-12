# .NET 11 MAUI: feature evaluation

Report only, no code changed. Paths are relative to `src/MultilingualClient/`.

Context: the app is a desktop tool. `MultilingualClient.csproj:4,6` builds only `net11.0-maccatalyst` and `net11.0-windows10.0.19041.0`. Android and iOS TFMs are not built. `Platforms/Android` and `Platforms/iOS` are leftover template files, and Tizen is commented out at line 9. Most Android and iOS features therefore have nothing to replace. The app has no tabs, maps, permissions, location or camera use.

| Feature | Relevant? | Where in the code | Reason |
|---|---|---|---|
| `Window.StatusBarTheme` | no | none (`App.xaml.cs:14` only creates a plain `Window`) | No status bar code exists, and the app targets desktop only. |
| `MauiAndroidSystemBarsUseMauiChrome` | no | none (`Platforms/Android/MainActivity.cs` has no system bar code) | Android is not a build target and nothing colours the system bars. |
| Dark splash (`DarkFile`, `DarkColor`, `DarkTintColor`) | maybe | `MultilingualClient.csproj:45` | A single `Color="#1F0870"` splash. A dark variant only matters if iOS or Android returns. |
| `MonochromeFile` on `MauiIcon` | no | `MultilingualClient.csproj:42` | Android themed icons only, and Android is not built. |
| `ResizeQuality` on `MauiImage` | no | `MultilingualClient.csproj:48-49` | The only `MauiImage` is the template `dotnet_bot.svg` (vector). The tree icons are PNGs in `Resources/EmbeddedImages` (`MultilingualClient.csproj:180-185`), embedded resources, not `MauiImage`. |
| TabbedPage badges | no | none (`ViewModels/BaseViewModel.cs:14,395` mention TabbedPage only for analytics) | The app has no `TabbedPage` and no badges. |
| `Permissions.Current` (`IPermissions`) | no | none | The code never calls `Permissions`. Folder access goes through `FolderPicker` (`ViewModels/MainViewModel.cs:273`). |
| `Permissions.PostNotifications` on iOS | no | none | The app sends no notifications. |
| MediaPicker process-death recovery | no | none | No `MediaPicker` or camera use. |
| `MediaPickerOptions.SaveToGallery` | no | none | No `MediaPicker` or gallery use. |
| `GeolocationListeningRequest.MinimumDistance` | no | none | No location use. |
| Passkeys | no | none | No login or account. The app works on local resx files. |
| `BoxView.Fill` with gradients | no | `Controls/TreeView/TreeView.cs:134-135` (`SelectionBoxView` solid colour) | Solid colours only. No gradients are used in the XAML. |
| `InvalidateStyle()` / `InvalidateVisualStates()` | no | `Views/AppShell.xaml:13-29` (static shell style via `BasedOn`) | Styles are declared in XAML. No code reassigns `Style`. |
| Back button accessibility label | maybe | `ViewModels/BaseViewModel.cs:37` (`ShowNavbarBackbutton`), `Views/AppShell.xaml` (single `ShellContent`, no `NavigationPage`) | Only one page is shown now. Useful if navigation is added, and `maui-accessibility` work could pick it up then. |
| Map clustering, `Pin.ImageSource`, click events | no | none | The app has no map control. |
| C# expressions in XAML | no | `Views/MainPage.xaml`, `Views/StartupView.xaml` (no converters in use; the commented converter at `StartupView.xaml:13` is dead code) | Nothing to replace. It is experimental and `EnablePreviewFeatures` is already `True` (`MultilingualClient.csproj:21`). Do not use in production. |
| `MauiEnableFullReadyToRun` (Android, Release) | no | none (`MultilingualClient.csproj:162-168` Release settings are Mac Catalyst only) | Android only, and not built. |
| New unit test templates (`androidtest`, `iostest`) | no | `../MultilingualRider/cli/Multilingual.Cli.Tests/Multilingual.Cli.Tests.csproj` (xUnit, `net10.0`) | The only tests cover the CLI and already run with `dotnet test`. The MAUI client has no tests. Android and iOS device tests add nothing for a desktop app. |

## Summary

- Relevant now: none.
- Maybe, only if mobile targets return or navigation is added: dark splash, back button accessibility label.
- Everything else has no matching code, or applies only to Android or iOS, which are not built.
