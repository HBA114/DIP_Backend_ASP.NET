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
        Dictionary<int, int> _histogramRed = new Dictionary<int, int>();
        Dictionary<int, int> _histogramGreen = new Dictionary<int, int>();
        Dictionary<int, int> _histogramBlue = new Dictionary<int, int>();
        
        SKBitmap bitmap = BitmapAndBase64.GetBitmap(imageData.base64ModifiedImageData);

        int x = bitmap.Width;
        int y = bitmap.Height;

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                int pixelValueRed = bitmap.GetPixel(i, j).Red;
                int pixelValueGreen = bitmap.GetPixel(i, j).Green;
                int pixelValueBlue = bitmap.GetPixel(i, j).Blue;
                if (_histogramRed.ContainsKey(pixelValueRed))
                    _histogramRed[pixelValueRed] += 1;
                else
                    _histogramRed.Add(pixelValueRed, 1);
                
                if (_histogramGreen.ContainsKey(pixelValueGreen))
                    _histogramGreen[pixelValueGreen] += 1;
                else
                    _histogramGreen.Add(pixelValueGreen, 1);
                
                if (_histogramBlue.ContainsKey(pixelValueBlue))
                    _histogramBlue[pixelValueBlue] += 1;
                else
                    _histogramBlue.Add(pixelValueBlue, 1);
            }
        }
        _histogramRed = _histogramRed.OrderBy(h => h.Key).ToDictionary(h => h.Key, h => h.Value);
        imageData.histogramRed = _histogramRed;
        imageData.histogramGreen = _histogramGreen;
        imageData.histogramBlue = _histogramBlue;
        return imageData;
    }

    public ImageData HistogramEqualization(ImageData imageData)
    {
        // string base64Image = imageData.base64ImageData;
        // byte[] imageArray = Convert.FromBase64String(base64Image);

        Dictionary<int, int> _histogramRed = ShowHistogram(imageData).histogramRed!;
        Dictionary<int, int> _histogramGreen = ShowHistogram(imageData).histogramGreen!;
        Dictionary<int, int> _histogramBlue = ShowHistogram(imageData).histogramBlue!;
        //! 

        // SKBitmap bitmap = SKBitmap.Decode(imageArray);
        SKBitmap bitmap = BitmapAndBase64.GetBitmap(imageData.base64ImageData);

        List<HistogramEqualizeTable> table = new List<HistogramEqualizeTable>();

        int x = bitmap.Width;
        int y = bitmap.Height;
    
        //! Transfer Function
        //! Probability of that gray Scale in whole image (nk / (height*width))
        int cumulativeSum = 0;
        foreach (var element in _histogramRed)
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

        imageData.base64ModifiedImageData = BitmapAndBase64.GetBase64Image(bitmap, imageData);
        imageData = ShowHistogram(imageData);
        return imageData;
    }
}