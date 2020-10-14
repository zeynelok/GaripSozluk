using System;
using System.Collections.Generic;
using System.Text;

namespace GaripSozluk.Data.Domain
{
   public interface IBaseEntity
    {
         int Id { get; set; }
        DateTime CreateDate { get; set; }
        DateTime? UpdateDate { get; set; }

    }
}
