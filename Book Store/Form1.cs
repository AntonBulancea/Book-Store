using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace Book_Store
{
    public partial class Form1 : Form
    {

        public Dictionary<string, string> BookList = new Dictionary<string, string>();
        public Dictionary<string, int> BookNumber = new Dictionary<string, int>();

        public List<string> BookNames = new List<string>();
        public List<string> BookDescriptionList = new List<string>();
        public List<string> FavBooks = new List<string>();
        public List<string> BookStyle = new List<string>();



        int y = 14, count, y1 = 0;

        public bool haveInfo = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Label but = new Label();
            but.Location = new Point(3, y);
            but.Size = new Size(326, 15);

            string style;

            if (radioButton1.Checked)
            {
                style = " (style: fantasy)";
            }
            else if (radioButton2.Checked)
            {
                style = " (style: img. literature)";
            }
            else if (radioButton3.Checked)
            {
                style = " (style: sci. literature)";
            }
            else if (radioButton4.Checked)
            {
                //ddsad
                //New Comment
                style = " (style: other)";
            }
            else
            {
                style = " (style: unknown)";
            }

            BookStyle.Add(style);

            but.Text = textBox1.Text + style;

            if (BookList.ContainsKey(textBox1.Text))
            {
                this.Text = "This name already exists!";
            }
            else
            {

                BookNames.Add(textBox1.Text);
                BookList.Add(textBox1.Text, textBox2.Text);
                BookStyle.Add(style);
                BookDescriptionList.Add(textBox2.Text);
                textBox1.Text = "";
                textBox2.Text = "";
                y += 20;
                count++;
                panel1.Controls.Add(but);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (BookList.ContainsKey(textBox3.Text))
            {
                label6.Text = BookList[textBox3.Text];
                textBox3.Text = "";
            }
            else
                this.Text = "Unknown Book";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!FavBooks.Contains(textBox3.Text) && BookList.ContainsKey(textBox3.Text))
            {
                Label a = new Label();
                a.Location = new Point(4, y1);
                a.Size = new Size(361, 15);
                FavBooks.Add(textBox3.Text);
                a.Text = textBox3.Text;
                y1 += 15;
                panel2.Controls.Add(a);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            File.WriteAllText("Book StoreF/Book list.txt", "");
            File.WriteAllText("Book StoreF/Book fav list.txt", "");
            File.WriteAllText("Book StoreF/Book style list.txt", "");

            int tea = 0;

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(
            "Book StoreF/Book list.txt",
            true))
            {
                for (tea = 0; tea < BookNames.Count(); tea++)
                {
                    file.WriteLine(BookNames[tea] + "," + BookDescriptionList[tea]);
                }

            }
            //this.Text = tea.ToString();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(
            "Book StoreF/Book fav list.txt",
            true))

            {
                for (int q = 0; q < FavBooks.Count(); q++)
                {
                    file.WriteLine(FavBooks[q]);
                }
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(
            "Book StoreF/Book style list.txt",
            true))

            {
                for (int q = 0; q < BookStyle.Count(); q += 2)
                {
                    file.WriteLine(BookStyle[q]);
                }
            }
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Settings st = new Settings();
            st.Show();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (BookList.ContainsKey(textBox4.Text))
            {
                BookList[textBox4.Text] = textBox5.Text;
                BookDescriptionList[unusefullFunc(textBox5.Text, 1)] = textBox5.Text;
                textBox4.Text = "";
                textBox5.Text = "";
            }
            else
                this.Text = "Unknown Key";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (BookList.ContainsKey(textBox3.Text))
            {
                BookList.Remove(textBox3.Text);
                BookStyle.Remove(textBox3.Text);
                BookNames.Remove(textBox3.Text);
                FavBooks.Remove(textBox3.Text);
                BookDescriptionList.Remove(textBox3.Text);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (FavBooks.Contains(textBox7.Text))
            {
                FavBooks.Remove(textBox7.Text);
                textBox7.Text = "";
            }
            else
                this.Text = "There is no any \"" + textBox7.Text + " \" book in your favourites list!";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Text = "Book List v2.2";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string textFile  = "Book StoreF/Book list.txt";
            string textFile2 = "Book StoreF/Book fav list.txt";
            string textFile3 = "Book StoreF/Book style list.txt";

            if (File.Exists(textFile3))
            {
                string[] lines = File.ReadAllLines(textFile3);
                foreach (string line in lines)
                {
                    BookStyle.Add(line);
                }
            }

            if (File.Exists(textFile2))
            {
                string[] lines = File.ReadAllLines(textFile2);
                foreach (string line in lines)
                {
                    //Console.Beep(500,500);
                    FavBooks.Add(line);
                    Label a = new Label();
                    a.Location = new Point(0, y1);
                    a.Size = new Size(361, 15);
                    a.Text = line;
                    y1 += 15;
                    panel2.Controls.Add(a);
                }
            }

            if (File.Exists(textFile))
            {
                string[] lines = File.ReadAllLines(textFile);
                List<string> linesDesc = new List<string>();

                int count = 0, i = 0;
                // this.Text = BookStyle.Count().ToString();
                foreach (string line in lines)
                {
                    string[] splitLine = line.Split(',');

                    Label but = new Label();
                    but.Location = new Point(3, y);
                    but.Size = new Size(326, 23);
                    but.Text = splitLine[0] + " " + BookStyle[i] + " ";
                    BookList.Add(splitLine[0], splitLine[1]);
                    BookNames.Add(splitLine[0]);
                    BookDescriptionList.Add(splitLine[1]);
                    y += 30;
                    panel1.Controls.Add(but);
                    i++;
                }
                count++;
            }
        }
        private int unusefullFunc(string name, int mode)
        {
            switch (mode)
            {
                case 1:
                    for (int i = 0; i < BookList.Count(); i++)
                    {
                        if (BookDescriptionList[i] == name)
                        {
                            return i;
                        }
                    }
                    break;
            }

            return 0;
        }
    }
}