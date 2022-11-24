using DIP_Backend.Entities;

namespace DIP_Backend.Repositories;

public class InMemoryImageRepository
{
    // ImageData imageData;
    public ImageData imageData { get; set; }
    public InMemoryImageRepository()
    {
        imageData = new ImageData();
    }

    public ImageData SetImageData(string data, string modifiedData, string fileType)
    {
        imageData.base64ImageData = data;
        imageData.base64ModifiedImageData = modifiedData;
        imageData.fileType = fileType;

        return imageData;
    }

    public ImageData GetImageData()
    {
        return imageData;
    }
}