using DIP_Backend.Enums;

namespace DIP_Backend.Dtos;

public class FilterDto
{
    public FilterType filterType { get; set; }
    public int? standartDeviation { get; set; }
}