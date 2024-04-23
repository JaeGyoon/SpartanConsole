using System;
using System.Text;
using static JG.Program;

namespace JG
{
    internal class Program
    {
        enum PlayerActionType
        {
            Quit = 0,
            ViewStatus = 1,
            Inventory = 2,
            OpenShop = 3,
        }

        enum ItemType
        {
            Armor = 0,
            Weapon = 1,
        }

        enum ItemName
        {
            NoviceArmor = 1,
            IronArmor = 2,
            spartanArmor = 3,

            oldSword = 4,
            bronzeAxe = 5,
            spartanSpear = 6
        }
        


        public class Item
        {
            public string? ItemName { get; set; }
            public int ItemOffense { get; set; }
            public int ItemDefense { get; set; }

            public string? ItemDesc { get; set; }

            public int ItemValue { get; set; }

            public int ItemType { get; set; }



            public Item(string itemName, int itemOffense, int itemDefense, string itemDesc, int itemValue, int itemType)
            {
                ItemName = itemName;
                ItemOffense = itemOffense;
                ItemDefense = itemDefense;
                ItemDesc = itemDesc;
                ItemValue = itemValue;
                ItemType = itemType;
            }

        }

        public class Shop
        {
            public List<Item> itemList = new List<Item>();

            public Shop() 
            {
                Item noviceArmor = new Item("수련자 갑옷", 0, 5, "수련에 도움을 주는 갑옷입니다.", 1000, (int)ItemType.Armor);
                Item ironArmor = new Item("무쇠 갑옷", 0, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 1500, (int)ItemType.Armor);
                Item spartanArmor = new Item("스파르타의 갑옷", 0, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500, (int)ItemType.Armor);

                Item oldSword = new Item("낡은 검", 2, 0, "쉽게 볼 수 있는 낡은 검 입니다.", 600, (int)ItemType.Weapon);
                Item bronzeAxe = new Item("청동 도끼", 5, 0, "어디선가 사용됐던거 같은 도끼입니다.", 1500, (int)ItemType.Weapon);
                Item spartanSpear = new Item("스파르타의 창", 7, 0, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2000, (int)ItemType.Weapon);

                itemList.Clear();

                itemList.Add(noviceArmor);
                itemList.Add(ironArmor);
                itemList.Add(spartanArmor);
                itemList.Add(oldSword);
                itemList.Add(bronzeAxe);
                itemList.Add(spartanSpear);

            }

        }

        public class Player
        {
            public int PlayerLevel { get; set; }
            public string PlayerName { get; set; }
            public string PlayerClass { get; set; }
            public int PlayerOffense { get; set; }
            public int PlayerDefense { get; set; }
            public int PlayerHP { get; set; }
            public int PlayerGold { get; set; }
            public List<Item> inventory = new List<Item>();


            public Player()
            {
                PlayerLevel = 1;
                PlayerName = "스파르탄";
                PlayerClass = "코딩전사";
                PlayerOffense = 10;
                PlayerDefense = 5;
                PlayerHP = 100;
                PlayerGold = 1500;
            }

        }

        

