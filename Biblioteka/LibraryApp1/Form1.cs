using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryApp1.Data;
using LibraryApp1.Models;
using System.Data.Entity;


namespace LibraryApp1
{
    public partial class Form1 : Form
    {

        IRepository repo;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private int ButtonReviewCount = 1;
        private int ReviewBookId = 0;
       

        public Form1()
        {
            repo = (IRepository)Program.ServiceProvider.GetService(typeof(IRepository));
            InitializeComponent();
            errorProvider1= new System.Windows.Forms.ErrorProvider();

            using (var context = new ModelContext())
            {

                if (!context.Database.Exists())
                {
                    context.Database.Create();
                }

                List<Book> books1 = context.Books.ToList();

                if (books1 != null && books1.Count() == 0)
                {
                    LibraryDataBaseInitializer lib = new LibraryDataBaseInitializer();
                    lib.Seed();
                }

                //dataGridView1.DataSource = repo.GetAllBooks();///////////////////////////////////////////////////////////////

                dataGridView1.DataSource = repo.GetLibraryView();
            }

            groupBox1.Visible = false;
            groupBox2.Visible = false;

        }



        private void CreateDatabase()
        {
            using (var context = new ModelContext())
            {

                if (!context.Database.Exists())
                {
                    context.Database.Create();
                }

                List<Book> books1 = context.Books.ToList();

                if (books1 != null && books1.Count() == 0)
                {
                    LibraryDataBaseInitializer lib = new LibraryDataBaseInitializer();
                    lib.Seed();
                }

                //dataGridView1.DataSource = context.Books.ToList();//////////////////////////////////////////////////////////////////////////////
                dataGridView1.DataSource = repo.GetLibraryView();
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            CreateDatabase();
            using (var context = new ModelContext())
            {             

                //dataGridView1.DataSource = context.Books.ToList();///////////////////////////////////////////////////////////////////////
                dataGridView1.DataSource = repo.GetLibraryView();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {     
                
            using (var context=new ModelContext())
            {



                List<BookAuthor> listBookAut = new List<BookAuthor>();

                listBookAut = context.BookAuthors.ToList();

                foreach (var item in listBookAut)
                {
                    context.BookAuthors.Remove(item);

                }
                context.SaveChanges();


                List<Author> listAuthor = new List<Author>();
                listAuthor = context.Authors.ToList();

                foreach (var item in listAuthor)
                {

                    context.Authors.Remove(item);
                    

                }
                context.SaveChanges();           
               

                List<Book> listBooks = new List<Book>();
                listBooks = context.Books.ToList();

                foreach (var item in listBooks)
                {

                    context.Reviews.RemoveRange(item.Reviews);
                    context.SaveChanges();
                    context.Books.Remove(item);
                    context.SaveChanges();
                }

                var connection =
    System.Configuration.ConfigurationManager.
    ConnectionStrings["con"].ConnectionString;
                System.Data.Entity.Database.Delete(connection);


                CreateDatabase();


                //dataGridView1.DataSource = new List<Book>();/////////////////////////////////////////////////////
                dataGridView1.DataSource = repo.GetLibraryView();
            }






            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using( var context=new ModelContext())
            {
               

                dataGridView1.DataSource = repo.GetLibraryView();
            }
        }

        private void button4_Click(object sender, EventArgs e)//count
        {
            using (var context = new ModelContext())
            {
                List<Book> books = context.Books.Include(x => x.Reviews).OrderByDescending(x => x.Reviews.Count).ToList();
                dataGridView1.DataSource = repo.GetLibraryView(books);
             
            }

        }

        private void button5_Click(object sender, EventArgs e) //average
        {

            using (var context = new ModelContext())
            {


                List<Book> books = context.Books.Include(x => x.Reviews).OrderByDescending(x => x.Reviews.Average(y=>y.Rating)).ToList();
              
                dataGridView1.DataSource = repo.GetLibraryView(books);


            }

        }



        List<Button>  GetStarButtons()
        {
            List<Button> list= new List<Button>();
            list.Add(button9);
            list.Add(button10);
            list.Add(button11);
            list.Add(button12);
            list.Add(button13);
            return list;
        }

        private void SetStarButtons()
        {
            button9.Image= Image.FromFile("../../Images/starOn.png");
            button10.Image = Image.FromFile("../../Images/starOff.png");
            button11.Image = Image.FromFile("../../Images/starOff.png");
            button12.Image = Image.FromFile("../../Images/starOff.png");
            button13.Image = Image.FromFile("../../Images/starOff.png");
        }


       


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)//Review Add
        {

            DataGridViewCellEventArgs ee = e;

            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                ReviewBookId = (int)dataGridView1.Rows[e.RowIndex].Cells[1].Value;

                groupBox2.Visible = true;
                BookReviewName.Text = repo.GetBookById(ReviewBookId).Title;
                BookReviewName.Text += repo.GetBookAuthors(ReviewBookId);
                BookReviewName.Font = new Font(BookReviewName.Font.FontFamily, 16);
                SetStarButtons();


            }

        }





        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            string SearchTitleText = textBox1.Text;

            using (var context = new ModelContext())
            {

                List<Book> books = context.Books.Where(x => x.Title.ToLower().StartsWith(SearchTitleText.ToLower())).ToList();
                //dataGridView1.DataSource = books;///////////////////////////////////

                dataGridView1.DataSource = repo.GetLibraryView(books);

            }
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            string Text = textBox2.Text.ToLower();

            using (var context = new ModelContext())
            {
                List<Book> list = context.Books.Include(x => x.BookAuthors).SelectMany(x => x.BookAuthors).Where(x => x.Author.Surname.StartsWith(Text) || x.Author.Firstname.StartsWith(Text)).Select(x => x.Book).ToList();
                //dataGridView1.DataSource = list;///////////////////////////////////
                dataGridView1.DataSource = repo.GetLibraryView(list);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            List<int> ListOfAuthors = new List<int>();                  
            List<TextBox> AuthorN = new List<TextBox>() {AuthorName1,AuthorName2,AuthorName3 };
            List<TextBox> AuthorS = new List<TextBox>() {AuthorSurname1,AuthorSurname2,AuthorSurname3 };

            ValidateAutor(errorProvider4,AuthorN,AuthorS);
            
            
            bool check = int.TryParse(bookPages.Text,out int xx);

            if(!check)
            {
                errorProvider3.SetError(bookPages, "Musisz wpisać ilość stron");

                return;
            }

           if(bookTitle!=null&&bookTitle.Text!=""&& bookPages.Text!=null&& Int32.Parse(bookPages.Text)>0)
            {
           int BookId=  repo.AddBook(bookTitle.Text, Int32.Parse(bookPages.Text));

                if(BookId!=0)
                {
             repo.AddAuthor(AuthorName1.Text, AuthorSurname1.Text, BookId);
            repo.AddAuthor(AuthorName2.Text, AuthorSurname2.Text, BookId);
            repo.AddAuthor(AuthorName3.Text, AuthorSurname3.Text, BookId);
                }
                else
                {
                    ErrorMessageBookAdd.Text = "Taka książka już istnieje";
                    return;
                }
            


            bookTitle.Text = "";
            bookPages.Text = "";
            AuthorName1.Text = "";
            AuthorSurname1.Text = "";
            AuthorName2.Text = "";
            AuthorSurname2.Text = "";
            AuthorName3.Text = "";
            AuthorSurname3.Text = "";
            }

            


            //dataGridView1.DataSource = repo.GetAllBooks();///////////////////////////////////////////////////////////


            dataGridView1.DataSource = repo.GetLibraryView();

            groupBox1.Visible = false;

        }





        private void bookTitle_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ValidateTitle();
        }
        private bool ValidateTitle()
        {
            bool bStatus = true;
            if (bookTitle.Text == "")
            {
                errorProvider2.SetError(bookTitle, "Musisz wpisać tytuł książki");
                bStatus = false;
            }
            else
                errorProvider2.SetError(bookTitle, "");
            return bStatus;
        }


