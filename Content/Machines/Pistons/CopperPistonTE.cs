using Terraria;
using Terraria.ModLoader;
using AutomationOverhaul.Content.Machines.Pistons;

namespace AutomationOverhaul.Content.Machines.Pistons
{
    public class CopperPistonTE : BasePistonTE {
        public override int MaxCooldown => 50*60;
        public override int PushDistance => 1; 
        public override bool CanPushMachines => false;
        public override float PitchVariance => 0.1f;
        
        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<CopperPiston>();
        }
    }
}