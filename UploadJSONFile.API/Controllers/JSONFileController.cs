using UploadJSONFile.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UploadJSONFile.Infrastructure;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using UploadJSONFile.Application.Services;
using System.IO;

namespace UploadJSONFile.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JSONFileController : ControllerBase
    {
        private readonly IJSONValidator _jsonFileService;

        private readonly DataContext _ctx;
        public JSONFileController(DataContext ctx, IJSONValidator jsonFileService)
        {
            _ctx = ctx;
            _jsonFileService = jsonFileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFilesAsync()
        {
            var files = await _ctx.JSONFiles.ToListAsync();
            if (files == null)
                return NotFound();
            return Ok(files);
        }


        [Route("{id}")]
        [HttpGet]

        public async Task<IActionResult> GetFileById(int id)
        {
            var file = await _ctx.JSONFiles.FirstOrDefaultAsync(f => f.Id == id);
           
            if (file == null)
                return NotFound();

            return Ok(file.JSONContent);
        }


        [HttpPost("upload")]
        public async Task<IActionResult> AddFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            if (file.ContentType != "application/json")
                return BadRequest("Only JSON files are allowed.");

         
            using (var streamReader = new StreamReader(file.OpenReadStream()))
            {
               

                // Deserialize the JSON content
                try
                { var jsonString = await streamReader.ReadToEndAsync();
                    bool isValid =  _jsonFileService.ValidateAndSaveJsonFileAsync(jsonString);
                    if (isValid)
                    {
                        
                        var newFile = new JSONFile
                        {
                            FileName = file.FileName,
                            JSONContent = jsonString
                        };
                        _ctx.JSONFiles.Add(newFile);
                        await _ctx.SaveChangesAsync();
                        return Ok("File uploaded and saved successfully");

                    }
                    else
                    {
                        return BadRequest("Invalid JSON file");
                    }

                    
                }
                catch (JsonException)
                {
                    return BadRequest("Invalid JSON file.");
                }
           

            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateFile(IFormFile updatefile, int id)
        {

            if (updatefile == null || updatefile.Length == 0)
                return BadRequest("No file uploaded.");

            if (updatefile.ContentType != "application/json")
                return BadRequest("Only JSON files are allowed.");

            var file = await _ctx.JSONFiles.FirstOrDefaultAsync(f => f.Id == id);
            
            if (file == null)
                return NotFound("No resource with the corresponding ID found");
           
             // Read the uploaded JSON file
             using (var streamReader = new StreamReader(updatefile.OpenReadStream()))
             {
                 
                 // Deserialize the JSON content
                 try
                 {
                    var jsonString = await streamReader.ReadToEndAsync();
                    bool isValid = _jsonFileService.ValidateAndSaveJsonFileAsync(jsonString);
                    if (isValid)
                    {

                        file.FileName = updatefile.FileName;
                        file.JSONContent = jsonString;

                        _ctx.JSONFiles.Update(file);
                        await _ctx.SaveChangesAsync();

                        return Ok("File updated and saved successfully");

                    }
                    else
                    {
                        return BadRequest("Invalid JSON file");
                    }

                }
                 catch (JsonException)
                 {
                     return BadRequest("Invalid JSON file.");
                 }
             }

           

        }

    }
}

