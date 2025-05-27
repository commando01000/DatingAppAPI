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
            var userId = await _accountService.GetCurrentUserId();

            var user = await _userManager.FindByIdAsync(userId);

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

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}
