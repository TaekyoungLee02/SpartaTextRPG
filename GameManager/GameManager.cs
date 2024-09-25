using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Character;
using TextRPG.Shop;

namespace TextRPG.GameManager
{
    internal class GameManager
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameManager();
                }
                return _instance;
            }
            private set { }
        }

        private BattleManager battleManager;
        private Shop.Shop shop;
        private DataManager dataManager;
        private SoundManager soundManager;
        private Player player;


        private GameManager()
        {
            battleManager = BattleManager.Instance;
            shop = Shop.Shop.Instance;
            dataManager = DataManager.Instance;
            soundManager = SoundManager.Instance;
            shop.SetShopList();
        }

        public void GameStart() 
        {
            soundManager.StartMusic(SoundManager.MusicType.Main);
            Opening();
            CharacterCreate();
            GameLoop();
        }

        private void GameLoop()
        {
            while (true)
            {
                int input = MainScene();

                switch(input)
                {
                    case 1:
                        StatusDisplay();
                        break;

                    case 2:
                        InventoryDisplay();
                        break;

                    case 3:
                        BagDisplay();
                        break;

                    case 4:
                        ShopDisplay();
                        break;

                    case 5:
                        DungeonEnter();
                        break;

                    case 6:
                        Rest();
                        break;

                    case 7:
                        SaveData();
                        break;

                    case 0:
                        player.Gold += 500;
                        break;
                }
            }
        }

        private void Opening()
        {
            Console.Clear();
            Console.Write(" _______  _______  _______  ______    _______  _______               \r\n" +
                          "|       ||       ||   _   ||    _ |  |       ||   _   |              \r\n" +
                          "|       ||       ||   _   ||    _ |  |       ||   _   |              \r\n" +
                          "|  _____||    _  ||  |_|  ||   | ||  |_     _||  |_|  |              \r\n" +
                          "| |_____ |   |_| ||       ||   |_||_   |   |  |       |              \r\n" +
                          "|_____  ||    ___||       ||    __  |  |   |  |       |              \r\n" +
                          " _____| ||   |    |   _   ||   |  | |  |   |  |   _   |              \r\n" +
                          "|_______||___|    |__| |__||___|  |_|  |___|  |__| |__|              \r\n" +
                          "       ______   __   __  __    _  _______  _______  _______  __    _ \r\n" +
                          "      |      | |  | |  ||  |  | ||       ||       ||       ||  |  | |\r\n" +
                          "      |  _    ||  | |  ||   |_| ||    ___||    ___||   _   ||   |_| |\r\n" +
                          "      | | |   ||  |_|  ||       ||   | __ |   |___ |  | |  ||       |\r\n" +
                          "      | |_|   ||       ||  _    ||   ||  ||    ___||  |_|  ||  _    |\r\n" +
                          "      |       ||       || | |   ||   |_| ||   |___ |       || | |   |\r\n" +
                          "      |______| |_______||_|  |__||_______||_______||_______||_|  |__|\r\n\n\n" + 
                          "계속하려면 엔터를 눌러주십시오...");
            Console.ReadLine();
        }

        private void CharacterCreate()
        {
            if(dataManager.Load(out Player tempPlayer, out bool[] tempShopData))
            {
                string dataSelect;

                Console.Clear();
                Console.Write("세이브 데이터가 존재합니다.\n" +
                              "세이브 데이터로 진행하시겠습니까?\n\n" +
                              "[세이브 데이터]\n");

                string[] status = tempPlayer.StatusInfo();
                Console.ForegroundColor = ConsoleColor.Red;
                Point p = new Point(20, 9);
                for (int i = 0; i < 3; i++)
                {
                    p.Draw("+ " + tempPlayer.Equipment[i].Stat);
                    p._y++;
                }
                Console.ResetColor();

                p = new Point(0, 4);
                for (int i = 0; i < Point.Y; i++)
                {
                    p.Draw(status[i]);
                    p._y++;
                }

                Console.Write("1. 세이브 데이터로 진행한다.\r\n" +
                              "2. 세이브 데이터를 삭제하고 새 데이터로 진행한다.\r\n" +
                              "\r\n" +
                              "원하시는 행동을 입력해주세요.\r\n" +
                              ">> ");
                dataSelect = Console.ReadLine();

                if (int.TryParse(dataSelect, out int result) && result > 0 && result < 3)
                {
                    if (result == 1)
                    {
                        player = tempPlayer;
                        shop.EquipSold = tempShopData;
                        return;
                    }
                    else dataManager.DeleteSaveData();
                }
            }

            string className;
            string name;
            Class playerClass;


                Console.Clear();
                Console.Write("당신은 스파르타 던전에 가기 위해 스파르타 마을을 향합니다.\r\n" +
                              "당신의 이름은 무엇입니까?\r\n" +
                              "\r\n" +
                              "\r\n" +
                              ">> ");
                name = Console.ReadLine();

            while (true)
            {
                Console.Clear();
                Console.Write("당신의 이름은 " +  name + "이군요.\n\n" +
                              "이번에는 직업을 정해야 합니다.\n" +
                              "\r\n" +
                              "1. 전사\r\n" +
                              "2. 도적\r\n" +
                              "3. 성기사\r\n" +
                              "4. 궁수\r\n" +
                              "\r\n" +
                              "당신은 어떤 직업입니까?\r\n" +
                              ">> ");
                className = Console.ReadLine();

                if(!(int.TryParse(className, out int value) && value > 0 && value < 5))
                {
                    Console.Write("다시 입력해 주십시오...");
                    Console.ReadLine();
                    continue;
                }

                switch(value)
                {
                    case 1:
                        playerClass = Class.WARRIOR;
                        className = "전사";
                        break;
                    case 2:
                        playerClass = Class.ROGUE;
                        className = "도적";
                        break;
                    case 3:
                        playerClass = Class.PALADIN;
                        className = "성기사";
                        break;
                    case 4:
                        playerClass = Class.ARCHER;
                        className = "궁수";
                        break;
                    default:
                        playerClass = Class.WARRIOR;
                        className = "";
                        break;
                }

                break;
            }

            player = new Player(name, playerClass);

            Console.Clear();
            Console.Write(className + " " + name + " 용사는 스파르타 마을에 도착했습니다.\n\n" + 
                          "계속하려면 엔터를 눌러주십시오...");
            Console.ReadLine();
        }

        private int MainScene()
        {
            while (true)
            {
                string input;

                Console.Clear();
                Console.Write("스파르타 마을에 오신 여러분 환영합니다.\r\n" +
                              "이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\r\n" +
                              "\r\n" +
                              "\r\n" +
                              "1. 상태 보기\r\n" +
                              "2. 인벤토리 (장비)\r\n" +
                              "3. 가방 (물약)\r\n" +
                              "4. 상점\r\n" +
                              "5. 던전 입장\r\n" +
                              "6. 휴식하기\r\n" +
                              "7. 저장하기\r\n" +
                              "\r\n" +
                              "원하시는 행동을 입력해주세요.\r\n" +
                              ">> ");
                input = Console.ReadLine();

                if (int.TryParse(input, out int result) && result > -1 && result < 8) return result;
                else
                {
                    Console.Write("다시 입력해 주십시오...");
                    Console.ReadLine();
                }
            }
        }

        private void StatusDisplay()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("상태 보기");
            Console.ResetColor();
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");

            string[] status = player.StatusInfo();
            Console.ForegroundColor = ConsoleColor.Red;
            Point p = new Point(20, 7);
            for (int i = 0; i < 3; i++)
            {
                p.Draw("+ " + player.Equipment[i].Stat);
                p._y++;
            }
            Console.ResetColor();

            p = new Point(0, 2);
            for (int i = 0; i < Point.Y; i++)
            {
                p.Draw(status[i]);
                p._y++;
            }

            p = new Point(0, 13);
            p.Draw("돌아가려면 엔터를 눌러주세요...");
            Console.ReadLine();
        }

        private void InventoryDisplay()
        {
            while(true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("인벤토리");
                Console.ResetColor();
                Console.WriteLine("보유 중인 장비를 관리할 수 있습니다.\n\n[장비 목록]\n\n");

                Point p = new Point(25, 6);

                foreach (var equip in player.Inventory)
                {
                    string equiping = "";
                    string stat = "";

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

                    Console.WriteLine(" - " + equiping + equip.Name);
                    p.Draw("|  " + stat + " +" + equip.Stat);
                    p._x += 15;
                    p.Draw("|  " + equip.Description + "\n");

                    p._x -= 15;
                    p._y++;
                }

                string input;

                Console.WriteLine("");
                Console.Write(
                    "1. 장착 관리\r\n" +
                    "2. 나가기\r\n" +
                    "\r\n" +
                    "원하시는 행동을 입력해주세요.\r\n" +
                    ">> ");
                input = Console.ReadLine();

                if (int.TryParse(input, out int result) && result > 0 && result < 3)
                {
                    if (result == 1)
                    {
                        EquipmentManage();
                    }
                    else return;
                }
                else
                {
                    Console.Write("다시 입력해 주십시오...");
                    Console.ReadLine();
                }
            }
        }

        private void EquipmentManage()
        {
            while(true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("인벤토리 - 장착 관리");
                Console.ResetColor();
                Console.WriteLine("보유 중인 장비를 관리할 수 있습니다.\n\n[장비 목록]\n\n");

                Point p = new Point(25, 6);
                int index = 1;

                foreach (var equip in player.Inventory)
                {
                    string equiping = "";
                    string stat = "";

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
                    p.Draw("|  " + equip.Description + "\n");

                    p._x -= 15;
                    p._y++;
                }

                string input;

                Console.WriteLine("");
                Console.Write(
                    "0. 나가기\r\n" +
                    "\r\n" +
                    "원하시는 행동을 입력해주세요.\r\n" +
                    ">> ");
                input = Console.ReadLine();

                if (int.TryParse(input, out int result) && result > -1 && result < player.Inventory.Count + 1)
                {
                    if (result == 0) return;

                    int type = (int)player.Inventory[result - 1].ItemType;

                    if (player.Inventory[result - 1].Name == player.Equipment[type].Name) player.Equipment[type] = new Item.Equipment();
                    else player.Equipment[type] = player.Inventory[result - 1];
                }
                else
                {
                    Console.Write("다시 입력해 주십시오...");
                    Console.ReadLine();
                }
            }
        }

        private void BagDisplay()
        {
            while(true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("가방");
                Console.ResetColor();
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n\n[아이템 목록]\n\n");

                Point p = new Point(25, 6);
                int index = 1;

                foreach (var item in player.Bag)
                {
                    Console.WriteLine(" - " + index++ + ". " + item.Name);
                    p.Draw("|  " + item.Amount + "개");
                    p._x += 10;
                    p.Draw("|  " + item.Heal + " 회복\n");

                    p._x -= 10;
                    p._y++;
                }

                string input;

                Console.WriteLine("");
                Console.Write(
                    "0. 나가기\r\n" +
                    "\r\n" +
                    "원하시는 행동을 입력해주세요.\r\n" +
                    ">> ");
                input = Console.ReadLine();

                if (int.TryParse(input, out int result) && result > -1 && result < player.Bag.Count + 1)
                {
                    if (result == 0) return;

                    if (player.CurHP == player.MaxHP)
                    {
                        Console.Write("HP가 이미 가득 차 있습니다...");
                        Console.ReadLine();
                        continue;
                    }

                    player.UseItem(result - 1);
                }
                else
                {
                    Console.Write("다시 입력해 주십시오...");
                    Console.ReadLine();
                }
            }
        }

        private void ShopDisplay()
        {
            soundManager.StopMusic();
            soundManager.StartMusic(SoundManager.MusicType.Shop);
            shop.EnterShop(player);
            soundManager.StopMusic();
            soundManager.StartMusic(SoundManager.MusicType.Main);
        }

        private void DungeonEnter()
        {
            while (true)
            {
                string input;

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("던전 입장");
                Console.ResetColor();
                Console.Write("여기서 던전에 입장할 수 있습니다.\n\n1. 쉬운 던전 | 보상 : 700 G\n2. 일반 던전 | 보상 : 1000 G\n3. 어려운 던전 | 보상 : 1500 G\n0. 나가기\n\n원하시는 행동을 입력해주세요.\n>> ");

                input = Console.ReadLine();

                if (int.TryParse(input, out int result) && result > -1 && result < 4)
                {
                    if (result == 0) return;

                    if (player.CurHP == 0)
                    {
                        Console.Write("체력이 없습니다. 마을에서 회복하거나 회복약을 쓴 후 다시 도전해 주십시오.");
                        Console.ReadLine();
                        continue;
                    }

                    int enemyLevel = 5;
                    int gold = 0;

                    switch (result)
                    {
                        case 1:
                            enemyLevel = player.Level;
                            gold = 700;
                            break;
                        case 2:
                            enemyLevel = player.Level + 1;
                            gold = 1000;
                            break;
                        case 3:
                            enemyLevel = player.Level + 3;
                            gold = 1500;
                            break;
                    }

                    Console.Clear();

                    battleManager.SetBattle(player, new Enemy("적", enemyLevel));

                    soundManager.StopMusic();
                    soundManager.StartMusic(SoundManager.MusicType.Battle);
                    int battleResult = battleManager.StartBattle();
                    soundManager.StopMusic();
                    soundManager.StartMusic(SoundManager.MusicType.Main);

                    if (battleResult == 0) player.Gold += gold;
                }
                else
                {
                    Console.Write("다시 입력해 주십시오...");
                    Console.ReadLine();
                }
            }
        }

        private void Rest()
        {
            while(true)
            {
                string input;

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("휴식하기");
                Console.ResetColor();
                Console.Write("500 G를 내면 체력을 회복할 수 있습니다.\n\n[보유 골드]\n" + player.Gold + " G\n\n1. 휴식하기\n2. 나가기\n\n원하시는 행동을 입력해주세요.\n>> ");

                input = Console.ReadLine();

                if (int.TryParse(input, out int result) && result > 0 && result < 3)
                {
                    if (result == 2) return;

                    if (player.Gold < 500)
                    {
                        Console.Write("골드가 부족합니다...");
                        Console.ReadLine();
                        continue;
                    }

                    player.Gold -= 500;

                    player.CurHP = player.MaxHP;

                    Console.Write("편하게 휴식했습니다. 체력이 전부 회복됩니다...");
                    Console.ReadLine();
                }
                else
                {
                    Console.Write("다시 입력해 주십시오...");
                    Console.ReadLine();
                }
            }
        }

        private void SaveData()
        {
            try
            {
                dataManager.InitSave(player, shop.EquipSold);
                dataManager.Save();
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("저장에 실패했습니다...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("저장에 성공했습니다!");
            Console.ReadLine();
        }
    }
}