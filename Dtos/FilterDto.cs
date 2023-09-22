using DIP_Backend.Enums;

namespace DIP_Backend.Dtos;

public record FilterDto(FilterType filterType, int filterSize, int? standardDeviation);
