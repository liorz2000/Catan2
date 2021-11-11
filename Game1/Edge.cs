using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game1
{
    class Edge
    {
        public int row;
        public int col;
        public int direction; // 1, 5 ,9

        public bool is_determine_is_port = false;
        public bool is_port;

        public string port; // "3:1", "tree", "sheep", "grain", "ore", "break" ,""
        public Dictionary<(int, int, bool), Vertex> vertices;

        public Color players_color;
        public Button self_button;

        public Edge(int row, int col, int direction, string port = "")
        {
            this.row = row;
            this.col = col;
            this.direction = direction;
            this.port = port;
            vertices = new Dictionary<(int, int,bool), Vertex>();
        }

        public Edge(int row, int col, int direction, string port , Color players_color)
        {
            this.row = row;
            this.col = col;
            this.direction = direction;
            this.port = port;
            vertices = new Dictionary<(int, int, bool), Vertex>();

            this.players_color = players_color;
        }

        public void set_button_vertexes(Point zero_hex_center, int step_size, int ratio)
        {
            
            Point O1 = new Point(zero_hex_center.X + row * step_size / 2 + col * step_size,
                   zero_hex_center.Y - row * (int)Math.Round(step_size / (2/Math.Sqrt(3))));
            Point v1 = new Point(0, 0);
            Point v2 = new Point(0, 0);
            Point O2 = new Point(0, 0);
            if (direction == 1)
            {
                O2 = new Point(step_size / 2, (int)Math.Round(-step_size / (2 / Math.Sqrt(3)))) + O1;
                v1 = new Point(0, (int)Math.Round(-step_size / Math.Sqrt(3))) + O1;
                v2 = O1 + O2 - v1;
            }
            else if (direction == 5)
            {
                O2 = new Point(step_size / 2, (int)Math.Round(step_size / (2 / Math.Sqrt(3)))) + O1;
                v1 = new Point(step_size / 2, (int)Math.Round(step_size / (2 * Math.Sqrt(3)))) + O1;
                v2 = O1 + O2 - v1;
            }
            else if (direction == 9)
            {
                O2 = new Point(-step_size, 0) + O1;
                v1 = new Point(-step_size / 2, (int)Math.Round(step_size / (2 * Math.Sqrt(3)))) + O1;
                v2 = O1 + O2 - v1;
            }
            Point p1 = new Point((v1.X * ratio + O1.X) / (ratio + 1), (v1.Y * ratio + O1.Y) / (ratio + 1));
            Point p2 = new Point((v1.X * ratio + O2.X) / (ratio + 1), (v1.Y * ratio + O2.Y) / (ratio + 1));
            Point p3 = new Point((v2.X * ratio + O2.X) / (ratio + 1), (v2.Y * ratio + O2.Y) / (ratio + 1));
            Point p4 = new Point((v2.X * ratio + O1.X) / (ratio + 1), (v2.Y * ratio + O1.Y) / (ratio + 1));

            Point[] points_e001 = new Point[] { p1, p2, p3, p4 };
            self_button.set_vertexes(points_e001);
            
        }

    }
}
