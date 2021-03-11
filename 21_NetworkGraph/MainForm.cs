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
            // describes the work data structure
            public WorkData(string name, Int32 time, List<string> parents)
            {
                this.Time = time;
                this.Name = name;
                this.Parents = parents;
            }

            public Int32 Time { get; set; }
            public string Name { get; set; }
            public List<string> Parents { get; set; } // work dependencies 
        }


        // describes the way data structure
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
            public string To { get; set; } // state to
            public string From { get; set; } // state from
            public string WorkId { get; set; }

        }

        // describes the node data structure
        private class Node
        {
            public Node(string name)
            {
                this.Name = name;
                this.Forward = 0;
                this.Backward = 0;
                this.Difference = 0;

            }

            public Int32 Forward { get; set; }
            public Int32 Backward { get; set; }
            public Int32 Difference { get; set; }
            public string Name { get; set; }
        }


        private GViewer gViewer = new GViewer();
        
        private Hashtable worksData = new Hashtable();

        private const string startingStateName = "START";
        private const string endingStateName = "FINISH";

        private List<string> availableWorksIds = new List<string>();
        private List<string> worksIdsWithoutChilds = new List<string>();
        private List<string> worksIdsWithoutParents = new List<string>();
        private List<string> worksIdsWithParentsAndChilds = new List<string>();

        private List<string> criticalWay = new List<string>();

        private Int32 lastStateNumber;

        // bidirectional graph
        private Hashtable stateInputs = new Hashtable(); //key: string("state_name") value: List<string>
        private Hashtable stateOutputs = new Hashtable(); //key: string("state_name") value: List<string>
        private Hashtable allStates = new Hashtable(); //key: string("state_name") value: Node
        private Hashtable worksForwardBypass = new Hashtable(); //key: string("work_id") value: Way
        private Hashtable worksBackwardBypass = new Hashtable(); //key: string("work_id") value: Way

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

            //example dataset (how to cook dinner)
            this.graphData.Rows.Add(new string[] {"1", "Buy pasta",                     "10",   ""});
            this.graphData.Rows.Add(new string[] {"2", "Buy sausages",                  "10",   ""});
            this.graphData.Rows.Add(new string[] {"3", "Buy sauce",                     "10",   ""});
            this.graphData.Rows.Add(new string[] {"4", "Boil the pasta",                "20",   "1"});
            this.graphData.Rows.Add(new string[] {"5", "Boil the sausages",             "5",    "2"});
            this.graphData.Rows.Add(new string[] {"6", "Put the pasta in a plate",      "1",    "4"});
            this.graphData.Rows.Add(new string[] {"7", "Put the sausages in a plate",   "1",    "5"});
            this.graphData.Rows.Add(new string[] {"8", "Add the sauce",                 "2",    "6,7,3"});
            this.graphData.Rows.Add(new string[] {"9", "Eat it!",                       "60",   "8"});

            this.splitContainer1.Panel1.ResumeLayout();
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            
            worksData.Clear();

            lastStateNumber = 1;
            worksForwardBypass.Clear();
            worksBackwardBypass.Clear();
            allStates.Clear();
            stateInputs.Clear();
            stateOutputs.Clear();

            parseTableInput();

            calculateBackwForwWays();

            worksIdsWithoutParents.Clear();
            worksIdsWithoutChilds.Clear();
            worksIdsWithParentsAndChilds.Clear();
            availableWorksIds.Clear();

            // finding critical way
            forwardMove();
            backwardMove();
            calculateCriticalWay();

            rebuildGraphView();
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
            (stateOutputs[way.From] as List<string>).Add(way.WorkId);
            (worksForwardBypass[way.WorkId] as List<Way>).Add(way);
        }

        private void addBackwardWay(Way way)
        {
            (stateInputs[way.From] as List<string>).Add(way.WorkId);
            (worksBackwardBypass[way.WorkId] as List<Way>).Add(way);
        }

        private bool filterFictitiousWorks(Way way)
        {
            return way.Time != 0;
        }

        private List<Way> createBidirWay(string from, Int32 time, string workId)
        {
            lastStateNumber++;
            if (!allStates.ContainsKey(lastStateNumber.ToString()))
            {
                allStates.Add(lastStateNumber.ToString(), new Node(lastStateNumber.ToString()));
                stateOutputs[lastStateNumber.ToString()] = new List<string>();
                stateInputs[lastStateNumber.ToString()] = new List<string>();
            }

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
            if (!allStates.ContainsKey(to))
            {
                allStates.Add(to, new Node(to));
                stateOutputs[to] = new List<string>();
                stateInputs[to] = new List<string>();
            }
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
            if (!allStates.ContainsKey(startingStateName))
            {
                allStates.Add(startingStateName, new Node(startingStateName));
                stateOutputs[startingStateName] = new List<string>();
                stateInputs[startingStateName] = new List<string>();
            }
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

        private void rebuildGraphView()
        {
            Graph g = new Graph("Graph");
            g.MinNodeWidth = 20;
            g.MinNodeHeight = 20;

            foreach (DictionaryEntry entry in worksForwardBypass)
            {
                foreach (var way in entry.Value as List<Way>)
                {
                    string title = way.WorkId + (way.Time == 0 ? "'" : "") + " " + way.Time.ToString();

                    var sFrom = allStates[way.From] as Node;
                    var sTo = allStates[way.To] as Node;

                    string wayFrom = way.From + "(" + sFrom.Forward + ";" + sFrom.Difference + ";" + sFrom.Backward + ")";
                    string wayTo = way.To + "(" + sTo.Forward + ";" + sTo.Difference + ";" + sTo.Backward + ")";
                    g.AddEdge(wayFrom, title, wayTo);

                }
            }

            gViewer.Graph = g;

            string text = "";
            foreach (var state in criticalWay)
            {
                text += "(" + state + ") => ";
            }
            text += "Total Lenght: " + (allStates[endingStateName] as Node).Forward;

            this.textCriticalWay.Text = text;

        }

        private void forwardMove()
        {
            Queue<string> states = new Queue<string>();

            // get first nodes
            {
                var outWorks = stateOutputs[startingStateName] as List<string>;

                foreach (var wId in outWorks)
                {
                    var ways = (worksForwardBypass[wId] as List<Way>)
                        .FindAll(filterFictitiousWorks);
                    states.Enqueue(ways[0].To);
                }
            }

            // processing all nodes
            while (states.Count > 0)
            {
                string curState = states.Dequeue();

                var prevWorks = stateInputs[curState] as List<string>;

                List<Int32> prevTimes = new List<Int32>();

                // calculating max time
                foreach (var work in prevWorks)
                {
                    var ways = (worksBackwardBypass[work] as List<Way>)
                        .FindAll((way) => { return way.From == curState; });

                    var prevNode = allStates[ways[0].To] as Node;

                    prevTimes.Add(prevNode.Forward + ways[0].Time);
                }

                (allStates[curState] as Node).Forward = prevTimes.Max();

                // add related nodes to queue
                var outWorks = stateOutputs[curState] as List<string>;

                foreach (var wId in outWorks)
                {
                    var ways = (worksForwardBypass[wId] as List<Way>)
                        .FindAll((way) => { return way.From == curState; });
                    
                    foreach (var way in ways)
                    {
                        states.Enqueue(way.To);
                    }
                }
            }
        }

        private void backwardMove()
        {
            (allStates[endingStateName] as Node).Backward = (allStates[endingStateName] as Node).Forward;

            Queue<string> states = new Queue<string>();

            // get first nodes
            {
                var outWorks = stateInputs[endingStateName] as List<string>;

                foreach (var wId in outWorks)
                {
                    var ways = (worksBackwardBypass[wId] as List<Way>)
                        .FindAll(filterFictitiousWorks);
                    states.Enqueue(ways[0].To);
                }
            }

            // processing all nodes
            while (states.Count > 0)
            {
                string curState = states.Dequeue();

                var prevWorks = stateOutputs[curState] as List<string>;

                List<Int32> prevTimes = new List<Int32>();

                // calculating min time
                foreach (var work in prevWorks)
                {
                    var ways = (worksForwardBypass[work] as List<Way>)
                        .FindAll((way) => { return way.From == curState; });

                    var prevNode = allStates[ways[0].To] as Node;

                    prevTimes.Add(prevNode.Backward - ways[0].Time);
                }

                (allStates[curState] as Node).Backward = prevTimes.Min();

                // add related nodes to queue
                var outWorks = stateInputs[curState] as List<string>;

                foreach (var wId in outWorks)
                {
                    var ways = (worksBackwardBypass[wId] as List<Way>)
                        .FindAll((way) => { return way.From == curState; });

                    foreach (var way in ways)
                    {
                        states.Enqueue(way.To);
                    }
                }
            }
        }

        private void calculateCriticalWay()
        {
            foreach (Node state in allStates.Values)
            {
                state.Difference = state.Backward - state.Forward;
            }

            string curState = startingStateName;

            criticalWay.Add(curState);

            while (curState != endingStateName)
            {
                var works = stateOutputs[curState] as List<string>;

                foreach(var wId in works)
                {
                    var ways = (worksForwardBypass[wId] as List<Way>)
                        .FindAll((way) => { 
                            return (allStates[way.To] as Node).Difference == 0 
                            && way.From == curState; 
                        });

                    if (ways.Count > 0)
                    {
                        curState = ways[0].To;
                        break;
                    }
                }

                criticalWay.Add(curState);
            }
        }
    }
}
