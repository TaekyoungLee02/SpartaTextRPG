using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Character;

namespace TextRPG.GameManager
{
    internal class BattleManager
    {
        private Character.Character[] characters;
        private readonly Point[] displayPoints;
        private Queue<Action> actionQueue;
        private Action playerAction;
        private Action enemyAction;
        private string[] narration;

        private static BattleManager _instance;
        public static BattleManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BattleManager();
                }
                return _instance;
            }
            private set { }
        }

        private BattleManager() 
        {
            characters = new Character.Character[2];
            displayPoints = new Point[] { new Point(0, 0), new Point(36, 0), new Point(0, 11), new Point(36, 11), new Point(0, 23) };
            actionQueue = new Queue<Action>();
            narration = new string[] { "" };
        }

        public int StartBattle()
        {
            bool end;
            int winner;
            actionQueue.Clear();

            do
            {
                end = BattleLoop(out winner);
            } while (!end);

            if (winner == 0)
            {
                SetNarration("승리했습니다! 레벨이 올랐습니다.");
                Console.Clear();
                BattleDisplay();
                Console.ReadLine();

                (characters[0] as Player).LevelUp();
            }
            else
            {
                SetNarration("패배했습니다... 던전 입구로 돌아갑니다. 체력을 회복하고 정비한 뒤 다시 시도해 주십시오.");
                Console.Clear();
                BattleDisplay();
                Console.ReadLine();
            }

            return winner;
        }

        private bool BattleLoop(out int winner)
        {
            Console.Clear();
            BattleDisplay();
            playerAction = GetPlayerCommand();
            enemyAction = GetEnemyCommand();

            if (characters[0].Speed + characters[0].Equipment[2].Stat > characters[1].Speed + characters[1].Equipment[2].Stat)
            {
                actionQueue.Enqueue(playerAction);
                actionQueue.Enqueue(enemyAction);
            }
            else
            {
                actionQueue.Enqueue(enemyAction);
                actionQueue.Enqueue(playerAction);
            }

            int damage = 0;

            for(int i = 0; i < 2; i ++)
            {
                var action = actionQueue.Dequeue();

                // calculating damage
                switch(action.behavior)
                {
                    case Action.Behavior.BASIC_ATTACK:
                        damage = characters[action.character].BasicAttack();

                        SetNarration(characters[action.character].Name + "의 공격!");
                        Console.Clear();
                        BattleDisplay();
                        Console.ReadLine();
                        break;

                    case Action.Behavior.ITEM:
                        characters[action.character].UseItem(action.num);
                        break;

                    case Action.Behavior.QUIT:
                        winner = -1;
                        return true;
                }

                // apply damage
                if(action.behavior != Action.Behavior.ITEM)
                {
                    int defender;
                    int defense;

                    if (action.character == 0) defender = 1;
                    else defender = 0;

                    defense = characters[defender].Defense + characters[defender].Equipment[1].Stat;

                    damage -= defense;

                    if (damage <= 0) damage = 1;
                    characters[defender].CurHP -= damage;

                    if (characters[defender].CurHP <= 0)
                    {
                        characters[defender].CurHP = 0;

                        SetNarrationDamage(damage, action.character);
                        Console.Clear();
                        BattleDisplay();
                        Console.ReadLine();

                        winner = action.character;
                        return true;
                    }

                    SetNarrationDamage(damage, action.character);
                    Console.Clear();
                    BattleDisplay();
                    Console.ReadLine();
                }
                else
                {
                    SetNarrationItem();
                    Console.Clear();
                    BattleDisplay();
                    Console.ReadLine();
                }
            }

            actionQueue.Clear();


            if (characters[0].CurHP <= 0)
            {
                winner = 1;
                return true;
            }
            else if (characters[1].CurHP <= 0)
            {
                winner = 0;
                return true;
            }
            else
            {
                winner = -1;
                return false;
            }
        }

        public void SetBattle(Character.Character player, Character.Character enemy)
        {
            characters[0] = player;
            characters[1] = enemy;
        }

        private void BattleDisplay(int point)
        {
            string[] display;

            switch (point)
            {
                case 0:
                    display = characters[1].StatusInfo(); break;
                case 1:
                    display = characters[1].Portait; break;
                case 2:
                    display = characters[0].Portait; break;
                case 3:
                    display = characters[0].StatusInfo(); break;
                case 4:
                    display = narration; break;
                default: display = null; break;
            }

            Point p = new Point(displayPoints[point]._x, displayPoints[point]._y);

            for (int i = 0; i < display.Length; i++)
            {
                p.Draw(display[i]);
                p._y++;
            }

            if(point == 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                p = new Point(56, displayPoints[point]._y + 4);

                for (int i = 0; i < 3; i++)
                {
                    p.Draw("+ " + characters[0].Equipment[i].Stat);
                    p._y++;
                }

                Console.ResetColor();
            }
        }
        private void BattleDisplay()
        {
            for (int i = 0; i < displayPoints.Length; i++)
            {
                BattleDisplay(i);
            }
        }

        private Action GetPlayerCommand()
        {
            string input;
            int behavior;

            while(true)
            {
                do
                {
                    narration = new string[]{
                        "   " + characters[0].Name + "(은)는 무엇을 할까?\n",
                        "\n",
                        "       1. 공격\n",
                        "       2. 가방\n",
                        "       3. 도망친다\n",
                        "\n",
                        "   원하시는 행동을 입력해주세요..."
                    };

                    Console.Clear();
                    BattleDisplay();
                    input = Console.ReadLine();
                } while (!int.TryParse(input, out behavior));

                if(behavior == 1)
                {
                    Action action = new Action
                    {
                        behavior = Action.Behavior.BASIC_ATTACK,
                        character = 0
                    };
                    return action;
                }
                else if (behavior == 2)
                {
                    if (characters[0].CurHP == characters[0].MaxHP)
                    {
                        Console.Write("\n HP가 이미 가득 차 있습니다...");
                        Console.ReadLine();
                        continue;
                    }

                    int item;
                    while(true)
                    {
                        narration = new string[characters[0].Bag.Count + 4];
                        do
                        {
                            narration[0] = "   " + characters[0].Name + "의 가방\n";
                            narration[1] = "\n";

                            for (int i = 0; i < characters[0].Bag.Count; i++)
                            {
                                narration[i + 2] = "       " + (i + 1) + ". " + characters[0].Bag[i].Name + "\n";
                            }

                            narration[narration.Length - 2] = "\n";
                            narration[narration.Length - 1] = "   사용할 아이템을 입력해주세요...";
                            Console.Clear();
                            BattleDisplay();

                            input = Console.ReadLine();
                        } while (!int.TryParse(input, out item));

                        if(item < 1 || item > characters[0].Bag.Count) continue;

                        Action action = new Action
                        {
                            behavior = Action.Behavior.ITEM,
                            num = item - 1,
                            character = 0
                        };
                        return action;
                    }
                }
                else if (behavior == 3)
                {
                    Action action = new Action
                    {
                        behavior = Action.Behavior.QUIT,
                        character = 0
                    };
                    return action;
                }
                else continue;
            }
        }

        private Action GetEnemyCommand()
        {
            Action action = new Action
            {
                behavior = Action.Behavior.BASIC_ATTACK,
                character = 1
            };
            return action;
        }

        private void SetNarration(string narration)
        {
            this.narration = new string[]{
                "   " + narration,
                "",
                "계속하려면 엔터 키를 누르십시오..."
            };
        }

        private void SetNarrationDamage(int damage, int character)
        {
            string temp;
            if (character == 0) temp = "적에게 " + damage + "데미지를 주었다!\n";
            else temp = damage + "데미지를 입었다!\n";

            narration = new string[]{
                "   " + temp,
                "",
                "계속하려면 엔터 키를 누르십시오..."
            };
        }

        private void SetNarrationItem()
        {

        }

        private struct Action
        {
            public enum Behavior
            {
                BASIC_ATTACK,
                ITEM,
                QUIT
            }

            public Behavior behavior;
            public int num;
            public int character;
        }
    }
}