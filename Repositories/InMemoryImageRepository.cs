using DIP_Backend.Entities;

namespace DIP_Backend.Repositories;

public class InMemoryImageRepository
{
    private ImageData _imageData;
    public InMemoryImageRepository()
    {
        _imageData = new ImageData();
    }

    public ImageData SetImageData(string data, string modifiedData, string fileType)
    {
        _imageData.base64ImageData = data;
        _imageData.base64ModifiedImageData = modifiedData;
        _imageData.fileType = fileType;

        return _imageData;
    }

    public ImageData GetImageData()
    {
        return _imageData;
    }
}
