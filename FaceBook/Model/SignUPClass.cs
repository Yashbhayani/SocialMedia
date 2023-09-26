using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceBookApp.Model
{
    public class SignUPClass
    {
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string Emial { get; set; } 
        public string Password { get; set; } 
        public IFormFile image { get; set; } 
    }
}
