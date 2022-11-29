using SkiaSharp;

namespace DIP_Backend.ImageOperations.BaseOperations;

public static class FileOperations
{
    public static async Task SaveImage(string base64Image, string filePath)
    {
        await Task.Run(() =>
        {
            byte[] bytes = Convert.FromBase64String(base64Image);

            using (var imageFile = new FileStream(filePath, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }
        });
    }
}