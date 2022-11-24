using DIP_Backend.Dtos;
using DIP_Backend.Entities;
using DIP_Backend.Enums;
using DIP_Backend.ImageOperations.Filter;
using DIP_Backend.ImageOperations.PreProcessing1;
using DIP_Backend.ImageOperations.PreProcessing2;
using DIP_Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DIP_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    InMemoryImageRepository _imageRepository;
    ColorOperations _colorOperations;
    HistogramOperations _histogramOperations;
    FilterOperations _filterOPerations;

    public ImageController(InMemoryImageRepository imageRepository, ColorOperations colorOperations, HistogramOperations histogramOperations, FilterOperations filterOperations)
    {
        _imageRepository = imageRepository;
        _colorOperations = colorOperations;
        _histogramOperations = histogramOperations;
        _filterOPerations = filterOperations;
    }

    [HttpPost]
    public IActionResult SetImageData(ImageDataDto imageDataDto)
    {
        var result = _imageRepository.SetImageData(imageDataDto.base64ImageData!, imageDataDto.base64ImageData!, imageDataDto.fileType!);
        return Ok(result);
    }

    [HttpGet]
    public IActionResult GetImageData()
    {
        return Ok(_imageRepository.GetImageData());
    }

    [HttpPost("PreProcessing1")]
    public IActionResult ApplyPreProcessing1(PreProcessing1Dto preProcessing1Dto)
    {
        ImageData imageData = _imageRepository.GetImageData();
        ImageData result;

        switch (preProcessing1Dto.operationType)
        {
            case PreProcessing1Type.GrayScale:
                result = _colorOperations.TurnToGrayScale(imageData);
                break;
            case PreProcessing1Type.BlackWhite:
                result = _colorOperations.TurnToBlackAndWhiteByTresholdValue(imageData, preProcessing1Dto.tresholdValue);
                break;
            default:
                return BadRequest();
        }

        return Ok(result);
    }

    [HttpPost("PreProcessing2")]
    public IActionResult ApplyPreProcessing2(PreProcessing2Dto preProcessing2Dto)
    {
        ImageData imageData = _imageRepository.GetImageData();
        ImageData result;

        switch (preProcessing2Dto.operationType)
        {
            case PreProcessing2Type.ShowHistogram:
                result = _histogramOperations.ShowHistogram(imageData);
                break;
            case PreProcessing2Type.HistogramEqualization:
                result = _histogramOperations.HistogramEqualization(imageData);
                break;
            default:
                return BadRequest();
        }

        return Ok(result);
    }

    [HttpPost("Filters")]
    public IActionResult ApplyFilters(FilterDto filterDto)
    {
        ImageData imageData = _imageRepository.GetImageData();
        ImageData result;

        switch (filterDto.filterType)
        {
            case FilterType.GaussianBlur:
                result = _filterOPerations.GaussianBlur(imageData, 0);
                break;
            case FilterType.Sharpness:
                result = _filterOPerations.Sharpness(imageData);
                break;
            case FilterType.EdgeDetect:
                result = _filterOPerations.EdgeDetect(imageData);
                break;
            case FilterType.Mean:
                result = _filterOPerations.Mean(imageData);
                break;
            default:
                return BadRequest();
        }

        return Ok(result);
    }



    [HttpPost("NextPage")]
    public IActionResult SaveImageData(ImageDataDto imageDataDto)
    {
        ImageData imageData = _imageRepository.GetImageData();
        _imageRepository.SetImageData(imageDataDto.base64ImageData!, imageDataDto.base64ImageData!, imageData.fileType!);
        return Ok();
    }
}