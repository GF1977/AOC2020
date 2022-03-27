using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Puzzle20
{
    public struct EdgesHash
    {
        public string N; public string E; public string S; public string W;
        public EdgesHash (string N, string E, string S, string W)
            {
                this.N = N; this.E = E; this.S = S; this.W = W;
            }
    }
    class Tile
    {
        public int id;
        public Dictionary<int, string> connections;
        private char[,] Photo;
        public List<EdgesHash> edgesHashes = new List<EdgesHash>();
        public Tile(string RawData, int nPhotoSize = 10)
        {
            Photo = new char[nPhotoSize, nPhotoSize];
            connections = new Dictionary<int, string>();
            string[] parts = RawData.Split("\r\n");
            string[] sId = parts[0].Split(" ");
            this.id = int.Parse(sId[1].Replace(":",""));

            for(int nRow = 0; nRow < nPhotoSize; nRow++)
                for (int nCol = 0; nCol < nPhotoSize; nCol++)
                    Photo[nRow, nCol] = parts[nRow + 1][nCol];

            EdgesHash Hash;
            Hash.N = GetEdge("N");
            Hash.E = GetEdge("E");
            Hash.S = GetEdge("S");
            Hash.W = GetEdge("W");

            edgesHashes.Add(Hash);

            GetRotatedEdges();
        }
        public void GetRotatedEdges()
        {
            List<EdgesHash> edgesHashesTMP = new List<EdgesHash>();
            foreach (EdgesHash EH in edgesHashes)
            {
                edgesHashesTMP.AddRange(Get4RotatedEdges(EH, flipType:"none"));
                edgesHashesTMP.AddRange(Get4RotatedEdges(EH, flipType:"X"));
                edgesHashesTMP.AddRange(Get4RotatedEdges(EH, flipType:"Y"));
            }
            edgesHashes.AddRange(edgesHashesTMP);
        }

        private List<EdgesHash> Get4RotatedEdges(EdgesHash EH, string flipType = "none")
        {
            List<EdgesHash> edgesHashesTMP = new List<EdgesHash>();
            EdgesHash EHtmp = EH;
            int n = 4;

            if (flipType == "none") 
                n = 3; // "none" means no flip, rotation only. We need only 3 rotations, as the rotation 4th is the original EH (rotation 0)
            else
                EHtmp = RotateRightOnce(Flip(EH, flipType));

            for (int i= 0; i < n; i++)
            {
                edgesHashesTMP.Add(EHtmp);
                EHtmp = RotateRightOnce(EHtmp);
            }

            return edgesHashesTMP;
        }

        public string InverseHash(string Hash)
        { 
            char[] HashChars = Hash.ToCharArray();
            Array.Reverse(HashChars);
            return  new string(HashChars);
        }

        public EdgesHash Flip(EdgesHash EH, string flip)
        {
            EdgesHash EHtmp = new EdgesHash(InverseHash(EH.N), InverseHash(EH.W), InverseHash(EH.S), InverseHash(EH.E));
            if (flip == "X")
            {
                EHtmp = new EdgesHash(InverseHash(EH.S), InverseHash(EH.E), InverseHash(EH.N), InverseHash(EH.W));
            }
           
            return EHtmp;
        }
        public EdgesHash RotateRightOnce(EdgesHash EH)
        {
            return new EdgesHash(EH.W, EH.N, EH.E, EH.S);
        }
        public string GetEdge(string Side)
        {
            (int  X, int  Y) Start      = (0,0);
            (int dX, int dY) D          = (0,0);

            switch (Side)
            {
                case "N": Start = (0, 0); D = ( 1,  0); break;
                case "E": Start = (9, 0); D = ( 0,  1); break;
                case "S": Start = (9, 9); D = (-1,  0); break;
                case "W": Start = (0, 9); D = ( 0, -1); break;
            }

            string nRes = String.Empty;
            for(int i = 0; i < 10;i++)
                {
                    nRes += Photo[Start.Y, Start.X];
                    Start.X += D.dX;
                    Start.Y += D.dY;
                }
            return nRes;
        }
    }
    class Program
    {
        // Answers:         Part one = 66020135789767   
        static List<Tile> Tiles = new List<Tile>();
        static Dictionary<string, int> Edges = new Dictionary<string, int>();
        static void Main(string[] args)
        {
            // parsing
            ParsingInputData(@"..\..\..\data_p.txt");
            CheckingEdges();

            long res = 1;
            foreach (Tile tile in Tiles)
                if (tile.connections.Count == 2)
                    res *= tile.id;

            Console.WriteLine("Part one: {0, 10:0}", res);

            if (res != 66020135789767)
                Console.WriteLine("Error!");
        }
        private static void CheckingEdges()
        {
            foreach (Tile tile in Tiles)
            {
                foreach (EdgesHash EH in tile.edgesHashes)
                {
                    AddMatchToDict(EH.N, tile);
                    AddMatchToDict(EH.E, tile);
                    AddMatchToDict(EH.S, tile);
                    AddMatchToDict(EH.W, tile);
                }
            }
        }
        private static void AddMatchToDict(string n, Tile tile)
        {
            bool b = Edges.TryAdd(n, tile.id);
            if (!b && tile.id != Edges[n])
            {
                tile.connections.TryAdd(Edges[n], n);
                Tile T = Tiles.Find(t => t.id == Edges[n]);
                T.connections.TryAdd(tile.id, n);
            }
        }
        private static void ParsingInputData(string filePath)
        {
            StreamReader file = new StreamReader(filePath);
            string[] Data = file.ReadToEnd().Split("\r\n\r\n");

            foreach(string s in Data)
                Tiles.Add(new Tile(s));
        }
    }
}