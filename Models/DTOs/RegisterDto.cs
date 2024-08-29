using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDRDExce.Models.DTOs
{
    public class RegisterDto : BaseLoginDto
    {
        public string FullName { get; set; }
        public string Password { get; set; }

    }
}