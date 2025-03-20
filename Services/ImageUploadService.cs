using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FlowerShop.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

public class ImageUploadService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ImageUploadService(
        IWebHostEnvironment webHostEnvironment
    )
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string> Save(IFormFile ImageFile)
    {
        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadsFolder);
        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
        string filePath = Path.Combine(uploadsFolder, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await ImageFile.CopyToAsync(fileStream);
        }

        return "/uploads/" + fileName;
    }

}
