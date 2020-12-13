using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace _24_NumericSearch_C_sharp
{
    public partial class MainForm : Form
    {
        private NumericMethodBasedDictionary dictionary = new NumericMethodBasedDictionary();
        public MainForm()
        {
            for (int i = 1; i <= 1000000; ++i)
            {
                string s = Convert.ToString(i);
                dictionary.AddEntry(new DictionaryEntry(s, s));
            }

            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string key = this.keyAddInput.Text;
            string desc = this.descAddInput.Text;
            if (key.Length == 0)
            {
                MessageBox.Show("Print word!", "Error", MessageBoxButtons.OK);
                return;
            }

            if (desc.Length == 0)
            {
                MessageBox.Show("Print description!", "Error", MessageBoxButtons.OK);
                return;
            }

            this.keyAddInput.Text = "";
            this.descAddInput.Text = "";

            dictionary.AddEntry(new DictionaryEntry(key, desc));
        }

        private void keyFindInput_TextChanged(object sender, EventArgs e)
        {
            string key = this.keyFindInput.Text;
            if (key.Length == 0)
            {
                return;
            }

            var set = dictionary.Find(new DictionaryEntry(key, ""));

            this.listBoxResult.Items.Clear();

            foreach (DictionaryEntry entry in set)
            {
                this.listBoxResult.Items.Add(entry.KeyS + " == " + entry.Description);
            }

            
        }
    }
}
