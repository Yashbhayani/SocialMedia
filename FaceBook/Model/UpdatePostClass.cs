using Microsoft.AspNetCore.Http;

namespace SocialMediaApplication.Model
{
    public class UpdatePostClass
    {
        public int Id { get; set; }
        public IFormFile image { get; set; }
        public string Content { get; set; }
    }
}
