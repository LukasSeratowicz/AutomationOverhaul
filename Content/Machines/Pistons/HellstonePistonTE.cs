using Terraria;
using Terraria.ModLoader;
using AutomationOverhaul.Content.Machines.Pistons;

namespace AutomationOverhaul.Content.Machines.Pistons
{
    public class HellstonePistonTE : BasePistonTE {
        public override int TargetTileID => ModContent.TileType<HellstonePiston>();
        public override int MaxCooldown => 1050; //17.5f*60;
        public override int PushDistance => 4; 
        public override bool CanPushMachines => true;
        public override float PitchVariance => 0.1f;
        
        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<HellstonePiston>();
        }
    }
}