using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Puzzle20
{
    public struct EdgesHash {public string N; public string E; public string S; public string W; }
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
            }

            edgesHashes.AddRange(edgesHashesTMP);
        }

        public string InverseHash(string Hash)
        { 
            char[] HashChars = Hash.ToCharArray();
            Array.Reverse(HashChars);
            return HashChars.ToString();
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
