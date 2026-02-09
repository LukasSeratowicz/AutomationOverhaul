using Terraria;
using Terraria.ModLoader;
using AutomationOverhaul.Content.Machines.Pistons;

namespace AutomationOverhaul.Content.Machines.Pistons
{
    public class LuminitePistonTE : BasePistonTE {
        public override int TargetTileID => ModContent.TileType<LuminitePiston>();
        public override int MaxCooldown => 60; //3.0f*60;
        public override int PushDistance => 10; 
        public override bool CanPushMachines => true;
        public override float PitchVariance => 0.1f;
        
        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<LuminitePiston>();
        }
    }
}