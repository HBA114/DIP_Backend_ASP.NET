using DIP_Backend.Enums;

namespace DIP_Backend.Dtos;

public class PreProcessing1Dto
{
    public PreProcessing1Types operationType { get; set; }
    public int? tresholdValue { get; set; }
}
