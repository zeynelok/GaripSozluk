using System;
using System.Collections.Generic;
using System.Text;

namespace GaripSozluk.Common.ViewModels
{
    public class BlockedUserVM
    {
        public int UserId { get; set; }
        public int BlockedUserId { get; set; }
        public string BlockedUserName { get; set; }

    }
}
