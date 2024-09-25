using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.Item
{
    public class Item
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public int Heal { get; set; }
        public int Price { get; set; }

        public Item() { }
        public Item(Item item, int amount)
        {
            Name = item.Name;
            Amount = amount;
            Heal = item.Heal;
            Price = item.Price;
        }
    }
}
