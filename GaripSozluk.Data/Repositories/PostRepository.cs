using GaripSozluk.Common.ViewModels;
using GaripSozluk.Data.Domain;
using GaripSozluk.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaripSozluk.Data.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        private readonly GaripSozlukDbContext _context;
        private readonly ICommentRepository _commentRepository;
        public PostRepository(GaripSozlukDbContext context, ICommentRepository commentRepository) : base(context)
        {
            _context = context;
            _commentRepository = commentRepository;
        }

        public ServiceStatus AddPost(PostVM model)
        {
            var serviceStatus = new ServiceStatus();
            var isTherePost = Get(x => x.Title == model.Title);
            if (isTherePost == null)
            {

                try
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {

                        var post = new Post();
                        post.Title = model.Title;
                        post.CreateDate = DateTime.Now;
                        post.UserId = model.UserId;
                        post.PostCategoryId = model.PostCategoryId;
                        post.ViewCount = 1;
                        var normalizedName = model.Title.ToLower();
                        char[] title = new char[] { 'ö', 'ü', 'ı', 'ç', 'ğ', 'ş', ' ' };
                        char[] normalized = new char[] { 'o', 'u', 'i', 'c', 'g', 's', '-' };

                        for (int i = 0; i < title.Length; i++)
                        {
                            normalizedName = normalizedName.Replace(title[i], normalized[i]);
                        }
                        post.NormalizedName = normalizedName;
                        var entity = Add(post);

                        int resultCount = SaveChanges();
                        if (resultCount == -1)
                        {
                            transaction.Rollback();
                            serviceStatus.Status = false;
                            return serviceStatus;
                        }

                        var comment = new Comment();
                        comment.PostId = entity.Id;
                        comment.Text = model.Comment;
                        comment.UserId = model.UserId;
                        comment.CreateDate = DateTime.Now;
                        _commentRepository.Add(comment);


                        resultCount = _commentRepository.SaveChanges();
                        if (resultCount == -1)
                        {
                            transaction.Rollback();
                            serviceStatus.Status = false;
                            return serviceStatus;
                        }
                        transaction.Commit();
                    }

                    serviceStatus.Status = true;
                    return serviceStatus;
                }
                catch (Exception)
                {


                }
            }
            return serviceStatus;
        }



        public IQueryable<Post> GetAllByCategoryId(int id)
        {
            return GetAll().Where(x => x.PostCategoryId == id);
        }
    }
}
