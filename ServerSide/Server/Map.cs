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
        public MapObject[][] Objects;

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            Objects = new MapObject[width / 32][];
            Objects = GenerateMap(width, height, Objects);
        }

        /// <summary>
        /// Temporary map generation solution
        /// </summary>
        /// <param name="width">Map Width (px)</param>
        /// <param name="height">Map Height (px)</param>
        /// <param name="Objects">Map object jagged array</param>
        /// <returns>Returns generated Map Object array (aka the map)</returns>
        private MapObject[][] GenerateMap(int width, int height, MapObject[][] Objects)
        {
            int countX = 0;

            Random rand = new Random();

            for (int i = 0; i < width; i += 32)
            {
                int countY = 0;
                MapObject[] objectArr = new MapObject[height / 32];
                for (int j = 0; j < height; j += 32)
                {
                    // Determine if wall or not
                    int objId = rand.Next(0, 10) == 1 ? 1 : 0;
                    objectArr[countY] = new MapObject(i + 16, j + 16, objId, objId == 1);
                    countY++;
                }
                Objects[countX] = objectArr;
                countX++;
            }

            return Objects;
        }
    }
}