        private void bookPages_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ValidatePages();
        }
        private bool ValidatePages()
        {
            bool check = false;

            try
            {
                Int32.Parse(bookPages.Text);
            }
            catch(Exception ex)
            {
                check = true;
            }


            bool bStatus = true;
            if (bookPages.Text == "")
            {
                errorProvider3.SetError(bookPages, "Musisz wpisać ilość stron");
                bStatus = false;
            }
            else if(check)
            {
                errorProvider3.SetError(bookPages, "Musisz wpisać liczbę");
                bStatus = false;
            }
            else
                errorProvider3.SetError(bookPages,"");
            return bStatus;
        }



       

        
       
        private bool ValidateAutor(ErrorProvider  er,List<TextBox> listtextboxName,List<TextBox> listtextboxSurname)
        {
            bool bStatus = true;
            bool checkValid = true;
          
            for (int i = 0; i < listtextboxName.Count; i++)
            {

                if(listtextboxName[i].Text!=""&&listtextboxSurname[i].Text!="")
                {
                    checkValid = false;
                }


            }



            if (checkValid)
            {
                er.SetError(listtextboxName[0], "Musisz podać choć jednego autora");
                bStatus = false;
            }           
            else
               er.SetError(listtextboxName[0], "");
            return bStatus;
        }

