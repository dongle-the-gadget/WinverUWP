# WinverUWP
A UWP version of winver.

[Microsoft Store](https://www.microsoft.com/p/winver-uwp/9n60s2vfmb7l) | [Discord channel](https://discord.gg/MUyRGUN4Ny) | [Privacy Policy](/PRIVACY.md)

![WinverUWP on Windows 11 (Dev)](/images/WinverUWP-dark-11.png)

## System requirements
Windows 10 Creators Update (1703) or newer using the x86, x64, ARM32 or ARM64 CPU architecture.

## Installing
**Note:** You need to manually uninstall Release 1.x as Release 2.0 and newer will not automatically uninstall.
### For Release 2 and newer
#### Using Microsoft Store (recommended)
You can download WinverUWP for free in [Microsoft Store](https://www.microsoft.com/p/winver-uwp/9n60s2vfmb7l). This option allows you to get automatic updates.

#### Package files
1. Go to the [Releases](https://github.com/dongle-the-gadget/WinverUWP/releases) page.
2. Look for the latest release (usually at the top of the page).
3. Download the correct architecture (x86, x64, arm32 or arm64).
4. Open the downloaded `appx` file and choose **Install** or **Update**.

**Note:** Choosing the correct architecture is critical, otherwise the app would fail to read the registry and crashes on launch.

### For Release 1.x
**Note:** These builds don't support ARM64 and has been officially deprecated.

1. Download [Release 1.1](https://github.com/dongle-the-gadget/WinverUWP/releases/download/v1.1.0.0/WinverUWPPackage_1.1.0.0.zip).
2. Extract the ZIP file.
3. Run the Install.ps1 file and the application will be installed.
   
   **Note:** During installation, PowerShell may ask you about execution policies and administrator privileges. For execution scopes, select **Unrestricted**, and for administrator privileges (User Account Control), select Yes or enter in your administrator credentials.

## Launching
You could launch this program in one of these three ways:
- As an app entry in Start, Search, PowerToys Run, ... (whatever has you)
- Using the app execution alias: `winveruwp.exe`
- Using the app protocol: `winveruwp://`

<!--
## Contributing
### Language translation
If you want to help translate, follow the steps below:
1. Create a fork of this repository.
2. In `WinverUWP\Strings` folder, create a new folder with your locale name (i.e. "en-us" for American English).
3. In the newly created folder, create a `Resources.resw` file, you may copy it from another locale folder as a reference.
4. Modify the new `Resources.resw` file with your translations. Reference the table below for required keys and example value in English.

   **Note:** References to "Microsoft" and "Windows" must be kept unchanged.

   | Key      | Example English value             |
   |----------|-----------------------------------|
   |AboutWindows.Text|About Windows|
   |WindowsSpecifications.Text|Windows specifications|
   |CopyButton.Content|Copy|
   |Edition.Text|Edition|
   |Version.Text|Version|
   |InstalledOn.Text|Installed on|
   |OSBuild.Text|OS build|
   |Experience.Text|Experience|
   |LegalLabel.Text|Legal|
   |LicenseTo.Text|This product is licensed under the Microsoft Software License Terms to:|
   |Trademark.Text|The Windows operating system and its user interface are protected by trademark and other pending or existing intellectual property rights in the United States and other countries or regions.|
   |OKButton.Content|OK|
   |ExperiencePack|Windows Feature Experience Pack|
   |Copied|Copied!|
5. In the `WinverUWP\Package.appxmanifest` file, look for `<Resources>`.
6. Create a new line after the tag with the value: `<Resource Language="(your locale here)"/>`.
7. Commit your translations into your fork.
8. Create a Pull request and wait for me to review it.
-->