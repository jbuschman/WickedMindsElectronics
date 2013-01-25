// -----------------------------------------------------------------------
// <copyright file="WMEVideoAPI.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace WME.ARDrone
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Drawing;
    using System.Windows.Media.Imaging;
    using System.IO;
    using System.Windows.Media;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class WMEVideoAPI
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IplImage
        {
            public int nSize;             /* sizeof(IplImage) */
            public int ID;                /* version (=0)*/
            public int nChannels;         /* Most of OpenCV functions support 1,2,3 or 4 channels */
            public int alphaChannel;      /* Ignored by OpenCV */
            public int depth;             /* Pixel depth in bits: IPL_DEPTH_8U, IPL_DEPTH_8S, IPL_DEPTH_16S,
                                       IPL_DEPTH_32S, IPL_DEPTH_32F and IPL_DEPTH_64F are supported.  */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public System.SByte[] colorModel;     /* Ignored by OpenCV */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public System.SByte[] channelSeq;     /* ditto */
            public int dataOrder;         /* 0 - interleaved color channels, 1 - separate color channels.
                                       cvCreateImage can only create interleaved images */
            public int origin;            /* 0 - top-left origin,
                                       1 - bottom-left origin (Windows bitmaps style).  */
            public int align;             /* Alignment of image rows (4 or 8).
                                       OpenCV ignores it and uses widthStep instead.    */
            public int width;             /* Image width in pixels.                           */
            public int height;            /* Image height in pixels.                          */
            public int pROI;    /* Image ROI. If NULL, the whole image is selected. */
            public int pMaskROI;      /* Must be NULL. */
            public int pImageId;                 /* "           " */
            public int pTileInfo;  /* "           " */
            public int imageSize;         /* Image data size in bytes
                                       (==image->height*image->widthStep
                                       in case of interleaved data)*/
            public uint pimageData;        /* Pointer to aligned image data.         */
            public int widthStep;         /* Size of aligned image row in bytes.    */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] BorderMode;     /* Ignored by OpenCV.                     */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] BorderConst;    /* Ditto.                                 */
            public uint pImageDataOrigin;  /* Pointer to very origin of image data
                                       (not necessarily aligned) -
                                       needed for correct deallocation */
            public static IplImage FromIntPtr(IntPtr ptr)
            {
                return (WMEVideoAPI.IplImage)Marshal.PtrToStructure(ptr, typeof(WMEVideoAPI.IplImage));
            }

            public static IplImage FromPtr32(int ptr)
            {
                return (WMEVideoAPI.IplImage)Marshal.PtrToStructure(new IntPtr(ptr), typeof(WMEVideoAPI.IplImage));
            }

            public static IplImage FromDrone()
            {
                return FromPtr32(WMEVideoAPI.DroneGetImage());
            }

            public byte[] imageData
            {
                get
                {
                    byte[] data = new byte[this.imageSize];
                    Marshal.Copy(new IntPtr(this.pimageData), data, 0, data.Length);
                    return data;
                }
            }

            public int RawStride
            {
                get
                {
                    return (this.width * PixelFormats.Bgr24.BitsPerPixel + 7) / 8;
                }
            }
        }

        [DllImport("WMEDroneAPI.dll", EntryPoint = "?Open@WMEDrone@ARDrone2@@SAHXZ"), SuppressUnmanagedCodeSecurity]
        public static extern int DroneOpen();

        [DllImport("WMEDroneAPI.dll", EntryPoint = "?GetImage@WMEDrone@ARDrone2@@SAPAU_IplImage@@XZ"), SuppressUnmanagedCodeSecurity]
        public static extern int DroneGetImage();

        [DllImport("WMEDroneAPI.dll", EntryPoint = "?ShowVideo@WMEDrone@ARDrone2@@SAXXZ"), SuppressUnmanagedCodeSecurity]
        public static extern int DroneShowVideo();

        [DllImport("WMEDroneAPI.dll", EntryPoint = "?Close@WMEDrone@ARDrone2@@SAXXZ"), SuppressUnmanagedCodeSecurity]
        public static extern void DroneClose();

        public static Bitmap BitmapSourceToBitmap(BitmapSource imageSource)
        {
            int width = imageSource.PixelWidth;
            int height = imageSource.PixelHeight;
            int stride = width * ((imageSource.Format.BitsPerPixel + 7) / 8);

            // byte[] bits = new byte[height * stride];

            MemoryStream strm = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageSource));
            encoder.Save(strm);

            return new System.Drawing.Bitmap(strm);
        }

    }
}
