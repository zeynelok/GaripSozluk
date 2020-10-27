using GaripSozluk.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaripSozluk.Data.Mappings
{
    public class PostMap:BaseMap<Post>
    {
        public override void Configure(EntityTypeBuilder<Post> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
            builder.Property(x => x.ViewCount).IsRequired();
            builder.Property(x => x.NormalizedName).HasMaxLength(100).IsRequired();

            builder.HasOne(x => x.User).WithMany(x => x.Posts).HasForeignKey(x => x.UserId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.PostCategory).WithMany(x => x.Posts).HasForeignKey(x => x.PostCategoryId).IsRequired();
        }
    }
}
