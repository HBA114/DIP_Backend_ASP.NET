using DIP_Backend.Entities;
using SkiaSharp;

namespace DIP_Backend.ImageOperations.BaseOperations;

public class BasicOperations
{
    public BasicOperations()
    {
    }

    public async Task SaveImageToFile(ImageData imageData, string fileType)
    {
        SKBitmap bitmap = BitmapAndBase64.GetBitmap(imageData.base64ImageData);
        await FileOperations.SaveImageWithFormat(bitmap, fileType);
    }
}