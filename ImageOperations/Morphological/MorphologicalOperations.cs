using DIP_Backend.Entities;
using DIP_Backend.ImageOperations.BaseOperations;
using SkiaSharp;

namespace DIP_Backend.ImageOperations.Morphological;

public class MorphologicalOperations
{
    public MorphologicalOperations()
    {
    }

    public async Task<ImageData> Erosion(ImageData imageData)
    {
        //! Give Black & White image and test it with that
        SKBitmap bitmap = BitmapAndBase64.GetBitmap(imageData.base64ImageData);
        SKBitmap bitmapOriginal = BitmapAndBase64.GetBitmap(imageData.base64ImageData);
        int x = bitmap.Width;
        int y = bitmap.Height;

        await Task.Run(() =>
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (i >= 1 && i <= x - 1 && j >= 1 && j <= y - 1)
                    {
                        bool isAllPart = true;
                        //! assuming black and white image and taking only red pixels value
                        int pixelValue = bitmapOriginal.GetPixel(i, j).Red;
                        for (int ix = i - 1; ix < i + 1; ix++)
                        {
                            for (int jy = j - 1; jy < j + 1; jy++)
                            {
                                if (bitmapOriginal.GetPixel(ix, jy).Red != pixelValue)
                                {
                                    isAllPart = false;
                                }
                            }
                        }

                        if (!isAllPart)
                        {
                            bitmap.SetPixel(i, j, SKColors.White);
                        }
                    }
                }
            }
        });

        imageData.base64ModifiedImageData = BitmapAndBase64.GetBase64Image(bitmap, imageData);

        return imageData;
    }

    public async Task<ImageData> Dilation(ImageData imageData)
    {
        //! Give Black & White image and test it with that
        SKBitmap bitmap = BitmapAndBase64.GetBitmap(imageData.base64ImageData);
        SKBitmap bitmapOriginal = BitmapAndBase64.GetBitmap(imageData.base64ImageData);
        int x = bitmap.Width;
        int y = bitmap.Height;

        await Task.Run(() =>
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (i >= 1 && i <= x - 1 && j >= 1 && j <= y - 1)
                    {
                        //! assuming black and white image and taking only red pixels value
                        int pixelValue = bitmapOriginal.GetPixel(i, j).Red;
                        for (int ix = i - 1; ix < i + 1; ix++)
                        {
                            for (int jy = j - 1; jy < j + 1; jy++)
                            {
                                if (pixelValue < 100)
                                    bitmap.SetPixel(ix, jy, new SKColor((byte)pixelValue, (byte)pixelValue, (byte)pixelValue));
                            }
                        }
                    }
                }
            }
        });

        imageData.base64ModifiedImageData = BitmapAndBase64.GetBase64Image(bitmap, imageData);

        return imageData;
    }
}