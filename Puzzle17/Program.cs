using System;
using System.Collections.Generic;
using System.IO;


namespace Puzzle17
{
    class Program
    {
        const int TERRA_SIZE = 26;
        const int TERRA_SHIFT = 12;
        
        static bool[,,] terra = new bool[TERRA_SIZE, TERRA_SIZE, TERRA_SIZE];
        static bool[,,] future_terra = new bool[TERRA_SIZE, TERRA_SIZE, TERRA_SIZE];

        static bool[,,,] terra4 = new bool[TERRA_SIZE, TERRA_SIZE, TERRA_SIZE, TERRA_SIZE];
        static bool[,,,] future_terra4 = new bool[TERRA_SIZE, TERRA_SIZE, TERRA_SIZE, TERRA_SIZE];

        static void Main(string[] args)
        {
            StreamReader file = new StreamReader(@"C:\Users\iopya\Source\Repos\AOC2020\Puzzle17\data_1.txt");
            //StreamReader file = new StreamReader(@"C:\Users\iopya\Source\Repos\AOC2020\Puzzle17\data_test.txt");
            string line = file.ReadLine();

            int x = TERRA_SHIFT;
            int y = TERRA_SHIFT;
            int z = TERRA_SHIFT;
            int w = TERRA_SHIFT;
            while (line != null)
            {
                x = TERRA_SHIFT;
                foreach (char c in line)
                {
                    bool state = false;
                    if (c == '#')
                        state = true;

                    terra[x, y, z] = state;
                    terra4[x, y, z, w] = state;
                    x++;
                }
                y++;
                line = file.ReadLine();
            }
            Console.WriteLine("Terra 3 dimensional");
            // Terra 3 dimensional
            int activeCells;
            for (int i = 0; i <= 6; i++)
            {
                activeCells = GetActiveCells();
                Console.WriteLine("Active cells = {0}", activeCells);
                SetFutureState();
                CopyTerra();
            }

            Console.WriteLine("Terra 4 dimensional");
            // Terra 4 dimensional
            for (int i = 0; i <= 6; i++)
            {
                activeCells = GetActiveCells4();
                Console.WriteLine("Active cells = {0}", activeCells);
                SetFutureState4();
                CopyTerra4();
            }


        }

        private static void CopyTerra()
        {
            for (int z = 0; z < TERRA_SIZE; z++)
                for (int y = 0; y < TERRA_SIZE; y++)
                    for (int x = 0; x < TERRA_SIZE; x++)
                    {
                        terra[x, y, z] = future_terra[x, y, z];
                        future_terra[x, y, z] = false;
                    }
        }

        static int GetActiveCells()
        {
            int res = 0;
            for (int z = 0; z < TERRA_SIZE; z++)
                for (int y = 0; y < TERRA_SIZE; y++)
                    for (int x = 0; x < TERRA_SIZE; x++)
                        if (terra[x, y, z]== true)
                            res++;

              return res;
        }

        static int GetActiveNeighbours(int x, int y, int z)
        {
            int res = 0;
            for (int d_z = -1; d_z <= 1 ; d_z++)
                for (int d_y = -1; d_y <= 1; d_y++)
                    for (int d_x = -1; d_x <= 1; d_x++)
                        if (terra[x+d_x, y+d_y,z + d_z] == true && !(d_z == 0 && d_y ==0 && d_x ==0))
                            res++;

            return res;
        }

        static void SetFutureState()
        {
            for (int z = 1; z < TERRA_SIZE-1; z++)
                for (int y = 1; y < TERRA_SIZE-1; y++)
                    for (int x = 1; x < TERRA_SIZE-1; x++)
                    {
                        int activeNeighbours = GetActiveNeighbours(x, y, z);
                        // If a cube is active and exactly 2 or 3 of its neighbors are also active, the cube remains active.Otherwise, the cube becomes inactive.
                        if (terra[x, y, z] == true)
                        {
                            if (activeNeighbours == 2 || activeNeighbours == 3)
                                future_terra[x, y, z] = true;
                            else
                                future_terra[x, y, z] = false;
                        }
                        // If a cube is inactive but exactly 3 of its neighbors are active, the cube becomes active. Otherwise, the cube remains inactive.
                        if (terra[x, y, z] == false)
                        {
                            if (activeNeighbours == 3)
                                future_terra[x, y, z] = true;
                            else
                                future_terra[x, y, z] = false;
                        }


                    }
                       
        }


        // Terra 4 dimensional
        private static void CopyTerra4()
        {
            for (int w = 0; w < TERRA_SIZE; w++)
                for (int z = 0; z < TERRA_SIZE; z++)
                    for (int y = 0; y < TERRA_SIZE; y++)
                        for (int x = 0; x < TERRA_SIZE; x++)
                        {
                            terra4[x, y, z,w] = future_terra4[x, y, z,w];
                            future_terra4[x, y, z,w] = false;
                        }
        }
        static int GetActiveCells4()
        {
            int res = 0;
            for (int w = 0; w < TERRA_SIZE; w++)
                for (int z = 0; z < TERRA_SIZE; z++)
                    for (int y = 0; y < TERRA_SIZE; y++)
                        for (int x = 0; x < TERRA_SIZE; x++)
                            if (terra4[x, y, z, w] == true)
                                res++;

            return res;
        }

        static int GetActiveNeighbours4(int x, int y, int z, int w)
        {
            int res = 0;
            for (int d_w = -1; d_w <= 1; d_w++)
                for (int d_z = -1; d_z <= 1; d_z++)
                    for (int d_y = -1; d_y <= 1; d_y++)
                        for (int d_x = -1; d_x <= 1; d_x++)
                            if (terra4[x + d_x, y + d_y, z + d_z, w + d_w] == true && !(d_z == 0 && d_y == 0 && d_x == 0 && d_w == 0))
                                res++;

            return res;
        }

        static void SetFutureState4()
        {
            for (int w = 1; w < TERRA_SIZE - 1; w++)
                for (int z = 1; z < TERRA_SIZE - 1; z++)
                    for (int y = 1; y < TERRA_SIZE - 1; y++)
                        for (int x = 1; x < TERRA_SIZE - 1; x++)
                        {
                            int activeNeighbours = GetActiveNeighbours4(x, y, z, w);
                            // If a cube is active and exactly 2 or 3 of its neighbors are also active, the cube remains active.Otherwise, the cube becomes inactive.
                            if (terra4[x, y, z, w] == true)
                            {
                                if (activeNeighbours == 2 || activeNeighbours == 3)
                                    future_terra4[x, y, z, w] = true;
                                else
                                    future_terra4[x, y, z, w] = false;
                            }
                            // If a cube is inactive but exactly 3 of its neighbors are active, the cube becomes active. Otherwise, the cube remains inactive.
                            if (terra4[x, y, z, w] == false)
                            {
                                if (activeNeighbours == 3)
                                    future_terra4[x, y, z, w] = true;
                                else
                                    future_terra4[x, y, z, w] = false;
                            }


                        }

        }
    }
}