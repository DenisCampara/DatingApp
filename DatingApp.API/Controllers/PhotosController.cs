using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    
    public class PhotosController: ControllerBase
    {
        private readonly IDatingRepository _datingRepositiry;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _options;
        private Cloudinary _cloudinary;
        public PhotosController(IDatingRepository datingRepositiry, IMapper mapper, IOptions<CloudinarySettings> options)
        {
            _datingRepositiry = datingRepositiry;
            _mapper = mapper;
            _options = options;

            Account acc = new Account(
                _options.Value.CloudName,
                _options.Value.ApiKey,
                _options.Value.ApiSecret
                );
            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}",Name ="GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepository = await _datingRepositiry.GetPhoto(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepository);

            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotosForUser(int userId, [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _datingRepositiry.GetUser(userId);

            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if(file.Length>0)
            {
                using(var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);

            if (!user.Photos.Any(x => x.IsMain))
                photo.IsMain = true;

            user.Photos.Add(photo);

            if(await _datingRepositiry.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { userId=userId, id=photo.Id }, photoToReturn);
            }

            return BadRequest("Could not add the photo");
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _datingRepositiry.GetUser(userId);
            
            if(!user.Photos.Any(x=>x.Id == id))
                return Unauthorized();

            var photoFromRepo = await _datingRepositiry.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("This is already the main photo.");

            var currentMainPhoto = await _datingRepositiry.GetMainPhoto(userId);
            currentMainPhoto.IsMain = false;
            photoFromRepo.IsMain = true;
            
            if (await _datingRepositiry.SaveAll())
                return NoContent();

            return BadRequest("Could not set photo to main.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _datingRepositiry.GetUser(userId);

            if (!user.Photos.Any(x => x.Id == id))
                return Unauthorized();

            var photoFromRepo = await _datingRepositiry.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("You cannot delete your main photo.");

            if (photoFromRepo.PublicId != null)
            {
                var deletionParams = new DeletionParams(photoFromRepo.PublicId);

                var deletionResult = _cloudinary.Destroy(deletionParams);

                if (deletionResult.Result == "ok")
                {
                    _datingRepositiry.Delete(photoFromRepo);
                }
            }

            if(photoFromRepo.PublicId == null)
            {
                _datingRepositiry.Delete(photoFromRepo);
            }

            if (await _datingRepositiry.SaveAll())
                return Ok();

            return BadRequest("Faild to delete the photo");
        }
    }
}
