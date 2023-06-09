using DZ5_Savchuk.Models;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace DZ5_Savchuk
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            using (LibraryDbContext db = new LibraryDbContext())
            {
                var books = db.Books
                    .OrderBy(x => x.Id)
                    .Select(x => new
                    {
                        Id = x.Id,
                        Title = x.Title,
                        AuthorFName = x.Author.FirstName,
                        AuthorLName = x.Author.LastName,
                        Price = x.Price,
                        Pages = x.Pages,
                        PublisherName = x.Publisher.PublisherName,
                        PublisherAddress = x.Publisher.Address

                    })
                .ToList();

                dataGridView1.DataSource = books;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            int c = 0;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                c = (int)row.Cells[0].Value;
            }
            using (var db = new LibraryDbContext())
            {

                var book = (from b in db.Books
                            where b.Id == c
                            select b)
                     .FirstOrDefault();
                if (MessageBox.Show("Видалити запис", "Повідомлення", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (book != null)
                    {
                        db.Books.Remove(book);
                    }
                    else
                    {
                        MessageBox.Show("Цієї книжки не існує у всесвіті!)");
                    }

                }
                db.SaveChanges();
            }
            LoadData();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            int selectedRowId = (int)dataGridView1.CurrentRow.Cells["Id"].Value;



            using (LibraryDbContext dbContext = new LibraryDbContext())
            {
                Book book = dbContext.Books.FirstOrDefault(b => b.Id == selectedRowId);
                if (book != null)
                {
                    // Заповнюємо поля форми редагування зі значеннями з вибраного запису
                    Author author = dbContext.Authors.FirstOrDefault(a => a.Id == book.AuthorId);
                    Publisher publisher = dbContext.Publishers.FirstOrDefault(p => p.Id == book.PublisherId);
                    AddEditForm form = new AddEditForm(book.Author.FirstName, book.Author.LastName, book.Publisher.PublisherName, book.Publisher.Address, book.Title, (int)book.Pages, (int)book.Price);

                    // Відображаємо форму редагування
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        author = dbContext.Authors.FirstOrDefault(a => a.FirstName == form.AuthorFName && a.LastName == form.AuthorLName);
                        if (author == null)
                        {
                            // Якщо автор не знайдений, створити новий запис для автора
                            author = new Author
                            {
                                Id = book.AuthorId,
                                FirstName = form.AuthorFName,
                                LastName = form.AuthorLName,
                            };
                            dbContext.Authors.Add(author);
                            dbContext.SaveChanges();
                        }

                        publisher = dbContext.Publishers.FirstOrDefault(p => p.PublisherName == form.PublisherName && p.Address == form.PublisherAddress);
                        if (publisher == null)
                        {
                            // Якщо видавництво не знайдене, створити новий запис для видавництва
                            publisher = new Publisher
                            {
                                Id = book.PublisherId,
                                PublisherName = form.PublisherName,
                                Address = form.PublisherAddress
                            };
                            dbContext.Publishers.Add(publisher);
                            dbContext.SaveChanges();
                        }

                        // Оновлюємо значення полів вибраного запису
                        book.Title = form.BookTitle;
                        book.AuthorId = author.Id;
                        book.Pages = form.Pages;
                        book.Price = form.Price;
                        book.PublisherId = publisher.Id;

                        dbContext.SaveChanges();
                    }
                }
                else
                {
                    MessageBox.Show("Книга не знайдена.");
                }
            }

            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddEditForm form = new AddEditForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                using (LibraryDbContext dbContext = new LibraryDbContext())
                {
                    Author author = dbContext.Authors.FirstOrDefault(a => a.FirstName == form.AuthorFName && a.LastName == form.AuthorLName);
                    if (author == null)
                    {
                        author = new Author
                        {
                            FirstName = form.AuthorFName,
                            LastName = form.AuthorLName,
                        };
                        dbContext.Authors.Add(author);
                        dbContext.SaveChanges();
                    }

                    Publisher publisher = dbContext.Publishers.FirstOrDefault(p => p.PublisherName == form.PublisherName && p.Address == form.PublisherAddress);
                    if (publisher == null)
                    {
                        publisher = new Publisher
                        {
                            PublisherName = form.PublisherName,
                            Address = form.PublisherAddress
                        };
                    }
                    dbContext.Publishers.Add(publisher);
                    dbContext.SaveChanges();

                    Book book = new Book
                    {
                        AuthorId = author.Id,
                        PublisherId = publisher.Id,
                        Title = form.BookTitle,
                        Pages = form.Pages,
                        Price = form.Price
                    };
                    dbContext.Books.Add(book);
                    dbContext.SaveChanges();
                }
            }
            LoadData();
        }
        public void AddData()
        {


        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}