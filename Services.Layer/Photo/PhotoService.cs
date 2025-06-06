using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Common.Layer;
using Data.Layer.Contexts;
using Data.Layer.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Repository.Layer.Interfaces;
using Services.Layer.DTOs;
using Services.Layer.Helpers;
using Services.Layer.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IAccountService _accountService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public PhotoService(IOptions<CloudinarySettings> config, UserManager<AppUser> userManager, IMapper mapper, IAccountService accountService)
        {
            var acc = new Account()
            {
                Cloud = config.Value.CloudName,
                ApiKey = config.Value.ApiKey,
                ApiSecret = config.Value.ApiSecret
            };
            _cloudinary = new Cloudinary(acc);

            _accountService = accountService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Response<PhotoDTO>> AddPhotoAsync(IFormFile file)
        {
            var user = await _accountService.GetCurrentUserAsync();

            if (user == null) return new Response<PhotoDTO>()
            {
                Data = null,
                Message = "User not found",
                StatusCode = 404,
                Status = false
            };

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
            };

            var UploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (UploadResult.Error != null) return new Response<PhotoDTO>()
            {
                Data = null,
                Message = UploadResult.Error.Message,
                StatusCode = 400,
                Status = false
            };

            var Photo = new Photo
            {
                Url = UploadResult.SecureUrl.AbsoluteUri,
                PublicId = UploadResult.PublicId,
                AppUserId = user.Id.ToString(),
                AppUser = user
            };

            user.Photos.Add(Photo);

            var mappedPhoto = _mapper.Map<PhotoDTO>(Photo);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) return new Response<PhotoDTO>()
            {
                Data = null,
                Message = "Problem adding photo",
                StatusCode = 400,
                Status = false
            };

            return new Response<PhotoDTO>()
            {
                Data = mappedPhoto,
                Message = "Photo added successfully",
                StatusCode = 200,
                Status = true
            };
        }

        public async Task<Response<PhotoDTO>> SetMainPhoto(int photoId)
        {
            var user = await _accountService.GetCurrentUserAsync();

            if (user == null) return new Response<PhotoDTO>()
            {
                Data = null,
                Message = "User not found",
                StatusCode = 404,
                Status = false
            };

            if (user.Photos == null || !user.Photos.Any())
            {
                return new Response<PhotoDTO>()
                {
                    Data = null,
                    Message = "User has no photos",
                    StatusCode = 404,
                    Status = false
                };
            }

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return new Response<PhotoDTO>()
            {
                Data = null,
                Message = "Photo not found",
                StatusCode = 404,
                Status = false
            };

            if (photo.IsMain) return new Response<PhotoDTO>()
            {
                Data = null,
                Message = "This is already your main photo",
                StatusCode = 400,
                Status = false
            };

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if (currentMain != null) currentMain.IsMain = false;

            photo.IsMain = true;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) return new Response<PhotoDTO>()
            {
                Data = null,
                Message = "Problem setting main photo",
                StatusCode = 400,
                Status = false
            };
            else
            {
                var mappedPhoto = _mapper.Map<PhotoDTO>(photo);
                return new Response<PhotoDTO>()
                {
                    Data = mappedPhoto,
                    Message = "Main photo set successfully",
                    StatusCode = 200,
                    Status = true
                };
            }
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var user = await _accountService.GetCurrentUserAsync();

            if (user == null) return null;

            var deleteParams = new DeletionParams(publicId.ToString());

            var photo = user.Photos.FirstOrDefault(x => x.PublicId == publicId.ToString());

            if (photo == null) return null;

            user.Photos.Remove(photo);
            await _userManager.UpdateAsync(user);
            return await _cloudinary.DestroyAsync(deleteParams);
        }

    }
}
