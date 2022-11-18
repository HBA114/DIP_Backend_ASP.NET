using DIP_Backend.Entities;

namespace DIP_Backend.Repositories;

public class InMemoryImageRepository
{
    ImageData imageData;
    public InMemoryImageRepository()
    {
        imageData = new ImageData();
    }

    public ImageData SetImageData(string data)
    {
        imageData.base64ImageData = data;
        imageData.base64ModifiedImageData = data;

        return imageData;
    }

    public ImageData GetImageData()
    {
        return imageData;
    }
}