using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaripSozluk.Business.Interfaces;
using GaripSozluk.Common.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GaripSozluk.WebApp.Controllers
{
    [Authorize]
    public class BlockedUserController : Controller
    {
        private readonly IBlockedUserService _blockedUserService;
        public BlockedUserController(IBlockedUserService blockedUserService)
        {
            _blockedUserService = blockedUserService;
        }

      
        public IActionResult GetBlockedUsers()
        {

            var blockedUsers = _blockedUserService.GetBlockedUsers();

            return View(blockedUsers);
        }

    
        public IActionResult AddBlockedUser(int blockedUserId)
        {
         
            var serviceStatus = _blockedUserService.AddBlockedUser(blockedUserId);

            return Redirect(Url.Action("Index", "Home"));
        }

        public IActionResult RemoveBlockedUser(int blockedUserId)
        {
            var serviceStatus = _blockedUserService.RemoveBlockedUser(blockedUserId);
            return Redirect(Url.Action("GetBlockedUsers","BlockedUser"));
        }
    }
}

