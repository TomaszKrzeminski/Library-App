using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp1.Models
{
   public interface IRepository
    {

        int AddBook(string Title, int Pages);
        void AddAuthor(string Firstname, string Surname, int BookId);
        int AddReview(int Rating, string ReviewText);
        bool AddReviewToBook(int BookId, int ReviewId);
        Book GetBookById(int BookId);
        List<Book> GetAllBooks();
        List<BookView> GetLibraryView(List<Book> list=null);

       string GetBookAuthors(int BookId);
             


    }
}
