using GaripSozluk.Business.Interfaces;
using GaripSozluk.Common.ViewModels;
using GaripSozluk.Data.Domain;
using GaripSozluk.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GaripSozluk.Business.Services
{
    public class BlockedUserService : IBlockedUserService
    {
        private readonly IBlockedUserRepository _blockedUserRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        public BlockedUserService(IBlockedUserRepository blockedUserRepository, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _blockedUserRepository = blockedUserRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        // Engellenen kullanıcıların listesinin çekilmesi
        public List<BlockedUserVM> GetBlockedUsers()
        {
            var httpUser = _httpContextAccessor.HttpContext.User;
            var userId = int.Parse(httpUser.Claims.ToList().Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var list = new List<BlockedUserVM>();
            var blockedUsers = _blockedUserRepository.GetAll(x => x.UserId == userId).Include("User");

            foreach (var item in blockedUsers)
            {
                var blocked = new BlockedUserVM();
                blocked.BlockedUserId = item.BlockedUserId;
                blocked.BlockedUserName = item.User.UserName;
                blocked.UserId = item.UserId;
                list.Add(blocked);
            }

            return list;
        }

        // Kullanıcı engelleme
        public ServiceStatus AddBlockedUser(int blockedUserId)
        {
            var serviceStatus = new ServiceStatus();
            var httpUser = _httpContextAccessor.HttpContext.User;
            var userId = int.Parse(httpUser.Claims.ToList().Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            if (userId != blockedUserId)
            {
                var blocked = _blockedUserRepository.Get(x => x.BlockedUserId == blockedUserId && x.UserId == userId);
                if (blocked == null)
                {
                    var blockedUser = new BlockedUser();
                    blockedUser.BlockedUserId = blockedUserId;
                    blockedUser.UserId = userId;
                    blockedUser.CreateDate = DateTime.Now;
                    _blockedUserRepository.Add(blockedUser);
                    try
                    {
                        _blockedUserRepository.SaveChanges();
                        serviceStatus.Status = true;
                        return serviceStatus;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }

            return serviceStatus;
        }

        // Engel Kaldırma
        public ServiceStatus RemoveBlockedUser(int blockedUserId)
        {
            var serviceStatus = new ServiceStatus();
            var httpUser = _httpContextAccessor.HttpContext.User;
            var userId = int.Parse(httpUser.Claims.ToList().Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var blockedUser = _blockedUserRepository.Get(x => x.BlockedUserId == blockedUserId && x.UserId == userId);
            _blockedUserRepository.Remove(blockedUser);
            try
            {
                _blockedUserRepository.SaveChanges();
                serviceStatus.Status = true;
                return serviceStatus;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
