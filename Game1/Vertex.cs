using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game1
{
    class Vertex
    {
        public int row;
        public int colum;
        public bool is_up_or_down;
        public Dictionary<(int, int, int), Edge> edges;
        public Dictionary<(int, int), Cell> cells;

        public Color player;
        public string bulding; //"Settlement" , "City", "Metropolin" 

        public Vertex(int row, int colum, bool is_up_or_down)
        {
            this.row = row;
            this.colum = colum;
            cells = new Dictionary<(int, int), Cell>();
            edges = new Dictionary<(int, int, int), Edge>();
        }

        public Vertex(int row, int colum, Color player, string bulding)
        {
            this.row = row;
            this.colum = colum;
            cells = new Dictionary<(int, int), Cell>();
            edges = new Dictionary<(int, int, int), Edge>();

            this.player = player;
            this.bulding = bulding;
        }

    }
}
