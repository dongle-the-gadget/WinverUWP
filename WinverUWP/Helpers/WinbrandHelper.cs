using System.Runtime.InteropServices;

namespace WinverUWP.Helpers;

public unsafe static partial class WinbrandHelper
{
    [LibraryImport("winbrand.dll", StringMarshalling = StringMarshalling.Utf16)]
    public static partial char* BrandingFormatString(string format);

    public static string GetWinbrand()
    {
        return new string(BrandingFormatString("%WINDOWS_LONG%"));
    }
}