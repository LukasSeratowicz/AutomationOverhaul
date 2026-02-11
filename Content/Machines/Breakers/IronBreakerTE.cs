using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Breakers
{
    public class IronBreakerTE : BaseBreakerTE 
    {
        public override int TierRangeBonus => 0;
        public override float TierSpeedMultiplier => 125f;
        public override int ValidTileType => ModContent.TileType<IronBreaker>();
    }
}