using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp1.Models
{
   public class BookView
    {

     public   int BookId { get; set; }
     public   string Title { get; set; }
     public int Pages { get; set; }
     public int? Authors { get; set; }
     public int? Reviews { get; set; }
     public double? ReviewsAVG { get; set; }


    }
}
