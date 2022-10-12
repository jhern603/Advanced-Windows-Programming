using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace assignment_3
{
    public class MasterVertex
    {
        public string Name { get; set; }
        public int D { get; set; }
        public MasterVertex Parent { get; set; }
    }
    class Vertex
    {
        public int Key { get; set; } = int.MaxValue;
        public int Parent { get; set; } = -1;
        public int V { get; set; }
        public bool IsProcessed { get; set; }
    }
    class GraphAlgorithms
    {
        public GraphAlgorithms(ToolStripProgressBar p, ToolStripStatusLabel l, StatusStrip statusStrip)
        {
            graphs = new Dictionary<string, int[][]>();
            mstSolutions = new Dictionary<string, Vertex[]>();
            ssspSolutions = new Dictionary<string, List<MasterVertex>>();
            this.p = p;
            this.l = l;
            this.statusStrip = statusStrip;
        }
        internal void GetMST(string fileName)
        {
            PriorityQueue<Vertex> queue = new PriorityQueue<Vertex>(true);
            int[][] graph = graphs[fileName];
            int vertexCount = graph.GetLength(0);
            //listing all vertices
            Vertex[] vertices = new Vertex[vertexCount];
            for (int i = 0; i < vertexCount; i++)
                vertices[i] = new Vertex() { Key = int.MaxValue, Parent = -1, V = i };
            //setting first one's key to zero
            vertices[0].Key = 0;

            //insertingvertices
            for (int i = 0; i < vertexCount; i++)
                queue.Enqueue(vertices[i].Key, vertices[i]);
            int n = Math.Min(Math.Max(25, fileName.Length / 2), fileName.Length);
            l.Text = "running Prim's to find MST for graph in ..." + fileName.Substring(fileName.Length - n, n);
            //statusStrip.Update();
            while (queue.Count > 0)
            {
                Action updateP = () => p.Value = (int)Math.Round(100 * (1 - queue.Count / (double)vertexCount));
                statusStrip.Invoke(updateP);
                Vertex minVertex = queue.Dequeue();
                int u = minVertex.V;
                vertices[u].IsProcessed = true;
                //alll edges from vertex u
                int[] edges = graph[minVertex.V];
                for (int v = 0; v < edges.Length; v++)
                {
                    if (graph[u][v] > 0 && !vertices[v].IsProcessed && graph[u][v] < vertices[v].Key)
                    {
                        vertices[v].Parent = u;
                        vertices[v].Key = graph[u][v];
                        //updating priority in queue since key is priority
                        queue.UpdatePriority(vertices[v], vertices[v].Key);
                    }
                }
            }
            //p.Value = 0;
            Action resetP = () => p.Value = 0;
            statusStrip.Invoke(resetP);
            l.Text = "Ready!";
            //statusStrip.Update();
            mstSolutions[fileName] = vertices;
        }

        internal void WriteMSTSolutionTo(string saveToFileName, string selectedGraphFileName)
        {
            StreamWriter fileStreamWriter = new StreamWriter(saveToFileName);
            Vertex[] vertices = mstSolutions[selectedGraphFileName];
            long totalWeight = 0;
            int n = Math.Min(Math.Max(25, selectedGraphFileName.Length / 2), selectedGraphFileName.Length);
            l.Text = "writing the MST of graph in ..." + selectedGraphFileName.Substring(selectedGraphFileName.Length - n, n);
            n = Math.Min(Math.Max(25, saveToFileName.Length / 2), saveToFileName.Length);
            l.Text += "to file ..." + saveToFileName.Substring(saveToFileName.Length - n, n);
            //statusStrip.Update();
            double i = 0;
            foreach (Vertex u in vertices)
            {
                Action updateP = () => p.Value = (int)Math.Round(100 * (i / vertices.Length));
                statusStrip.Invoke(updateP);
                if (u.Parent >= 0)
                {
                    fileStreamWriter.WriteLine("Vertex {0} to Vertex {1} weight is: {2}", u.V, u.Parent, u.Key);
                    totalWeight += u.Key;
                }
                i++;
            }
            Console.WriteLine("Total Weight: {0}", totalWeight);
            fileStreamWriter.WriteLine("Total Weight: {0}", totalWeight);
            fileStreamWriter.Close();
            //p.Value = 0;
            Action resetP = () => p.Value = 0;
            statusStrip.Invoke(resetP);
            l.Text = "Ready!";
            //statusStrip.Update();
        }

        internal void ReadGraphFromCSVFile(string graphFileName)
        {
            int n = Math.Min(Math.Max(25, graphFileName.Length / 2), graphFileName.Length);
            l.Text = "Loading the graph from ..." + graphFileName.Substring(graphFileName.Length - n, n);
            //statusStrip.Update();
            //l.Visible = true;
            l.Alignment = ToolStripItemAlignment.Left;
            string[] lines = File.ReadAllLines(graphFileName);
            int[][] matrix = new int[lines.Length][];
            int i = 0;
            foreach (string line in lines)
            {
                Action updateP = () => p.Value = (int)Math.Round(100 * i / (double)lines.Length);
                statusStrip.Invoke(updateP);
                string[] items = line.Split(',');
                matrix[i] = new int[items.Length];
                int j = 0;
                foreach (string item in items)
                    matrix[i][j++] = Int32.Parse(item);
                i++;
            }
            //p.Value = 0;
            Action resetP = () => p.Value = 0;
            statusStrip.Invoke(resetP);
            l.Text = "Ready!";
            //statusStrip.Update();
            //l.Visible = false;
            graphs[graphFileName] = matrix;
        }

        internal void ReadGraphFromTXTFile(string graphFileName)
        {
            int n = Math.Min(Math.Max(25, graphFileName.Length / 2), graphFileName.Length);
            l.Text = "Loading the graph from ..." + graphFileName.Substring(graphFileName.Length - n, n);
            //statusStrip.Update();
            //l.Visible = true;
            l.Alignment = ToolStripItemAlignment.Left;
            string[] lines = File.ReadAllLines(graphFileName);
            int[][] matrix = new int[lines.Length][];
            int i = 0;
            foreach (string line in lines)
            {
                Action updateP = () => p.Value = (int)Math.Round(100 * i / (double)lines.Length);
                statusStrip.Invoke(updateP);
                string[] items = line.Split('\t');
                matrix[i] = new int[items.Length];
                int j = 0;
                foreach (string item in items)
                    matrix[i][j++] = Int32.Parse(item);
                i++;
            }
            //p.Value = 0;
            Action resetP = () => p.Value = 0;
            statusStrip.Invoke(resetP);
            l.Text = "Ready!";
            //statusStrip.Update();
            graphs[graphFileName] = matrix;
        }

        internal void Dijkstra(string fileName)
        {
            int source = 0;
            int n = Math.Min(Math.Max(25, fileName.Length / 2), fileName.Length);
            l.Text = "running Dijsktra's to find shortest paths from source " + source + " for graph in ..."
             + fileName.Substring(fileName.Length - n, n);
            //statusStrip.Update();
            int[][] graph = graphs[fileName];
            MasterVertex[] vertices = new MasterVertex[graph.GetLength(0)];
            //Source MasterVertex

            for (int i = 0; i < vertices.Length; i++)
                vertices[i] = new MasterVertex() { Name = i.ToString() };
            InitializeSingleSource(vertices, vertices[source]);
            List<MasterVertex> result = new List<MasterVertex>();
            //adding all MasterVertex to priority queue
            PriorityQueue<MasterVertex> queue = new PriorityQueue<MasterVertex>(true);
            for (int i = 0; i < vertices.Length; i++)
                queue.Enqueue(vertices[i].D, vertices[i]);

            //treversing to all vertices
            int j = 0;
            double maxJ = vertices.Length * (double)vertices.Length;
            while (queue.Count > 0)
            {
                var u = queue.Dequeue();
                result.Add(u);
                //again traversing to all vertices
                for (int v = 0; v < graph[Convert.ToInt32(u.Name)].Length; v++)
                {
                    Action updateP = () => p.Value = (int)Math.Round(100 * (j++) / maxJ);
                    statusStrip.Invoke(updateP);
                    //p.Value = (int)Math.Round(100 * (j++) / maxJ);
                    if (graph[Convert.ToInt32(u.Name)][v] > 0)
                    {
                        Relax(u, vertices[v], graph[Convert.ToInt32(u.Name)][v]);
                        //updating priority value since distance is changed
                        queue.UpdatePriority(vertices[v], vertices[v].D);
                    }
                }
            }
            Action resetP = () => p.Value = 0;
            statusStrip.Invoke(resetP);
            //p.Value = 0;
            l.Text = "Ready!";
            //statusStrip.Update();
            ssspSolutions[fileName] = result;
        }

        internal void WriteSSSPSolutionTo(string saveToFileName, string selectedGraphFileName)
        {
            StreamWriter fileStreamWriter = new StreamWriter(saveToFileName);
            List<MasterVertex> vertices = ssspSolutions[selectedGraphFileName];
            int n = Math.Min(Math.Max(25, selectedGraphFileName.Length / 2), selectedGraphFileName.Length);
            l.Text = "writing the shortest paths of graph in ..." + selectedGraphFileName.Substring(selectedGraphFileName.Length - n, n);
            n = Math.Min(Math.Max(25, saveToFileName.Length / 2), saveToFileName.Length);
            l.Text += "to file ..." + saveToFileName.Substring(saveToFileName.Length - n, n);
            //statusStrip.Update();
            double i = 0;
            foreach (MasterVertex u in vertices)
            {
                Action updateP = () => p.Value = (int)Math.Round(100 * (i / vertices.Count));
                statusStrip.Invoke(updateP);
                PrintPath(fileStreamWriter, vertices[0], u, vertices);
                fileStreamWriter.WriteLine();
                i++;
            }
            fileStreamWriter.Close();
            //p.Value = 0;
            Action resetP = () => p.Value = 0;
            statusStrip.Invoke(resetP);
            l.Text = "Ready!";
            //statusStrip.Update();
        }
        private static void PrintPath(StreamWriter fileStreamWriter, MasterVertex u, MasterVertex v, List<MasterVertex> vertices)
        {
            if (v != u)
            {
                PrintPath(fileStreamWriter, u, v.Parent, vertices);
                //Console.WriteLine("Vertax {0} distance: {1}", v.Name, v.D);
                fileStreamWriter.Write(", " + v.Name);
            }
            else
                fileStreamWriter.Write(v.Name);
            //Console.WriteLine("Vertax {0} distance: {1}", v.Name, v.D);
        }

        private static void InitializeSingleSource(MasterVertex[] vertices, MasterVertex s)
        {
            foreach (MasterVertex v in vertices)
            {
                v.D = int.MaxValue;
                v.Parent = null;
            }
            s.D = 0;
        }

        private static void Relax(MasterVertex u, MasterVertex v, int weight)
        {
            if (v.D > u.D + weight)
            {
                v.D = u.D + weight;
                v.Parent = u;
            }
        }
        private Dictionary<string, int[][]> graphs;
        private Dictionary<string, Vertex[]> mstSolutions;
        private Dictionary<string, List<MasterVertex>> ssspSolutions;
        private ToolStripProgressBar p { get; }
        private ToolStripStatusLabel l { get; }
        private StatusStrip statusStrip { get; }
    }
}
