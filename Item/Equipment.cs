using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.Item
{
    public class Equipment
    {
        public Type ItemType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stat { get; set; }
        public int Price { get; set; }

        public enum Type
        {
            Weapon,
            Armor,
            Shoes
        }

        public Equipment() { }
        public Equipment(Equipment equip)
        {
            this.ItemType = equip.ItemType;
            this.Name = equip.Name;
            this.Description = equip.Description;
            this.Price = equip.Price;
            this.Stat = equip.Stat;
        }
    }
}
