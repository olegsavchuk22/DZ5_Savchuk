using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DZ5_Savchuk
{
    public partial class AddEditForm : Form
    {
        public string AuthorFName
        {
            get { return textBox1.Text; }

        }
        public string AuthorLName
        {
            get { return textBox2.Text; }

        }
        public string PublisherName
        {
            get { return textBox3.Text; }

        }
        public string PublisherAddress
        {
            get { return textBox4.Text; }

        }
        public string BookTitle
        {
            get { return textBox5.Text; }

        }
        public int Pages
        {
            get { return int.Parse(textBox6.Text); }

        }
        public int Price
        {
            get { return int.Parse(textBox6.Text); }

        }
        public AddEditForm()
        {
            InitializeComponent();
        }
        public AddEditForm(string afn, string aln, string pn, string pa, string bt, int pg, int pr)
        {
            InitializeComponent();
            textBox1.Text = afn;
            textBox2.Text = aln;
            textBox3.Text = pn;
            textBox4.Text = pa;
            textBox5.Text = bt;
            textBox6.Text = pg.ToString();
            textBox7.Text = pr.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
