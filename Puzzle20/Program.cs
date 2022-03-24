using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Puzzle20
{
    public struct EdgesHash {public string N; public string E; public string S; public string W; }
    class Tile
    {
        public int id;
        public Dictionary<int, int> connections;
        private char[,] Photo = new char[10, 10];
        public List<EdgesHash> edgesHashes = new List<EdgesHash>();
        public Tile(string RawData)
        {
            connections = new Dictionary<int, int>();
            string[] parts = RawData.Split("\r\n");
            string[] sId = parts[0].Split(" ");
            this.id = int.Parse(sId[1].Replace(":",""));

            for(int nRow = 0; nRow < parts.Length - 1; nRow++)
                for (int nCol = 0; nCol < 10; nCol++)
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
                EdgesHash[] HashRotated = new EdgesHash[3];

                HashRotated[0] = RotateRightOnce(EH);
                HashRotated[1] = RotateRightOnce(HashRotated[0]);
                HashRotated[2] = RotateRightOnce(HashRotated[1]);

                foreach (EdgesHash EHrotated in HashRotated)
                    edgesHashesTMP.Add(EHrotated);


                HashRotated = new EdgesHash[4];

                HashRotated[0] = RotateRightOnce(FlipX(EH));
                HashRotated[1] = RotateRightOnce(HashRotated[0]);
                HashRotated[2] = RotateRightOnce(HashRotated[1]);
                HashRotated[3] = RotateRightOnce(HashRotated[2]);

                foreach (EdgesHash EHrotated in HashRotated)
                    edgesHashesTMP.Add(EHrotated);


                HashRotated = new EdgesHash[4];

                HashRotated[0] = RotateRightOnce(FlipY(EH));
                HashRotated[1] = RotateRightOnce(HashRotated[0]);
                HashRotated[2] = RotateRightOnce(HashRotated[1]);
                HashRotated[3] = RotateRightOnce(HashRotated[2]);

                foreach (EdgesHash EHrotated in HashRotated)
                    edgesHashesTMP.Add(EHrotated);

            }

            edgesHashes.AddRange(edgesHashesTMP);
        }

        public string InverseHash(string Hash)
        { 
            char[] HashChars = Hash.ToCharArray();
            Array.Reverse(HashChars);
            return  new string(HashChars);
        }

        public EdgesHash FlipX(EdgesHash EH)
        {
            EdgesHash EHtmp;
            EHtmp.N = InverseHash(EH.S); EHtmp.E = InverseHash(EH.E); EHtmp.S = InverseHash(EH.N); EHtmp.W = InverseHash(EH.W);
            return EHtmp;
        }

        public EdgesHash FlipY(EdgesHash EH)
        {
            EdgesHash EHtmp;
            EHtmp.N = InverseHash(EH.N); EHtmp.E = InverseHash(EH.W); EHtmp.S = InverseHash(EH.S); EHtmp.W = InverseHash(EH.E);
            return EHtmp;
        }

        public EdgesHash RotateRightOnce(EdgesHash EH)
        {
            EdgesHash EHtmp;
            EHtmp.N = EH.W; EHtmp.E = EH.N; EHtmp.S = EH.E; EHtmp.W = EH.S;
            return EHtmp;
        }

        public string GetEdge(string Side)
        {
            (int X, int Y) Start    = (0,0);
            (int dX, int dY) D      = (0,0);

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
        
        static List<Tile> Tiles = new List<Tile>();
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


        }

        private static void CheckingEdges()
        {
            Dictionary<string, int> Edges = new Dictionary<string, int>();

            foreach (Tile tile in Tiles)
            {
                int n = 0;
                foreach (EdgesHash EH in tile.edgesHashes)
                {
                    bool b = Edges.TryAdd(EH.N, tile.id);
                    if (!b && tile.id != Edges[EH.N])
                    {
                        tile.connections.TryAdd(Edges[EH.N], 0);
                        Tile T = Tiles.Find(t => t.id == Edges[EH.N]);
                        T.connections.TryAdd(tile.id, 0);
                    }
                        //Console.WriteLine("Tile: {0}  &   Tile: {1}", tile.id, Edges[EH.N]);

                    b = Edges.TryAdd(EH.E, tile.id);
                    if (!b && tile.id != Edges[EH.E])
                    {
                        tile.connections.TryAdd(Edges[EH.E], 0);
                        Tile T = Tiles.Find(t => t.id == Edges[EH.E]);
                        T.connections.TryAdd(tile.id, 0);
                    }
                    //tile.connections.TryAdd(Edges[EH.E].Substring(0, 4), "");
                    //Console.WriteLine("Tile: {0}  &   Tile: {1}", tile.id, Edges[EH.E]);

                    b = Edges.TryAdd(EH.S, tile.id);
                    if (!b && tile.id != Edges[EH.S])
                    {
                        tile.connections.TryAdd(Edges[EH.S], 0);
                        Tile T = Tiles.Find(t => t.id == Edges[EH.S]);
                        T.connections.TryAdd(tile.id, 0);
                    }
                    //    tile.connections.TryAdd(Edges[EH.S].Substring(0, 4), "");
                    //Console.WriteLine("Tile: {0}  &   Tile: {1}", tile.id, Edges[EH.S]);


                    b = Edges.TryAdd(EH.W, tile.id);
                    if (!b && tile.id != Edges[EH.W])
                    {
                        tile.connections.TryAdd(Edges[EH.W], 0);
                        Tile T = Tiles.Find(t => t.id == Edges[EH.W]);
                        T.connections.TryAdd(tile.id, 0);
                    }
                    //    tile.connections.TryAdd(Edges[EH.W].Substring(0, 4), "");
                    //Console.WriteLine("Tile: {0}  &   Tile: {1}", tile.id, Edges[EH.W]);

                    n++;
                }
            }
        }

        private static void ParsingInputData(string filePath)
        {
            StreamReader file = new StreamReader(filePath);
            string[] Data = file.ReadToEnd().Split("\r\n\r\n");

            foreach(string s in Data)
            {
                Tile T = new Tile(s);
                Tiles.Add(T);
            }
        }
    }
}
