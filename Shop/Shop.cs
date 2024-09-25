using System;
using System.Collections.Generic;
using TextRPG.Character;
using TextRPG.GameManager;
using TextRPG.Item;

namespace TextRPG.Shop
{
    public class Shop
    {
        private static Shop _instance;
        public static Shop Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Shop();
                }
                return _instance;
            }
            private set { }
        }

        private List<Equipment> EquipList { get; set; } //6
        private List<Item.Item> ItemList { get; set; } //2
        public bool[] EquipSold { get; set; }

        private Shop()
        {
            EquipList = new List<Equipment>();
            ItemList = new List<Item.Item>();
        }

        public void SetShopList()
        {
            SetEquipList();
            SetItemList();
        }

        private void SetEquipList()
        {
            EquipList.Add(new Equipment() { ItemType = Equipment.Type.Armor, Name = "수련자 갑옷", Description = "수련에 도움을 주는 갑옷입니다. ", Price = 1000, Stat = 5 });
            EquipList.Add(new Equipment() { ItemType = Equipment.Type.Armor, Name = "무쇠갑옷", Description = "무쇠로 만들어져 튼튼한 갑옷입니다.  ", Price = 1800, Stat = 9 });
            EquipList.Add(new Equipment() { ItemType = Equipment.Type.Armor, Name = "스파르타의 갑옷", Description = "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", Price = 3500, Stat = 15 });
            EquipList.Add(new Equipment() { ItemType = Equipment.Type.Weapon, Name = "낡은 검", Description = "쉽게 볼 수 있는 낡은 검 입니다.", Price = 600, Stat = 2 });
            EquipList.Add(new Equipment() { ItemType = Equipment.Type.Weapon, Name = "청동 도끼", Description = "어디선가 사용됐던거 같은 도끼입니다.", Price = 1500, Stat = 5 });
            EquipList.Add(new Equipment() { ItemType = Equipment.Type.Weapon, Name = "스파르타의 창", Description = "스파르타의 전사들이 사용했다는 전설의 창입니다.", Price = 2100, Stat = 7 });
            EquipList.Add(new Equipment() { ItemType = Equipment.Type.Shoes, Name = "너덜너덜한 운동화", Description = "누군가가 신었던 것 같은 고약한 냄새가 나는 운동화입니다.", Price = 400, Stat = 3 });
            EquipList.Add(new Equipment() { ItemType = Equipment.Type.Shoes, Name = "기능성 런닝화", Description = "바람을 느끼며 한결 빠르게 달릴 수 있게 해 주는 런닝화입니다.", Price = 1100, Stat = 7 });
            EquipList.Add(new Equipment() { ItemType = Equipment.Type.Shoes, Name = "스파르타의 군화", Description = "스파르타의 전사들이 사용했다는 전설의 군화입니다.", Price = 1700, Stat = 10 });
            EquipList.Add(new Equipment() { ItemType = Equipment.Type.Weapon, Name = "척준경의 대검", Description = "혈혈단신으로 천 명의 적도 베어넘길 수 있는 검입니다.", Price = 4200, Stat = 21 });
            EquipList.Add(new Equipment() { ItemType = Equipment.Type.Shoes, Name = "헤르메스의 전령", Description = "얼마나 먼 거리라도 한달음에 갈 수 있다고 알려진 신의 신발입니다.", Price = 3700, Stat = 28 });
            EquipList.Add(new Equipment() { ItemType = Equipment.Type.Armor, Name = "요동의 성벽", Description = "그 어떤 공격에도 꿈쩍 않는다고 전해지는 전설의 방어구입니다.", Price = 4500, Stat = 27 });
            EquipList.Add(new Equipment() { ItemType = Equipment.Type.Weapon, Name = "고주몽의 신화", Description = "고대의 힘으로 버무려진 신의 활입니다.", Price = 6600, Stat = 35 });

            EquipSold = new bool[EquipList.Count];
        }

        private void SetItemList()
        {
            ItemList.Add(new Item.Item() { Name = "회복약", Heal = 4, Price = 5 });
            ItemList.Add(new Item.Item() { Name = "중급 회복약", Heal = 10, Price = 10 });
            ItemList.Add(new Item.Item() { Name = "고급 회복약", Heal = 21, Price = 17 });
            ItemList.Add(new Item.Item() { Name = "엘릭서", Heal = 50, Price = 30 });
            ItemList.Add(new Item.Item() { Name = "신의 눈물", Heal = 100, Price = 45 });
        }

        public void EnterShop(Player player)
        {
            bool purchase = false;

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("상점");
                if (purchase) Console.WriteLine(" - 아이템 구매");
                else Console.WriteLine();
                Console.ResetColor();
                Console.WriteLine("필요한 아이템과 장비를 얻을 수 있는 상점입니다.\n\n[보유 골드]\n" + player.Gold + " G\n\n[장비 목록]");

                Point p = new Point(25, 7);

                int index = 1;
                string number = "";


                foreach (var equip in EquipList)
                {
                    string stat = "";
                    string sold = "";

                    if (EquipSold[index - 1] == true) sold = "판매 완료";
                    else sold = equip.Price + " G";

                    if (purchase) number = index + ". ";
                    index++;

                    switch (equip.ItemType)
                    {
                        case Item.Equipment.Type.Weapon:
                            stat = "공격력";
                            break;
                        case Item.Equipment.Type.Armor:
                            stat = "방어력";
                            break;
                        case Item.Equipment.Type.Shoes:
                            stat = "스피드";
                            break;
                    }

                    Console.WriteLine(" - " + number + equip.Name);
                    p.Draw("|  " + stat + " +" + equip.Stat);
                    p._x += 15;
                    p.Draw("|  " + equip.Description + " | " + sold + "\n");

                    p._x -= 15;
                    p._y++;
                }

                Console.WriteLine("\n[아이템 목록]");
                p._y += 2;

                foreach (var item in ItemList)
                {
                    if (purchase) number = index + ". ";
                    index++;

                    Console.WriteLine(" - " + number + item.Name);
                    p.Draw("|  " + item.Heal + " 회복");
                    p._x += 15;
                    p.Draw("|  " + item.Price + " G\n");

                    p._x -= 15;
                    p._y++;
                }

                string input;

                if (purchase)
                {
                    Console.WriteLine("");
                    Console.Write(
                        "0. 뒤로 가기\r\n" +
                        "\r\n" +
                        "구매하실 물건 번호를 입력해주세요.\r\n" +
                        ">> ");
                    input = Console.ReadLine();

                    if (int.TryParse(input, out int result) && result > -1 && result < EquipList.Count + ItemList.Count + 1)
                    {
                        int itemnum;

                        if (result == 0)
                        {
                            purchase = false;
                        }
                        
                        else if (result <= EquipList.Count)
                        {
                            itemnum = result - 1;

                            if (EquipSold[itemnum] == true)
                            {
                                Console.Write("이미 판매된 아이템입니다...");
                                Console.ReadLine();
                                continue;
                            }
                            if (EquipList[itemnum].Price > player.Gold)
                            {
                                Console.Write("골드가 부족합니다...");
                                Console.ReadLine();
                                continue;
                            }

                            player.Gold -= EquipList[itemnum].Price;
                            EquipSold[itemnum] = true;

                            player.Inventory.Add(new Equipment(EquipList[itemnum]));

                            Console.Write(EquipList[itemnum].Name + "(을)를 구매했습니다! 감사합니다!...");
                            Console.ReadLine();
                        }

                        else
                        {
                            itemnum = result - EquipList.Count - 1;

                            if (ItemList[itemnum].Price > player.Gold)
                            {
                                Console.Write("골드가 부족합니다...");
                                Console.ReadLine();
                                continue;
                            }

                            player.Gold -= ItemList[itemnum].Price;

                            Item.Item exist = null;

                            foreach(var bag in player.Bag)
                            {
                                if (bag.Name == ItemList[itemnum].Name) exist = bag;
                            }

                            if (exist != null) exist.Amount++;
                            else player.Bag.Add(new Item.Item(ItemList[itemnum], 1));

                            Console.Write(ItemList[itemnum].Name + "(을)를 구매했습니다! 감사합니다!...");
                            Console.ReadLine();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("");
                    Console.Write(
                        "1. 아이템 구매\r\n" +
                        "2. 아이템 판매\r\n" +
                        "0. 나가기\r\n" +
                        "\r\n" +
                        "원하시는 행동을 입력해주세요.\r\n" +
                        ">> ");
                    input = Console.ReadLine();

                    if (int.TryParse(input, out int result) && result > -1 && result < 3)
                    {
                        switch(result)
                        {
                            case 1:
                                purchase = true;
                                break;

                            case 2:
                                ShopSell(player);
                                break;
                            case 0:
                                return;
                            default: break;
                        }
                    }
                    else
                    {
                        Console.Write("다시 입력해 주십시오...");
                        Console.ReadLine();
                    }
                }
            }
        }

        private void ShopSell(Player player)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("상점 - 장비 판매");
                Console.ResetColor();
                Console.WriteLine("상점에서 자신의 장비를 팔 수 있습니다.\n\n[보유 골드]\n" + player.Gold + " G\n\n[장비 목록]");

                Point p = new Point(25, 7);

                int index = 1;
                int sellPrice;

                foreach (var equip in player.Inventory)
                {
                    string equiping = "";
                    string stat = "";
                    sellPrice = (int)(equip.Price * 0.85f);

                    switch (equip.ItemType)
                    {
                        case Item.Equipment.Type.Weapon:
                            if (equip.Name == player.Equipment[0].Name) equiping = "[E]";
                            stat = "공격력";
                            break;
                        case Item.Equipment.Type.Armor:
                            if (equip.Name == player.Equipment[1].Name) equiping = "[E]";
                            stat = "방어력";
                            break;
                        case Item.Equipment.Type.Shoes:
                            if (equip.Name == player.Equipment[2].Name) equiping = "[E]";
                            stat = "스피드";
                            break;
                    }

                    Console.WriteLine(" - " + index++ + ". " + equiping + equip.Name);
                    p.Draw("|  " + stat + " +" + equip.Stat);
                    p._x += 15;
                    p.Draw("|  " + equip.Description + " | " + sellPrice + " G\n");

                    p._x -= 15;
                    p._y++;
                }

                string input;

                Console.WriteLine("");
                Console.Write(
                    "0. 뒤로 가기\r\n" +
                    "\r\n" +
                    "판매하실 물건 번호를 입력해주세요.\r\n" +
                    ">> ");
                input = Console.ReadLine();

                if (int.TryParse(input, out int result) && result > -1 && result < player.Inventory.Count + 1)
                {
                    if (result == 0)
                    {
                        return;
                    }

                    sellPrice = (int)(player.Inventory[result - 1].Price * 0.85f);
                    player.Gold += sellPrice;

                    Console.Write(player.Inventory[result - 1].Name + "(을)를 판매했습니다! 잘 가...");

                    for(int i = 0; i < 3; i++)
                    {
                        if (player.Equipment[i].Name == player.Inventory[result - 1].Name) player.Equipment[i] = new Equipment();
                    }
                    player.Inventory.Remove(player.Inventory[result - 1]);
                    Console.ReadLine();
                }
                else
                {
                    Console.Write("다시 입력해 주십시오...");
                    Console.ReadLine();
                }
            }
        }
    }
}
