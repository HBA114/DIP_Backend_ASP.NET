using DIP_Backend.Entities;
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
        string base64Image = imageData.base64ImageData;

        byte[] imageArray = Convert.FromBase64String(base64Image);

        SKBitmap bitmap = SKBitmap.Decode(imageArray);

        int height = bitmap.Height;
        int width = bitmap.Width;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int sum = 0;
                sum += bitmap.GetPixel(i, j).Red;
                sum += bitmap.GetPixel(i, j).Green;
                sum += bitmap.GetPixel(i, j).Blue;
                int mean = sum / 3;
                bitmap.SetPixel(i, j, new SKColor((byte)mean, (byte)mean, (byte)mean));
            }
        }

        FileStream output = File.OpenWrite("modifiedImage.jpeg");
        SKManagedWStream outputStream = new SKManagedWStream(output);

        bitmap.Encode(outputStream, format: SKEncodedImageFormat.Jpeg, quality: 100);

        outputStream.Dispose();
        output.Close();

        byte[] imageArray1 = File.ReadAllBytes("modifiedImage.jpeg");

        imageData.base64ModifiedImageData = Convert.ToBase64String(imageArray1);
        return imageData;
    }
}