        static void Main(string[] args)
        {    
            // 플레이어 생성
            Player player = new Player();

            Shop shop = new Shop();            
                                 
            ActionSelect(player);



            void ActionSelect(Player player)
            {
                // 게임 시작시 간단한 소개 말과 마을에서 할 수 있는 행동을 알려줍니다.
                Introduction();

                // 원하는 행동의 숫자를 타이핑하면 실행합니다. 
                // 1 ~3 이외 입력시 -잘못된 입력입니다 출력
                while (true)
                {
                    int parseNumber = ActionLoop();

                    if (parseNumber == (int)PlayerActionType.ViewStatus ||
                        parseNumber == (int)PlayerActionType.Inventory ||
                        parseNumber == (int)PlayerActionType.OpenShop)
                    {
                        PlayerAction(parseNumber, player);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
            }

            void Introduction()
            {
                StringBuilder stringBuilder = new StringBuilder("스파르타 마을에 오신 여러분 환영합니다. \n")
                                                    .AppendLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n")
                                                    .AppendLine("1. 상태 보기")
                                                    .AppendLine("2. 인벤토리")
                                                    .AppendLine("3. 상점");
                Console.WriteLine(stringBuilder);
            }

            int ActionLoop()
            {
                while (true)
                {
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    string inputString = Console.ReadLine();
                    int parseNumber = 0;

                    if (int.TryParse(inputString, out parseNumber))
                    {
                        return parseNumber;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
            }

            void PlayerAction(int parseNumber, Player player)
            {
                switch ((PlayerActionType)parseNumber)
                {
                    case PlayerActionType.ViewStatus:

                        ViewStatus(player);
                        //Console.WriteLine("플레이어 정보");
                        break;
                    case PlayerActionType.Inventory:
                        Console.WriteLine("인벤토리");
                        break;
                    case PlayerActionType.OpenShop:
                        OpenShop(player);
                        //Console.WriteLine("상점");
                        break;
                    default:
                        Console.WriteLine("플레이어 액션 예외처리");
                        break;
                }
            }

            void ViewStatus(Player player)
            {
                StringBuilder statusText = new StringBuilder("상태 보기\n")
                                                    .AppendLine("캐릭터의 정보가 표시됩니다.\n")
                                                    .AppendLine($"Lv.{player.PlayerLevel.ToString("D2")} ")
                                                    .AppendLine($"{player.PlayerName} ({player.PlayerClass})")
                                                    .AppendLine($"공격력: {player.PlayerOffense}")
                                                    .AppendLine($"방어력: {player.PlayerDefense}")
                                                    .AppendLine($"체  력: {player.PlayerHP}")
                                                    .AppendLine($"Gold: {player.PlayerGold} \n")
                                                    .AppendLine($"0. 나가기");


                Console.WriteLine(statusText);

                while (true)
                {
                    int parseNumber = ActionLoop();

                    if (parseNumber == (int)PlayerActionType.Quit)
                    {
                        ActionSelect(player);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
            }

            void OpenShop(Player player)
            {


                StringBuilder shopText = new StringBuilder("상점\n")
                                                    .AppendLine("필요한 아이템을 얻을 수 있는 상점입니다..\n")
                                                    .AppendLine("[보유 골드]")
                                                    .AppendLine($"{player.PlayerGold} G\n")
                                                    .AppendLine("[아이템 목록]");

                for (int i = 0; i < shop.itemList.Count; i++)
                {
                    shopText.Append($"{i + 1}.")
                            .Append($"{shop.itemList[i].ItemName}\t")
                            .Append($"{shop.itemList[i].ItemOffense}\t")
                            .Append($"{shop.itemList[i].ItemDefense}\t")
                            .Append($"{shop.itemList[i].ItemDesc}\t")
                            .AppendLine($"{shop.itemList[i].ItemValue}\t");
                }

                shopText.AppendLine($"\n0. 나가기");

                Console.WriteLine(shopText);


                while (true)
                {
                    int parseNumber = ActionLoop();

                    if (parseNumber == (int)ItemName.NoviceArmor  ||
                        parseNumber == (int)ItemName.IronArmor    ||
                        parseNumber == (int)ItemName.spartanArmor ||
                        parseNumber == (int)ItemName.oldSword ||
                        parseNumber == (int)ItemName.bronzeAxe ||
                        parseNumber == (int)ItemName.spartanSpear )
                    {
                        ItemBuy(parseNumber, player);                        
                    }
                    else if (parseNumber == (int)PlayerActionType.Quit)
                    {
                        ActionSelect(player);
                        break;
                    }

                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
            }

            void ItemBuy(int itemNumber, Player player)
            {
                int shopIndex = itemNumber - 1;

                bool hasExist = player.inventory.Contains(shop.itemList[shopIndex]);

                if ( hasExist )
                {
                    Console.WriteLine("이미 구매한 아이텝입니다.");
                }
                else if ( player.PlayerGold > shop.itemList[shopIndex].ItemValue)
                {
                    player.PlayerGold -= shop.itemList[shopIndex].ItemValue;
                    player.inventory.Add(shop.itemList[shopIndex]);

                    Console.WriteLine("구매를 완료했습니다.");
                }
                else
                {
                    Console.WriteLine("Gold가 부족합니다.");
                }
            }

        }

        
                
    }
}

