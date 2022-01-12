#include "pch.h"
#include "Winbrand.h"

Platform::String^ WinverNative::Winbrand::BrandingFormatString(Platform::String^ format) 
{
	typedef HMODULE(WINAPI* pLoadLibrary)(_In_ LPCTSTR);
	static const auto procLoadLibrary = reinterpret_cast<pLoadLibrary>(GetProcAddress("Kernel32.dll", "LoadLibraryW")); //GetProcAddress is a part of RegistryRT, include it from there
	auto hModuleWinbrand = procLoadLibrary(L"WINBRAND.DLL");
	auto brandingFormatStringDelegate = (WinverNative::Winbrand::BRANDINGFORMATSTRING)GetProcAddress(hModuleWinbrand, "BrandingFormatString");
	auto test = brandingFormatStringDelegate(LPWSTR(format->Data()));
	FreeLibrary(hModuleWinbrand);
	Platform::String^ castedString = ref new Platform::String(test);
	return castedString;
}