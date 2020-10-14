using GaripSozluk.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaripSozluk.Data.Mappings
{
    public class BlockedUserMap:BaseMap<BlockedUser>
    {
        public override void Configure(EntityTypeBuilder<BlockedUser> builder)
        {
           
        }

       
    }
}
