using Microsoft.AspNetCore.Http;

namespace SocialMediaApplication.Model
{
    public class AddPostClass
    {
        public IFormFile image { get; set; }
        public string Content { get; set; }
    }
}
