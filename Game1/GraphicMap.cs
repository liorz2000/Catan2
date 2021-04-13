using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game1
{

    class GraphicMap
    {

        static double r = 2/Math.Sqrt(3);
        

        public logic_map lmap = new logic_map(new HashSet<(int, int, string)>());
        public int step_size;
        public Point zero_hex_center;
        public Dictionary<string, Texture2D> textures;


        public List<Button> cell_buttons = new List<Button>();

        public GraphicMap(logic_map lmap_given, int step_size_given, Point zero_hex_center_given, Dictionary<string, Texture2D> textures_given )
        {
            lmap = lmap_given;
            step_size = step_size_given;
            zero_hex_center = zero_hex_center_given;
            textures = textures_given;

            Texture2D[] hex_textures = new Texture2D[] { textures["gray_hex"], textures["yellow_hex"], textures["blue_hex"] };
            foreach ((int,int) cellii in lmap.cells.Keys)
            {
                Rectangle hex_rec = new Rectangle(new Point(zero_hex_center.X - step_size/2 + cellii.Item1 * step_size/2  + cellii.Item2 * step_size ,
                    zero_hex_center.Y - (int)Math.Round(step_size * r/2) + cellii.Item1* (int)Math.Round(step_size/r) )
                    , new Point(step_size, (int)Math.Round(r*step_size)));
                Button new_hex = new Button(hex_rec, hex_textures, Game1.literally_nothing, "rectangel", new string[] { "normal" });
                cell_buttons.Add(new_hex);
            }
        }
    }
}
