using Microsoft.Xna.Framework;
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
        public bool is_have_base_type;
        public bool is_have_cube_num;
        public bool is_have_resource;
        //static public 
        public Dictionary<(int,int, bool), Vertex > vertices;
        

        public string resource; // "tree", "sheep", "grain", "ore", "break" ,"desert"
        public int cube_num; //2,3,4,5,6,8,9,10,11,12
        public string base_type; // "land", "sea"

        //The graphic part of Cell:
        public Dictionary<string, Texture2D> opitional_texturs;
        //public string current_texture;
        public Button self_button;
        public Button num_button;  
        

        public Cell(int row, int colum) 
        {
            this.row = row;
            this.colum = colum;
            is_have_base_type = false;
            is_have_cube_num = false;
            is_have_resource = false;
            vertices = new Dictionary<(int, int, bool), Vertex>();
        }
        public void add_base_type(string base_type)
        {
            is_have_base_type = true;
            this.base_type = base_type;
           
        }

        public void add_cube_num(int cube_num, Texture2D texture2D, int raduis)
        {
            is_have_cube_num = true;
            this.cube_num = cube_num;
            int x_center = self_button.rectangle.X + self_button.rectangle.Width / 2;
            int y_center = self_button.rectangle.Y + self_button.rectangle.Height / 2;
            Rectangle r = new Rectangle(new Point(x_center - raduis, y_center - raduis), new Point(2 * raduis, 2 * raduis));
            num_button = new Button(r, texture2D, self_button.action, self_button.shape, self_button.activity_phases, self_button.layer + 1);
        }

        public void add_resource(string resource, Texture2D texture2D)
        {
            is_have_resource = true;
            this.resource = resource;
            self_button = new Button(self_button.rectangle, texture2D, self_button.action, self_button.shape, self_button.activity_phases);
        }
        /*public Cell(int row, int colum, string base_type, int cube_num)
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
        }*/



    }
}
