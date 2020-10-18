using GaripSozluk.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaripSozluk.Data.Mappings
{
    public class LogMap : BaseMap<Log>
    {
        public override void Configure(EntityTypeBuilder<Log> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.IPAddress).HasMaxLength(15);
            builder.Property(x => x.RequestMethod).HasMaxLength(10);
            builder.Property(x => x.RequestPath);
            builder.Property(x => x.ResponseStatusCode);
            builder.Property(x => x.RoutePath);
            builder.Property(x => x.TraceIdentifier);
            builder.Property(x => x.UserAgent);

        }
    }
}
