using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TextRPG.Character
{
    internal class Enemy : Character
    {
        public Enemy(string name, int level) : base()
        {
            int levelCoeff = level -= 5;
            int power = 5;

            Name = name;
            Level = 5 + levelCoeff;
            MaxHP = 10 + (levelCoeff * power);
            CurHP = 10 + (levelCoeff * power);
            Attack = 10 + (levelCoeff * power);
            Defense = 10 + (levelCoeff * power);
            Speed = 10 + (levelCoeff * power);

            Portait = new string[] { "" };
        }

        public override string[] StatusInfo()
        {
            string[] temp = new string[]
            {
                "",
                "   이름 : " + Name,
                "   레벨 : " + Level,
                "   HP : " + CurHP + " / " + MaxHP,
                "",
                "",
                "",
                "",
                "",
                "",
                ""
            };

            return temp;
        }
    }
}
