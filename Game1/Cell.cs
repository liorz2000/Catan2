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
        //static public 
        public Dictionary<(int,int, bool), Vertex > vertices;
        public string situation; // "Creation of map", "In Game"


        public string resource; // "", "tree", "sheep", "grain", "ore", "break" ,"desert"
        public int cube_num;


        //The graphic part of Cell:
        public Dictionary<string, Texture2D> opitional_texturs;
        public string current_texture;
        public Button self_button;
        public Button num_button;  
        

        public Cell(int row, int colum, string base_type = "") 
        {
            this.row = row;
            this.colum = colum;
            this.base_type = base_type;
            vertices = new Dictionary<(int, int, bool), Vertex>();
            
            current_texture = base_type;
            situation = "Creation of map";
        }

        public Cell(int row, int colum, string base_type, int cube_num)
        {
            this.row = row;
            this.colum = colum;
            this.base_type = base_type;
            vertices = new Dictionary<(int, int, bool), Vertex>();

            this.cube_num = cube_num;

            current_texture = base_type;
            situation = "In Game";
        }
        public Cell(int row, int colum, string base_type, int cube_num, string resource)
        {
            this.row = row;
            this.colum = colum;
            this.base_type = base_type;
            vertices = new Dictionary<(int, int, bool), Vertex>();

            this.cube_num = cube_num;
            this.resource = resource;
            
            current_texture = resource;
            situation = "In Game";
        }

        public void initialize_grphics()
        {
            if (situation == "Creation of map")
            {
                
            }
            if (situation == "In Game")
            {

            }

        }
        public void update_graphics()
        {
            if (situation == "Creation of map")
            {

            }
            if (situation == "In Game")
            {

            }
        }



    }
}
