using System;
using System.Collections.Generic;
using System.Text;

namespace GaripSozluk.Common.ViewModels
{
   public class ApiVM
    {
        public string[] author_name { get; set; }
        public string title { get; set; }
        public string[] publisher { get; set; }
        public int[] publish_year { get; set; }
        public int first_publish_year { get; set; }
        public string[] isbn { get; set; }
    }
}
