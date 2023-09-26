using System;
using System.Collections.Generic;

#nullable disable

namespace FaceBook.MainModels
{
    public partial class LikeTable
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
