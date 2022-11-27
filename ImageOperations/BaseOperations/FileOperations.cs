using SkiaSharp;

namespace DIP_Backend.ImageOperations.BaseOperations;

public static class FileOperations
{
    public static async Task SaveImageWithFormat(SKBitmap bitmap, string filePath)
    {
        await Task.Run(() =>
        {
            string outputFileName = filePath;
            string fileType = filePath.Split(".").Last();

            SKEncodedImageFormat format;

            FileStream output = File.OpenWrite(outputFileName);
            if (fileType == "jpeg" || fileType == "jpg")
                format = SKEncodedImageFormat.Jpeg;
            else if (fileType == "png")
                format = SKEncodedImageFormat.Png;
            else
                format = SKEncodedImageFormat.Bmp;

            SKManagedWStream outputStream = new SKManagedWStream(output);

            bitmap.Encode(outputStream, format: format, quality: 100);

            outputStream.Dispose();
            output.Close();
        });
    }
}