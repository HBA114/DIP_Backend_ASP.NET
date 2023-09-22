namespace DIP_Backend.Dtos;

public record ImageDataDto(string? base64ImageData, string? filePath, string? fileType, Dictionary<int,int>? histogram);
