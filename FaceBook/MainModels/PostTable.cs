using System;
using System.Collections.Generic;

#nullable disable

namespace FaceBook.MainModels
{
    public partial class PostTable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
