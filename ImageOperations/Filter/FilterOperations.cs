using DIP_Backend.Entities;
using DIP_Backend.ImageOperations.BaseOperations;
using SkiaSharp;

namespace DIP_Backend.ImageOperations.Filter;

public class FilterOperations
{
    public FilterOperations()
    {
    }

    public ImageData GaussianBlur(ImageData imageData, double standartDeviation)
    {

        return imageData;
    }

    public ImageData Sharpness(ImageData imageData)
    {

        return imageData;
    }

    public ImageData EdgeDetect(ImageData imageData)
    {
        return imageData;
    }

    public ImageData Mean(ImageData imageData)
    {
        //! 6x6 Mean Filter
        SKBitmap bitmap = BitmapAndBase64.GetBitmap(imageData.base64ModifiedImageData);
        int x = bitmap.Width;
        int y = bitmap.Height;

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                //* gets every pixel
                // assuming gray image
                //! Edge pixels not calculation not implemented yet
                int sumRed = 0;
                int sumGreen = 0;
                int sumBlue = 0;
                if ((i > 3 && i < x - 3) && (j > 3 && j < y - 3))
                {
                    for (int ix = i - 3; ix < i + 3; ix++)
                    {
                        for (int jy = j - 3; jy < j + 3; jy++)
                        {
                            sumRed += bitmap.GetPixel(ix, jy).Red;
                            sumGreen += bitmap.GetPixel(ix, jy).Green;
                            sumBlue += bitmap.GetPixel(ix, jy).Blue;
                        }
                    }
                }
                if (sumRed != 0)
                    bitmap.SetPixel(i, j, new SKColor((byte)(sumRed / 36), (byte)(sumGreen / 36), (byte)(sumBlue / 36)));
            }
        }

        imageData.base64ModifiedImageData = BitmapAndBase64.GetBase64Image(bitmap, imageData);

        return imageData;
    }
}