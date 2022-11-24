using DIP_Backend.Enums;

namespace DIP_Backend.Dtos;

public class PreProcessing2Dto
{
    public PreProcessing2Types operationType { get; set; }
    public int? toneCount { get; set; }
}