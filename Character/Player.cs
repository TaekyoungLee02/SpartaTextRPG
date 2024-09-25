using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.Character
{
    public class Player : Character
    {
        public int[] StatRate { get; set; }
        public Class Class { get; set; }
        public int Gold { get; set; }

        public Player(string name, Class playerClass) : base()
        { 
            Class = playerClass;
            Name = name;
            Level = 5;
            MaxHP = 10;
            CurHP = 10;
            Attack = 10;
            Defense = 10;
            Speed = 10;
            Gold = 1500;

            switch (playerClass)
            {
                case Class.WARRIOR:
                    StatRate = new int[] { 6, 7, 6, 4 };
                    MaxHP = 11;
                    CurHP = 11;
                    Attack = 11;
                    Defense = 11;
                    break;

                case Class.ROGUE:
                    StatRate = new int[] { 4, 7, 4, 8 };
                    Speed = 13;
                    break;

                case Class.PALADIN:
                    StatRate = new int[] { 7, 5, 8, 3 };
                    MaxHP = 12;
                    CurHP = 12;
                    Defense = 12;
                    break;

                case Class.ARCHER:
                    StatRate = new int[] { 4, 9, 4, 6 };
                    Attack = 13;
                    break;
            }

            Portait = new string[]{"              ######                \n",
                                   "           =*****!!~~:;             \n",
                                   "          @*********;~@@            \n",
                                   "          @@$****$$@@==             \n",
                                   "           @@@@@@@@@=.=             \n",
                                   "           @@@@@@@~@@~              \n",
                                   "         -*****;=@=.:-              \n",
                                   "         @@@===**:@~  =;  @         \n",
                                   "        *~@*. $**#~~~$-    !        \n",
                                   "         @**!!=*$=@!=:-...!~        \n",
                                   "          $$$$$$$~=  !$$            \n" };
        }

        public Player() : base()
        {

        }

        public void LevelUp()
        {
            Random random = new Random();
            Level++;

            for(int i = 0; i < StatRate.Length; i++)
            {
                int up = random.Next(1, 11);
                if (up < StatRate[i]) up = 4;
                else up = 1;

                switch(i)
                {
                    case 0: MaxHP += up; CurHP += up; break;
                    case 1: Attack += up; break;
                    case 2: Defense += up; break;
                    case 3: Speed += up; break;
                }
            }
        }

        public void Refresh()
        {
            CurHP = MaxHP;
        }

        public override string[] StatusInfo()
        {
            string playerClass = "";
            switch(Class)
            {
                case Class.WARRIOR: playerClass = "전사"; break;
                case Class.ROGUE: playerClass = "도적"; break;
                case Class.PALADIN: playerClass = "성기사"; break;
                case Class.ARCHER: playerClass = "궁수"; break;
            }

            string[] temp = new string[]
            {
                "",
                "   이름 : " + Name,
                "   직업 : " + playerClass,
                "   레벨 : " + Level,
                "   HP : " + CurHP + " / " + MaxHP,
                "   공격력 : " + Attack,
                "   방어력 : " + Defense,
                "   스피드 : " + Speed,
                "   장비 : " + Equipment[0].Name + " | " +  Equipment[1].Name + " | " + Equipment[2].Name,
                "",
                ""
            };

            return temp;
        }
    }
}
