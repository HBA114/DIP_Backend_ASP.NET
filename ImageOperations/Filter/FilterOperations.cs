using DIP_Backend.Entities;
using DIP_Backend.ImageOperations.BaseOperations;
using SkiaSharp;

namespace DIP_Backend.ImageOperations.Filter;

public class FilterOperations
{
    public FilterOperations()
    {
    }

    public ImageData GaussianBlur(ImageData imageData, double standartDeviation)
    {

        return imageData;
    }

    public async Task<ImageData> Sharpening(ImageData imageData, int filterSize)
    {
        SKBitmap bitmap = BitmapAndBase64.GetBitmap(imageData.base64ModifiedImageData);
        SKBitmap bitmapOriginal = BitmapAndBase64.GetBitmap(imageData.base64ModifiedImageData);

        int filterSizeCalculated = (filterSize - 1) / 2;

        int x = bitmap.Width;
        int y = bitmap.Height;

        // int filterSize = 1; // 3x3

        await Task.Run(() =>
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    int sumRed = 0;
                    int sumGreen = 0;
                    int sumBlue = 0;
                    if (i > filterSizeCalculated && i < x - filterSizeCalculated && j > filterSizeCalculated && j < y - filterSizeCalculated)
                    {
                        for (int ix = i - filterSizeCalculated; ix <= i + filterSizeCalculated; ix++)
                        {
                            for (int jy = j - filterSizeCalculated; jy <= j + filterSizeCalculated; jy++)
                            {
                                if ((ix >= 0 && ix < x) && (jy >= 0 && jy < y))
                                {
                                    if (ix == i && jy == j)
                                    {
                                        sumRed += (filterSize * filterSize) * bitmapOriginal.GetPixel(ix, jy).Red;
                                        sumGreen += (filterSize * filterSize) * bitmapOriginal.GetPixel(ix, jy).Green;
                                        sumBlue += (filterSize * filterSize) * bitmapOriginal.GetPixel(ix, jy).Blue;
                                    }
                                    else
                                    {
                                        sumRed -= bitmapOriginal.GetPixel(ix, jy).Red;
                                        sumGreen -= bitmapOriginal.GetPixel(ix, jy).Green;
                                        sumBlue -= bitmapOriginal.GetPixel(ix, jy).Blue;
                                    }
                                    // else if ((ix == i - 1 || ix == i + 1) && (jy == j - 1 || jy == j + 1))
                                    // {
                                    //     sumRed -= bitmapOriginal.GetPixel(ix, jy).Red;
                                    //     sumGreen -= bitmapOriginal.GetPixel(ix, jy).Green;
                                    //     sumBlue -= bitmapOriginal.GetPixel(ix, jy).Blue;
                                    // }
                                }
                            }
                        }
                        bitmap.SetPixel(i, j, new SKColor((byte)(sumRed), (byte)(sumGreen), (byte)(sumBlue)));
                    }
                }
            }
        });

        imageData.base64ModifiedImageData = BitmapAndBase64.GetBase64Image(bitmap, imageData);

        return imageData;
    }

    public ImageData EdgeDetect(ImageData imageData)
    {
        return imageData;
    }

    public async Task<ImageData> Mean(ImageData imageData, int filterSize)
    {
        SKBitmap bitmap = BitmapAndBase64.GetBitmap(imageData.base64ModifiedImageData);
        SKBitmap originalBitmap = BitmapAndBase64.GetBitmap(imageData.base64ModifiedImageData);

        int filterSizeCalculated = (filterSize - 1) / 2;

        int x = bitmap.Width;
        int y = bitmap.Height;

        await Task.Run(() =>
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    int sumRed = 0;
                    int sumGreen = 0;
                    int sumBlue = 0;
                    int readedPixelCount = 0;
                    for (int ix = i - filterSizeCalculated; ix <= i + filterSizeCalculated; ix++)
                    {
                        for (int jy = j - filterSizeCalculated; jy <= j + filterSizeCalculated; jy++)
                        {
                            if ((ix >= 0 && ix < x) && (jy >= 0 && jy < y))
                            {
                                sumRed += originalBitmap.GetPixel(ix, jy).Red;
                                sumGreen += originalBitmap.GetPixel(ix, jy).Green;
                                sumBlue += originalBitmap.GetPixel(ix, jy).Blue;
                                readedPixelCount++;
                            }
                        }
                    }
                    bitmap.SetPixel(i, j, new SKColor((byte)(sumRed / readedPixelCount), (byte)(sumGreen / readedPixelCount), (byte)(sumBlue / readedPixelCount)));
                }
            }
        });

        imageData.base64ModifiedImageData = BitmapAndBase64.GetBase64Image(bitmap, imageData);

        return imageData;
    }

    public async Task<ImageData> Median(ImageData imageData, int filterSize)
    {
        int filterSizeCalculated = (filterSize - 1) / 2;

        SKBitmap bitmap = BitmapAndBase64.GetBitmap(imageData.base64ModifiedImageData);
        SKBitmap originalBitmap = BitmapAndBase64.GetBitmap(imageData.base64ModifiedImageData);
        int x = bitmap.Width;
        int y = bitmap.Height;

        await Task.Run(() =>
        {
            for (int i = filterSizeCalculated; i < x - filterSizeCalculated; i++)
            {
                for (int j = filterSizeCalculated; j < y - filterSizeCalculated; j++)
                {
                    List<int> listRed = new List<int>();
                    List<int> listGreen = new List<int>();
                    List<int> listBlue = new List<int>();
                    for (int ix = i - filterSizeCalculated; ix < i + filterSizeCalculated; ix++)
                    {
                        for (int jy = j - filterSizeCalculated; jy < j + filterSizeCalculated; jy++)
                        {
                            listRed.Add(originalBitmap.GetPixel(ix, jy).Red);
                            listGreen.Add(originalBitmap.GetPixel(ix, jy).Green);
                            listBlue.Add(originalBitmap.GetPixel(ix, jy).Blue);
                        }
                    }

                    listRed.Sort();
                    listGreen.Sort();
                    listBlue.Sort();
                    int newRed = 0, newGreen = 0, newBlue = 0;

                    if (listRed.Count % 2 == 0)
                        newRed = (listRed[listRed.Count / 2] + listRed[(listRed.Count / 2) + 1]) / 2;
                    else
                        newRed = listRed[listRed.Count / 2];

                    if (listGreen.Count % 2 == 0)
                        newGreen = (listGreen[listGreen.Count / 2] + listGreen[(listGreen.Count / 2) + 1]) / 2;
                    else
                        newGreen = listGreen[listGreen.Count / 2];

                    if (listBlue.Count % 2 == 0)
                        newBlue = (listBlue[listBlue.Count / 2] + listBlue[(listBlue.Count / 2) + 1]) / 2;
                    else
                        newBlue = listBlue[listBlue.Count / 2];

                    bitmap.SetPixel(i, j, new SKColor((byte)(newRed), (byte)(newGreen), (byte)(newBlue)));
                }
            }
        });

        imageData.base64ModifiedImageData = BitmapAndBase64.GetBase64Image(bitmap, imageData);
        return imageData;
    }

    public async Task<ImageData> ContraHarmonical(ImageData imageData, int filterSize)
    {
        SKBitmap bitmap = BitmapAndBase64.GetBitmap(imageData.base64ImageData);
        SKBitmap originalBitmap = BitmapAndBase64.GetBitmap(imageData.base64ImageData);

        // int filterSizeCalculated = (filterSize - 1) / 2;
        int filterSizeCalculated = 1;
        int Q = -1;
        int QTop = Math.Abs(Q + 1);
        int QSub = Math.Abs(Q);

        int x = bitmap.Width;
        int y = bitmap.Height;

        await Task.Run(() =>
        {
            for (int i = filterSizeCalculated; i < x - filterSizeCalculated; i++)
            {
                for (int j = filterSizeCalculated; j < y - filterSizeCalculated; j++)
                {
                    //! assuming blach and white salt or pepper noise image
                    int sumRed = 0;
                    int sumGreen = 0;
                    int sumBlue = 0;

                    int sumRedS = 0;
                    int sumGreenS = 0;
                    int sumBlueS = 0;
                    for (int ix = i - filterSizeCalculated; ix <= i + filterSizeCalculated; ix++)
                    {
                        for (int jy = j - filterSizeCalculated; jy <= j + filterSizeCalculated; jy++)
                        {
                            // if ((ix >= 0 && ix < x) && (jy >= 0 && jy < y))
                            // {
                            sumRed += int.Parse(Math.Round(Math.Pow(originalBitmap.GetPixel(ix, jy).Red, QTop)).ToString()) == 0 ? 1 : int.Parse(Math.Round(Math.Pow(originalBitmap.GetPixel(ix, jy).Red, QTop)).ToString());
                            sumGreen += int.Parse(Math.Round(Math.Pow(originalBitmap.GetPixel(ix, jy).Green, QTop)).ToString()) == 0 ? 1 : int.Parse(Math.Round(Math.Pow(originalBitmap.GetPixel(ix, jy).Green, QTop)).ToString());
                            sumBlue += int.Parse(Math.Round(Math.Pow(originalBitmap.GetPixel(ix, jy).Blue, QTop)).ToString()) == 0 ? 1 : int.Parse(Math.Round(Math.Pow(originalBitmap.GetPixel(ix, jy).Blue, QTop)).ToString());

                            sumRedS += int.Parse(Math.Round(Math.Pow(originalBitmap.GetPixel(ix, jy).Red, QSub)).ToString()) == 0 ? 1 : int.Parse(Math.Round(Math.Pow(originalBitmap.GetPixel(ix, jy).Red, QSub)).ToString());
                            sumGreenS += int.Parse(Math.Round(Math.Pow(originalBitmap.GetPixel(ix, jy).Green, QSub)).ToString()) == 0 ? 1 : int.Parse(Math.Round(Math.Pow(originalBitmap.GetPixel(ix, jy).Green, QSub)).ToString());
                            sumBlueS += int.Parse(Math.Round(Math.Pow(originalBitmap.GetPixel(ix, jy).Blue, QSub)).ToString()) == 0 ? 1 : int.Parse(Math.Round(Math.Pow(originalBitmap.GetPixel(ix, jy).Blue, QSub)).ToString());
                            // }
                        }
                    }
                    int newRed = 0;
                    int newGreen = 0;
                    int newBlue = 0;
                    if (Q + 1 < 0)
                    {
                        newRed = sumRedS / sumRed;
                        newGreen = sumGreenS / sumGreen;
                        newBlue = sumBlueS / sumBlue;
                    }
                    else if (Q + 1 == 0)
                    {
                        newRed = sumRed * sumRedS;
                        newGreen = sumGreen * sumGreenS;
                        newBlue = sumBlue * sumBlueS;
                    }
                    else
                    {
                        newRed = sumRed / sumRedS;
                        newGreen = sumGreen / sumGreenS;
                        newBlue = sumBlue / sumBlueS;
                    }
                    bitmap.SetPixel(i, j, new SKColor((byte)(newRed), (byte)(newGreen), (byte)(newBlue)));

                }
            }
        });

        imageData.base64ModifiedImageData = BitmapAndBase64.GetBase64Image(bitmap, imageData);

        return imageData;
    }

}