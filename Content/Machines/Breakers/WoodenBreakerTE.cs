using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Breakers
{
    public class WoodenBreakerTE : BaseBreakerTE 
    {
        public override int TierRangeBonus => 0;
        public override float TierSpeedMultiplier => 166.9565217391f;
        public override int ValidTileType => ModContent.TileType<WoodenBreaker>();
    }
}