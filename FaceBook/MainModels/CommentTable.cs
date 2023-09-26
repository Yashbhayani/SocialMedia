using System;
using System.Collections.Generic;

#nullable disable

namespace FaceBook.MainModels
{
    public partial class CommentTable
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
