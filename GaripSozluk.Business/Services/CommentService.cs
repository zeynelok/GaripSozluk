using GaripSozluk.Business.Interfaces;
using GaripSozluk.Common.ViewModels;
using GaripSozluk.Data.Domain;
using GaripSozluk.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace GaripSozluk.Business.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBlockedUserService _blockedUserService;
        public CommentService(ICommentRepository commentRepository, IHttpContextAccessor httpContextAccessor, IBlockedUserService blockedUserService)
        {
            _commentRepository = commentRepository;
            _httpContextAccessor = httpContextAccessor;
            _blockedUserService = blockedUserService;
        }

        // post Id ye göre tüm yorumları getirme
        public List<CommentRowVM> GetAllByPostId(int postId)
        {
            var httpUser = _httpContextAccessor.HttpContext.User;

            List<int> blockedUserIds = new List<int>();
            var Comments = new List<CommentRowVM>();
            var commentEntities = _commentRepository.GetAll(x => x.PostId == postId).Include("User");

            if (httpUser.Identity.IsAuthenticated == true)
            {
                var blockedUsers = _blockedUserService.GetBlockedUsers();

                foreach (var item in blockedUsers)
                {
                    blockedUserIds.Add(item.BlockedUserId);
                }
                commentEntities = commentEntities.Where(x => !blockedUserIds.Contains(x.UserId));
            }

            foreach (var item in commentEntities)
            {
                var result = new CommentRowVM();
                result.Text = item.Text;
                result.UserId = item.UserId;
                result.CommentDate = item.UpdateDate ?? item.CreateDate;
                result.UserName = item.User.UserName;
                Comments.Add(result);
            }

            return Comments;
        }

        //Yorum ekleme
        public ServiceStatus AddComment(CommentVM model, int postId)
        {
            var serviceStatus = new ServiceStatus();
            var httpUser = _httpContextAccessor.HttpContext.User;
            var claims = int.Parse(httpUser.Claims.ToList().Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var comment = new Comment();
            comment.CreateDate = DateTime.Now;
            comment.PostId = postId;
            comment.UserId = claims;
            comment.Text = model.Content;
            _commentRepository.Add(comment);
            try
            {
                _commentRepository.SaveChanges();
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
