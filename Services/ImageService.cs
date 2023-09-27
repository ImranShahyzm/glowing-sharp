using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
namespace SagErpBlazor.Services
{
    public class ImageService
{
    private readonly IWebHostEnvironment _env;

    public ImageService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveImageAsync(string folderPath, string fileName, string ConvertedImage)
    {
            try
            {

                 var GetMimType= DataUriHelper.GetMimeType(ConvertedImage);

                if(!string.IsNullOrEmpty(GetMimType))
                {
                  
                    fileName = fileName + GetMimType;
                }
                int base64DataIndex = ConvertedImage.IndexOf(',') + 1;
                byte[] imageData = Convert.FromBase64String(ConvertedImage.Substring(base64DataIndex));

                string uploadFolder = Path.Combine(_env.WebRootPath, folderPath);


                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }


                string filePath = Path.Combine(uploadFolder, fileName);

                // Save the image to the specified location
                await File.WriteAllBytesAsync(filePath, imageData);

                // Return the URL path of the saved image (relative to the web root)
                return Path.Combine(folderPath, fileName).Replace('\\', '/');

            }catch(Exception e)
            {
                return e.Message;
            }
    }
}
}