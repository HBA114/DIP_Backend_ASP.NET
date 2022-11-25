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

    public ImageData Sharpness(ImageData imageData)
    {

        return imageData;
    }

    public ImageData EdgeDetect(ImageData imageData)
    {
        return imageData;
    }

    public ImageData Mean(ImageData imageData)
    {
        //! 6x6 Mean Filter
        SKBitmap bitmap = BitmapAndBase64.GetBitmap(imageData.base64ImageData);
        int x = bitmap.Width;
        int y = bitmap.Height;

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                //* gets every pixel
                int sumRed = 0;
                int sumGreen = 0;
                int sumBlue = 0;
                int readedPixelCount = 0;
                for (int ix = i - 4; ix <= i + 4; ix++)
                {
                    for (int jy = j - 4; jy <= j + 4; jy++)
                    {
                        if ((ix >= 0 && ix <= x) && (jy >= 0 && jy <= y))
                        {
                            sumRed += bitmap.GetPixel(ix, jy).Red;
                            sumGreen += bitmap.GetPixel(ix, jy).Green;
                            sumBlue += bitmap.GetPixel(ix, jy).Blue;
                            readedPixelCount++;
                        }
                    }
                }
                bitmap.SetPixel(i, j, new SKColor((byte)(sumRed / readedPixelCount), (byte)(sumGreen / readedPixelCount), (byte)(sumBlue / readedPixelCount)));
                // if ((i > 3 && i < x - 3) && (j > 3 && j < y - 3))
                // {
                //     for (int ix = i - 3; ix < i + 3; ix++)
                //     {
                //         for (int jy = j - 3; jy < j + 3; jy++)
                //         {
                //             sumRed += bitmap.GetPixel(ix, jy).Red;
                //             sumGreen += bitmap.GetPixel(ix, jy).Green;
                //             sumBlue += bitmap.GetPixel(ix, jy).Blue;
                //         }
                //     }
                // }
                // if (sumRed != 0)
                //     bitmap.SetPixel(i, j, new SKColor((byte)(sumRed / 36), (byte)(sumGreen / 36), (byte)(sumBlue / 36)));
            }
        }

        //! Code mostly can not reach here
        imageData.base64ModifiedImageData = BitmapAndBase64.GetBase64Image(bitmap, imageData);

        return imageData;
    }

    public ImageData Median(ImageData imageData)
    {
        SKBitmap bitmap = BitmapAndBase64.GetBitmap(imageData.base64ModifiedImageData);
        int x = bitmap.Width;
        int y = bitmap.Height;

        for (int i = 3; i < x - 3; i++)
        {
            for (int j = 3; j < y - 3; j++)
            {
                //* gets every pixel
                // assuming gray image
                //! Edge pixels not calculation not implemented yet
                List<int> listRed = new List<int>();
                List<int> listGreen = new List<int>();
                List<int> listBlue = new List<int>();
                for (int ix = i - 3; ix < i + 3; ix++)
                {
                    for (int jy = j - 3; jy < j + 3; jy++)
                    {
                        listRed.Add(bitmap.GetPixel(ix, jy).Red);
                        listGreen.Add(bitmap.GetPixel(ix, jy).Green);
                        listBlue.Add(bitmap.GetPixel(ix, jy).Blue);
                    }
                }

                listRed.Sort();
                listGreen.Sort();
                listBlue.Sort();
                int newRed = 0, newGreen = 0, newBlue = 0;

                if (listRed.Count % 2 == 0)
                {
                    newRed = (listRed[listRed.Count / 2] + listRed[(listRed.Count / 2) + 1]) / 2;
                }
                else
                {
                    newRed = listRed[listRed.Count / 2];
                }

                if (listGreen.Count % 2 == 0)
                {
                    newGreen = (listGreen[listGreen.Count / 2] + listGreen[(listGreen.Count / 2) + 1]) / 2;
                }
                else
                {
                    newGreen = listGreen[listGreen.Count / 2];
                }

                if (listBlue.Count % 2 == 0)
                {
                    newBlue = (listBlue[listBlue.Count / 2] + listBlue[(listBlue.Count / 2) + 1]) / 2;
                }
                else
                {
                    newBlue = listBlue[listBlue.Count / 2];
                }

                bitmap.SetPixel(i, j, new SKColor((byte)(newRed), (byte)(newGreen), (byte)(newBlue)));
            }
        }

        imageData.base64ModifiedImageData = BitmapAndBase64.GetBase64Image(bitmap, imageData);
        return imageData;
    }
}