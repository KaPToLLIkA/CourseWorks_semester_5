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
        private class WorkData
        {
            public WorkData(string name, Int32 time, List<string> parents)
            {
                this.Time = time;
                this.Name = name;
                this.Parents = parents;
            }

            public Int32 Time { get; set; }
            public string Name { get; set; }
            public List<string> Parents { get; set; }
        }


        private class Way
        {
            public Way(string from, Int32 time, string to, string workId)
            {
                this.WorkId = workId;
                this.From = from;
                this.Time = time;
                this.To = to;
            }


            public Int32 Time { get; set; }
            public string To { get; set; }
            public string From { get; set; }
            public string WorkId { get; set; }

        }


        private GViewer gViewer = new GViewer();
        
        private Hashtable worksData = new Hashtable();

        private string startingStateName = "START";
        private string endingStateName = "FINISH";

        private List<string> availableWorksIds = new List<string>();
        private List<string> worksIdsWithoutChilds = new List<string>();
        private List<string> worksIdsWithoutParents = new List<string>();
        private List<string> worksIdsWithParentsAndChilds = new List<string>();

        private Int32 lastStateNumber;
        private List<string> allStates = new List<string>();
        private Hashtable worksForwardBypass = new Hashtable();
        private Hashtable worksBackwardBypass = new Hashtable();
        
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

        private void rebuildGraphView()
        {
            Graph g = new Graph("Graph");

            foreach (DictionaryEntry entry in worksForwardBypass)
            {
                foreach (var way in entry.Value as List<Way>)
                {
                    string title = way.Time == 0 ? "F 0" : "" + way.Time.ToString();
                    g.AddEdge(way.From, title, way.To);
                }
            }

            gViewer.Graph = g;
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            availableWorksIds.Clear();
            worksData.Clear();

            lastStateNumber = 1;
            worksForwardBypass.Clear();
            worksBackwardBypass.Clear();
            allStates.Clear();

            worksIdsWithoutParents.Clear();
            worksIdsWithoutChilds.Clear();
            worksIdsWithParentsAndChilds.Clear();

            parseTableInput();

            calculateBackwForwWays();
            rebuildGraphView();

            forwardMove();
            backwardMove();
        }

        private void parseTableInput()
        {
           

            // fill available works ids
            foreach (DataGridViewRow row in this.graphData.Rows)
            {
                if (!row.IsNewRow)
                {
                    if (availableWorksIds.Contains(row.Cells[0].Value as string))
                    {
                        MessageBox.Show(
                           "Duplicate ids! Id: "
                           + row.Cells[0].Value as string
                           + " duplicated.",
                           "ERROR",
                           MessageBoxButtons.OK, MessageBoxIcon.Error
                           );
                        return;
                    }

                    availableWorksIds.Add(row.Cells[0].Value as string);
                }
            }

            worksIdsWithoutChilds = this.availableWorksIds.ToList<string>();

            // fill works info
            foreach (DataGridViewRow row in this.graphData.Rows)
            {
                if (!row.IsNewRow)
                {
                    //getting parents
                    string workId = row.Cells[0].Value.ToString();
                    string workName = row.Cells[1].Value.ToString();
                    List<string> parents = row.Cells[3].Value.ToString().Split(',').ToList<string>();
                    parents.RemoveAll(s => String.IsNullOrEmpty(s));

                    //getting time
                    Int32 time;
                    try
                    {
                        time = Convert.ToInt32(row.Cells[2].Value);
                    }
                    catch (FormatException ex)
                    {
                        MessageBox.Show(
                                "Time is not a number!", "ERROR",
                                MessageBoxButtons.OK, MessageBoxIcon.Error
                                );
                        return;
                    }

                    if (parents.Count == 0)
                    {
                        worksIdsWithoutParents.Add(workId);
                    }
                    else
                    {
                        //checking parents
                        foreach (var id in parents)
                        {
                            if (!availableWorksIds.Contains(id))
                            {
                                MessageBox.Show(
                                    "Can't find id: " + id + " in id's column", "ERROR",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error
                                    );
                                return;
                            }
                            worksIdsWithoutChilds.Remove(id);
                        }
                    }

                    worksData.Add(workId, new WorkData(workName, time, parents));
                }
            }

            // check the availability of final works
            //
            // WARN: the situation of no final work may not be possible
            if (worksIdsWithoutChilds.Count == 0)
            {
                MessageBox.Show(
                           "There must be at least one final job!",
                           "ERROR",
                           MessageBoxButtons.OK, MessageBoxIcon.Error
                           );
            }

            // check the availability of initial works
            //
            // WARN: the situation of no initial work may not be possible
            if (worksIdsWithoutParents.Count == 0)
            {
                MessageBox.Show(
                           "There must be at least one initial job!",
                           "ERROR",
                           MessageBoxButtons.OK, MessageBoxIcon.Error
                           );
            }

            // init hashtables
            foreach (var id in availableWorksIds)
            {
                worksBackwardBypass.Add(id, new List<Way>());
                worksForwardBypass.Add(id, new List<Way>());
            }

            // find work ids with childs and with parents
            worksIdsWithParentsAndChilds = this.availableWorksIds.ToList<string>();
            // remove final works
            foreach (var id in worksIdsWithoutChilds)
            {
                if (worksIdsWithParentsAndChilds.Contains(id))
                {
                    worksIdsWithParentsAndChilds.Remove(id);
                }
            }
            // remove initial works
            foreach (var id in worksIdsWithoutParents)
            {
                if (worksIdsWithParentsAndChilds.Contains(id))
                {
                    worksIdsWithParentsAndChilds.Remove(id);
                }
            }
        }

        private void addForwardWay(Way way)
        {
            (worksForwardBypass[way.WorkId] as List<Way>).Add(way);
        }

        private void addBackwardWay(Way way)
        {
            (worksBackwardBypass[way.WorkId] as List<Way>).Add(way);
        }

        private bool filterFictitiousWorks(Way way)
        {
            return way.Time != 0;
        }

        private List<Way> createBidirWay(string from, Int32 time, string workId)
        {
            lastStateNumber++;
            allStates.Add(lastStateNumber.ToString());

            List<Way> ways = new List<Way>();

            ways.Add(new Way(
                from, time, lastStateNumber.ToString(), workId
                ));

            ways.Add(new Way(
                lastStateNumber.ToString(), time, from, workId
                ));
            return ways;
        }

        private List<Way> createBidirWay(string from, Int32 time, string to, string workId)
        {
            allStates.Add(to);
            List<Way> ways = new List<Way>();

            ways.Add(new Way(
                from, time, to, workId
                ));

            ways.Add(new Way(
                to, time, from, workId
                ));
            return ways;
        }

        private void calculateBackwForwWays()
        {
            // processing initial works
            allStates.Add(startingStateName);
            foreach (var id in worksIdsWithoutParents)
            {
                var workDt = (worksData[id] as WorkData);
                var bidirWays = createBidirWay(startingStateName, workDt.Time, id);

                addForwardWay(bidirWays[0]);
                addBackwardWay(bidirWays[1]);
            }

            // processing interim works
            bool idWasNotProcessed = true;
            while (idWasNotProcessed)
            {
                idWasNotProcessed = false;
                foreach (var id in worksIdsWithParentsAndChilds)
                {
                    var workDt = (worksData[id] as WorkData);
                    var parents = workDt.Parents;

                    if (parents.Count == 1)
                    {
                        var prevWays = (worksForwardBypass[parents[0]] as List<Way>).FindAll(filterFictitiousWorks);

                        if (prevWays.Count > 1)
                        {
                            MessageBox.Show(
                               "The work was completed more than once! WId: " +
                               parents[0],
                               "ERROR",
                               MessageBoxButtons.OK, MessageBoxIcon.Error
                               );
                        }
                        else if (prevWays.Count == 1)
                        {
                            var bidirWays = createBidirWay(prevWays[0].To, workDt.Time, id);

                            addForwardWay(bidirWays[0]);
                            addBackwardWay(bidirWays[1]);
                        } 
                        else
                        {
                            idWasNotProcessed = true;
                        }
                    }
                    else
                    {
                        // checking the availability of previous works
                        bool available = true;
                        foreach (var pid in parents)
                        {
                            var prevWays = (worksForwardBypass[parents[0]] as List<Way>).FindAll(filterFictitiousWorks);

                            if (prevWays.Count > 1)
                            {
                                MessageBox.Show(
                                   "The work was completed more than once! WId: " +
                                   parents[0],
                                   "ERROR",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error
                                   );
                            }
                            else if (prevWays.Count == 0)
                            {
                                available = false;
                                idWasNotProcessed = true;
                            }
                        }

                        // building ways
                        if (available)
                        {
                            lastStateNumber++;
                            string fictitiosStateName = lastStateNumber.ToString();
                            //building fictitious works
                            foreach (var pid in parents)
                            {
                                var prevWays = (worksForwardBypass[pid] as List<Way>).FindAll(filterFictitiousWorks);

                                var fictitiousBidirWays = createBidirWay(prevWays[0].To, 0, fictitiosStateName, pid);

                                addForwardWay(fictitiousBidirWays[0]);
                                addBackwardWay(fictitiousBidirWays[1]);
                            }

                            //building normal work
                            var bidirWays = createBidirWay(fictitiosStateName, workDt.Time, id);

                            addForwardWay(bidirWays[0]);
                            addBackwardWay(bidirWays[1]);
                        }
                    }
                }
            }

            // processing final works
            foreach (var id in worksIdsWithoutChilds)
            {
                var workDt = (worksData[id] as WorkData);
                var parents = workDt.Parents;

                if (parents.Count == 1)
                {
                    var prevWays = (worksForwardBypass[parents[0]] as List<Way>).FindAll(filterFictitiousWorks);

                    if (prevWays.Count > 1)
                    {
                        MessageBox.Show(
                           "The work was completed more than once! WId: " +
                           parents[0],
                           "ERROR",
                           MessageBoxButtons.OK, MessageBoxIcon.Error
                           );
                    }
                    else if (prevWays.Count == 1)
                    {
                        var bidirWays = createBidirWay(prevWays[0].To, workDt.Time, endingStateName, id);

                        addForwardWay(bidirWays[0]);
                        addBackwardWay(bidirWays[1]);
                    }
                }
                else
                {
                    lastStateNumber++;
                    string fictitiosStateName = lastStateNumber.ToString();
                    //building fictitious works
                    foreach (var pid in parents)
                    {
                        var prevWays = (worksForwardBypass[pid] as List<Way>).FindAll(filterFictitiousWorks);

                        if (prevWays.Count > 1)
                        {
                            MessageBox.Show(
                               "The work was completed more than once! WId: " +
                               parents[0],
                               "ERROR",
                               MessageBoxButtons.OK, MessageBoxIcon.Error
                               );
                        }
                        else if (prevWays.Count == 1)
                        {
                            var fictitiousBidirWays = createBidirWay(prevWays[0].To, 0, fictitiosStateName, pid);

                            addForwardWay(fictitiousBidirWays[0]);
                            addBackwardWay(fictitiousBidirWays[1]);
                        }
                    }

                    //building normal work
                    var bidirWays = createBidirWay(fictitiosStateName, workDt.Time, endingStateName, id);

                    addForwardWay(bidirWays[0]);
                    addBackwardWay(bidirWays[1]);
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
