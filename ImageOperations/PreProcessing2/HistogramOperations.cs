using DIP_Backend.Dtos;
using DIP_Backend.Entities;
using DIP_Backend.ImageOperations.BaseOperations;
using SkiaSharp;

namespace DIP_Backend.ImageOperations.PreProcessing2;

public class HistogramOperations
{
    public HistogramOperations()
    {
    }

    public ImageData ShowHistogram(ImageData imageData)
    {
        Dictionary<int, int> _histogram = new Dictionary<int, int>();
        string base64Image = imageData.base64ImageData;
        imageData.base64ModifiedImageData = base64Image;
        byte[] imageArray = Convert.FromBase64String(base64Image);

        SKBitmap bitmap = SKBitmap.Decode(imageArray);

        int x = bitmap.Width;
        int y = bitmap.Height;

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                int pixelValue = bitmap.GetPixel(i, j).Red;
                if (_histogram.ContainsKey(pixelValue))
                    _histogram[pixelValue] += 1;
                else
                    _histogram.Add(pixelValue, 1);

                // if (pixelValue != 0 && pixelValue != 255){
                //     Console.WriteLine("Not Good!");
                // }
            }
        }
        _histogram = _histogram.OrderBy(h => h.Key).ToDictionary(h => h.Key, h => h.Value);
        imageData.histogram = _histogram;
        return imageData;
    }

    public ImageData HistogramEqualization(ImageData imageData)
    {
        string base64Image = imageData.base64ImageData;
        byte[] imageArray = Convert.FromBase64String(base64Image);

        Dictionary<int, int> _histogram = ShowHistogram(imageData).histogram!;

        SKBitmap bitmap = SKBitmap.Decode(imageArray);

        List<HistogramEqualizeTable> table = new List<HistogramEqualizeTable>();

        int x = bitmap.Width;
        int y = bitmap.Height;

        //! Transfer Function
        //! Probability of that gray Scale in whole image (nk / (height*width))
        int cumulativeSum = 0;
        foreach (var element in _histogram)
        {
            cumulativeSum += element.Value;
            table.Add(new()
            {
                GrayTone = element.Key,
                CumulativeSummary = cumulativeSum,
                CumulativeProbability = Math.Round((double)cumulativeSum / (double)(y * x), 2),
                NewGrayTone = int.Parse(Math.Round(((table.Count - 1) * Math.Round((double)cumulativeSum / (double)(y * x), 2))).ToString()),
            });
        }
        table.OrderBy(x => x.GrayTone);

        Dictionary<int, int> newHistogram = new Dictionary<int, int>();
        foreach (var item in table)
        {
            if (!newHistogram.ContainsKey(item.GrayTone))
            {
                newHistogram.Add(item.GrayTone, item.NewGrayTone);
            }
        }

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                // int newVal = table.Where(x => x.GrayTone == bitmap.GetPixel(i,j).Red).First().NewGrayTone;
                int newVal = newHistogram[bitmap.GetPixel(i, j).Red];
                bitmap.SetPixel(i, j, new SKColor((byte)newVal, (byte)newVal, (byte)newVal));
            }
        }

        foreach (var item in table)
        {
            Console.WriteLine("Tone : " + item.GrayTone + ", Cumulative Sum : " + item.CumulativeSummary +
            ", Cumulative Probability : " + item.CumulativeProbability + ", New Tone : " + item.NewGrayTone);
        }

        imageData.base64ModifiedImageData = BitmapToBase64.GetBase64Image(bitmap, imageData);
        return imageData;
    }
}