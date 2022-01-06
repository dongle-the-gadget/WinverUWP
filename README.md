# WinverUWP
A UWP version of winver.

[Discord channel](https://discord.gg/MUyRGUN4Ny)

![WinverUWP on Windows 11 (Dev)](/images/WinverUWP-dark-11.png)

## System requirements
Windows 10 October 2018 Update (1809) or newer using the x86, x64 ~~or ARM64~~ (lol) CPU architecture.

## Installing
1. Download the latest version from [GitHub Releases](https://github.com/dongle-the-gadget/WinverUWP/releases).
2. Download the ZIP file and extract it.
3. Run the Install.ps1 file and the application will be installed.
   
   **Note:** During installation, PowerShell may ask you about execution policies and administrator privileges. For execution scopes, select **Unrestricted**, and for administrator privileges (User Account Control), select Yes or enter in your administrator credentials.

## Launching
You could launch this program in one of these three ways:
- As an app entry in Start, Search, PowerToys Run, ... (whatever has you)
- Using the app execution alias: `winveruwp.exe`
- Using the app protocol: `winveruwp://`

## Contributing
### Language translation
**Note:** instructions below are for version 2.0 and newer, which is available on this branch.

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
