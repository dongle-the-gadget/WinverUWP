#pragma once
namespace WinverNative {
	public ref class Winbrand sealed {
	private:
		typedef LPWSTR(*BRANDINGFORMATSTRING)(LPWSTR format);
	public:
		static Platform::String^ Winbrand::BrandingFormatString(Platform::String^ format);
	};
}
