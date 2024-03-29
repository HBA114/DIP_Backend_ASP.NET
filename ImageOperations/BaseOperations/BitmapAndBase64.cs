using DIP_Backend.Entities;

using SkiaSharp;

namespace DIP_Backend.ImageOperations.BaseOperations;

public static class BitmapAndBase64
{
    public static string GetBase64Image(SKBitmap bitmap, ImageData imageData)
    {
        // string fileType = imageData.fileType!;
        string outputFileName = "modifiedImage." + "jpeg";

        SKEncodedImageFormat format = SKEncodedImageFormat.Jpeg;
        // SkiaSharp not supports bmp format. So base64 image operations runs on jpeg format

        FileStream output = File.OpenWrite(outputFileName);

        SKManagedWStream outputStream = new SKManagedWStream(output);

        bitmap.Encode(outputStream, format: format, quality: 100);

        outputStream.Dispose();
        output.Close();

        byte[] imageArray1 = File.ReadAllBytes(outputFileName);

        File.Delete(outputFileName);
        return Convert.ToBase64String(imageArray1);
    }

    public static SKBitmap GetBitmap(string base64Image)
    {
        byte[] imageArray = Convert.FromBase64String(base64Image);

        return SKBitmap.Decode(imageArray);
    }
}
