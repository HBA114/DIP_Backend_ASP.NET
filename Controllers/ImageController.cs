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

        return preProcessing1Dto.operationType switch
        {
            PreProcessing1Type.GrayScale => Ok(_colorOperations.TurnToGrayScale(result)),
            PreProcessing1Type.BlackWhite => Ok(_colorOperations.TurnToBlackAndWhiteByThresholdValue(result, preProcessing1Dto.thresholdValue)),
            _ => Ok(result)
        };
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

        return preProcessing2Dto.operationType switch
        {
            PreProcessing2Type.ShowHistogram => Ok(_histogramOperations.ShowHistogram(result)),
            PreProcessing2Type.HistogramEqualization => Ok(_histogramOperations.HistogramEqualization(result)),
            _ => BadRequest()
        };
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

        return filterDto.filterType switch
        {
            FilterType.GaussianBlur => Ok(_filterOperations.GaussianBlur(result, 0)),
            FilterType.Sharpening => Ok(await _filterOperations.Sharpening(result, filterDto.filterSize)),
            FilterType.EdgeDetect => Ok(_filterOperations.EdgeDetect(result)),
            FilterType.Mean => Ok(await _filterOperations.Mean(result, filterDto.filterSize)),
            FilterType.Median => Ok(await _filterOperations.Median(result, filterDto.filterSize)),
            FilterType.ContraHarmonical => Ok(await _filterOperations.ContraHarmonical(result, filterDto.filterSize)),
            _ => BadRequest()
        };
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

        return morphologicalDto.operationType switch
        {
            MorphologicalType.Erosion => Ok(await _morphologicalOperations.Erosion(result)),
            MorphologicalType.Dilation => Ok(await _morphologicalOperations.Dilation(result)),
            MorphologicalType.Skeletonization => throw new NotImplementedException("Skeletonization Not Implemented"),
            _ => BadRequest()
        };
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
