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
        public Dictionary<string, bool> CartStatus = new Dictionary<string, bool>();

        //Lists
        public List<string> BookNames = new List<string>();
        public List<string> BookDescriptionList = new List<string>();
        public List<string> FavBooks = new List<string>();
        public List<string> BookStyle = new List<string>();
        public List<string> BooksInCart = new List<string>();

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

            but.Text = textBox1.Text + " (style: )" + style;
            //if name in list exists in book list
            if (BookList.ContainsKey(textBox1.Text))
            {
                //then writing that there is error
                this.Text = "This name already exists!";
            }
            else
            {
                //else,adding new label to the panel 
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
            //is book list contains book name,what was written in text box
            if (BookList.ContainsKey(textBox3.Text))
            {
                //change book description
                label6.Text = BookList[textBox3.Text];
                textBox3.Text = "";
            }
            else
                this.Text = "Unknown Book";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //is book wasn't added to array with fav books before and it exists
            if (!FavBooks.Contains(textBox3.Text) && BookList.ContainsKey(textBox3.Text))
            {
                //we'r creating new label 
                Label a = new Label();
                a.Location = new Point(4, y1);
                a.Size = new Size(361, 15);
                FavBooks.Add(textBox3.Text);
                isFav.Add(textBox3.Text, true);
                a.Text = textBox3.Text;
                y1 += 15; //plus 15 to y pos
                panel2.Controls.Add(a);
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
                    string isFavourite = "0";
                    string CartStatus = "0";

                    if (isFav.ContainsKey(BookNames[i]) && isFav[BookNames[i]]) //if there is name in fav book list 
                        isFavourite = "1";

                    Style.ToString();

                    string sqlExpression = "INSERT INTO BookTable (BookName,BookDescription," + //sql command
                    "BookStyle,UserName,isFavourite,CartStatus) VALUES " +
                    "(' " + Name + "','" + Desc + "','" + Style + "',"
                    + "'" + CurrentName + "'," + isFavourite + "," + CartStatus + ")";
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

        private void button7_Click(object sender, EventArgs e)
        {
            if (BookList.ContainsKey(textBox4.Text)) //changing description
            {
                BookList[textBox4.Text] = textBox5.Text;
                BookDescriptionList[unusefullFunc(textBox5.Text, 1)] = textBox5.Text;
            }
            else
                this.Text = "Unknown Key";
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
                int y = 14;
                for (int i = 0; i < BookNames.Count; i++)
                {
                    Label but = new Label(); // crete label that represent books
                    but.Location = new Point(3, y);
                    but.Size = new Size(326, 15);
                    but.Text = BookNames[i] + " (style: " + BookStyle[i] + "";
                    panel1.Controls.Add(but);
                    y += 20;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e) // adding to fav list
        {
            if (FavBooks.Contains(textBox7.Text))
            {
                FavBooks.Remove(textBox7.Text);
                isFav.Remove(textBox7.Text);


                int y = 14;
                for (int i = 0; i < BookNames.Count; i++)
                {
                    Label but = new Label();
                    but.Location = new Point(3, y);
                    but.Size = new Size(326, 15);
                    but.Text = BookNames[i] + " (style: " + BookStyle[i] + "";
                    panel1.Controls.Add(but);
                    y += 20;
                }
            }
            else
                this.Text = "There is no any \"" + textBox7.Text + " \" book in your favourites list!";
        }

        private void button9_Click(object sender, EventArgs e) //Not matter
        {
            this.Text = "Book List v2.2";
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e) //load all info
        {
            using (SqlConnection sql = new SqlConnection(connectionString))
            {
                sql.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM BookListDataBase.dbo.BookTable", sql);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        object name = reader.GetValue(0); //get info
                        object description = reader.GetValue(1);
                        object style = reader.GetValue(2);
                        object isFavourite = reader.GetValue(3);
                        object user = reader.GetValue(4);

                        if (user.Equals(CurrentName)) //was written for future accounts adds
                        {
                            if (!BookNames.Contains(name))
                            {
                                Label but = new Label(); // add label to book list
                                but.Location = new Point(3, y);
                                but.Size = new Size(326, 15);
                                BookStyle.Add(style.ToString());
                                but.Text = name.ToString() + style;
                                BookNames.Add(name.ToString());
                                BookList.Add(name.ToString(), description.ToString());
                                BookStyle.Add(style.ToString());
                                BookDescriptionList.Add(description.ToString());
                                y += 20;
                                count++;
                                panel1.Controls.Add(but);

                                if (isFavourite.ToString() == "True")
                                {
                                    Label a = new Label(); // add label to fav books list
                                    a.Location = new Point(4, y1);
                                    a.Size = new Size(361, 15);
                                    FavBooks.Add(name.ToString());
                                    isFav.Add(name.ToString(), true);
                                    a.Text = name.ToString();
                                    y1 += 15;
                                    panel2.Controls.Add(a);
                                }
                            }
                        }
                    }
                }
                sql.Close();
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
            }

            return 0;
        }
    }
}