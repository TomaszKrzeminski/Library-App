using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp1.Data;

namespace LibraryApp1.Models
{
    public class Book
    {     

        public Book()
        {
            Reviews = new List<Review>();
            BookAuthors = new List<BookAuthor>();
        }

        public int BookId { get; set; }

        public string Title { get; set; }

        public int Pages { get; set; }

        public virtual IList<Review> Reviews { get; set; }
        public  IList<BookAuthor> BookAuthors { get; set; }

    }



    public class BookAuthor
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }




    public class Author
    {
               



        public Author()
        {
            BookAuthors = new List<BookAuthor>();
        }


        public int AuthorId { get; set; }

        public string Firstname { get; set; }

        public string Surname { get; set; }

        public virtual IList<BookAuthor> BookAuthors { get; set; }

    }



    public class ModelContext : DbContext
    {

        public ModelContext() : base("name=con")
        {
           
        }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<BookAuthor>()
       .HasKey(c => new { c.BookId, c.AuthorId });

            modelBuilder.Entity<Book>()
       .HasMany(c => c.BookAuthors)
       .WithRequired()
       .HasForeignKey(c => c.BookId);

            modelBuilder.Entity<Author>()
               .HasMany(c => c.BookAuthors)
               .WithRequired()
               .HasForeignKey(c => c.BookId);

        }















        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }







    }


    public class Review
    {
        public int ReviewId { get; set; }

        public int Rating { get; set; } //1-5

        public string ReviewText { get; set; }


        public Book Book { get; set; }

    }







}
