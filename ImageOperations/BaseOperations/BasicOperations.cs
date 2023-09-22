using DIP_Backend.Entities;

using SkiaSharp;

namespace DIP_Backend.ImageOperations.BaseOperations;

public class BasicOperations
{
    public async Task SaveImageToFile(ImageData imageData, string filePath)
    {
        SKBitmap bitmap = BitmapAndBase64.GetBitmap(imageData.base64ModifiedImageData);
        await FileOperations.SaveImage(imageData.base64ModifiedImageData, filePath);
    }
}
