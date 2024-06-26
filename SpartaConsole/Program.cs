﻿using System.Text;
using Newtonsoft.Json;

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
            EnterTheDungeon = 4,
            Rest = 5,

            Equipment = 1,

            ItemBuy = 1,
            ItemSell = 2,

            
            ReTry = 1,
        }

        enum ItemType
        {
            Armor = 0,
            Weapon = 1,
            Helmet = 2,
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
        
        enum DunGeonType
        {
            Easy = 1,
            Normal = 2,
            Hard = 3,
        }


        public class Item
        {
            public string ItemName { get; set; }
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

            public Item()
            {

            }

        }

        public class Shop
        {
            public List<Item> itemList = new List<Item>();

            public Shop() 
            {
                Item noviceArmor = new Item("수련자 갑옷", 0, 5, "수련에 도움을 주는 갑옷입니다.", 1000, (int)ItemType.Armor);
                Item ironArmor = new Item("무쇠 갑옷", 0, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000, (int)ItemType.Armor);
                Item spartanArmor = new Item("스파르타 갑옷", 0, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500, (int)ItemType.Armor);

                Item oldSword = new Item("낡은 검", 2, 0, "쉽게 볼 수 있는 낡은 검 입니다.", 600, (int)ItemType.Weapon);
                Item bronzeAxe = new Item("청동 도끼", 5, 0, "어디선가 사용됐던거 같은 도끼입니다.", 1500, (int)ItemType.Weapon);
                Item spartanSpear = new Item("스파르타 창", 7, 0, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 3000, (int)ItemType.Weapon);

                Item spartanHelmet = new Item("스파르타 투구", 0, 9, "스파르타의 전사들이 사용했다는 전설의 투구입니다.", 3500, (int)ItemType.Helmet);

                itemList.Clear();

                itemList.Add(noviceArmor);
                itemList.Add(ironArmor);
                itemList.Add(spartanArmor);
                itemList.Add(oldSword);
                itemList.Add(bronzeAxe);
                itemList.Add(spartanSpear);
                itemList.Add(spartanHelmet);

            }

        }

        public class Player
        {
            public int PlayerLevel { get; set; }
            public string PlayerName { get; set; }
            public string PlayerClass { get; set; }
            public float PlayerOffense { get; set; }
            public float PlayerDefense { get; set; }
            public int PlayerHP { get; set; }
            public int PlayerGold { get; set; }

            public int PlayerMaximumHP { get; set; }


            public List<Item> inventory = new List<Item>();

            public Item[] equippedItem = new Item[Enum.GetNames(typeof(ItemType)).Length];

            public Player()
            {
                PlayerLevel = 1;
                PlayerName = "스파르탄";
                PlayerClass = "코딩전사";
                PlayerOffense = 10;
                PlayerDefense = 5;
                PlayerHP = 100;
                PlayerMaximumHP = 100;

                PlayerGold = 1500;
            }

        }

        

        static void Main(string[] args)
        {    
            // 플레이어 생성
            Player player = LoadDataCheck();

            //LoadDataCheck();

            Shop shop = new Shop();

            ItemSync();

                                 
            ActionSelect();

            void ActionSelect()
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
                        parseNumber == (int)PlayerActionType.OpenShop   ||
                        parseNumber == (int)PlayerActionType.EnterTheDungeon ||
                        parseNumber == (int)PlayerActionType.Rest)
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
                                                    .AppendLine("3. 상점")
                                                    .AppendLine("4. 던전 입장")
                                                    .AppendLine("5. 휴식하기");
                Console.WriteLine(stringBuilder);
            }

            int ActionLoop()
            {
                string saveData = JsonConvert.SerializeObject(player);
                File.WriteAllText("saveData.json", saveData);


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
                        OpenInventory(player);
                        //Console.WriteLine("인벤토리");
                        break;
                    case PlayerActionType.OpenShop:
                        OpenShop(player);
                        //Console.WriteLine("상점");
                        break;
                    case PlayerActionType.EnterTheDungeon:
                        EnterTheDunGeon();
                        break;
                    case PlayerActionType.Rest:
                        Rest();
                        break;
                    default:
                        Console.WriteLine("플레이어 액션 예외처리");
                        break;
                }
            }

            void ViewStatus(Player player)
            {
                int bonusOffense = 0;
                int bonusDefense = 0;

                for( int i = 0; i < player.equippedItem.Length; i++ )
                {
                    if (player.equippedItem[i] != null)
                    {
                        bonusOffense += player.equippedItem[i].ItemOffense;
                        bonusDefense += player.equippedItem[i].ItemDefense;
                    }
                }

                StringBuilder statusText = new StringBuilder("상태 보기\n")
                                                    .AppendLine("캐릭터의 정보가 표시됩니다.\n")
                                                    .AppendLine($"Lv.{player.PlayerLevel.ToString("D2")} ")
                                                    .AppendLine($"{player.PlayerName} ({player.PlayerClass})");

                if (bonusOffense > 0)
                {
                    statusText.AppendLine($"공격력: {player.PlayerOffense + bonusOffense} (+ {bonusOffense})");
                }
                else
                {
                    statusText.AppendLine($"공격력: {player.PlayerOffense + bonusOffense}");
                }

                if (bonusDefense > 0)
                {
                    statusText.AppendLine($"방어력: {player.PlayerDefense + bonusDefense} (+ {bonusDefense})");
                }
                else
                {
                    statusText.AppendLine($"방어력: {player.PlayerDefense + bonusDefense}");
                }



                /*.AppendLine($"공격력: {player.PlayerOffense + bonusOffense} (+ {bonusOffense})")
                .AppendLine($"방어력: {player.PlayerDefense + bonusDefense} (+ {bonusDefense})")*/
                statusText.AppendLine($"체  력: {player.PlayerHP}")
                                                    .AppendLine($"Gold: {player.PlayerGold} \n")
                                                    .AppendLine($"0. 나가기");


                Console.WriteLine(statusText);

                while (true)
                {
                    int parseNumber = ActionLoop();

                    if (parseNumber == (int)PlayerActionType.Quit)
                    {
                        ActionSelect();
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
                                                    .AppendLine("필요한 아이템을 얻을 수 있는 상점입니다.\n")
                                                    .AppendLine("[보유 골드]")
                                                    .AppendLine($"{player.PlayerGold} G\n")
                                                    .AppendLine("[아이템 목록]");

                for (int i = 0; i < shop.itemList.Count; i++)
                {
                    shopText/*.Append($"{i + 1}.")*/
                            .Append($"{shop.itemList[i].ItemName}\t")
                            .Append($"공격력 + {shop.itemList[i].ItemOffense}\t")
                            .Append($"방어력 + {shop.itemList[i].ItemDefense}\t")
                            .Append($"{shop.itemList[i].ItemDesc}\t");

                    if (player.inventory.Contains(shop.itemList[i]) )
                    {
                        shopText.AppendLine($"구매완료\t");
                    }
                    else
                    {
                        shopText.AppendLine($"가격: {shop.itemList[i].ItemValue} G\t");
                    }
                    

                    
                }
                shopText.AppendLine("");

                shopText.AppendLine($"1. 아이템 구매");
                shopText.AppendLine($"2. 아이템 판매");

                shopText.AppendLine($"0. 나가기");

                Console.WriteLine(shopText);


                while (true)
                {
                    int parseNumber = ActionLoop();
                                        
                    if (parseNumber == (int)PlayerActionType.ItemBuy)
                    {
                        ShopBuyMode();
                    }
                    else if (parseNumber == (int)PlayerActionType.ItemSell)
                    {
                        ShopSellMode();
                    }
                    else if (parseNumber == (int)PlayerActionType.Quit)
                    {
                        ActionSelect();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
            }

            void ShopBuyMode()
            {
                StringBuilder shopText = new StringBuilder("상점 - 아이템 구매\n")
                                                    .AppendLine("필요한 아이템을 얻을 수 있는 상점입니다.\n")
                                                    .AppendLine("[보유 골드]")
                                                    .AppendLine($"{player.PlayerGold} G\n")
                                                    .AppendLine("[아이템 목록]");

                for (int i = 0; i < shop.itemList.Count; i++)
                {
                    shopText.Append($"{i + 1}.")
                            .Append($"{shop.itemList[i].ItemName}\t")
                            .Append($"공격력 + {shop.itemList[i].ItemOffense}\t")
                            .Append($"방어력 + {shop.itemList[i].ItemDefense}\t")
                            .Append($"{shop.itemList[i].ItemDesc}\t");

                    if (player.inventory.Contains(shop.itemList[i]))
                    {
                        shopText.AppendLine($"구매완료\t");
                    }
                    else
                    {
                        shopText.AppendLine($"가격: {shop.itemList[i].ItemValue} G\t");
                    }
                }
                shopText.AppendLine("");
                shopText.AppendLine($"0. 나가기");

                Console.WriteLine(shopText);

                while (true)
                {
                    int parseNumber = ActionLoop();

                    if (parseNumber > 0 && parseNumber <= shop.itemList.Count)
                    {
                        ItemBuy(parseNumber, player);
                    }
                    else if (parseNumber == (int)PlayerActionType.Quit)
                    {
                        OpenShop(player);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
            }

            void ShopSellMode()
            {
                StringBuilder shopText = new StringBuilder("상점 - 아이템 판매\n")
                                                    .AppendLine("필요한 아이템을 얻을 수 있는 상점입니다.\n")
                                                    .AppendLine("[보유 골드]")
                                                    .AppendLine($"{player.PlayerGold} G\n")
                                                    .AppendLine("[아이템 목록]");

                for (int i = 0; i < player.inventory.Count; i++)
                {
                    shopText.Append($"{i + 1}.")
                            .Append($"{player.inventory[i].ItemName}\t")
                            .Append($"공격력 + {player.inventory[i].ItemOffense}\t")
                            .Append($"방어력 + {player.inventory[i].ItemDefense}\t")
                            .Append($"{player.inventory[i].ItemDesc}\t");

                    
                    shopText.AppendLine($"가격: { (player.inventory[i].ItemValue) * 0.85} G\t");
                    
                }
                shopText.AppendLine("");
                shopText.AppendLine($"0. 나가기");

                Console.WriteLine(shopText);

                while (true)
                {
                    int parseNumber = ActionLoop();

                    if (parseNumber > 0 && parseNumber <= shop.itemList.Count)
                    {
                        ItemSell(parseNumber);                        
                    }                    
                    else if (parseNumber == (int)PlayerActionType.Quit)
                    {
                        OpenShop(player);
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
                else if ( player.PlayerGold >= shop.itemList[shopIndex].ItemValue)
                {
                    player.PlayerGold -= shop.itemList[shopIndex].ItemValue;
                    player.inventory.Add(shop.itemList[shopIndex]);

                    Console.WriteLine("구매를 완료했습니다.");
                    ShopBuyMode();
                }
                else
                {
                    Console.WriteLine("Gold가 부족합니다.");
                }
            }

            void ItemSell(int parseNumber)
            {
                int inventoryIndex = parseNumber - 1;

                Console.WriteLine(inventoryIndex);
                Console.WriteLine(player.inventory[inventoryIndex].ItemName);

                player.PlayerGold += (int)(player.inventory[inventoryIndex].ItemValue * 0.85f);

                player.inventory.Remove(player.inventory[inventoryIndex]);

                ShopSellMode();

            }

            void OpenInventory(Player player)
            {
                StringBuilder inventoryText = new StringBuilder("인벤토리\n")
                                                    .AppendLine("보유 중인 아이템을 관리할 수 있습니다.\n")
                                                    .AppendLine("[아이템 목록]");

                for (int i = 0; i < player.inventory.Count; i++)
                {
                    if ( player.equippedItem.Contains(player.inventory[i]) )
                    {
                        inventoryText.Append("[E]");
                    }

                    inventoryText
                            .Append($"{player.inventory[i].ItemName}\t")
                            .Append($"{player.inventory[i].ItemOffense}\t")
                            .Append($"{player.inventory[i].ItemDefense}\t")
                            .AppendLine($"{player.inventory[i].ItemDesc}\t");
                            
                }

                inventoryText.AppendLine($"\n1. 장착 관리");
                inventoryText.AppendLine($"0. 나가기");


                Console.WriteLine(inventoryText);

                while (true)
                {
                    int parseNumber = ActionLoop();

                    if (parseNumber == (int)PlayerActionType.Equipment)
                    {
                        EquipmentManagement();
                        break;                        
                    }
                    else if (parseNumber == (int)PlayerActionType.Quit)
                    {
                        ActionSelect();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
            }

            void EquipmentManagement()
            {

                StringBuilder equipmentText = new StringBuilder("인벤토리 - 장착 관리\n")
                                                    .AppendLine("보유 중인 아이템을 관리할 수 있습니다.\n")
                                                    .AppendLine("[아이템 목록]");

                for (int i = 0; i < player.inventory.Count; i++)
                {
                    equipmentText.Append($"{i + 1} ");

                    if (player.equippedItem.Contains(player.inventory[i]) )
                    {
                        equipmentText.Append("[E]");
                    }

                    equipmentText.Append($"{player.inventory[i].ItemName}\t")
                            .Append($"{player.inventory[i].ItemOffense}\t")
                            .Append($"{player.inventory[i].ItemDefense}\t")
                            .AppendLine($"{player.inventory[i].ItemDesc}\t");

                }
               
                equipmentText.AppendLine($"\n0. 나가기");

                Console.WriteLine(equipmentText);

                // 입력값을 받고 ( 확인 체크까지 )

                // 내 인벤토리 카운트보다 입력값이 작다면 ( 해당 인벤토리에 있는 인덱스를 입력했다면 )

                while (true)
                {
                    int parseNumber = ActionLoop();

                    if (parseNumber > 0 && parseNumber < player.inventory.Count + 1)
                    {
                        // 장비 장착하는 로직 추가
                        Equip(parseNumber);
                    }
                    else if (parseNumber == (int)PlayerActionType.Quit)
                    {
                        OpenInventory(player);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
            }

            void Equip(int parseNumber)
            {
                int inventoryIndex = parseNumber - 1;

               if ( player.equippedItem.Contains(player.inventory[inventoryIndex]) )
                {
                    player.equippedItem[player.inventory[inventoryIndex].ItemType] = null;
                }
                else
                {
                    player.equippedItem[player.inventory[inventoryIndex].ItemType] = player.inventory[inventoryIndex];
                }

                /*for ( int i = 0; i < player.equippedItem.Length; i++ )
                {
                    if (player.equippedItem[i] != null)
                    {
                        Console.WriteLine(player.equippedItem[i].ItemName);
                    }
                    
                }*/

                EquipmentManagement();
            }

            void EnterTheDunGeon()
            {
                StringBuilder dungeonText = new StringBuilder("던전입장\n")
                                                        .AppendLine("이곳에서 던전을 들어가기전 활동을 할 수 있습니다.\n")

                .AppendLine($"1. 쉬운 던전\t 방어력 5 이상 권장")
                .AppendLine($"2. 일반 던전\t 방어력 11 이상 권장")
                .AppendLine($"3. 어려운 던전\t 방어력 17 이상 권장")
                .AppendLine($"0. 나가기");

                Console.WriteLine(dungeonText);


                while (true)
                {
                    int parseNumber = ActionLoop();

                    /*if (parseNumber > 0 && parseNumber <= shop.itemList.Count)
                    {
                        ItemBuy(parseNumber, player);                        
                    }*/
                    if (parseNumber == (int)DunGeonType.Easy)
                    {
                        DungeonFight(parseNumber);
                        break;
                    }
                    else if (parseNumber == (int)DunGeonType.Normal)
                    {
                        DungeonFight(parseNumber);
                        break;
                    }
                    else if (parseNumber == (int)DunGeonType.Hard)
                    {
                        DungeonFight(parseNumber);
                        break;
                    }
                    else if (parseNumber == (int)PlayerActionType.Quit)
                    {
                        ActionSelect();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
            }

            void DungeonFight(int parseNumber)
            {
                float bonusOffense = 0;
                int bonusDefense = 0;

                int beforeHP = player.PlayerHP;
                int beforeGold = player.PlayerGold;

                for (int i = 0; i < player.equippedItem.Length; i++)
                {
                    if (player.equippedItem[i] != null)
                    {
                        bonusOffense += player.equippedItem[i].ItemOffense;
                        bonusDefense += player.equippedItem[i].ItemDefense;
                    }
                }

                float totalOffense = player.PlayerOffense + bonusOffense;
                float totalDefense = player.PlayerDefense + bonusDefense;

                int defenseCondition = 0;

                switch ( parseNumber )
                {
                    case (int)DunGeonType.Easy:
                        defenseCondition = 5;
                        break;
                    case (int)DunGeonType.Normal:
                        defenseCondition = 11;
                        break;
                    case (int)DunGeonType.Hard:
                        defenseCondition = 17;
                        break;
                    default:
                        break;
                }

                if (totalDefense < defenseCondition)
                {
                    int dice = new Random().Next(1, 11);

                    if ( dice > 4)
                    {
                        player.PlayerHP /= 2;

                        if ( player.PlayerHP <= 0 ) 
                        {
                            Dead();
                            return;
                        }

                        StringBuilder failText = new StringBuilder("던전 클리어 실패!\n")
                                                        .AppendLine("당신은 던전 클리어에 실패하고 체력에 일부를 잃고 도망쳤습니다.\n")
                                                        .AppendLine($"당신의 현재 체력: {player.PlayerHP}");

                        Console.WriteLine(failText);
                        ActionSelect();
                        return;
                    }
                }

                int costHP = new Random().Next(20, 36);

                costHP -= ((int)totalDefense - defenseCondition);
                costHP = costHP < 0 ? 0 : costHP;
                player.PlayerHP -= costHP;

                if (player.PlayerHP <= 0)
                {
                    Dead();
                    return;
                }

                float clearReward = 0;

                switch (parseNumber)
                {
                    case (int)DunGeonType.Easy:
                        clearReward = 1000;
                        break;
                    case (int)DunGeonType.Normal:
                        clearReward = 1700;
                        break;
                    case (int)DunGeonType.Hard:
                        clearReward = 2500;
                        break;
                    default:
                        break;
                }

                float bonusReward = new Random().Next((int)totalOffense, ((int)totalOffense * 2) + 1);

                clearReward *= (1 + (bonusReward / 100));

                player.PlayerGold += (int)clearReward;

                StringBuilder clearText = new StringBuilder("던전 클리어 성공!\n")
                                                        .AppendLine("축하합니다!!\n");

                switch (parseNumber)
                {
                    case (int)DunGeonType.Easy:
                        clearText.Append("쉬운 던전");
                        break;
                    case (int)DunGeonType.Normal:
                        clearText.Append("일반 던전");
                        break;
                    case (int)DunGeonType.Hard:
                        clearText.Append("어려운 던전");
                        break;                    
                }
                clearText.AppendLine("을 클리어 하였습니다.\n")
                    .AppendLine("[탐험 결과]")
                    .AppendLine($"추가 보상 : {bonusReward} %")
                    .AppendLine($"체력 {beforeHP} -> {player.PlayerHP}")
                    .AppendLine($"Gold {beforeGold} G -> {player.PlayerGold} G \n")
                    .AppendLine($"0. 나가기");

                Console.WriteLine(clearText);

                if ( player.PlayerLevel < 5)
                {
                    player.PlayerLevel++;
                    player.PlayerOffense += 0.5f;
                    player.PlayerDefense += 1f;

                }

                while (true)
                {
                    parseNumber = ActionLoop();

                    if (parseNumber == (int)PlayerActionType.Quit)
                    {
                        ActionSelect();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
            }

            void Dead()
            {
                Console.WriteLine("당신은 사망하였습니다. 게임을 다시 시작하시겠습니까?");

                while (true)
                {
                    int parseNumber = ActionLoop();
                                        
                    if (parseNumber == (int)PlayerActionType.ReTry)
                    {
                        player = new Player();
                        
                        shop = new Shop();

                        ActionSelect();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }

                }
                
                
            }

            void Rest()
            {
                StringBuilder restText = new StringBuilder("휴식하기\n")
                                                        .AppendLine($"500 G 를 내면 체력을 40회복할 수 있습니다.")
                                                        .AppendLine($"현재 체력 : {player.PlayerHP} \n ")
                                                        .AppendLine($"보유 골드 : {player.PlayerGold} G \n ")

                .AppendLine($"1. 휴식하기")
                .AppendLine($"0. 나가기");

                Console.WriteLine(restText);


                while (true)
                {
                    int parseNumber = ActionLoop();

                    if (parseNumber == (int)DunGeonType.Easy)
                    {
                        Heal();
                        break;
                    }
                    else if (parseNumber == (int)PlayerActionType.Quit)
                    {
                        ActionSelect();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
            }

            void Heal()
            {
                if ( player.PlayerGold < 500)
                {
                    Console.WriteLine("Gold가 부족합니다.");                    
                }
                else
                {
                    player.PlayerGold -= 500;
                    player.PlayerHP += 40;

                    if ( player.PlayerHP >= player.PlayerMaximumHP)
                    {
                        player.PlayerHP = player.PlayerMaximumHP;
                    }
                }

                Rest();
            }


            Player LoadDataCheck()
            {
                if ( File.Exists("saveData.json"))
                {
                    string loadJson = File.ReadAllText("saveData.json");

                    Player loadPlayer = JsonConvert.DeserializeObject<Player>(loadJson);

                    return loadPlayer;
                }
                else
                {              
                    return new Player();
                }
            }

            void ItemSync()
            {
                
                for ( int i = 0; i < shop.itemList.Count; i++)
                {
                    for (int j = 0; j < player.inventory.Count; j++)
                    {
                        if (shop.itemList[i].ItemName.Equals(player.inventory[j].ItemName))
                        {
                            player.inventory[j] = shop.itemList[i];
                        }
                    }

                    for (int k = 0; k < player.equippedItem.Length; k++)
                    {
                        if (player.equippedItem[k] == null)
                        {
                            continue;
                        }

                        if (shop.itemList[i].ItemName.Equals(player.equippedItem[k].ItemName))
                        {
                            player.equippedItem[k] = shop.itemList[i];
                        }
                    }
                }
            }
        }
    }
}

