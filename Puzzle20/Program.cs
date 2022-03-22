using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Puzzle20
{
    public struct EdgesHash {public int N; public int E; public int S; public int W; }
    class Tile
    {
        private int id;
        private char[,] Photo = new char[10, 10];
        public List<EdgesHash> edgesHashes = new List<EdgesHash>();
        public Tile(string RawData)
        {
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
        }

        public int GetEdge(string Side)
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

            int nRes = 0;
            for(int i = 0; i < 10;i++)
                {
                    nRes *= 2;
                    if (Photo[Start.Y, Start.X] == '#')
                        nRes += 1;

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
            ParsingInputData(@"..\..\..\data_t.txt");

            Console.WriteLine("Part one: {0, 10:0}", 0);


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
