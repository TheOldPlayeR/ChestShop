using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChestShop
{
    public class ShopItem
    {
        public int ItemID;
        public int Price;
        public int Stack;
        public byte Prefix;

        public ShopItem(int itemID, int price, int stack, byte prefix)
        {
            ItemID = itemID;
            Price = price;
            Stack = stack;
            Prefix = prefix;
        }
    }
}
