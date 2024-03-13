using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadJSONFile.Domain.Models
{
    public class JSONFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string JSONContent { get; set; }
    }


}
