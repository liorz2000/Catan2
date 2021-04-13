using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Game1
{
     public class Member
    {

        public string user_name { get; set; }
        public string password_hash { get; set; }
        public int all_hp { get; set; }
        public int level { get; set; }
        public int hp_now { get; set; }
        public int num_of_games_that_played { get; set; }
        public int num_game_that_won { get; set; }

        public Member(string un, string pass_hash)
        {
            this.user_name = un;
            this.password_hash = pass_hash;
            this.all_hp = 0;
            this.level = 1;
            this.hp_now = 0;
            this.num_of_games_that_played = 0;
            this.num_game_that_won = 0;
        }
        public void add_hp(int new_hp)
        {
            all_hp += new_hp;
            hp_now += new_hp;
            while (hp_now >= 10 * level)
            {
                hp_now -= 10 * level;
                level += 1;
            }
        }

    }
}
