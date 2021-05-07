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
            "plus", "minus", "minus_red", "continue", "gray_hex", "blue_hex", "yellow_hex",
            "num_circle"});

        private Dictionary<string, Texture2D> textures_to_load = new Dictionary<string, Texture2D>();
        private Dictionary<string, Button>[] buttons_to_show = new Dictionary<string, Button>[3];
        private string activity_phase = "normal"; // normal, decleration
        private Dictionary<string, Button> completly_change_of_buttons = null;
        //private Dictionary<string, GraphicMap> maps_to_show = new Dictionary<string, GraphicMap>();

        private List<string> names_of_SpriteFont_to_load = new List<string>(new string[] { "title_font", "24font" });
        private Dictionary<string, SpriteFont> SpriteFont_to_load = new Dictionary<string, SpriteFont>();
        private Dictionary<string, GraphicText>[] text_to_show = new Dictionary<string, GraphicText>[3];
        private Dictionary<string, GraphicText> completly_change_of_texts = null;

        private Dictionary<string, int> nums_to_remember = new Dictionary <string ,int>();

        private Button text_field_that_now = new Button();
        private bool is_writing_now = false;
        private Keys[] keys_that_prresed_copy = new Keys[0];
        

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


            /*if (player == null)
            {
                menu_no_one();
            }
            else
            {
                menu_someone(player);
            }*/
            //lern_draw_gray_cells();
            draw_2();
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
            

            
           

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront);
             
            foreach (var button in buttons_to_show[0].Values)
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



            foreach (var gtext in text_to_show[0].Values)
            {
                
                spriteBatch.DrawString(gtext.font, gtext.text, gtext.location, gtext.txtColor, 0, new Vector2(0, 0), 1, 0, layer_func(gtext.layer));
            }
           
            spriteBatch.End();

            base.Draw(gameTime);
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
            nums_to_remember.Add("map size", 5);

            GraphicText title = new GraphicText(SpriteFont_to_load["title_font"], "Map Creation", new Vector2(700, 100), Color.Black);
            completly_change_of_texts.Add("title", title);

            GraphicText hello = new GraphicText(SpriteFont_to_load["24font"], "Size of map: " + Convert.ToString(nums_to_remember["map size"]) , new Vector2(700, 400), Color.Black);
            completly_change_of_texts.Add("hello", hello);


        }
        private void lern_draw_gray_cells()
        {
            //logic_map lmap = new logic_map(new HashSet<(int, int, string)>(){ (0, 0, ""), (0, 1, ""), (1, 0, ""), (1, 1, ""), (2,-1 , "") , (2, 0, "") , (2, 1, "") });
            HashSet<(int, int, string)> cells = new HashSet<(int, int, string)>() { (0, 0, ""), (0, 1, ""), (1, 0, ""), (1, 1, ""), (2, -1, ""), (2, 0, ""), (2, 1, "") };
            GraphicMap gmap = new GraphicMap(cells);
            Dictionary<string, Texture2D> texture_dict = new Dictionary<string, Texture2D>();
            texture_dict.Add("gray_hex", textures_to_load["gray_hex"]);
            texture_dict.Add("yellow_hex", textures_to_load["yellow_hex"]);
            texture_dict.Add("blue_hex", textures_to_load["blue_hex"]);
            gmap.Add_button_cells( 100, new Point (200,200), texture_dict);
            foreach (Button button in gmap.cell_buttons)
            {
                buttons_to_show[1].Add(button.rectangle.X.ToString() + "," + button.rectangle.Y.ToString(), button);
            }
        }
        public void draw_2()
        {
            Rectangle rec = new Rectangle(new Point(50, 50), new Point(180, 180));
            Button num2 = new Button(rec, textures_to_load["num_circle"], SaveExit, "rectangel", new string[] { "normal" });
            buttons_to_show[1].Add("num2", num2);
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
        public void login(string user_name)
        {
            
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
        public void declear(string what_declear)
        {
            activity_phase = "declear";
            Rectangle declear_rec = new Rectangle(new Point(500, 300), new Point(700, 480));
            Button declear = new Button(declear_rec, textures_to_load["yellow"], literally_nothing, "rectangel", new string[] { "declear" }, 2);
            buttons_to_show[1].Add("declear", declear);
            Rectangle ok_rec = new Rectangle(new Point(800, 680), new Point(100, 100));
            Button ok = new Button(ok_rec, textures_to_load["ok"], when_ok, "rectangel", new string[] { "declear" }, 3);
            buttons_to_show[1].Add("ok", ok);
            GraphicText declear_text = new GraphicText(SpriteFont_to_load["24font"], what_declear , new Vector2(500, 300), Color.Black, 3);
            text_to_show[1].Add("declear_text", declear_text);

        }
        public void when_ok()
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

    }
}
