using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Breakers
{
    public class SilverBreakerTE : BaseBreakerTE 
    {
        public override int TierRangeBonus => 0;
        public override float TierSpeedMultiplier => 105f;
        public override int ValidTileType => ModContent.TileType<SilverBreaker>();
    }
}