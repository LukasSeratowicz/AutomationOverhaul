using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Breakers
{
    public class HellstoneBreakerTE : BaseBreakerTE 
    {
        public override int TierRangeBonus => 2;
        public override float TierSpeedMultiplier => 60f;
        public override int ValidTileType => ModContent.TileType<HellstoneBreaker>();
    }
}