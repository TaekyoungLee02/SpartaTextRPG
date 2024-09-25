using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Item;

namespace TextRPG.Character
{
    public class Character
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int MaxHP { get; set; }
        public int CurHP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        public Item.Equipment[] Equipment { get; set; }
        public string[] Portait { get; set; }
        public List<Item.Item> Bag { get; set; }
        public List<Item.Equipment> Inventory { get; set; }

        protected Character() 
        {
            Equipment = new Item.Equipment[3];
            for (int i = 0; i < Equipment.Length; i++)
            {
                Equipment[i] = new Item.Equipment();
            }

            Bag = new List<Item.Item>();
            Inventory = new List<Item.Equipment>();
        }
        protected Character(string name, int level, int maxHP, int attack, int defense, int speed) : this()
        {
            Name = name;
            Level = level;
            MaxHP = maxHP;
            CurHP = maxHP;
            Attack = attack; 
            Defense = defense; 
            Speed = speed;
        }

        public int BasicAttack() { return Attack + Equipment[0].Stat; }
        public void UseItem(int number) 
        {
            Item.Item item = Bag[number];

            item.Amount--;
            if(item.Amount <= 0) Bag.Remove(item);

            CurHP += item.Heal;
            if(CurHP >= MaxHP) CurHP = MaxHP;
        }
        public virtual string[] StatusInfo() { return null; }
    }

    public enum Class
    {
        WARRIOR, // ATTACK DEFENSE SP
        ROGUE, // SPEED SP
        PALADIN, // DEFNSE SP
        ARCHER, // ATTACK SP
    }
}
