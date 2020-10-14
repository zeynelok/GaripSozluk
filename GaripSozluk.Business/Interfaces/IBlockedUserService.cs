using GaripSozluk.Common.ViewModels;
using GaripSozluk.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GaripSozluk.Business.Interfaces
{
    public interface IBlockedUserService
    {
        ServiceStatus AddBlockedUser(int blockedUserId);
        List<BlockedUserVM> GetBlockedUsers();
        ServiceStatus RemoveBlockedUser(int blockedUserId);
    }
}
