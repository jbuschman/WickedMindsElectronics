#include "ardrone/ardrone.h"
#include "ardrone/WMEDrone.h"
#include <stdexcept>

using namespace std;

namespace ARDrone2
{
	// AR.Drone class
	ARDrone ardrone;

    int WMEDrone::Open()
    {
		// Initialize
		if (!ardrone.open()) {
			// printf("Failed to initialize.\n");
			return -1;
		}
		return 0;
    }

    void WMEDrone::Close()
    {
		// See you
		ardrone.close();
    }

    void WMEDrone::ShowVideo()
    {
		// Main loop
		while (!GetAsyncKeyState(VK_ESCAPE)) 
		{
			// Update
			if (!ardrone.update()) break;

			// Get an image
			IplImage *image = ardrone.getImage();
			  
			// Display the image
			cvShowImage("camera", image);
	        cvWaitKey(1);
		}
    }

    IplImage* WMEDrone::GetImage()
    {
		// Get an image
        return ardrone.getImage();
    }
}