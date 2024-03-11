using System;
using System.Collections.Generic;

namespace RoomsGenerator
{
    public class Vector2Int
    {
        public int x;
        public int y;

        public Vector2Int(int X, int Y)
        {
            x = X;
            y = Y;
        }
    }

    class Program
    {
        static int width = 120;
        static int heigh = 29;
        static Random rand = new Random();
        static int roomsMin = 20;
        static int roomsMax = 20;
        static int roomsCount = rand.Next(roomsMin, roomsMax + 1);
        static int[] roomSize = { 5, 13, 3, 7 };
        static bool[,] Map = new bool[width, heigh];

        static List<int> roomsX = new List<int>();
        static List<int> roomsY = new List<int>();

        static int errors = 0;
        static void Main(string[] args)
        {
            Generate();
            Console.ReadLine();
        }
        static void Generate()
        {
            GenerateBorder();
            int startRoomsCount = roomsCount;
            while (roomsCount > 0) { if (!GenerateRoom()) break; };
            roomsCount = startRoomsCount;
            for (int i = 0; i < roomsCount-1; i ++)
            {
                GeneratePath(roomsX[i], roomsY[i], roomsX[i+1], roomsY[i+1]);
            }
            MapDraw();
        }
        static void GenerateBorder()
        {
            for (int y = 0; y < heigh; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (y == 0 || y == heigh - 1 || x == 0 || x == width - 1)
                    {
                        Map[x, y] = true;
                    }
                }
            }
        }

        static void MapDraw()
        {
            for (int y = 0; y < heigh; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (Map[x, y] == true)
                    {
                        Console.Write('█');
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
            }
        }

        static bool CheckPlace(int xStart, int yStart, int xEnd, int yEnd)
        {
            for (int x = xStart - 1; x < xEnd + 2; x++)
            {
                for (int y = yStart - 1; y < yEnd + 2; y++)
                {
                    if (Map[x, y] == true)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static bool GenerateRoom()
        {
            int xStart = rand.Next(2, width - roomSize[1] - 2);
            int yStart = rand.Next(2, heigh - roomSize[3] - 2);

            int xSize = rand.Next(roomSize[0], roomSize[1]) + xStart;
            int ySize = rand.Next(roomSize[2], roomSize[3]) + yStart;

            if (CheckPlace(xStart, yStart, xSize, ySize) == false)
            {
                if (errors > 1000) { return false; }
                errors++;
                return true;
            }
            else
            {
                errors = 0;
                roomsX.Add(Convert.ToInt32(((xStart + xSize) / 2)));
                roomsY.Add(Convert.ToInt32(((yStart + ySize) / 2)));

                for (int x = xStart; x < xSize + 1; x++)
                {
                    for (int y = yStart; y < ySize + 1; y++)
                    {
                        Map[x, y] = true;
                    }
                }
                roomsCount--;
                return true;
            }
        }

        static bool CheckVector2Int(List<Vector2Int> list, Vector2Int vector)
        {
            for(int i = 0; i < list.Count; i ++)
            {
                if (list[i].x == vector.x && list[i].y == vector.y) return true;
            }
            return false;
        }

        static void GeneratePath(int xStart, int yStart, int xEnd, int yEnd)
        {
            List<Vector2Int> visited = new List<Vector2Int>();

            Stack<Vector2Int> frontier = new Stack<Vector2Int>();
            Stack<Vector2Int> path = new Stack<Vector2Int>();

            frontier.Push(new Vector2Int(xStart, yStart));

            Vector2Int current = new Vector2Int(xStart, yStart);
            int added = 0;
            while (frontier.Count > 0)
            {
                visited.Add(current);
                added = 0;
                bool priorityY;
                bool priorityAddX;
                bool priorityAddY;

                if (Math.Abs(current.x - xEnd) > Math.Abs(current.y - yEnd))
                {
                    priorityY = false;
                } else
                {
                    priorityY = true;
                }

                if (current.x - xEnd >= 0)
                {
                    priorityAddX = false;
                } else
                {
                    priorityAddX = true;
                }

                if (current.y - yEnd >= 0)
                {
                    priorityAddY = false;
                }
                else
                {
                    priorityAddY = true;
                }


                if (current.x == xEnd && current.y == yEnd)
                {
                    break;
                }
                if (!CheckVector2Int(visited, new Vector2Int(current.x + 1, current.y)) && current.x < width - 2)
                {
                    frontier.Push(new Vector2Int(current.x + 1, current.y)); added++;
                }
                if (!CheckVector2Int(visited, new Vector2Int(current.x - 1, current.y)) && current.x > 2 && !priorityAddX)
                {
                    if (added > 0)
                    {
                        added--;
                        frontier.Pop();
                    }
                    frontier.Push(new Vector2Int(current.x - 1, current.y)); added++;
                }
                if (!CheckVector2Int(visited, new Vector2Int(current.x, current.y + 1)) && current.y < heigh - 2 && priorityY)
                {
                    if (added > 0)
                    {
                        added--;
                        frontier.Pop();
                    }
                    frontier.Push(new Vector2Int(current.x, current.y + 1)); added++;
                }
                if (!CheckVector2Int(visited, new Vector2Int(current.x, current.y - 1)) && current.y > 2 && (priorityY) && (!priorityAddY))
                {
                    if (added > 0)
                    {
                        added--;
                        frontier.Pop();
                    }
                    frontier.Push(new Vector2Int(current.x, current.y - 1)); added++;
                }
                if (added == 0)
                {
                    current = frontier.Pop();
                }
                else
                {
                    current = frontier.Peek();
                    //Console.WriteLine(current.y);
                    //Console.WriteLine(current.x);
                    //Console.WriteLine();
                }
            }
            while(frontier.Count > 0)
            {
                current = frontier.Pop();
                Map[current.x, current.y] = true;
            }
        }
    }
}