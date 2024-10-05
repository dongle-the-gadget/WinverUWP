using System.Globalization;
using System.Runtime.InteropServices;

namespace WinverUWP.Helpers;

public unsafe partial class CultureInfoHelper
{
    [LibraryImport("api-ms-win-core-localization-l1-2-1.dll", StringMarshalling = StringMarshalling.Utf16)]
    private static partial int GetLocaleInfoEx(string lpLocaleName, uint LCType, char* lpLCData, int cchData);

    private const uint LOCALE_SNAME = 0x0000005c;
    private const string LOCALE_NAME_USER_DEFAULT = null;
    private const string LOCALE_NAME_SYSTEM_DEFAULT = "!x-sys-default-locale";

    private const int BUFFER_SIZE = 530;


    public static CultureInfo GetCurrentCulture()
    {
        var name = InvokeGetLocaleInfoEx(LOCALE_NAME_USER_DEFAULT, LOCALE_SNAME);

        if (name == null)
        {
            name = InvokeGetLocaleInfoEx(LOCALE_NAME_SYSTEM_DEFAULT, LOCALE_SNAME);

            if (name == null)
                return CultureInfo.InvariantCulture;
        }

        return new CultureInfo(name);
    }


    private static string InvokeGetLocaleInfoEx(string lpLocaleName, uint LCType)
    {
        char* lcData = stackalloc char[BUFFER_SIZE];
        int result = GetLocaleInfoEx(lpLocaleName, LCType, lcData, BUFFER_SIZE);
        return new(lcData);
    }
}
