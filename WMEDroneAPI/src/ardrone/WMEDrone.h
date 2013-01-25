#define WMEDRONEDLL_API __declspec(dllexport) 

namespace ARDrone2
{
// This class is exported from the MathFuncsDll.dll
	class WMEDrone
	{
	public: 
		// Returns a + b
		static WMEDRONEDLL_API int Open(); 

		// Returns a - b
		static WMEDRONEDLL_API void Close(); 

		// Returns a * b
		static WMEDRONEDLL_API void ShowVideo(); 

		// Returns a / b
		// Throws const std::invalid_argument& if b is 0
		static WMEDRONEDLL_API IplImage* GetImage(); 
	};
}
