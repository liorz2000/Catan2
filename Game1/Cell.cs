using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game1
{
    class Cell
    {
        public int row;
        public int colum;
        public string base_type; // "sea","land", ""-?
        public Dictionary<(int,int, bool), Vertex > vertices;

        public string resource; // "", "tree", "sheep", "grain", "ore", "break" ,"desert"
        public int cube_num;

        public Dictionary<string, Texture2D> opitional_texturs;
        public string current_texture;
        public Cell(int row, int colum, string base_type = "") 
        {
            this.row = row;
            this.colum = colum;
            this.base_type = base_type;
            vertices = new Dictionary<(int, int, bool), Vertex>();
        }

        public Cell(int row, int colum, string base_type, int cube_num)
        {
            this.row = row;
            this.colum = colum;
            this.base_type = base_type;
            vertices = new Dictionary<(int, int, bool), Vertex>();

            this.cube_num = cube_num;
        }

        public Cell(int row, int colum, string base_type, int cube_num, string resource)
        {
            this.row = row;
            this.colum = colum;
            this.base_type = base_type;
            vertices = new Dictionary<(int, int, bool), Vertex>();

            this.cube_num = cube_num;
            this.resource = resource;
        }

    }
}
