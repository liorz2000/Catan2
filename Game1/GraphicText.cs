using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Game1
{
    public class GraphicText
    {
        public SpriteFont font;
        public string text;
        public Vector2 location;
        public Color txtColor;
        public int layer;

        public GraphicText(SpriteFont font, string text, Vector2 location, Color txtColor, int layer = 1)
        {
            this.font = font;
            this.text = text;
            this.location = location;
            this.txtColor = txtColor;
            this.layer = layer;
        }
    }
}
