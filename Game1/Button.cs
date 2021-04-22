using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    class Button
    {
        public Rectangle rectangle;
        public Texture2D texture2D;
        public string shape;
        public Action action;
        public string[] activity_phases;
        public bool is_clicked_now;
        public bool is_text_field;
        public int layer;

        public double angel;
        public double raduis;
        public Point center;
        public Point[] vertices;
        
        public GraphicText gtext;
        public bool is_writing;
        public Texture2D[] when_writing_and_not;
        public string text_info;
        public string textfield_type; // or "regular" or "password"

        public bool is_multipul_texture_array;
        public Texture2D[] all_textures;
        public int phase_index;

        public bool is_multipul_texture_dictionery;
        public Dictionary<string, Texture2D> dictionary_textures;
        public string phase_string;

        public Button ()
         //button nothig
        {

        }
        public Button (Rectangle rectangle, Texture2D texture2D, Action action, string shape, string[] activity_phases, int layer = 1  )
        // regular button
        {
            this.rectangle = rectangle;
            this.texture2D = texture2D;
            this.shape = shape;
            this.action = action;
            this.activity_phases = activity_phases;
            this.is_clicked_now = false;
            this.layer = layer;
            
            this.is_text_field = false;
            this.is_multipul_texture_array = false;
        }


        public Button(Rectangle rectangle, Texture2D texture2D, Action action, string shape, string[] activity_phases, GraphicText gtext, Texture2D when_writing, string textfield_type = "regular", int layer = 1)
        // textField button 
        {
            this.is_text_field = true;
            this.gtext = gtext;
            this.is_writing = false;
            this.when_writing_and_not = new Texture2D []{texture2D, when_writing};
            this.action = action;
            this.text_info = "";
            this.textfield_type = textfield_type;

            this.rectangle = rectangle;
            this.texture2D = texture2D;
            this.shape = shape;
            this.activity_phases = activity_phases;
            this.is_clicked_now = false;
            this.layer = layer;

            is_multipul_texture_array = false;
        }
        public Button(Rectangle rectangle, Texture2D[] all_textures, Action action, string shape, string[] activity_phases, int layer = 1)
        // filping texture
        {
            this.rectangle = rectangle;
            this.shape = shape;
            this.action = action;
            this.activity_phases = activity_phases;
            this.is_clicked_now = false;
            this.is_text_field = false;
            this.layer = layer;

            this.is_multipul_texture_array = true;
            this.all_textures = all_textures;
            this.phase_index = 0;
        }

        public Button(Rectangle rectangle, Dictionary<string, Texture2D> dictionary_textures, Action action, string shape, string[] activity_phases, int layer = 1)
        {
            this.rectangle = rectangle;
            this.shape = shape;
            this.action = action;
            this.activity_phases = activity_phases;
            this.is_clicked_now = false;
            this.is_text_field = false;
            this.layer = layer;

            this.is_multipul_texture_array = true;
            this.all_textures = all_textures;
            this.phase_index = 0;
        }

        public bool Is_mouse_on()
        {
            MouseState mouse = Mouse.GetState();
            if (shape == null)
            {
                return false;
            }
            if (shape == "rectangel")
            {
                return (rectangle.X <= mouse.X & mouse.X <= rectangle.X + rectangle.Width
                   & rectangle.Y <= mouse.Y & mouse.Y <= rectangle.Y + rectangle.Height);
            }
            if (shape == "hex_in_rec")
            {

            }
            if (shape == "circle")
            {
                return false;
            }
            if (shape == "polygon")
            {
                return false;
            }
            return false;
        }
        public void create_a_polygon(Point [] new_vertices, Point new_center)
        {

            this.vertices = new_vertices;
            this.center = new_center;

        }
        public void rotate(double rot_angel)
        {

        }
        public void SetText(string str)
        {
            this.gtext.text  = str;
        }
        public void SetTexture()
        {
            if (is_writing)
            {
                texture2D = when_writing_and_not[1];
            }
            else
            {
                texture2D = when_writing_and_not[0];
            }
        }


    }
}
