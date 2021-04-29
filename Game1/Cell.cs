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
        

        public string resource; // "", "tree", "sheep", "grain", "ore", "break" ,"desert"
        public int cube_num;


        //The graphic part of Cell:
        public string situation; // "Cell_base_type", "Cell_with_resource", "Cell_with_reasource_and_num"
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
            situation = "Cell_base_type";
        }

        public Cell(int row, int colum, string base_type, int cube_num)
        {
            this.row = row;
            this.colum = colum;
            this.base_type = base_type;
            vertices = new Dictionary<(int, int, bool), Vertex>();

            this.cube_num = cube_num;

            current_texture = base_type;
            situation = "Cell_with_num";
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
            situation = "Cell_with_num_and_reasource";
        }

        public void initialize_grphics(Dictionary<string, Texture2D> opitional_texturs, Button self_button)
        // Get regular button , 
        {
            if (situation == "Cell_base_type")
            {
                this.opitional_texturs = opitional_texturs;
                this.self_button = self_button;

            }
            if (situation == "Cell_with_num")
            {
                this.opitional_texturs = opitional_texturs;
                this.self_button = self_button;

            }
            if (situation == "Cell_with_num_and_reasource")
            {
                this.opitional_texturs = opitional_texturs;
            }

        }
        public void update_graphics(string new_texture)
        {
            if (situation == "Cell_base_type")
            {

            }
            if (situation == "Cell_with_resource")
            {

            }
            if (situation == "Cell_with_reasource_and_num")
            {

            }
        }



    }
}
