using DIP_Backend.Dtos;
using DIP_Backend.ImageOperations.PreProcessing1;
using DIP_Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DIP_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    InMemoryImageRepository _imageRepository;
    ColorOperations _colorOperations;

    public ImageController(InMemoryImageRepository imageRepository, ColorOperations colorOperations)
    {
        _imageRepository = imageRepository;
        _colorOperations = colorOperations;
    }

    [HttpPost]
    public IActionResult SetImageData(ImageDataDto imageDataDto)
    {
        var result = _imageRepository.SetImageData(imageDataDto.base64ImageData);
        return Ok(result);
    }

    [HttpGet]
    public IActionResult GetImageData()
    {
        return Ok(_imageRepository.GetImageData());
    }

    [HttpPost("PreProcessing1")]
    public IActionResult ApplyPreProcessing1()
    {
        //! There is 1 of 4 option in this request data

        //! 1. Turn image to gray scale
        //! 2. Turn grey image to Blach & White with treshold value (eşik değer)
        //? 3. Zoom in - Zoom out (May be handle in frontend)
        //? 4. Cut a place from image (May be handle in frontend)

        return Ok();
    }
}