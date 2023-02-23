using System.Runtime.InteropServices;
using System.Text;
using static WinverUWP.Helpers.Win32Headers;

namespace WinverUWP.Helpers
{
    public unsafe static class WinbrandHelper
    {
        public static string GetWinbrand()
        {
            fixed(char* pModuleName = "Winbrand.dll")
            {
                HMODULE module = LoadLibraryW((ushort*)pModuleName);
                fixed(byte* pProc = Encoding.ASCII.GetBytes("BrandingFormatString"))
                {
                    var func = (delegate* unmanaged[Stdcall]<ushort*, ushort*>)GetProcAddress(module, (sbyte*)pProc);
                    fixed (char* pFormatName = "%WINDOWS_LONG%")
                    {
                        ushort* branding = func((ushort*)pFormatName);
                        FreeLibrary(module);
                        return new string((char*)branding);
                    }
                }
            }
        }
    }
}
