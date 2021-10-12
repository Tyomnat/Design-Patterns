using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }

        //public int[][] Objects;
        public MapObject[][] Objects;

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            Objects = new MapObject[width / 32][];
            int countX = 0;
            int countY = 0;

            Random rand = new Random();

            for (int i = 0; i < width; i += 32)
            {
                countY = 0;
                MapObject[] objectArr = new MapObject[height / 32];
                for (int j = 0; j < height; j += 32)
                {

                    int objId = 0;
                    objId = rand.Next(0, 10) == 1 ? 1 : 0;
                    objectArr[countY] = new MapObject(i + 16, j + 16, objId, objId == 1);
                    countY++;
                }
                Objects[countX] = objectArr;
                countX++;
            }
            // Objects[8][8].Id = 1;
            //Objects = new int[height][];

            //for (int j = 0; j < height; j++)
            //{
            //    int[] arr = new int[width];
            //    for (int i = 0; i < width; i++)
            //    {
            //        arr[i] = 0;
            //    }
            //    Objects[j] = arr;
            //}

            //GenerateMap();
        }

    }
}
