using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp1.Models;

namespace LibraryApp1.Data
{
    public class LibraryDataBaseInitializer 
    {




      public void Seed()
        {

            Random rnd = new Random();

            int SeedBook(string Title, int Pages, List<int> ListOfId)
            {
                try
                {


                   using(var context=new ModelContext())
                    {
                     Book book = new Book();
                    book.Title = Title;
                    book.Pages = Pages;
                    context.Books.Add(book);
                    context.SaveChanges();

                    foreach (var x in ListOfId)
                    {
                        Review r = context.Reviews.Find(x);
                        book.Reviews.Add(r);
                        context.SaveChanges();
                    }




                    return book.BookId;
                    }


                   

                }
                catch (Exception ex)
                {
                    return 0;
                }

            }

            void SeedAuthor(string Firstname, string Surname, int BookId)
            {


                try
                {

                    
                    using (var context = new ModelContext())
                    {
                    Author author = new Author();
                    author.Firstname = Firstname;
                    author.Surname = Surname;

                    context.Authors.Add(author);
                    context.SaveChanges();

                   
                   

                   BookAuthor bookAuthor = new BookAuthor();
                        Author author1 = context.Authors.Find(author.AuthorId);
                   
                     
                    Book book = context.Books.Find(BookId);
                    
                    bookAuthor.AuthorId = author1.AuthorId;
                    bookAuthor.BookId = book.BookId;
                    bookAuthor.Author = author1;
                    bookAuthor.Book = book;

                    context.BookAuthors.Add(bookAuthor);
                    context.SaveChanges();


                    
                    
                    
                    book.BookAuthors.Add(bookAuthor);
                    author1.BookAuthors.Add(bookAuthor);
                    context.SaveChanges();
                    }





                }
                catch (Exception ex)
                {

                }

            }

            int SeedReview(int Rating, string ReviewText)
            {



                try
                {


                    using (var context = new ModelContext())
                    {
                     Review review = new Review();
                    review.Rating = Rating;
                    review.ReviewText = ReviewText;

                    context.Reviews.Add(review);
                    context.SaveChanges();
                     return review.ReviewId;
                    }

                   

                   
                }
                catch (Exception ex)
                {
                    return 0;
                }



            }

            void AddEntity(string BookTitle, string FirstName, string Surname,int ReviewCount)
            {
                List<int> reviewList = new List<int>();

                for (int i = 0; i < ReviewCount; i++)
                {

                    int reviewId = SeedReview(rnd.Next(1, 5), "Review Text Example " + i);
                    reviewList.Add(reviewId);
                }

                int bookId = SeedBook(BookTitle, rnd.Next(100, 1000), reviewList);

                SeedAuthor(FirstName, Surname, bookId);

                //context.SaveChanges();

            }

            AddEntity("Atlas chmur", "David", "Mitchell",13);
            AddEntity("Miasto kości", "Cassandra", "Clare", 12);
            AddEntity("Jeździec miedziany", "Paullina", "Simons",11);
            AddEntity("Iliada", "", "Homer",10);
            AddEntity("Antygona", "", "Sofokles",9);
            AddEntity("Kordian", "Juliusz", "Słowacki",8);
            AddEntity("Lalka ", "Bolesław", "Prus",7);
            AddEntity("Gloria victis", "Eliza", "Orzeszkowa",6);
            AddEntity("Potop", "Henryk", "Sienkiewicz",5);
            AddEntity("Wesele", "Stanisław", "Wyspiański",4);
            AddEntity("Rok 1984", "George", "Orwell",3);
            AddEntity("Dżuma", "Albert", "Camus",2);            


        }
    }
}
