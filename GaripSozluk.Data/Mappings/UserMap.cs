using GaripSozluk.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaripSozluk.Data.Mappings
{
    public class UserMap: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
        
            builder.Property(x => x.BirthDate).IsRequired();
            builder.Property(x => x.CreateDate).IsRequired();
            builder.Property(x => x.UpdateDate);

            builder.HasMany(x => x.BlockedUsers)
                .WithOne(b => b.User).HasForeignKey(x=>x.BlockedUserId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
