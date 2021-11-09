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

        public bool is_determine_is_port = false;
        public bool is_port;

        public string port; // "3:1", "tree", "sheep", "grain", "ore", "break" ,""
        public Dictionary<(int, int, bool), Vertex> vertices;

        public Color players_color;
        public Button self_button;

        public Edge(int row, int colum, int direction, string port = "")
        {
            this.row = row;
            this.colum = colum;
            this.direction = direction;
            this.port = port;
            vertices = new Dictionary<(int, int,bool), Vertex>();
        }

        public Edge(int row, int colum, int direction, string port , Color players_color)
        {
            this.row = row;
            this.colum = colum;
            this.direction = direction;
            this.port = port;
            vertices = new Dictionary<(int, int, bool), Vertex>();

            this.players_color = players_color;
        }

    }
}
