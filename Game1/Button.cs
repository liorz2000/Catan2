using System;
using System.Collections.Generic;
//using Math;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    public class Button
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
        //public Point center;
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
        // regular
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

            this.is_multipul_texture_dictionery = false;
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

            this.is_multipul_texture_dictionery = false;
        }
        public Button(Rectangle rectangle, Texture2D[] all_textures, Action action, string shape, string[] activity_phases, int layer = 1)
        // filping texture array
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

            this.is_multipul_texture_dictionery = false;
        }

        public Button(Rectangle rectangle, Dictionary<string, Texture2D> dictionary_textures, Action action, string shape, string[] activity_phases, int layer = 1)
        //fliping texture dictionery
        {
            this.rectangle = rectangle;
            this.shape = shape;
            this.action = action;
            this.activity_phases = activity_phases;
            this.is_clicked_now = false;
            this.is_text_field = false;
            this.layer = layer;

            this.is_multipul_texture_array = false;

            this.is_multipul_texture_dictionery = true;
            this.dictionary_textures = new Dictionary<string, Texture2D>();
            this.phase_string = "";
        }

        public Button(Point[] new_vertices, Texture2D texture2D, Action action, string[] activity_phases, int layer = 1)
        // regular button, but polygon, one color!!!
        // new verteces must be with clock direction!!!
        {
            set_vertexes(new_vertices);

            this.texture2D = texture2D;
            (int, int) only_test = cut_texture();
            shape = "polygon";
            this.action = action;
            this.activity_phases = activity_phases;
            this.is_clicked_now = false;
            this.is_text_field = false;
            this.layer = layer;

            this.is_text_field = false;
            this.is_multipul_texture_array = false;

            this.is_multipul_texture_dictionery = false;

        }

        public void set_vertexes(Point[] new_vertices)
        {
            int min_x = new_vertices[0].X;
            int max_x = new_vertices[0].X;
            int min_y = new_vertices[0].Y;
            int max_y = new_vertices[0].Y;
            foreach (Point v in new_vertices)
            {
                if (v.X > max_x)
                {
                    max_x = v.X;
                }
                if (v.X < min_x)
                {
                    min_x = v.X;
                }
                if (v.Y > max_y)
                {
                    max_y = v.Y;
                }
                if (v.Y < min_y)
                {
                    min_y = v.Y;
                }
            }

            rectangle = new Rectangle(new Point(min_x, min_y), new Point(max_x - min_x, max_y - min_y));
            vertices = new_vertices;

        }
        
        public (int, int) cut_texture()
        {
            Color[] c = new Color[texture2D.Width * texture2D.Height];
            texture2D.GetData(c);
            Color[,] c_mat = new Color[texture2D.Height, texture2D.Width];//maybe oposite

          
            for (int col = 0; col < texture2D.Width; col++)
                {
                for (int row = 0; row < texture2D.Height; row++)
                {
                    try
                    {
                       
                        c_mat[row, col] = c[row * texture2D.Width + col];
                    }
                    catch
                    {
                        Game1.declear(Convert.ToString(row) + ", " + Convert.ToString(col));
                        return (row, col);

                    }
                    int point_of_texcture_pixel_in_rec_x = rectangle.X + col * rectangle.Width / texture2D.Width;
                    int point_of_texcture_pixel_in_rec_y = rectangle.Y + row * rectangle.Height / texture2D.Height;
                    Point p = new Point(point_of_texcture_pixel_in_rec_x, point_of_texcture_pixel_in_rec_y);
                    if (!check_if_in_polygon(p))
                    {
                        c_mat[row, col] = Color.Transparent;
                    }
                    c[row * texture2D.Width + col] = c_mat[row, col];
                }
            }
            texture2D.SetData(c);
            return (-1, -1);
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
                int constt = rectangle.Width * rectangle.Height +_hex_in_rec_compute_help_func(rectangle.X, rectangle.Y);
                return (rectangle.X <= mouse.X & mouse.X <= rectangle.X + rectangle.Width &
                    _hex_in_rec_compute_help_func(mouse.X, mouse.Y) >= constt &
                    _hex_in_rec_compute_help_func(2 * rectangle.X + rectangle.Width- mouse.X, mouse.Y) >= constt &
                    _hex_in_rec_compute_help_func(mouse.X, 2* rectangle.Y +rectangle.Height - mouse.Y) >= constt &
                    _hex_in_rec_compute_help_func(2 * rectangle.X + rectangle.Width - mouse.X, 2 * rectangle.Y + rectangle.Height - mouse.Y) >= constt);
            }
            if (shape == "circle")
            {
                //return false;
                //Math.Pow(value, power)
                return Math.Pow(mouse.X - (rectangle.X + rectangle.Width / 2), 2) / Math.Pow(rectangle.Width / 2, 2) +
                    Math.Pow(mouse.Y - (rectangle.Y + rectangle.Height / 2), 2) / Math.Pow(rectangle.Height / 2, 2) <= 1;
            }
            if (shape == "polygon")
            {
                return check_if_in_polygon(mouse.Position);
            }
            return false;
        }

        public int _hex_in_rec_compute_help_func(int x, int y)
        {
            return 4 * rectangle.Width * y + 2 * rectangle.Height * x;
        }
        public bool check_if_in_polygon(Point point)
        {

            int len = vertices.Length;
            bool in_polygon = true;

            for (int i = 0; i <len; i ++)
            {
                in_polygon &= check_if_clock_wise(vertices[i], vertices[(i+1)%len], point);
            }
            return in_polygon;

        }
        public static bool check_if_clock_wise(Point p1, Point p2, Point p3)
        {
            return p1.X * p2.Y + p2.X * p3.Y + p3.X * p1.Y - (p1.X * p3.Y + p2.X * p1.Y + p3.X * p2.Y) > 0;
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
