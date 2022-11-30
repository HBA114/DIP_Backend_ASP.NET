using DIP_Backend.Dtos;
using DIP_Backend.Entities;
using DIP_Backend.Enums;
using DIP_Backend.ImageOperations.BaseOperations;
using DIP_Backend.ImageOperations.Filter;
using DIP_Backend.ImageOperations.Morphological;
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
    BasicOperations _basicOperations;
    ColorOperations _colorOperations;
    HistogramOperations _histogramOperations;
    FilterOperations _filterOperations;
    MorphologicalOperations _morphologicalOperations;

    public ImageController(InMemoryImageRepository imageRepository, BasicOperations basicOperations, ColorOperations colorOperations, HistogramOperations histogramOperations, FilterOperations filterOperations, MorphologicalOperations morphologicalOperations)
    {
        _imageRepository = imageRepository;
        _basicOperations = basicOperations;
        _colorOperations = colorOperations;
        _histogramOperations = histogramOperations;
        _filterOperations = filterOperations;
        _morphologicalOperations = morphologicalOperations;
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
            histogramRed = imageData.histogramRed,
            histogramGreen = imageData.histogramGreen,
            histogramBlue = imageData.histogramBlue,
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
            histogramRed = imageData.histogramRed,
            histogramGreen = imageData.histogramGreen,
            histogramBlue = imageData.histogramBlue,
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
            histogramRed = imageData.histogramRed,
            histogramGreen = imageData.histogramGreen,
            histogramBlue = imageData.histogramBlue,
        };

        switch (filterDto.filterType)
        {
            case FilterType.GaussianBlur:
                result = _filterOperations.GaussianBlur(result, 0);
                break;
            case FilterType.Sharpness:
                result = await _filterOperations.Sharpening(result, filterDto.filterSize);
                break;
            case FilterType.EdgeDetect:
                result = _filterOperations.EdgeDetect(result);
                break;
            case FilterType.Mean:
                result = await _filterOperations.Mean(result, filterDto.filterSize);
                break;
            case FilterType.Median:
                result = await _filterOperations.Median(result, filterDto.filterSize);
                break;
            case FilterType.ContraHarmonical:
                result = await _filterOperations.ContraHarmonical(result, filterDto.filterSize);
                break;
            default:
                return BadRequest();
        }

        return Ok(result);
    }

    [HttpPost("Morphological")]
    public async Task<IActionResult> Morphological(MorphologicalDto morphologicalDto)
    {
        ImageData imageData = _imageRepository.GetImageData();
        ImageData result = new()
        {
            base64ImageData = imageData.base64ImageData,
            base64ModifiedImageData = imageData.base64ModifiedImageData,
            fileType = imageData.fileType,
            histogramRed = imageData.histogramRed,
            histogramGreen = imageData.histogramGreen,
            histogramBlue = imageData.histogramBlue,
        };

        switch (morphologicalDto.operationType)
        {
            case MorphologicalType.Erosion:
                result = await _morphologicalOperations.Erosion(result);
                break;
            case MorphologicalType.Dilation:
                result = await _morphologicalOperations.Dilation(result);
                break;
            case MorphologicalType.Skeletonization:
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

    [HttpPost("SaveFile")]
    public async Task<IActionResult> SaveToFileAsync(ImageDataDto imageDataDto)
    {
        ImageData imageData = new()
        {
            base64ImageData = imageDataDto.base64ImageData!,
            base64ModifiedImageData = imageDataDto.base64ImageData!,
            fileType = imageDataDto.fileType
        };
        await _basicOperations.SaveImageToFile(imageData, imageDataDto.filePath!);
        return Ok();
    }
}