        private void button7_Click(object sender, EventArgs e)
        {

            groupBox1.Visible = true;


        }

        private void button8_Click(object sender, EventArgs e)//Review Save
        {


            if(ReviewBookId>0)
            {
           int ReviewId= repo.AddReview(ButtonReviewCount, textBox3.Text);

            repo.AddReviewToBook(ReviewBookId, ReviewId);

            groupBox2.Visible = false;
            textBox3.Text = "";
            ButtonReviewCount = 1;
            }

        
           


            dataGridView1.DataSource = repo.GetLibraryView();

        }
           private void button14_Click(object sender, EventArgs e)//Review Exit
        {
            textBox3.Text = "";
            ButtonReviewCount = 1;
            groupBox2.Visible = false;
        }




        public void SetButtonOn(Button button)
        {
            button.Image = Image.FromFile("../../Images/starOn.png");
           
        }

        public void SetButtonOff(Button button)
        {
            
            button.Image = Image.FromFile("../../Images/starOff.png");
        }

        public void SetAllButtonsOff(List<Button> list)
        {

            foreach (var button in list)
            {
                SetButtonOff(button);
            }


        }     


        private void button15_Click(object sender, EventArgs e)//Add book exit
        {

            bookTitle.Text = "";
            bookPages.Text = "";
            AuthorName1.Text = "";
            AuthorSurname1.Text = "";
            AuthorName2.Text = "";
            AuthorSurname2.Text = "";
            AuthorName3.Text = "";
            AuthorSurname3.Text = "";
            groupBox1.Visible = false;
        }


        private void button9_Click_1(object sender, EventArgs e)
        {
            
            List<Button> list = GetStarButtons();
            SetAllButtonsOff(list);

            SetButtonOn(list[0]);

            ButtonReviewCount = 1;

        }

        private void button10_Click_1(object sender, EventArgs e)
        {

            

            int count = 0;
            List<Button> list = GetStarButtons();
            SetAllButtonsOff(list);
            Button x = (Button)sender;

            foreach (var item in list)
            {
                SetButtonOn(item);
                count++;
                if (item == x)
                {
                    break;
                }
            }

            ButtonReviewCount = count;
           
        }           



        private void button11_Click_1(object sender, EventArgs e)
        {
            int count = 0;
            List<Button> list = GetStarButtons();
            SetAllButtonsOff(list);
            Button x = (Button)sender;

            foreach (var item in list)
            {
                SetButtonOn(item);
                count++;
                if (item == x)
                {
                    break;
                }
            }

            ButtonReviewCount = count;
          
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int count = 0;
            List<Button> list = GetStarButtons();
            SetAllButtonsOff(list);
            Button x = (Button)sender;

            foreach (var item in list)
            {
                SetButtonOn(item);
                count++;
                if (item == x)
                {
                    break;
                }
            }

            ButtonReviewCount = count;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int count = 0;
            List<Button> list = GetStarButtons();
            SetAllButtonsOff(list);
            Button x = (Button)sender;

            foreach (var item in list)
            {
                SetButtonOn(item);
                count++;
                if (item == x)
                {
                    break;
                }
            }

            ButtonReviewCount = count;
        }

        
    }
}
