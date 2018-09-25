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

        public ChestShop(List<ShopItem> shopItems, int chestIndex, Point position)
        {
            ShopItems = shopItems;
            ChestIndex = chestIndex;
            Position = position;
        }

        public void SaveTo(string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}
