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

namespace Book_Store
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                File.WriteAllText("Book StoreF/Book style list.txt", "");
                File.WriteAllText("Book StoreF/Book fav list.txt", "");
                File.WriteAllText("Book StoreF/Book list.txt", "");
            }
            else if (radioButton2.Checked) {
                File.WriteAllText("Book StoreF/Book fav list.txt", "");
            }
        }

      
        private void button2_Click(object sender, EventArgs e)
        {
            System.Media.SystemSounds.Asterisk.Play();
            MessageBox.Show("Book list v2.1 by Anton", "Book list");
        }

       
        private void button4_Click_1(object sender, EventArgs e)
        {

        }
    }
}
