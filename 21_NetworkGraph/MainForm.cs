using Microsoft.Glee.Drawing;
using Microsoft.Glee.GraphViewerGdi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _21_NetworkGraph
{
    public partial class MainForm : Form
    {
        private class Way
        {
            public Way(Int32 time, string prevP)
            {
                this.Time = time;
                this.PreviousP = prevP;
            }


            public Int32 Time { get; set; }
            public string PreviousP { get; set; }

        }


        private GViewer gViewer = new GViewer();


        private Hashtable workNames = new Hashtable();
        private Hashtable nodeNames = new Hashtable();
        private Hashtable worksFor = new Hashtable();
        private Hashtable worksBack = new Hashtable();
        private Hashtable visited = new Hashtable();
        private List<string> worksWithoutParents = new List<string>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string[,] columns = new string[,]
            {
                {"id", "Work id",},
                {"name", "Work name",},
                {"time", "Time",},
                {"links", "Links (id, id, ...)",},
            };

            gViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel1.Controls.Add(gViewer);

            for (int i = 0; i < 4; ++i)
            {
                this.graphData.Columns.Add(columns[i,0], columns[i,1]);
            }

            //example dataset
            this.graphData.Rows.Add(new string[] {"1", "Create part 1", "5",    ""});
            this.graphData.Rows.Add(new string[] {"2", "Create part 2", "3",    ""});
            this.graphData.Rows.Add(new string[] {"3", "Create part 3", "10",   ""});
            this.graphData.Rows.Add(new string[] {"4", "Make part 1",   "7",    "1"});
            this.graphData.Rows.Add(new string[] {"5", "Make part 2",   "10",   "2"});
            this.graphData.Rows.Add(new string[] {"6", "Make part 3",   "5",    "4,5"});
            this.graphData.Rows.Add(new string[] {"7", "Make part 4",   "9",    "2,3"});
            this.graphData.Rows.Add(new string[] {"8", "Final work",    "4",    "6,7"});
            this.graphData.Rows.Add(new string[] {"9", "Testing",       "2",    "8"});

            this.splitContainer1.Panel1.ResumeLayout();
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            Graph g = new Graph("Graph");

            workNames.Clear();
            nodeNames.Clear();
            worksFor.Clear();
            worksBack.Clear();

            // fill works names
            foreach (DataGridViewRow row in this.graphData.Rows)
            {
                if (!row.IsNewRow)
                {
                    workNames.Add(row.Cells[0].Value, row.Cells[1].Value);
                }
            }

            // fill works info
            foreach (DataGridViewRow row in this.graphData.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                string work = row.Cells[0].Value.ToString();
                List<string> ids = row.Cells[3].Value.ToString().Split(',').ToList<string>();
                ids.RemoveAll(s => String.IsNullOrEmpty(s));
                Int32 time;
                try
                {
                    time = Convert.ToInt32(row.Cells[2].Value);
                } catch(FormatException ex)
                {
                    MessageBox.Show(
                            "Time is not a number!", "ERROR",
                            MessageBoxButtons.OK, MessageBoxIcon.Error
                            );
                    return;
                } 

                foreach (var id in ids)
                {
                    if (!workNames.Contains(id))
                    {
                        MessageBox.Show(
                            "Can't find id: " + id + " in id's column", "ERROR", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error
                            );
                        return;
                    }

                    if (!worksFor.Contains(work))
                    {
                        worksFor.Add(work, new List<Way>());
                    }
                    (worksFor[work] as List<Way>).Add(new Way(time, id));

                    if (!worksBack.Contains(id))
                    {
                        worksBack.Add(id, new List<Way>());
                    }
                    (worksBack[id] as List<Way>).Add(new Way(time, work));
                }
            }
            
            // find initial works

            foreach (var id in workNames.Keys)
            {
                if (!worksFor.ContainsKey(id))
                {
                    worksWithoutParents.Add(id as string);
                }
            }

            // find duplicate works and create fake works

            foreach (var id in worksBack.Keys)
            {
                //List<Way> l = worksBack[id] as List<Way>;
                
                
                //while (l.Count > 1)
                //{
                //    l.RemoveAt(l.Count - 1);
                //}
            }


            visited.Clear();
            foreach(var p in workNames.Keys)
            {
                Boolean f = false;
                visited.Add(p, f);
            }


            gViewer.Graph = g;
        }

        private void RecAddEdge(Graph g, string cur, List<Way> next)
        {
            if (next == null) return;
            if (!Convert.ToBoolean(visited[cur]))
            {

            }
            foreach(Way w in next)
            {
                Edge e = new Edge(cur, w.Time.ToString(), w.PreviousP);
                if (!g.Edges.Contains(e))
                {
                    g.AddEdge(e.Source, e.EdgeAttr.Label, e.Target);
                    RecAddEdge(g, w.PreviousP, (worksBack[w.PreviousP] as List<Way>));
                }
            }

        }

        private void forwardMove()
        {

        }

        private void backwardMove()
        {

        }

    }
}
