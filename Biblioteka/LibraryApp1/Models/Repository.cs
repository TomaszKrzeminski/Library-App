using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Data.Entity;

namespace LibraryApp1.Models
{
    class Repository:IRepository
    {     


        public Repository()
        {

        }

      public   List<BookView> GetLibraryView(List<Book> listBooks=null)
        {

            List<BookView> list = new List<BookView>();

            try
            {


               


                if(listBooks!=null)
                {
                    using(var context=new ModelContext())
                        {
                        foreach (var item in listBooks)
                        {



                            //Book xx = context.Books.Include(x => x.Reviews).Include(x => x.BookAuthors).Include(x => x.BookAuthors.Select(y => y.Author)).Where(z => z.BookId == item.BookId).FirstOrDefault();
                            //BookView bookView = new BookView() { Title = xx.Title, Pages = xx.Pages, BookId = xx.BookId, Authors = xx.BookAuthors.Count(), Reviews = (int?)xx.Reviews.Count() ?? 0, ReviewsAVG = Math.Round((double?)xx.Reviews.Average(y => y.Rating) ?? 0, 1) };
                            //list.Add(bookView);



                            Book xx = context.Books.Include(x => x.Reviews).Include(x => x.BookAuthors).Include(x => x.BookAuthors.Select(y => y.Author)).Where(z => z.BookId == item.BookId).FirstOrDefault();
                            BookView bookView = new BookView() { Title = xx.Title, Pages = xx.Pages, BookId = xx.BookId, Authors = xx.BookAuthors.Count() };

                            int revCount = 0;
                            double average = 0;
                            try
                            {
                                revCount = (int?)xx.Reviews.Count() ?? 0;
                                average = Math.Round((double?)xx.Reviews.Average(y => y.Rating) ?? 0, 1);
                            }
                            catch(Exception ex)
                            {

                            }

                            bookView.Reviews = revCount;
                            bookView.ReviewsAVG = average;

                            list.Add(bookView);






                        }




                    }

                }
                else
                {
               using (var context = new ModelContext())
                {

                    list = context.Books.Include(x=>x.Reviews).Include(x=>x.BookAuthors).Include(x=>x.BookAuthors.Select(y=>y.Author)).Select(x => new BookView() { Title = x.Title,Pages=x.Pages,BookId=x.BookId,Authors=x.BookAuthors.Count(), Reviews = (int?)x.Reviews.Count()??0, ReviewsAVG =Math.Round((double?) x.Reviews.Average(y => y.Rating)??0,1 )}).ToList();

                }
                }


                return list;
            }
           catch (Exception ex)
            {
                return list;
            }


        }

        public void AddAuthor(string Firstname, string Surname, int BookId)
        {
            try
            {

                Firstname = Firstname.Trim();
                Surname = Surname.Trim();

                if(Firstname.Replace(" ","")!=""&&Surname.Replace(" ","")!="")
                {
                    using (var context = new ModelContext())
                    {

                        Author author;

                        author = context.Authors.Where(x => x.Firstname.ToLower() == Firstname.ToLower() && x.Surname.ToLower() == Surname.ToLower()).FirstOrDefault();

                        if (author == null)
                        {
                            author = new Author();
                            author.Firstname = Firstname;
                            author.Surname = Surname;

                            context.Authors.Add(author);
                            context.SaveChanges();
                        }


                        Book book = context.Books.Find(BookId);
                        BookAuthor bookAuthor = new BookAuthor();
                        bookAuthor.AuthorId = author.AuthorId;
                        bookAuthor.BookId = book.BookId;
                        bookAuthor.Author = author;
                        bookAuthor.Book = book;

                        book.BookAuthors.Add(bookAuthor);
                        author.BookAuthors.Add(bookAuthor);
                        context.SaveChanges();
                    }
                }



                



               

                    












            }
            catch (Exception ex)
            {

            }
        }
        public int AddBook(string Title, int Pages)
        {
            try
            {

                

                using(var context=new ModelContext())
                {


                  string  Title2= Regex.Replace(Title, @"\s+", "");

                    Book bookcheck = context.Books.Where(x =>x.Title.Replace(" ","").ToLower() == Title2).FirstOrDefault();

                    if(bookcheck==null)
                    {
                    Book book = new Book();
                    book.Title = Title.Trim();
                    book.Pages = Pages;
                    context.Books.Add(book);
                    context.SaveChanges();

                     return book.BookId;
                    }
                    else
                    {
                        return 0;
                    }
                
                }


               

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int AddReview(int Rating, string ReviewText)
        {
            try
            {
                using(var context=new ModelContext())
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
        public Book GetBookById(int BookId)
        {
            try
            {
                using(var context=new ModelContext())
                {
                    return context.Books.Find(BookId);
                }
            }
            catch(Exception ex)
            {
                return new Book();
            }
        }
        public bool AddReviewToBook(int BookId, int ReviewId)
        {
            try
            {


                using(var context=new ModelContext())
                {
                    Book book = context.Books.Find(BookId);
                    Review review = context.Reviews.Find(ReviewId);
                    book.Reviews.Add(review);
                    context.SaveChanges();
                }


               


                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public List<Book> GetAllBooks()
        {
            try
            {
                using(var context=new ModelContext())
                {
                  return   context.Books.ToList();
                }              

            }
            catch (Exception)
            {

                return new List<Book>();
            }
        }

        public string GetBookAuthors(int BookId)
        {
            try
            {
                using(var context=new ModelContext())
                {

                    List<Author> list = context.Books.Include(x => x.BookAuthors.Select(y => y.Author)).Where(x => x.BookId == BookId).SelectMany(x => x.BookAuthors.Select(y => y.Author)).ToList();

                    string text = " ";

                  string line=  System.Environment.NewLine;
                      foreach (var item in list)
                    {

                        text +=line+ item.Firstname + " " + item.Surname;

                    }

                    return text;

                }

            }
            catch(Exception ex)
            {
                return "";
            }
        }
    }
}
