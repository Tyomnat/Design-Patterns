using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatternsClientSide
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
        }
    }
}
