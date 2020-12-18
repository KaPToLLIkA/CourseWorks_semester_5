using Microsoft.Glee.Drawing;
using Microsoft.Glee.GraphViewerGdi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graph_Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Graph g = new Graph("Example1");

            string[] nodeNames = new string[] { "N1", "N2", "N3", "N4", "N5", };

            for (int i = 1; i < nodeNames.Length; ++i)
            {
                g.AddEdge(nodeNames[i], nodeNames[i] + " - " + nodeNames[i - 1], nodeNames[i - 1]);
            }

            GViewer v = new GViewer();
            v.Size = new System.Drawing.Size(400, 400);
            v.Graph = g;
            this.splitContainer1.Panel1.SuspendLayout();
            v.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Panel1.Controls.Add(v);

            this.splitContainer1.Panel1.ResumeLayout();

            


        }
    }
}
