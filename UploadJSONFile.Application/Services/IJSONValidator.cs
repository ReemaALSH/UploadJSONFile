using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadJSONFile.Application.Services
{
    public interface IJSONValidator
    {
       bool ValidateAndSaveJsonFileAsync(string jsonContent);
    }
}
