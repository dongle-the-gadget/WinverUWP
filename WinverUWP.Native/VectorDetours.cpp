#include "pch.h"
#include <detours/detours.h>
#include "VectorDetours.h"

using namespace WinverUWP::Native;
using namespace Platform;

const DWORD one = 1;

typedef LONG
(WINAPI* RegQueryValueExFunction)(
	_In_ HKEY hKey,
	_In_opt_ LPCWSTR lpValueName,
	_Reserved_ LPDWORD lpReserved,
	_Out_opt_ LPDWORD lpType,
	_Out_writes_bytes_to_opt_(*lpcbData, *lpcbData) __out_data_source(REGISTRY) LPBYTE lpData,
	_When_(lpData == NULL, _Out_opt_) _When_(lpData != NULL, _Inout_opt_) LPDWORD lpcbData
);

RegQueryValueExFunction originalFunc = nullptr;

LONG detourFunc(HKEY hKey, LPCWSTR lpValueName, LPDWORD lpReserved, LPDWORD lpType, LPBYTE lpData, LPDWORD lpcbData)
{
	// Thanks to Ahmed Walid (@AhmedWalid605 on Twitter and ahmed605 on GitHub) for finding this trick.
	if (lpValueName != nullptr &&
		(_wcsicmp(lpValueName, L"EnableWUCShapes") == 0 ||
		_wcsicmp(lpValueName, L"EnableContainerVisuals") == 0 ||
		_wcsicmp(lpValueName, L"EnableSpriteVisuals") == 0 ||
		_wcsicmp(lpValueName, L"SpriteVisualsTestMode") == 0))
	{
		*lpData = one;
		*lpcbData = sizeof(one);
		return ERROR_SUCCESS;
	}

	return originalFunc(hKey, lpValueName, lpReserved, lpType, lpData, lpcbData);
}

Boolean VectorDetours::EnableVectorRendering()
{
	auto module = GetModuleHandleW(L"kernelbase.dll");
	if (!module)
		return false;
	originalFunc = (RegQueryValueExFunction)GetProcAddress(module, "RegQueryValueExW");
	if (!originalFunc)
		return false;
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	auto detourResult = DetourAttach(&(PVOID&)originalFunc, detourFunc);
	DetourTransactionCommit();
	return detourResult == NO_ERROR;
}