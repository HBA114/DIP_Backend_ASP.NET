using DIP_Backend.Dtos;
using DIP_Backend.Entities;
using SkiaSharp;

namespace DIP_Backend.ImageOperations.PreProcessing2;

public class HistogramOperations
{
    public HistogramOperations()
    {
    }

    public ImageDataDto ShowHistogram(ImageData imageData)
    {
        Dictionary<int, int> _histogram = new Dictionary<int, int>();
        string base64Image = imageData.base64ModifiedImageData;
        byte[] imageArray = Convert.FromBase64String(base64Image);

        SKBitmap bitmap = SKBitmap.Decode(imageArray);

        int height = bitmap.Height;
        int width = bitmap.Width;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int pixelValue = bitmap.GetPixel(i, j).Red;
                if (_histogram.ContainsKey(pixelValue))
                    _histogram[pixelValue] += 1;
                else
                    _histogram.Add(pixelValue, 1);
                
                if (pixelValue != 0 && pixelValue != 255){
                    Console.Beep();
                }
            }
        }

        ImageDataDto imageDataDto = new()
        {
            base64ImageData = imageData.base64ModifiedImageData,
            histogram = _histogram
        };
        return imageDataDto;
    }
}