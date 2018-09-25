using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChestShop
{
    public class ChestShop
    {
        public List<ShopItem> ShopItems = new List<ShopItem>();
        public int ChestIndex;
        public Point Position;
        public string OpenChestMessage = "Welcome to the chest shop, click on an item to buy it.";

        public ChestShop(List<ShopItem> shopItems, int chestIndex, Point position, string openChestMessage = "Welcome to the chest shop, click on an item to buy it.")
        {
            ShopItems = shopItems;
            ChestIndex = chestIndex;
            Position = position;
            OpenChestMessage = openChestMessage;
        }

        public void SaveTo(string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public static ChestShop Read(string path)
        {
            if (!File.Exists(path))
            {
                ChestShop chestShop = new ChestShop(new List<ShopItem>(), 0, Point.Zero);
                File.WriteAllText(path, JsonConvert.SerializeObject(chestShop, Formatting.Indented));
                return chestShop;
            }
            return JsonConvert.DeserializeObject<ChestShop>(File.ReadAllText(path));
        }
    }
}
