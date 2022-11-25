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
    FilterOperations _filterOperations;

    public ImageController(InMemoryImageRepository imageRepository, ColorOperations colorOperations, HistogramOperations histogramOperations, FilterOperations filterOperations)
    {
        _imageRepository = imageRepository;
        _colorOperations = colorOperations;
        _histogramOperations = histogramOperations;
        _filterOperations = filterOperations;
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
        ImageData result = new()
        {
            base64ImageData = imageData.base64ImageData,
            base64ModifiedImageData = imageData.base64ModifiedImageData,
            fileType = imageData.fileType,
            histogram = imageData.histogram
        };

        switch (preProcessing1Dto.operationType)
        {
            case PreProcessing1Type.GrayScale:
                result = _colorOperations.TurnToGrayScale(result);
                break;
            case PreProcessing1Type.BlackWhite:
                result = _colorOperations.TurnToBlackAndWhiteByTresholdValue(result, preProcessing1Dto.tresholdValue);
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
        ImageData result = new()
        {
            base64ImageData = imageData.base64ImageData,
            base64ModifiedImageData = imageData.base64ModifiedImageData,
            fileType = imageData.fileType,
            histogram = imageData.histogram
        };

        switch (preProcessing2Dto.operationType)
        {
            case PreProcessing2Type.ShowHistogram:
                result = _histogramOperations.ShowHistogram(result);
                break;
            case PreProcessing2Type.HistogramEqualization:
                result = _histogramOperations.HistogramEqualization(result);
                break;
            default:
                return BadRequest();
        }

        return Ok(result);
    }

    [HttpPost("Filters")]
    public async Task<IActionResult> ApplyFilters(FilterDto filterDto)
    {
        ImageData imageData = _imageRepository.GetImageData();
        ImageData result = new()
        {
            base64ImageData = imageData.base64ImageData,
            base64ModifiedImageData = imageData.base64ModifiedImageData,
            fileType = imageData.fileType,
            histogram = imageData.histogram
        };

        if (filterDto.filterType == FilterType.Mean)
        {
            Task<ImageData> rt = _filterOperations.Mean(result);
            result = await rt;
        }

        if (filterDto.filterType == FilterType.Median)
        {
            result = _filterOperations.Median(result);
        }

        // switch (filterDto.filterType)
        // {
        //     case FilterType.GaussianBlur:
        //         result = _filterOperations.GaussianBlur(result, 0);
        //         break;
        //     case FilterType.Sharpness:
        //         result = _filterOperations.Sharpness(result);
        //         break;
        //     case FilterType.EdgeDetect:
        //         result = _filterOperations.EdgeDetect(result);
        //         break;
        //     case FilterType.Mean:
        //         result = _filterOperations.Mean(result);
        //         break;
        //     case FilterType.Median:
        //         result = _filterOperations.Median(result);
        //         break;
        //     default:
        //         return BadRequest();
        // }

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