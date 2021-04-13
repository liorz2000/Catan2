using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game1
{
    class Edge
    {
        public int row;
        public int colum;
        public int direction; // 1, 5 ,9
        public string port; // "3:1", "tree", "sheep", "grain", "ore", "break" ,""
        public Dictionary<(int, int, bool), Vertex> vertices;

        public Color road;


        public Edge(int row, int colum, int direction, string port = "")
        {
            this.row = row;
            this.colum = colum;
            this.direction = direction;
            this.port = port;
            vertices = new Dictionary<(int, int,bool), Vertex>();
        }

        public Edge(int row, int colum, int direction, string port , Color road)
        {
            this.row = row;
            this.colum = colum;
            this.direction = direction;
            this.port = port;
            vertices = new Dictionary<(int, int, bool), Vertex>();

            this.road = road;
        }

    }
}
