using System;
using System.Collections.Generic;

#nullable disable

namespace FaceBook.MainModels
{
    public partial class UserTable
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
