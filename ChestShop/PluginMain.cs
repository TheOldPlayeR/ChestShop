using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

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
        }
        
        private void OnChestOpen(object sender, GetDataHandlers.ChestOpenEventArgs args)
        {
            bool flag = !this.isSelectingChest[args.Player.Index];
            if (!flag)
            {
                int index = Chest.FindChest(args.X, args.Y);
                ChestShop chestShop = new ChestShop(new List<ShopItem>(), index, new Point(args.X, args.Y));
                chestShop.SaveTo(Path.Combine(TShock.SavePath, "ChestShop", string.Format("Chest_{0}.json", index)));
            }
        }
        
        private void ChestShopCommand(CommandArgs args)
        {
            TSPlayer p = args.Player;
            bool selectingChest = this.isSelectingChest[p.Index];
            bool flag = !selectingChest;
            if (flag)
            {
                p.SendInfoMessage("Open a chest to save it's information to Chest_<id>.json, from which you can modify the shop.");
                this.isSelectingChest[p.Index] = true;
            }
            else
            {
                bool flag2 = selectingChest;
                if (flag2)
                {
                    p.SendInfoMessage("Canceled selection.");
                    this.isSelectingChest[p.Index] = false;
                }
            }
        }
    }
}
