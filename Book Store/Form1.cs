/*                 Info:
 * 
 * 
 *
 */

// System libs init
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Book_Store
{
    public partial class Form1 : Form
    {
        //Dictionaries
        public Dictionary<string, string> BookList = new Dictionary<string, string>();
        public Dictionary<string, int> BookNumber = new Dictionary<string, int>();
        public Dictionary<string, bool> isFav = new Dictionary<string, bool>();
        public Dictionary<string, int> BookPrice = new Dictionary<string, int>();
        public Dictionary<string, string> PriceCurrency = new Dictionary<string, string>();
        public Dictionary<string, int> BookPageCounter = new Dictionary<string, int>();

        //Lists 
        public List<string> BooksInCart = new List<string>();
        public List<string> BookNames = new List<string>();
        public List<string> BookDescriptionList = new List<string>();
        public List<string> FavBooks = new List<string>();
        public List<string> BookStyle = new List<string>();

        public string connectionString = @"Data Source=WIN-8IOA84HLB36;Initial Catalog=BookListDataBase;Integrated Security=True";
        private string CurrentName = "Anton";

        int y = 14, count, y1 = 0;

        public bool haveInfo = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Create new label
            Label but = new Label();
            but.Location = new Point(3, y);
            but.Size = new Size(326, 15);

            string style;

            //Choose style,cheking if anybody has clicked on rabiobutton 
            if (radioButton1.Checked)
            {
                style = "fantasy";
            }
            else if (radioButton2.Checked)
            {
                style = "img.literature";
            }
            else if (radioButton3.Checked)
            {
                style = "sci.literature";
            }
            else if (radioButton4.Checked)
            {
                style = "other";
            }
            else
            {
                style = "unknown";
            }

            BookStyle.Add(style);

            but.Text = textBox1.Text + " (style: " + style + ")";

            //if name in list exists in book list
            if (BookList.ContainsKey(textBox1.Text))
            {
                //then writing that there is error
                this.Text = "This name already exists!";
            }
            else
            {
                //else,adding new label to the panel 

                if (textBox4.Text != "" && comboBox1.Text != "")
                {
                    BookPrice.Add(textBox1.Text, Int32.Parse(textBox4.Text));
                    PriceCurrency.Add(textBox1.Text, comboBox1.Text);
                }
                else
                {
                    BookPrice.Add(textBox1.Text, 0);
                    PriceCurrency.Add(textBox1.Text, "-");
                }

                if (comboBox2.Text != "")
                {
                    BookPageCounter.Add(textBox1.Text, Int32.Parse(comboBox2.Text));
                }
                else
                {
                    BookPageCounter.Add(textBox1.Text, 0);
                }


                BookNames.Add(textBox1.Text);
                BookList.Add(textBox1.Text, textBox2.Text);
                BookStyle.Add(style);
                BookDescriptionList.Add(textBox2.Text);
                textBox1.Text = "";
                textBox2.Text = "";
                comboBox2.Text = "";
                textBox4.Text = "";
                comboBox1.Text = "";
                y += 20;
                count++;
                panel1.Controls.Add(but);
            }
        }

        private void InstBookName(string Style, string BookName, string BookDesc, string price, string PriceCur, string pages)
        {
            //Create new label
            Label but = new Label();
            but.Location = new Point(3, y);
            but.Size = new Size(326, 15);

            BookStyle.Add(Style);

            but.Text = BookName + " (style: " + Style + ")";

            //if name in list exists in book list
            if (BookList.ContainsKey(BookName))
            {
                //then writing that there is error
                this.Text = "This name already exists!";
            }
            else
            {
                //else,adding new label to the panel 

                if (price != "" && PriceCur != "")
                {
                    BookPrice.Add(BookName, Int32.Parse(price));
                    PriceCurrency.Add(BookName, PriceCur);
                }
                else
                {
                    BookPrice.Add(BookName, 0);
                    PriceCurrency.Add(BookName, "-");
                }

                if (pages != "")
                {
                    BookPageCounter.Add(BookName, Int32.Parse(pages));
                }
                else
                {
                    BookPageCounter.Add(BookName, 0);
                }


                BookNames.Add(BookName);
                BookList.Add(BookName, BookDesc);
                BookStyle.Add(Style);
                BookDescriptionList.Add(BookDesc);
                y += 20;
                count++;
                panel1.Controls.Add(but);

                this.Text = BookNames[0].ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString)) //Creating new sql connection
            {
                connection.Open(); //opening it

                using (var command = new SqlCommand("DELETE FROM BookTable", connection)) //delete all info from sql table
                {
                    command.ExecuteNonQuery();
                }

                for (int i = 0; i < BookNames.Count; i++)
                {
                    string Name = BookNames[i].ToString();
                    string Desc = BookDescriptionList[i].ToString();
                    string Style = BookStyle[i].ToString();
                    int Page = BookPageCounter[Name];
                    string PriceCurency = PriceCurrency[Name];
                    int Price = BookPrice[Name];

                    string isFavourite = "0";
                    string CartStatus = "0";

                    if (isFav.ContainsKey(BookNames[i]) && isFav[BookNames[i]]) //if there is name in fav book list 
                        isFavourite = "1";

                    Style.ToString();

                    string sqlExpression = "INSERT INTO BookTable (BookName,BookDescription," + //sql command
                    "BookStyle,UserName,isFavourite,CartStatus,PageCount,Price,PriceCurrency) VALUES " +
                    "('" + Name + "','" + Desc + "','" + Style + "',"
                    + "'" + CurrentName + "'," + isFavourite + "," +
                    CartStatus + "," + Page + "," + Price + ",'" +
                    PriceCurency + "')";

                    SqlCommand command = new SqlCommand(sqlExpression, connection); //givving command to sql server
                    command.ExecuteNonQuery();
                }
                this.Close();
            }

            //this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Settings st = new Settings(); //open settings
            st.Show();
        }

        private void textBox5_TextChanged(object sender, EventArgs e) //not matter
        {

        }

        private void button2_Click(object sender, EventArgs e)//  Та самая функция удаления книг
        {
            if (BookList.ContainsKey(textBox3.Text))
            {
                BookList.Remove(textBox3.Text); //delete all info
                BookStyle.Remove(textBox3.Text);
                BookNames.Remove(textBox3.Text);
                FavBooks.Remove(textBox3.Text);
                BookDescriptionList.Remove(textBox3.Text);
                panel1.Invalidate();    // clear book in list  

                checkBox3.Checked = false;
                checkBox3.Checked = false;
                label10.Text = "Book deleted";
                label11.Text = "Book deleted";
                textBox8.Text = "";
                comboBox3.Text = "";
                textBox7.Text = "";
                comboBox4.Text = "";

                int y = 14;
                for (int i = 0; i < BookNames.Count; i++)
                {
                    Label but = new Label(); // crete    label that represent books
                    but.Location = new Point(3, y);
                    but.Size = new Size(326, 15);
                    but.Text = BookNames[i] + " (style: " + BookStyle[i] + "";
                    panel1.Controls.Add(but);
                    y += 20;
                }
            }
        }

        private void button9_Click(object sender, EventArgs e) //Not matter
        {
            this.Text = "Book List v2.2";
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked && BookNames.Contains(textBox3.Text) && !FavBooks.Contains(textBox3.Text))
            {
                Label a = new Label();
                a.Location = new Point(4, y1);
                a.Size = new Size(361, 15);
                FavBooks.Add(textBox3.Text);
                if (!isFav.ContainsKey(textBox3.Text))
                    isFav.Add(textBox3.Text, true);
                else
                    isFav[textBox3.Text] = true;

                a.Text = textBox3.Text;
                y1 += 15; //plus 15 to y pos
                panel2.Controls.Add(a);
            }
            else if (!checkBox3.Checked && FavBooks.Contains(textBox3.Text))
            {
                FavBooks.Remove(textBox7.Text);
                isFav.Remove(textBox7.Text);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (BookNames.Contains(textBox3.Text))
            {
                textBox8.Text = BookList[textBox3.Text].ToString();
                textBox7.Text = BookPrice[textBox3.Text].ToString();
                label10.Text = BookList[textBox3.Text];
                label11.Text = "Book name: " + textBox3.Text;
                comboBox3.Text = BookPageCounter[textBox3.Text].ToString();
                comboBox4.Text = PriceCurrency[textBox3.Text];



                //textBox5.Text = BookPrice[textBox3.Text].ToString();

                if (FavBooks.Contains(textBox3.Text))
                {
                    checkBox3.Checked = true;
                }
                else
                {
                    checkBox3.Checked = false;
                }
            }
            else
            {
                checkBox3.Checked = false;
                checkBox3.Checked = false;
                label10.Text = "Unknown book";
                label11.Text = "Book name: Unknown";
                textBox8.Text = "";
                comboBox3.Text = "";
                textBox7.Text = "";
                comboBox4.Text = "";

            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            BookList[textBox3.Text] = textBox8.Text;
            if (textBox7.Text != "")
            {
                BookPrice[textBox3.Text] = Int32.Parse(textBox7.Text);
            }

            if (comboBox4.Text != "")
            {
                PriceCurrency[textBox3.Text] = comboBox4.Text;
            }

            if (comboBox3.Text != "")
            {
                BookPageCounter[textBox3.Text] = Int32.Parse(comboBox3.Text);
            }
        }
        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox3.Checked && !BooksInCart.Contains(textBox3.Text) && BookNames.Contains(textBox3.Text))
            {
                BooksInCart.Add(textBox3.Text);
            }
            else if (!checkBox3.Checked && BooksInCart.Contains(textBox3.Text))
            {
                BooksInCart.Remove(textBox3.Text);
            }
        }

        private void Form1_Load(object sender, EventArgs e) //load all info
        {
            comboBox2.Items.Add("Write number");
            comboBox3.Items.Add("Write number");
            for (int i = 0; i <= 2000; i += 100)
            {
                if (i != 0)
                {
                    comboBox2.Items.Add(i.ToString());
                    comboBox3.Items.Add(i.ToString());
                }
            }

            comboBox1.Items.Add(" Euro");
            comboBox1.Items.Add(" Dollars");
            comboBox1.Items.Add(" Lei");

            comboBox4.Items.Add(" Euro");
            comboBox4.Items.Add(" Dollars");
            comboBox4.Items.Add(" Lei");

            using (SqlConnection sql = new SqlConnection(connectionString))
            {
                sql.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM BookListDataBase.dbo.BookTable", sql);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        object name1 = reader.GetValue(0).ToString(); //get info
                        object description = reader.GetValue(1).ToString();
                        object style = reader.GetValue(2);
                        object user = reader.GetValue(3);
                        object isFavourite = reader.GetValue(4);

                        object CartStatus = reader.GetValue(5);
                        object Pages = reader.GetValue(6);
                        object Price = reader.GetValue(7);
                        object PriseCurrency = reader.GetValue(8);


                        string name = name1.ToString().Substring(1);


                        if (true) //was written for future accounts adds
                        {
                            InstBookName(style.ToString(), name.ToString(), description.ToString(),
                            Price.ToString(), PriseCurrency.ToString(), Pages.ToString());
                        }
                    }
                }
                sql.Close();
                this.Text = "Book List v2.2";
            }
        }
        private int unusefullFunc(string name, int mode) // not matter
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

                case 2:
                    for (int i = 0; i < BookList.Count(); i++)
                    {
                        if (BookStyle[i] == name)
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