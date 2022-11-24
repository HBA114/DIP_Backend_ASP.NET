namespace DIP_Backend.Dtos;

public class ImageDataDto
{
    public string? base64ImageData { get; set; }
    public string? fileType { get; set; }
    public Dictionary<int,int>? histogram { get; set; }
}