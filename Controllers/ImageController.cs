using DIP_Backend.Dtos;
using DIP_Backend.Entities;
using DIP_Backend.Enums;
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

    public ImageController(InMemoryImageRepository imageRepository, ColorOperations colorOperations, HistogramOperations histogramOperations)
    {
        _imageRepository = imageRepository;
        _colorOperations = colorOperations;
        _histogramOperations = histogramOperations;
    }

    [HttpPost]
    public IActionResult SetImageData(ImageDataDto imageDataDto)
    {
        var result = _imageRepository.SetImageData(imageDataDto.base64ImageData, imageDataDto.base64ImageData);
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
        //! There is 1 of 4 option in this request data
        // if (preProcessing1Dto.operationType == PreProcessing1Types.GrayScale)
        //     return Ok(_colorOperations.TurnToGrayScale(_imageRepository.GetImageData()));
        ImageData imageData = _imageRepository.GetImageData();
        ImageData result;

        switch (preProcessing1Dto.operationType)
        {
            case PreProcessing1Types.GrayScale:
                result = _colorOperations.TurnToGrayScale(imageData);
                break;
            case PreProcessing1Types.BlackWhite:
                result = _colorOperations.TurnToBlackAndWhiteByTresholdValue(imageData, preProcessing1Dto.tresholdValue);
                break;
            default:
                return BadRequest();
        }
        _imageRepository.SetImageData(imageData.base64ImageData, imageData.base64ModifiedImageData);
        return Ok(result);

        //* 1. Turn image to gray scale
        //* 2. Turn grey image to Black & White with treshold value (eşik değer)
        //! 3. Zoom in - Zoom out (May be handle in frontend)
        //! 4. Cut a place from image (May be handle in frontend)
    }

    [HttpPost("PreProcessing2")]
    public IActionResult ApplyPreProcessing2(PreProcessing2Dto preProcessing2Dto)
    {
        //! There is 1 of 4 option in this request data
        // if (preProcessing1Dto.operationType == PreProcessing1Types.GrayScale)
        //     return Ok(_colorOperations.TurnToGrayScale(_imageRepository.GetImageData()));
        ImageData imageData = _imageRepository.GetImageData();

        switch (preProcessing2Dto.operationType)
        {
            case PreProcessing2Types.ShowHistogram:
                return Ok(_histogramOperations.ShowHistogram(imageData));
            // case PreProcessing2Types.HistogramEqualization:
            //     return Ok(_colorOperations.TurnToBlackAndWhiteByTresholdValue(imageData, preProcessing2Dto.tresholdValue));
            default:
                return BadRequest();
        }

        //! 1. Turn image to gray scale
        //! 2. Turn grey image to Black & White with treshold value (eşik değer)
        //? 3. Zoom in - Zoom out (May be handle in frontend)
        //? 4. Cut a place from image (May be handle in frontend)
    }
}