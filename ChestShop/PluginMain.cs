using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using Terraria.ID;

namespace ChestShop
{
    [ApiVersion(2, 1)]
    public class PluginMain : TerrariaPlugin
    {
        public PluginMain(Main game) : base(game)
        {
        }

        public override string Name
        {
            get
            {
                return "ChestShop";
            }
        }
        
        public override Version Version
        {
            get
            {
                return new Version(1, 0);
            }
        }

        public override string Author
        {
            get
            {
                return "AshtonB (aka TheOldPlayeR)";
            }
        }

        public bool[] isSelectingChest = new bool[255];
        
        public override void Initialize()
        {
            for (int i = 0; i < 255; i++)
            {
                this.isSelectingChest[i] = false;
            }
            string filePath = Path.Combine(TShock.SavePath, "ChestShop");
            bool flag = !Directory.Exists(filePath);
            if (flag)
            {
                Directory.CreateDirectory(filePath);
            }

            Commands.ChatCommands.Add(new Command("chestshop.manage", new CommandDelegate(this.ChestShopCommand), new string[]
            {
                "chestshop"
            }));

            GetDataHandlers.ChestOpen += new EventHandler<GetDataHandlers.ChestOpenEventArgs>(this.OnChestOpen);

            ServerApi.Hooks.NetGetData.Register(this, OnNetGetData);
        }

        private void OnNetGetData(GetDataEventArgs args)
        {
            using (var stream = new MemoryStream(args.Msg.readBuffer, args.Index, args.Length))
            {
                using (var reader = new BinaryReader(stream))
                {
                    if (args.MsgID == PacketTypes.ChestItem)
                    {
                        int chestID = reader.ReadInt16();
                        byte itemSlot = reader.ReadByte();
                        int stack = reader.ReadInt16();
                        byte prefix = reader.ReadByte();
                        int itemID = reader.ReadInt16();

                        TSPlayer player = TShock.Players[args.Index];
                        ShopItem shopItem = ChestShop.Read(Path.Combine(TShock.SavePath, "ChestShop", "Chest_" + chestID + ".json")).ShopItems[itemSlot];

                        if (player.BuyingOnClick())
                        {
                            Item item = TShock.Utils.GetItemById(itemID);
                            player.GiveItem(shopItem.ItemID, item.Name, item.width, item.height, shopItem.Stack, shopItem.Prefix);
                            args.Handled = true;
                        }
                        else
                        {
                            player.SetShopSlotID(itemSlot);
                            player.SendInfoMessage("Item Details:\n" +
                                                   "Item: [i:{0}]\n" +
                                                   "Stack: {1}\n" +
                                                   "Prefix: {2}\n" +
                                                   "Price: {3}", shopItem.ItemID, shopItem.Stack, shopItem.Prefix, shopItem.Price);
                        }
                    }
                }
            }
        }

        private void OnChestOpen(object sender, GetDataHandlers.ChestOpenEventArgs args)
        {
            bool flag = !isSelectingChest[args.Player.Index];
            int index = Chest.FindChest(args.X, args.Y);
            if (!flag)
            {
                Chest chest = Main.chest[index];
                List<ShopItem> chestItems = new List<ShopItem>();
                foreach (var item in chest.item)
                {
                    if (item == null || item.netID == 0) continue;

                    chestItems.Add(new ShopItem(item.netID, 0, item.stack, item.prefix));
                }
                ChestShop chestShop = new ChestShop(chestItems, index, new Point(args.X, args.Y));
                chestShop.SaveTo(Path.Combine(TShock.SavePath, "ChestShop", string.Format("Chest_{0}.json", index)));
                args.Player.SendInfoMessage("Saved chest to Chest_{0}.", index);
                isSelectingChest[args.Player.Index] = false;
            }

            if (PlayerOpenedChestShop(args.Player, index))
            {
                args.Player.SendInfoMessage("How to shop:\n" +
                                            "Click on an item to get details about it.\n" +
                                            "Click on it again to buy it.");


            }
        }

        public bool PlayerOpenedChestShop(TSPlayer player, int chestIndex)
        {
            foreach (var chestShop in GetChestShops())
            {
                if (chestShop.ChestIndex == chestIndex)
                {
                    player.SendInfoMessage(chestShop.OpenChestMessage);
                    return true;
                }
            }
            return false;
        }

        public List<ChestShop> GetChestShops()
        {
            List<ChestShop> chestShops = new List<ChestShop>();
            foreach (var file in Directory.EnumerateFiles(Path.Combine(TShock.SavePath, "ChestShop")))
            {
                TSPlayer.Server.SendInfoMessage("Reading chest file {0}...", file);
                chestShops.Add(ChestShop.Read(file));
            }
            return chestShops;
        }
        
        private void ChestShopCommand(CommandArgs args)
        {
            TSPlayer p = args.Player;

            bool selectingChest = isSelectingChest[p.Index];
            bool flag = !selectingChest;
            if (flag)
            {
                p.SendInfoMessage("Open a chest to save it's information to Chest_<id>.json, from which you can modify the shop.");
                isSelectingChest[p.Index] = true;
            }
            else
            {
                bool flag2 = selectingChest;
                if (flag2)
                {
                    p.SendInfoMessage("Canceled selection.");
                    isSelectingChest[p.Index] = false;
                }
            }
        }
    }
}
