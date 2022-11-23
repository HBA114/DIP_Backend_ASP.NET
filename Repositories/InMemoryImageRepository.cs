using DIP_Backend.Entities;

namespace DIP_Backend.Repositories;

public class InMemoryImageRepository
{
    ImageData imageData;
    public InMemoryImageRepository()
    {
        imageData = new ImageData();
    }

    public ImageData SetImageData(string data, string modifiedData)
    {
        imageData.base64ImageData = data;
        imageData.base64ModifiedImageData = modifiedData;

        return imageData;
    }

    public ImageData GetImageData()
    {
        return imageData;
    }
}