using System;
using System.Collections.Generic;

#nullable disable

namespace FaceBook.MainModels
{
    public partial class FriendRequestTable
    {
        public int Id { get; set; }
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
