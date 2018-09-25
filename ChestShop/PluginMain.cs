using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;
using TerrariaApi.Server;
using Terraria;

namespace ChestShop
{
    public class PluginMain : TerrariaPlugin
    {
        public PluginMain(Main game) : base(game)
        {
        }

        public override string Name => "ChestShop";

        public override Version Version => new Version(1,0);

        public override string Author => "AshtonB (aka TheOldPlayeR)";

        public override void Initialize()
        {
            
        }
    }
}
