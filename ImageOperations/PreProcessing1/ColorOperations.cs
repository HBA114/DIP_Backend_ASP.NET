using DIP_Backend.Entities;
using DIP_Backend.ImageOperations.BaseOperations;
using SkiaSharp;

namespace DIP_Backend.ImageOperations.PreProcessing1;

public class ColorOperations
{
    // Create Image Byte[] from Current Image Data base64
    public ColorOperations()
    {
    }

    public ImageData TurnToGrayScale(ImageData imageData)
    {
        //TODO: turn base64 image data to byte array (save then read as bytes) 
        //TODO: and modify pixel values
        // string base64Image = imageData.base64ImageData;

        // byte[] imageArray = Convert.FromBase64String(base64Image);

        // SKBitmap bitmap = SKBitmap.Decode(imageArray);
        SKBitmap bitmap = BitmapAndBase64.GetBitmap(imageData.base64ImageData);

        int x = bitmap.Width;
        int y = bitmap.Height;

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                int sum = 0;
                sum += bitmap.GetPixel(i, j).Red;
                sum += bitmap.GetPixel(i, j).Green;
                sum += bitmap.GetPixel(i, j).Blue;
                int mean = sum / 3;
                bitmap.SetPixel(i, j, new SKColor((byte)mean, (byte)mean, (byte)mean));
            }
        }

        imageData.base64ModifiedImageData = BitmapAndBase64.GetBase64Image(bitmap, imageData);
        return imageData;
    }

    public ImageData TurnToBlackAndWhiteByThresholdValue(ImageData imageData, int? tresholdValue)
    {
        imageData = TurnToGrayScale(imageData);

        // string base64Image = imageData.base64ModifiedImageData;

        // byte[] imageArray = Convert.FromBase64String(base64Image);

        // SKBitmap bitmap = SKBitmap.Decode(imageArray);
        SKBitmap bitmap = BitmapAndBase64.GetBitmap(imageData.base64ModifiedImageData);

        int x = bitmap.Width;
        int y = bitmap.Height;

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (bitmap.GetPixel(i, j).Green < tresholdValue)
                    bitmap.SetPixel(i, j, SKColors.Black);
                else
                    bitmap.SetPixel(i, j, SKColors.White);
            }
        }

        imageData.base64ModifiedImageData = BitmapAndBase64.GetBase64Image(bitmap, imageData);
        return imageData;
    }
}
