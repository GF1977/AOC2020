using System;
using System.Collections.Generic;
using System.IO;


namespace Puzzle17
{
    class Program
    {
        public const int nArraySize = 500;
        public const int nTerra0 = nArraySize / 2;

        // Terra size (5x5)
        public static int nSize = 21;
        public static int nCenter = (nSize - 1) / 2;

        public static Level[] AllTerras = new Level[nArraySize];
        public static Level[] AllTerrasNew = new Level[nArraySize];

        public class Level
        {
            public char[,] nSlice;
            public Level()
            {
                nSlice = new char[nSize, nSize];
                this.EmptyTerra();
            }

            public void CopyTerra(Level TerraNew)
            {
                for (int r = 0; r < nSize; r++)
                    for (int c = 0; c < nSize; c++)
                        this.nSlice[r, c] = TerraNew.nSlice[r, c];
            }


            public void EmptyTerra()
            {
                for (int r = 0; r < nSize; r++)
                    for (int c = 0; c < nSize; c++)
                        this.nSlice[r, c] = '.';

            }
        }


        static void Main(string[] args)
        {
            bool bStop = false;
            Level TerraVanilla = new Level();

            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();
            int nOffset = (nSize - line.Length) / 2;


            int nRowNumber = nOffset;
            while (line != null)
            {
                int nColNumber = nOffset;
                foreach (char c in line)
                {
                    TerraVanilla.nSlice[nRowNumber, nColNumber] = c;
                    nColNumber++;
                }
                nRowNumber++;
                line = file.ReadLine();
            }

            for (int i = 0; i < nArraySize; i++)
            {
                AllTerras[i] = new Level();
                AllTerrasNew[i] = new Level();
            }


            AllTerras[nTerra0].CopyTerra(TerraVanilla);

            //the main cycle - works till the goal is achieved

            int nDeltaMax = 1;
            int z = 0;

            AllTerras[nTerra0].nSlice[nCenter, nCenter] = '.';


            while (!bStop)
            {
                int nTerraID = nTerra0;
                ShowTerra(nTerraID);
                Console.ReadKey();


                // Part two
                if (z - 1 == 200)
                {
                    Console.WriteLine("Part TWO: {0}", GetBugsCount());
                    bStop = true;
                }
                // Part two end


                for (int nDelta = -z; nDelta < nDeltaMax - z; nDelta++)  // trick here is to get consequence -1,0,1    -2,-1,0,1,2   -3,-2,-1,0,1,2,3  etc
                    for (int r = 0; r < nSize; r++)
                        for (int c = 0; c < nSize; c++)
                        {
                            int nNeighbors = CheckNeighbors(r, c, nTerraID + nDelta);

                            AllTerrasNew[nTerraID + nDelta].nSlice[r, c] = AllTerras[nTerraID + nDelta].nSlice[r, c];

                            if (AllTerras[nTerraID + nDelta].nSlice[r, c] == '#')
                            {
                                if ((nNeighbors == 2 || nNeighbors == 3))
                                    AllTerrasNew[nTerraID + nDelta].nSlice[r, c] = '#';
                                else
                                    AllTerrasNew[nTerraID + nDelta].nSlice[r, c] = '.';
                            }
                            else
                            {

                                if (nNeighbors == 3 )
                                    AllTerrasNew[nTerraID + nDelta].nSlice[r, c] = '#';
                                else
                                    AllTerrasNew[nTerraID + nDelta].nSlice[r, c] = '.';
                            }

                        }

                if (true)
                {
                    z++;
                    nDeltaMax += 2;
                }

                for (int m = -z; m < z; m++)
                    AllTerras[nTerraID + m].CopyTerra(AllTerrasNew[nTerraID + m]);


            }
            //Console.WriteLine("Press any key");
            //Console.ReadKey();
        }

        private static int GetBugsCount()
        {
            int nResult = 0;
            for (int m = 0; m < nArraySize; m++)
                for (int r = 0; r < nSize; r++)
                    for (int c = 0; c < nSize; c++)
                        if (AllTerras[m].nSlice[r, c] == '#')
                            nResult++;


            return nResult;
        }

        private static int CheckNeighbors(int r, int c, int nTerraID)
        {
            int bResult = 0;
            int nTerraUpOrDown = 0; // neutral

            for(int nSlice = -1; nSlice <= 1; nSlice ++)
                for (int x = r - 1; x <= r + 1; x++) 
                    for (int y = c - 1; y <= c + 1; y++) 
                        if(IsValidCoordinate(x,y) && (nTerraID + nSlice) < nArraySize && (nTerraID + nSlice) >= 0)
                            if (AllTerras[nTerraID + nSlice].nSlice[x, y] == '#')
                                bResult++;

            return bResult;
        }


        private static bool IsValidCoordinate(int x, int y)
        {
            if (x >= 0 && x < nSize && y >= 0 && y < nSize)
                return true;
            else
                return false;
        }




        private static void ShowTerra(int nTerraID)
        {
            Console.WriteLine("Terra: {0}", nTerraID);
            for (int r = 0; r < nSize; r++)
            {
                for (int c = 0; c < nSize; c++)
                    Console.Write(AllTerras[nTerraID].nSlice[r, c]);
                Console.WriteLine();
            }
            Console.WriteLine();
        }



    }
}