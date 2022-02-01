using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using System.Linq;


namespace Game1
{
    /*public class Account
    {
        public string Email { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public IList<string> Roles { get; set; }
    }*/
    public class Game1 : Game
    {
        
        private GraphicsDeviceManager graphicsDeviceManager { get; set; }

        private bool mouse_LeftButton_copy = false;

        private Member player = null;
        private string members_file = @"~\..\..\..\..\members.json";
        private string current_act = "";
        private string window_name; // "sign or login", "map creation, size"

        private List<string> names_of_texture_to_load = new List<string>(new string[] {"tree", "sheep", "grain", "ore", "break",
            "exit", "create_map","local_game","sign in", "login", "logout", "white_rec", "gray_rec", "back", "send", "ok",
            "Invite friends", "yellow",
            "plus", "minus", "minus_red", "continue", "gray_hex", "blue_hex", "yellow_hex","blue_yellow_hex",
            "num_circle", "2", "3", "4", "5", "6", "8", "9", "10", "11", "12"
        ,"help", "stage2", "texture_test", "green", "red"});
        private static Dictionary<string, Texture2D> textures_to_load = new Dictionary<string, Texture2D>();
        private static Dictionary<string, Button>[] buttons_to_show = new Dictionary<string, Button>[3];
        private static string activity_phase = "normal"; // normal, decleration
        private Dictionary<string, Button> completly_change_of_buttons = null;
        //private Dictionary<string, GraphicMap> maps_to_show = new Dictionary<string, GraphicMap>();

        private List<string> names_of_SpriteFont_to_load = new List<string>(new string[] { "title_font", "24font",  @"fonts\24font"});
        private static Dictionary<string, SpriteFont> SpriteFont_to_load = new Dictionary<string, SpriteFont>();
        private static Dictionary<string, GraphicText>[] text_to_show = new Dictionary<string, GraphicText>[3];
        private Dictionary<string, GraphicText> completly_change_of_texts = null;

        private Dictionary<string, int> nums_to_remember = new Dictionary <string ,int>();

        private Button text_field_that_now = new Button();
        private bool is_writing_now = false;
        private Keys[] keys_that_prresed_copy = new Keys[0];

        private GraphicMap gmap = null;

        public Game1()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            /*IEnumerable<int> squares = Enumerable.Range(-10, 10);
            foreach (var i in Enumerable.Range(-10, 10))
            {

            }*/
            Window.AllowUserResizing = true;
            Window.Title = "Catan";
            IsMouseVisible = true;

            Content.RootDirectory = "Content";

            for (int i = 0; i <= 2; i++)
            {
                buttons_to_show[i] = new Dictionary<string, Button>();
                text_to_show[i] = new Dictionary<string, GraphicText>();
            }
            string player_name = System.IO.File.ReadAllText(@"~\..\..\..\..\player_name.txt");

            if (player_name == "")
            {
                player = null;
            }
            else
            {
                player = find_player(player_name.Substring(0, player_name.Length -2));
            }

            

            base.Initialize();
            graphicsDeviceManager.IsFullScreen = true;
            graphicsDeviceManager.ToggleFullScreen();
            graphicsDeviceManager.ApplyChanges();
        }



