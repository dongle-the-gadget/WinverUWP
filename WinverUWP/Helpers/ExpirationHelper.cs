using System;

namespace WinverUWP.Helpers;

public static class ExpirationHelper
{
    // Thanks to @dhrdlicka for the code
    public unsafe static DateTime? GetSystemExpiration() => *(long*)0x7ffe02c8 switch
    {
        0 => null,
        var x => DateTime.FromFileTime(x)
    };
}