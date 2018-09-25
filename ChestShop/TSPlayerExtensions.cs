using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace ChestShop
{
    public static class TSPlayerExtensions
    {
        public static void SetShopSlotID(this TSPlayer player, int slotID)
        {
            player.SetData("slotID", slotID);
        }

        public static int GetShopSlotID(this TSPlayer player)
        {
            player.SetData("oldslotID", player.GetData<int>("slotID"));
            return player.GetData<int>("slotID");
        }

        public static bool BuyingOnClick(this TSPlayer player)
        {
            if (player.GetData<int>("slotID") == player.GetData<int>("oldslotID"))
            {
                return true;
            }

            return false;
        }
    }
}
