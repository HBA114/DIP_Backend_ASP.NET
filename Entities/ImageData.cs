namespace DIP_Backend.Entities;

public class ImageData
{
    public string base64ImageData { get; set; }
    public string base64ModifiedImageData { get; set; }
    public string? fileType { get; set; }
    public Dictionary<int, int>? histogramRed { get; set; }
    public Dictionary<int, int>? histogramGreen { get; set; }
    public Dictionary<int, int>? histogramBlue { get; set; }

    public ImageData()
    {
        base64ImageData = "";
        base64ModifiedImageData = "";
    }

    public ImageData(string base64ImageData, string base64ModifiedImageData)
    {
        this.base64ImageData = base64ImageData;
        this.base64ModifiedImageData = base64ModifiedImageData;
    }
}