        protected override void LoadContent()
        {
            Color[] c = new Color[] { };
            int length = 0;
            foreach (var name in names_of_texture_to_load)
            {
                textures_to_load.Add(name, Content.Load<Texture2D>(name));
                c = new Color[textures_to_load[name].Width * textures_to_load[name].Height];
                textures_to_load[name].GetData(c);
                length = c.Length;
                for (int i = 0; i < length; i++)
                {
                    if (c[i] == Color.White)
                    {
                        c[i] = Color.Transparent;
                    }

                }
                textures_to_load[name].SetData(c);

            }

            foreach (var name in names_of_SpriteFont_to_load)
            {
                SpriteFont_to_load.Add(name, Content.Load<SpriteFont>(name));

            }
            dowhite(textures_to_load["white_rec"]);

            //Texture2D texture2D = new Texture2D(GraphicsDevice, 100, 100);

            menu();
            //when_create_map();
            //change_font_size(@"fonts\24font", 24);
            //check_clock_wise();
            //lern_draw_gray_cells();
            //draw_2();
            //check_texture_info();
            //check_is_in_polygon();
            //draw_polygon();
            //build_the_map(new HashSet<(int, int)>() { (0, 0), (0, 1), (0, 2),(0,3),(0,4),(0,5), (1, 0), (1, 1), (1, 2), (2, -1), (2, 0), (2, 1), (2,2) });
            //draw_the_map();
            base.LoadContent();
            //lern_serialize();
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            KeyboardState keyboard = Keyboard.GetState();

            Keys[] keys = keyboard.GetPressedKeys();

            if (mouse.LeftButton == ButtonState.Pressed && mouse_LeftButton_copy == false)
            {
                mouse_LeftButton_copy = true;
                foreach (var button in buttons_to_show[0].Values)
                {
                    if (button.activity_phases.Contains(activity_phase) && button.Is_mouse_on())
                    {
                        button.is_clicked_now = true;
                    }
                }

                if (gmap != null)
                {
                    foreach (var cell in gmap.cells.Values)
                    {
                        if (cell.self_button != null)
                        {
                            if (cell.self_button.activity_phases.Contains(activity_phase) && cell.self_button.Is_mouse_on())
                            {
                                cell.self_button.is_clicked_now = true;
                            }
                        }

                        if(cell.is_have_cube_num)
                        {
                            if (cell.self_button.activity_phases.Contains(activity_phase) && cell.self_button.Is_mouse_on())
                            {
                                cell.self_button.is_clicked_now = true;
                            }
                        }
                    }

                    foreach (var edge in gmap.edges.Values)
                    {
                        if (edge.self_button != null)
                        {
                            if (edge.self_button.activity_phases.Contains(activity_phase) && edge.self_button.Is_mouse_on())
                            {
                                edge.self_button.is_clicked_now = true;
                            }
                        }
                    }
                }
            }
            if (mouse.LeftButton == ButtonState.Released)
            {
                if (mouse_LeftButton_copy)
                {
                    is_writing_now = false;
                    text_field_that_now = new Button();
                }
                mouse_LeftButton_copy = false;
                foreach (var button in buttons_to_show[0].Values)
                {
                    update_is_clicked_now(button);
                }

                if (gmap != null)
                {
                    foreach (var cell in gmap.cells.Values)
                    {
                        update_is_clicked_now(cell.self_button);
                        if (cell.is_have_cube_num)
                        {
                            update_is_clicked_now(cell.num_button);
                        }
                    }
                    foreach (var edge in gmap.edges.Values)
                    {
                        if (edge.self_button != null)
                        {
                            update_is_clicked_now(edge.self_button);
                        }
                    }

                }


            }
            if (is_writing_now)
            {
                foreach (var key in keys_that_prresed_copy)
                {
                    if (!keyboard.GetPressedKeys().Contains(key))
                    {
                        write(key, keyboard);
                    }
                }
            }
            
            keys_that_prresed_copy = keyboard.GetPressedKeys();

            if (completly_change_of_buttons == null)
            {
                foreach (var button in buttons_to_show[1])
                {
                    buttons_to_show[0].Add(button.Key, button.Value);
                }
                buttons_to_show[1] = new Dictionary<string, Button>();
                foreach (var button in buttons_to_show[2])
                {
                    buttons_to_show[0].Remove(button.Key);
                }
                buttons_to_show[2] = new Dictionary<string, Button>();
            }
            else
            {
                buttons_to_show[0] = completly_change_of_buttons;
                completly_change_of_buttons = null;
            }

            if (completly_change_of_texts  == null)
            {
                foreach (var gtext in text_to_show[1])
                {
                    text_to_show[0].Add(gtext.Key, gtext.Value);
                }
                text_to_show[1] = new Dictionary<string, GraphicText>();
                foreach (var gtext in text_to_show[2])
                {
                    text_to_show[0].Remove(gtext.Key);
                }
                text_to_show[2] = new Dictionary<string, GraphicText>();
            }
            else
            {
                text_to_show[0] = completly_change_of_texts;
                completly_change_of_texts = null;
            }



            if (gmap != null && gmap.stage == 1 && text_to_show[0].ContainsKey("random land"))
            {
                update_rand_land_txt_color();
            }
            if (gmap != null && gmap.stage == 2 && text_to_show[0].ContainsKey("random ports"))
            {
                update_rand_ports_txt_color();
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {   

            SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront);

            
            foreach (var button in buttons_to_show[0].Values)
            {
                draw_button(button, spriteBatch);
            }

            

            foreach (var gtext in text_to_show[0].Values)
            {
                
                spriteBatch.DrawString(gtext.font, gtext.text, gtext.location, gtext.txtColor, 0, new Vector2(0, 0), 1, 0, layer_func(gtext.layer));
            }

            if (gmap != null)
            {
                foreach (var cell in gmap.cells.Values)
                {
                    draw_button(cell.self_button, spriteBatch);
                    if (cell.is_have_cube_num)
                    {
                        draw_button(cell.num_button, spriteBatch);
                    }
                }
                foreach (var edge in gmap.edges.Values)
                {
                    if (edge.self_button != null)
                    {
                        draw_button(edge.self_button, spriteBatch);
                    }
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public Texture2D copy_texture(Texture2D texture)
        {
            int length = texture.Width * texture.Height;
            Texture2D new_texture = new Texture2D(GraphicsDevice, texture.Width,texture.Height);
            Color[] old_texture_data = new Color[length];
            Color[] new_texture_data = new Color[length];
            texture.GetData(old_texture_data);
            for (int i = 0; i < length; i++)
            {
                new_texture_data[i] = old_texture_data[i];
            }
            new_texture.SetData(new_texture_data);
            return new_texture;
        }
        public void draw_button(Button button, SpriteBatch spriteBatch)
        {
            if (button != null)
            {
                if (button.is_multipul_texture_array)
                {
                    spriteBatch.Draw(button.all_textures[button.phase_index], button.rectangle, null, Color.White, 0, new Vector2(0, 0), 0, layer_func(button.layer));
                }
                else
                {
                    if (button == text_field_that_now)
                    {
                        spriteBatch.Draw(button.when_writing_and_not[1], button.rectangle, null, Color.White, 0, new Vector2(0, 0), 0, layer_func(button.layer));
                    }
                    else
                    {
                        spriteBatch.Draw(button.texture2D, button.rectangle, null, Color.White, 0, new Vector2(0, 0), 0, layer_func(button.layer));
                    }
                    if (button.is_text_field == true)
                    {
                        spriteBatch.DrawString(button.gtext.font, button.gtext.text, button.gtext.location, button.gtext.txtColor, 0, new Vector2(0, 0), 1, 0, layer_func(button.layer, true));
                    }
                }
            }
        }
        public void update_is_clicked_now(Button button)
        {
            if (button != null)
            {

                if (button.activity_phases.Contains(activity_phase) && button.is_clicked_now && button.Is_mouse_on())
                {
                    button.action();
                    if (button.is_text_field)
                    {
                        text_field_that_now = button;
                        is_writing_now = true;
                    }
                    if (button.is_multipul_texture_array)
                    {
                        button.phase_index = (button.phase_index + 1) % (button.all_textures.Length);
                    }
                }
                button.is_clicked_now = false;
            }
        }
        public float layer_func( int n, bool half = false)
        {
            if (half)
            {
                return (float)(1 / ((float)(n) + 1.5));
            }
            return (float)(1 / ((float)(n) + 1));
        }
        public void SaveExit()
        {
            string docPath = @"~\..\..\..\..\";
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "player_name.txt")))
            {
                if (player == null)
                {
                    outputFile.WriteLine("");
                    Exit();
                    return;
                }
                outputFile.WriteLine(player.user_name);
                Exit();
            }
        }
        public void menu()
        {
            nums_to_remember = new Dictionary<string, int>();
            gmap = null;
            if (player == null)
            {
                menu_no_one();
            }
            else
            {
                menu_someone(player);
            }
        }
        public void menu_no_one()
        {
            completly_change_of_buttons = new Dictionary<string, Button>();

            Rectangle exit_rec = new Rectangle(new Point(50, 900), new Point(180, 90));
            Button exit = new Button(exit_rec, textures_to_load["exit"], SaveExit, "rectangel", new string[] { "normal" });
            completly_change_of_buttons.Add("exit", exit);

            Rectangle sign_in_rec = new Rectangle(new Point(50, 700), new Point(180, 90));
            Button sign_in = new Button(sign_in_rec, textures_to_load["sign in"], when_sign_in, "rectangel", new string[] { "normal" });
            completly_change_of_buttons.Add("sign_in", sign_in);

            Rectangle login_rec = new Rectangle(new Point(50, 800), new Point(180, 90));
            Button login = new Button(login_rec, textures_to_load["login"], when_login, "rectangel", new string[] { "normal" });
            completly_change_of_buttons.Add("login", login);

            completly_change_of_texts = new Dictionary<string, GraphicText>();

            GraphicText title = new GraphicText(SpriteFont_to_load["title_font"], "Catan", new Vector2(700, 100), Color.Black);
            completly_change_of_texts.Add("title", title);

        }
        public void menu_someone(Member player)
        {
            completly_change_of_buttons = new Dictionary<string, Button>();

            Rectangle exit_rec = new Rectangle(new Point(50, 900), new Point(180, 90));
            Button exit = new Button(exit_rec, textures_to_load["exit"], SaveExit, "rectangel", new string[] { "normal" });
            
            Rectangle logout_rec = new Rectangle(new Point(50, 800), new Point(180, 90));
            Button logout = new Button(logout_rec, textures_to_load["logout"], when_log_out, "rectangel", new string[] { "normal" });

            Rectangle local_game_rec = new Rectangle(new Point(50, 700), new Point(180, 90));
            Button local_game = new Button(local_game_rec, textures_to_load["local_game"], literally_nothing , "rectangel", new string[] { "normal" });

            Rectangle Invite_friends_rec = new Rectangle(new Point(50, 600), new Point(180, 90));
            Button Invite_friends = new Button(Invite_friends_rec, textures_to_load["Invite friends"], literally_nothing , "rectangel", new string[] { "normal" });

            //"create_map"

            Rectangle create_map_rec = new Rectangle(new Point(50, 500), new Point(180, 90));
            Button create_map = new Button(create_map_rec, textures_to_load["create_map"], when_create_map, "rectangel", new string[] { "normal" });

            completly_change_of_buttons.Add("logout", logout);
            completly_change_of_buttons.Add("Invite_friends", Invite_friends);
            completly_change_of_buttons.Add("local_game", local_game);
            completly_change_of_buttons.Add("exit", exit);
            completly_change_of_buttons.Add("create_map", create_map);


            completly_change_of_texts = new Dictionary<string, GraphicText>();
            
            GraphicText title = new GraphicText(SpriteFont_to_load["title_font"], "Catan", new Vector2(700, 100), Color.Black);
            completly_change_of_texts.Add("title", title);

            GraphicText hello = new GraphicText(SpriteFont_to_load["24font"], "Hello " + player.user_name , new Vector2(100, 100), Color.Black);
            completly_change_of_texts.Add("hello", hello);
        }
        public void when_create_map()
        {
            completly_change_of_buttons = new Dictionary<string, Button>();
            completly_change_of_texts = new Dictionary<string, GraphicText>();

            GraphicText title = new GraphicText(SpriteFont_to_load["title_font"], "Map Creation", new Vector2(500, 50), Color.Black);
            completly_change_of_texts.Add("title", title);

            Rectangle map_size_plus_min_rec = new Rectangle(new Point(100, 250), new Point(200, 100));
            add_plus_minus_system("map size", 5, map_size_plus_min_rec, when_increase_map_size, when_decrease_map_size);

            Rectangle hex_size_plus_min_rec = new Rectangle(new Point(100, 370), new Point(200, 100));
            add_plus_minus_system("hex size", 100, hex_size_plus_min_rec, when_increase_hex_size, when_decrease_hex_size);

            Rectangle random_land_plus_min_rec = new Rectangle(new Point(100, 490), new Point(220, 110));
            add_plus_minus_system("random land", 0, random_land_plus_min_rec, when_increase_rand_land, when_decrease_rand_land);

            Rectangle help_rec = new Rectangle(new Point(100, 620), new Point(180, 75));
            Button help = new Button(help_rec, textures_to_load["help"], help_func, "rectangel", new string[] { "normal" });
            completly_change_of_buttons.Add("help", help);
            
            Rectangle stage2_rec = new Rectangle(new Point(100, 720), new Point(180, 75));
            Button stage2 = new Button(stage2_rec, textures_to_load["stage2"], pass_to_stage_2, "rectangel", new string[] { "normal" });
            completly_change_of_buttons.Add("stage2", stage2);
            
            Rectangle back_rec = new Rectangle(new Point(100, 820), new Point(180, 75));
            Button back = new Button(back_rec, textures_to_load["back"], menu, "rectangel", new string[] { "normal" });
            completly_change_of_buttons.Add("back", back);
            
            create_a_rec_map(nums_to_remember["map size"]);

            //draw_some_edges();
        }
        public void pass_to_stage_2()
        {
            if (text_to_show[0]["random land"].txtColor == Color.Red)
            {
                declear("Not valid random land balance");
            }
            else
            {
                gmap.level_up_to_stage2();
                if (gmap.stage == 2)
                {
                    remove_plus_minus_system("map size");
                    remove_plus_minus_system("random land");
                    Rectangle random_ports_plus_min_rec = new Rectangle(new Point(100, 490), new Point(220, 110));
                    add_plus_minus_system("random ports", 0, random_ports_plus_min_rec, when_increase_rand_ports, when_decrease_rand_ports);
                }
            }
        }
        public void help_func()
        {
            if (gmap.stage == 1)
            {
                declear("You are in stage 1 of map creaiton!\n\n" +
                    "Yellow cells are land cells.\n" +
                    "Blue cells are sea cells.\n" +
                    "Yellow-Blue are random 'land or sea' hexs.\n" +
                    "With 'random land' you detrmine how much land\n" +
                    "cells will be among the random.");
            }
            else if (gmap.stage == 2) 
            {
                declear("You are in stage 2 of mao creation,");
                declear("the stage of port placing!", "\n\n");
                declear("Gray roads == no port there");
                declear("Green roads == port there");
                declear("Red roads == random ports");
                declear("random ports plus_minus system == ");
                declear("== how many random will be ports.");

            }
        }
        public void add_plus_minus_system(string num_to_remember_name, int num_to_remember, Rectangle backround_rec, Action inc_func, Action dec_func)
        {
            nums_to_remember.Add(num_to_remember_name, num_to_remember);

            //GraphicText systems_text = new GraphicText(SpriteFont_to_load["24font"], num_to_remember_name +": " + Convert.ToString(nums_to_remember[num_to_remember_name]), new Vector2(100, 400), Color.Black, 2);
            GraphicText systems_text = new GraphicText(SpriteFont_to_load["24font"], num_to_remember_name + ": " + Convert.ToString(nums_to_remember[num_to_remember_name]), new Vector2(backround_rec.X, backround_rec.Y), Color.Black, 2);
            text_to_show[1].Add(num_to_remember_name, systems_text);

            //Rectangle map_size_backround_rec = new Rectangle(new Point(100, 400), new Point(200, 100));
            Button backround = new Button(backround_rec, textures_to_load["yellow"], literally_nothing, "rectangel", new string[] { "normal" });
            buttons_to_show[1].Add(num_to_remember_name + "_backround", backround);

            //Rectangle plus_rec = new Rectangle(new Point(200, 450), new Point(50, 50));
            Rectangle plus_rec = new Rectangle(new Point(backround_rec.X + backround_rec.Width/2, backround_rec.Y + backround_rec.Height /2),
                new Point(backround_rec.Width / 4, backround_rec.Height / 2));
            Button plus = new Button(plus_rec, textures_to_load["plus"], inc_func, "rectangel", new string[] { "normal" }, 2);
            buttons_to_show[1].Add(num_to_remember_name + "_plus", plus);

            //Rectangle minus_rec = new Rectangle(new Point(100, 450), new Point(50, 50));
            Rectangle minus_rec = new Rectangle(new Point(backround_rec.X, backround_rec.Y + backround_rec.Height / 2),
                new Point(backround_rec.Width / 4, backround_rec.Height / 2));
            Button minus = new Button(minus_rec, textures_to_load["minus"], dec_func, "rectangel", new string[] { "normal" }, 2);
            buttons_to_show[1].Add(num_to_remember_name + "_minus", minus);
        }
        public void remove_plus_minus_system(string num_to_remember_name)
        {
            nums_to_remember.Remove(num_to_remember_name);
            text_to_show[2].Add(num_to_remember_name, text_to_show[0][num_to_remember_name]);
            buttons_to_show[2].Add(num_to_remember_name + "_backround", buttons_to_show[0][num_to_remember_name + "_backround"]);
            buttons_to_show[2].Add(num_to_remember_name + "_plus", buttons_to_show[0][num_to_remember_name + "_plus"]);
            buttons_to_show[2].Add(num_to_remember_name + "_minus", buttons_to_show[0][num_to_remember_name + "_minus"]);
        }
        public void when_decrease_map_size()
        {
            nums_to_remember["map size"] -= 1;
            text_to_show[0]["map size"].text = "map size: " + Convert.ToString(nums_to_remember["map size"]);
            //create_a_rec_map(nums_to_remember["map size"]);
            int size = nums_to_remember["map size"];
            for (int j = 0; j < size +1; j++ )
            {
                gmap.remove_cell_stage1((size, j-size/2));
            }
            for (int i = 0; i < size ; i++)
            {
                gmap.remove_cell_stage1((i, size - i/2));
            }
        }
        public void when_increase_map_size()
        {
            nums_to_remember["map size"] += 1;
            text_to_show[0]["map size"].text = "map size: " + Convert.ToString(nums_to_remember["map size"]);
            //create_a_rec_map(nums_to_remember["map size"]);
            int size = nums_to_remember["map size"]-1;
            for (int j = 0; j < size + 1; j++)
            {
                gmap.add_cell_stage1((size, j - size / 2));
            }
            for (int i = 0; i < size; i++)
            {
                gmap.add_cell_stage1((i, size - i / 2));
            }
        }
        public void update_rand_land_txt_color()
        {
            if (nums_to_remember["random land"] < 0 || nums_to_remember["random land"] > gmap.get_random_sea_land_number_stage1())
            {
                text_to_show[0]["random land"].txtColor = Color.Red;
            }
            else
            {
                text_to_show[0]["random land"].txtColor = Color.Black;
            }
        }
        public void update_rand_ports_txt_color()
        {
            if (nums_to_remember["random ports"] < 0 || nums_to_remember["random ports"] > gmap.get_random_ports_number_stage2())
            {
                text_to_show[0]["random ports"].txtColor = Color.Red;
            }
            else
            {
                text_to_show[0]["random ports"].txtColor = Color.Black;
            }
        }
        public void when_decrease_rand_land()
        {
            nums_to_remember["random land"] -= 1;
            text_to_show[0]["random land"].text = "random land: " + Convert.ToString(nums_to_remember["random land"]);
        }
        public void when_increase_rand_land()
        {
            nums_to_remember["random land"] += 1;
            text_to_show[0]["random land"].text = "random land: " + Convert.ToString(nums_to_remember["random land"]);
        }
        public void when_decrease_rand_ports()
        {
            nums_to_remember["random ports"] -= 1;
            text_to_show[0]["random ports"].text = "random ports: " + Convert.ToString(nums_to_remember["random ports"]);
        }
        public void when_increase_rand_ports()
        {
            nums_to_remember["random ports"] += 1;
            text_to_show[0]["random ports"].text = "random ports: " + Convert.ToString(nums_to_remember["random ports"]);
        }
        public void when_decrease_hex_size()
        {
            nums_to_remember["hex size"] -= 10;
            text_to_show[0]["hex size"].text = "hex size: " + Convert.ToString(nums_to_remember["hex size"]);
            gmap.set_step_size(gmap.step_size - 10);
        }
        public void when_increase_hex_size()
        {
            nums_to_remember["hex size"] += 10;
            text_to_show[0]["hex size"].text = "hex size: " + Convert.ToString(nums_to_remember["hex size"]);
            gmap.set_step_size(gmap.step_size + 10);
        }

        public void create_a_rec_map(int size)
        {
            HashSet<(int, int)> rec_indexes = new HashSet<(int, int)>();
            int i;
            int j;
            for (i = 0; i < size; i++)
            {
                //k = size - i / 2;
                for (j = 0; j < size; j++) 
                {
                    rec_indexes.Add((i, j-i/2));
                }
            }
            gmap = new GraphicMap(textures_to_load, rec_indexes, this);
            gmap.add_cell_self_buttons_stage1(100, new Point(400, 300));
        }
        public void when_log_out()
        {
            player = null;
            menu_no_one();
        }
        public void sing_in_login_common_code()
        {
            GraphicText user_name = new GraphicText(SpriteFont_to_load["24font"], "user name:", new Vector2(700, 300), Color.Black);
            text_to_show[1].Add("user_name", user_name);

            Rectangle user_rec = new Rectangle(new Point(700, 350), new Point(200, 50));
            GraphicText user_name_writing = new GraphicText(SpriteFont_to_load["24font"], "", new Vector2(700, 350), Color.Black);
            Button user_name_text_field = new Button(user_rec, textures_to_load["white_rec"], literally_nothing, "rectangel",new string[] {"normal"}, user_name_writing, textures_to_load["gray_rec"]);
            buttons_to_show[1].Add("user_name_text_field", user_name_text_field);

            GraphicText password = new GraphicText(SpriteFont_to_load["24font"], "password:", new Vector2(700, 400), Color.Black);
            text_to_show[1].Add("password", password);

            Rectangle pass_rec = new Rectangle(new Point(700, 450), new Point(200, 50));
            GraphicText password_writing = new GraphicText(SpriteFont_to_load["24font"], "", new Vector2(700, 450), Color.Black);
            Button password_text_field = new Button(pass_rec, textures_to_load["white_rec"],  literally_nothing, "rectangel", new string[] { "normal" }, password_writing, textures_to_load["gray_rec"],"password");
            buttons_to_show[1].Add("password_text_field", password_text_field);

            Rectangle back_rec = new Rectangle(new Point(700, 600), new Point(180, 90));
            Button back = new Button(back_rec, textures_to_load["back"], when_back, "rectangel", new string[] { "normal" });
            buttons_to_show[1].Add("back", back);

            Rectangle send_rec = new Rectangle(new Point(900, 600), new Point(180, 90));
            Button send = new Button(send_rec, textures_to_load["send"], when_send, "rectangel", new string[] { "normal" });
            buttons_to_show[1].Add("send", send);
        }
        public void when_sign_in()
        {
            if (current_act == "")
            {
                sing_in_login_common_code();
                current_act = "sign_in";

                GraphicText action_expliner = new GraphicText(SpriteFont_to_load["24font"], "(sign in attempt)", new Vector2(700, 550), Color.Black);
                text_to_show[1].Add("action_expliner", action_expliner);
            }
            if (current_act == "login")
            {
                text_to_show[0]["action_expliner"].text = "(sign in attempt)";
                current_act = "sign_in";
            }
        }
        public void when_login()
        {
            if (current_act == "")
            {
                sing_in_login_common_code();
                current_act = "login";

                GraphicText action_expliner = new GraphicText(SpriteFont_to_load["24font"], "(login attempt)", new Vector2(700, 550), Color.Black);
                text_to_show[1].Add("action_expliner", action_expliner);
            }
            if (current_act == "sign_in")
            {
                text_to_show[0]["action_expliner"].text = "(login attempt)";
                current_act = "login";
            }
        }
        public void when_back()
        {
            buttons_to_show[2].Add("back", buttons_to_show[0]["back"]);
            buttons_to_show[2].Add("send", buttons_to_show[0]["send"]);
            buttons_to_show[2].Add("user_name_text_field", buttons_to_show[0]["user_name_text_field"]);
            buttons_to_show[2].Add("password_text_field", buttons_to_show[0]["password_text_field"]);

            text_to_show[2].Add("action_expliner", text_to_show[0]["action_expliner"]);
            text_to_show[2].Add("password", text_to_show[0]["password"]);
            text_to_show[2].Add("user_name", text_to_show[0]["user_name"]);

            current_act = "";
            text_field_that_now = new Button();
        }
        public void when_send()
        {
            string user_name = buttons_to_show[0]["user_name_text_field"].text_info;
            string password = buttons_to_show[0]["password_text_field"].text_info;
            if (user_name == "" )
            {
                declear("user name empty");
                return;
            }
            if (password == "")
            {
                declear("password is empty");
                return;
            }
            string password_hash = MD5Hash.Hash.Content(password);
            List<Member> members = Load_members();
            ///////////////////// send when sing in ///////////////////////
            if (current_act == "sign_in")
            {
                
                if (members != null)
                {
                    foreach (var member in members)
                    {
                        if (user_name == member.user_name)
                        {
                            declear("This user name alredy in use");
                            return;
                        }
                        if (password_hash == member.password_hash)
                        {
                            declear("This password alredy in use");
                            return;
                        }
                    }
                }
                else
                {
                    members = new List<Member>();
                }
                
                members.Add(new Member(user_name, password_hash));
                string members_string = JsonConvert.SerializeObject(members.ToArray());
                System.IO.File.WriteAllText(members_file, members_string);
                player = new Member(user_name, password_hash);
                current_act = "";
                menu_someone(player);
            }
            //////////////  send when login //////////////////////////
            if (current_act == "login")
            {
                if (members != null)
                {
                    Member potentional_member = find_player(user_name);
                    if (potentional_member == null)
                    {
                        declear("No Such Member");
                        return;
                    }
                    else
                    {
                        if (potentional_member.password_hash == password_hash)
                        {
                            player = new Member(user_name, password_hash);
                            current_act = "";
                            menu_someone(player);
                            return;
                        }
                        else
                        {
                            declear("Wrong password!");
                            return;
                        }
                    }
                }
                else
                {
                    declear("No Such Member");
                    return;
                }
            }
        }
        public static Member find_player(string user_name)
        {
            if (user_name == "")
            {
                return null;
            }
            List<Member> members = Load_members();
            if (members != null)
            {
                foreach (var member in members)
                {
                    if (member.user_name == user_name)
                    {
                        return member;
                    }
                }
            }

            //this dont need to happen
            Member m = null;
            return m;
        }
        public static void declear(string what_declear, string end = "\n", string font_name = "24font")
        {
            if (activity_phase == "normal")
            {
                activity_phase = "declear";
                Rectangle declear_rec = new Rectangle(new Point(500, 300), new Point(700, 480));
                Button declear = new Button(declear_rec, textures_to_load["yellow"], literally_nothing, "rectangel", new string[] { "declear" }, 2);
                buttons_to_show[1].Add("declear", declear);
                Rectangle ok_rec = new Rectangle(new Point(800, 680), new Point(100, 100));
                Button ok = new Button(ok_rec, textures_to_load["ok"], when_ok, "rectangel", new string[] { "declear" }, 3);
                buttons_to_show[1].Add("ok", ok);
                GraphicText declear_text = new GraphicText(SpriteFont_to_load[font_name], what_declear + end, new Vector2(500, 300), Color.Black, 3);
                text_to_show[1].Add("declear_text", declear_text);
            }
            else if (activity_phase == "declear")
            {
                if (text_to_show[1].Keys.Contains("declear_text"))
                {
                    text_to_show[1]["declear_text"].text += what_declear + end;
                }
                else if(text_to_show[0].Keys.Contains("declear_text"))
                {
                    text_to_show[0]["declear_text"].text += what_declear + end;
                }
            }

        }
        public static void when_ok()
        {
            activity_phase = "normal";
            buttons_to_show[2].Add("declear", buttons_to_show[0]["declear"]);
            buttons_to_show[2].Add("ok", buttons_to_show[0]["ok"]);
            text_to_show[2].Add("declear_text", text_to_show[0]["declear_text"]);
        }
        public static void literally_nothing()
        {

        }
        public void dowhite(Texture2D texture)
        {
            Color[] c = new Color[texture.Width * texture.Height];
            texture.GetData(c);
            int length = c.Length;
            for (int i = 0; i < length; i++)
            {
                c[i] = Color.White;
            }
            texture.SetData(c);
        }

        public void write(Keys key, KeyboardState keyboard)

        {
            int num = (int)key;

            if (num >=65 && num <= 90) // Letters
            {
                if (keyboard.CapsLock  ^ (keyboard.IsKeyDown (Keys.LeftShift ) || keyboard.IsKeyDown(Keys.RightShift )))
                {
                    text_field_that_now.text_info  += (char)num;
                }
                else
                {
                    text_field_that_now.text_info += (char)(num + 32);
                }
            }
            if (num == 8) // Back
            {
                if (text_field_that_now.text_info == "")
                {
                    return;
                }
                text_field_that_now.text_info = text_field_that_now.text_info.Substring(0, text_field_that_now.text_info.Length -1);
                
            }
            if (num >= 48 && num <= 57) //numbers_and_their_symbols
            {
                if(keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift))
                {
                    string[] symbol_arr =new string[] { ")", "!", "@", "#", "$", "%", "^", "&", "*", "(" };
                    text_field_that_now.text_info += symbol_arr[num - 48];
                }
                else
                {
                    text_field_that_now.text_info += (num - 48).ToString();
                }
                
            }
            if (num >= 186 && num <= 192) // another symbols
            {
                if (keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift))
                {
                    string[] symbol_arr = new string[] { ":", "+", "<", "_", ">", "?", "~"};
                    text_field_that_now.text_info += symbol_arr[num - 186];
                }
                else
                {
                    string[] symbol_arr = new string[] { ";", "=", ",", "-", ".", "/", "`" };
                    text_field_that_now.text_info += symbol_arr[num - 186];
                }
                
            }
            if (num >= 219 && num <= 222) // another symbols im hakovets
            {
                if (keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift))
                {
                    string[] symbol_arr = new string[] { "{", "|", "}", "\"" };
                    text_field_that_now.text_info += symbol_arr[num - 219];
                }
                else
                {
                    string[] symbol_arr = new string[] { "[", "\\", "]", "'" };
                    text_field_that_now.text_info += symbol_arr[num - 219];
                }
               
            }

            if (text_field_that_now.textfield_type == "password")
            {
                text_field_that_now.gtext.text = String.Concat(Enumerable.Repeat ("*", text_field_that_now.text_info.Length ) );
            }
            else
            {
                text_field_that_now.gtext.text = text_field_that_now.text_info;
            }
            //text_field_that_now.gtext.text += key.ToString();


        }
        public static List<Member> Load_members()
        {
            string file_name = @"~\..\..\..\..\members.json";
            using (StreamReader r = new StreamReader(file_name))
            {
                string json = r.ReadToEnd();
                List<Member> items = JsonConvert.DeserializeObject<List<Member>>(json);
                return items;
            }
        }

        /// <summary>
        /// Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests
        /// Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests
        /// Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests Tests
        /// </summary>
        public void check_is_in_polygon()
        {
            Point p1 = new Point(100, 100);
            Point p2 = new Point(200, 100);
            Point p4 = new Point(200, 200);
            Point p3 = new Point(100, 200);

            Point q1 = new Point(150, 150);
            Point q2 = new Point(150, 50);
            //public Button(Point[] new_vertices, Texture2D texture2D, Action action, string[] activity_phases, int layer = 1)
            Button b = new Button(this, new Point[] { p1, p2, p4, p3 }, textures_to_load["texture_test"], literally_nothing, new string[] { "normal" });
            declear(Convert.ToString(b.check_if_in_polygon(q1))
                + ", " + Convert.ToString(b.check_if_in_polygon(q2)));
        }
        public void draw_polygon()
        {
            Point p1 = new Point(100, 100);
            Point p2 = new Point(200, 100);
            Point p4 = new Point(200, 200);
            Point p3 = new Point(100, 200);
            Point p5 = new Point(150, 150);

            Point q1 = new Point(100, 300);
            Point q2 = new Point(200, 300);
            Point q4 = new Point(200, 400);
            Point q3 = new Point(100, 400);
            Point q5 = new Point(50, 350);
            Button b = new Button(this, new Point[] { p1, p2, p4 }, copy_texture(textures_to_load["texture_test"]), literally_nothing, new string[] { "normal" });

            Rectangle r = new Rectangle(p1, p1);
            Button c = new Button(r, copy_texture(textures_to_load["gray_rec"]), literally_nothing, "rectangel", new string[] { "normal" });

            Button b1 = new Button(this, new Point[] { q1, q2, q4, q3, q5 }, copy_texture(textures_to_load["texture_test"]), literally_nothing, new string[] { "normal" });


            buttons_to_show[1].Add("c", c);
            buttons_to_show[1].Add("b", b);
            buttons_to_show[1].Add("b1", b1);

        }
        public void draw_some_edges() 
        {
            /*//r = 2/ sqrt(3)
            Point O1 = gmap.zero_hex_center;
            Point O2 = new Point(gmap.step_size / 2, (int)Math.Round(-gmap.step_size / gmap.r)) + O1;
            Point v1 = new Point(0, (int)Math.Round(-gmap.step_size / Math.Sqrt(3))) + O1;
            Point v2 = O1 + O2 - v1;

            Point p1 = new Point((v1.X * 4 + O1.X) / 5, (v1.Y * 4 + O1.Y) / 5);
            Point p2 = new Point((v1.X * 4 + O2.X) / 5, (v1.Y * 4 + O2.Y) / 5);
            Point p3 = new Point((v2.X * 4 + O2.X) / 5, (v2.Y * 4 + O2.Y) / 5);
            Point p4 = new Point((v2.X * 4 + O1.X) / 5, (v2.Y * 4 + O1.Y) / 5);

            Point[] points_e001 = new Point[] {p1,p2,p3,p4};
            Button e001 = new Button(points_e001, copy_texture(textures_to_load["gray_rec"]), literally_nothing, new string[] { "normal" }, 2);
            buttons_to_show[1].Add("e001", e001);*/
            draw_one_edge((0, 0, 1));
            draw_one_edge((0, 1, 1));
            draw_one_edge((0, 0, 5));
            draw_one_edge((0, 0, 9));


        }

        public void draw_one_edge((int, int ,int ) edgeiii, int ratio = 5)
        {
            int row = edgeiii.Item1;
            int col = edgeiii.Item2;
            Point O1 = new Point(gmap.zero_hex_center.X + row * gmap.step_size / 2 + col * gmap.step_size,
                   gmap.zero_hex_center.Y - row * (int)Math.Round(gmap.step_size / gmap.r));
            Point v1 = new Point(0, 0);
            Point v2 = new Point(0, 0);
            Point O2 = new Point(0, 0);
            if (edgeiii.Item3 == 1)
            {
                O2 = new Point(gmap.step_size / 2, (int)Math.Round(-gmap.step_size / gmap.r)) + O1;
                v1 = new Point(0, (int)Math.Round(-gmap.step_size / Math.Sqrt(3))) + O1;
                v2 = O1 + O2 - v1;
            }
            else if(edgeiii.Item3 == 5)
            {
                O2 = new Point(gmap.step_size / 2, (int)Math.Round(gmap.step_size / gmap.r)) + O1;
                v1 = new Point(gmap.step_size / 2, (int)Math.Round(gmap.step_size /(2* Math.Sqrt(3)))) + O1;
                v2 = O1 + O2 - v1;
            }
            else if (edgeiii.Item3 == 9)
            {
                O2 = new Point(-gmap.step_size, 0) + O1;
                v1 = new Point(-gmap.step_size / 2, (int)Math.Round(gmap.step_size / (2 * Math.Sqrt(3)))) + O1;
                v2 = O1 + O2 - v1;
            }
            Point p1 = new Point((v1.X * ratio + O1.X) / (ratio+1), (v1.Y * ratio + O1.Y) / (ratio + 1));
            Point p2 = new Point((v1.X * ratio + O2.X) / (ratio + 1), (v1.Y * ratio + O2.Y) / (ratio + 1));
            Point p3 = new Point((v2.X * ratio + O2.X) / (ratio + 1), (v2.Y * ratio + O2.Y) / (ratio + 1));
            Point p4 = new Point((v2.X * ratio + O1.X) / (ratio + 1), (v2.Y * ratio + O1.Y) / (ratio + 1));

            Point[] points_e001 = new Point[] { p1, p2, p3, p4 };
            Button e001 = new Button(this, points_e001, copy_texture(textures_to_load["gray_rec"]), literally_nothing, new string[] { "normal" }, 2);
            buttons_to_show[1].Add("e" + Convert.ToString(edgeiii.Item1)
                + Convert.ToString(edgeiii.Item2)
                + Convert.ToString(edgeiii.Item3), e001);
        }
        public void check_texture_info()
        {

            Texture2D texture = textures_to_load["texture_test"];
            int length = texture.Width * texture.Height;
            Rectangle r = new Rectangle(new Point(100, 300), new Point(100, 100));
            Button c = new Button(r, texture, literally_nothing, "rectangle", new string[] { "normal" });
            buttons_to_show[1].Add("c", c);
            Color[] data = new Color[length];
            texture.GetData(data);

            declear("Width: " + Convert.ToString(texture.Width) +
                "\n" + "Height: " + Convert.ToString(texture.Height) +
                "\n" + "10's color: " + Convert.ToString(data[10]) +
                "\n" + "70's color: " + Convert.ToString(data[70]));

        }
        public void check_clock_wise()
        {
            Point p1 = new Point(100, 100);
            Point p2 = new Point(200, 100);
            Point p3 = new Point(100, 200);
            Point p4 = new Point(200, 200);
            declear(Convert.ToString(Button.check_if_clock_wise(p1, p2, p3))
                + ", " + Convert.ToString(Button.check_if_clock_wise(p1, p2, p4))
                + ", " + Convert.ToString(Button.check_if_clock_wise(p1, p3, p2)));

            Button b = new Button(this, new Point[] { p1, p2, p3 }, copy_texture(textures_to_load["white_rec"]), literally_nothing, new string[] { "normal" });
            buttons_to_show[1].Add("b", b);

            Button c = new Button(new Rectangle(new Point(100, 300), new Point(100, 100)), textures_to_load["white_rec"], literally_nothing, "rectangle", new string[] { "normal" });
            buttons_to_show[1].Add("c", c);

        }
        private void build_the_map(HashSet<(int, int)> cells)
        {
            gmap = new GraphicMap(textures_to_load, cells, this);
            gmap.add_cell_self_buttons_stage1(100, new Point(400, 300));
            //gmap.cells[(0, 0)].add_resource("tree", textures_to_load["tree"]);
            gmap.cells[(0, 0)].add_cube_num(5, textures_to_load["5"], gmap.step_size / 4);
            gmap.cells[(0, 1)].add_cube_num(2, textures_to_load["2"], gmap.step_size / 4);
            gmap.cells[(0, 2)].add_cube_num(3, textures_to_load["3"], gmap.step_size / 4);
            gmap.cells[(1, 0)].add_cube_num(4, textures_to_load["4"], gmap.step_size / 4);
            gmap.cells[(1, 1)].add_cube_num(6, textures_to_load["6"], gmap.step_size / 4);
            gmap.cells[(2, -1)].add_cube_num(8, textures_to_load["8"], gmap.step_size / 4);
            gmap.cells[(2, 0)].add_cube_num(9, textures_to_load["9"], gmap.step_size / 4);
            gmap.cells[(2, 1)].add_cube_num(10, textures_to_load["10"], gmap.step_size / 4);
            gmap.cells[(1, 2)].add_cube_num(11, textures_to_load["11"], gmap.step_size / 4);
            gmap.cells[(2, 2)].add_cube_num(12, textures_to_load["12"], gmap.step_size / 4);
        }

        public void draw_2(Point p, int r)
        {
            Rectangle rec2 = new Rectangle(new Point(p.X - r, p.Y -r), new Point(2*r, 2*r));
            Button num2 = new Button(rec2, textures_to_load["2"], SaveExit, "circle", new string[] { "normal" },2);
            buttons_to_show[1].Add("num2" + Convert.ToString(p), num2);
        }
        public void change_font_size(string font_name, int new_size)
        {
            string file_name = @"~\..\..\..\..\Content\" + font_name + ".spritefont";
            string text = File.ReadAllText(file_name);
            string[] lines = text.Split("\n");
            string size_line = lines[19];
            //declear(text);
            lines[19] = size_line.Substring(0, 10) + Convert.ToString(new_size) + size_line.Substring(size_line.Length - 7, 7);
            string all_text = string.Join("\n", lines);

            using (StreamWriter outputFile = new StreamWriter(file_name))
            {
                outputFile.WriteLine(all_text);
            }
            SpriteFont_to_load[font_name] = Content.Load<SpriteFont>(font_name);
            declear(file_name,"\n", font_name);
        }
    }
}
