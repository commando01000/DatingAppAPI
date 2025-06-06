using CloudinaryDotNet.Actions;
using Common.Layer;
using Microsoft.AspNetCore.Http;
using Services.Layer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer
{
    public interface IPhotoService
    {
        Task<Response<PhotoDTO>> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
        Task<Response<PhotoDTO>> SetMainPhoto(int publicId);
    }
}
