using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Breakers
{
    public class PlatinumBreakerTE : BaseBreakerTE 
    {
        public override int TierRangeBonus => 1;
        public override float TierSpeedMultiplier => 90f;
        public override int ValidTileType => ModContent.TileType<PlatinumBreaker>();
    }
}