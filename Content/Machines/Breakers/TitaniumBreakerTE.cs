using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Breakers
{
    public class TitaniumBreakerTE : BaseBreakerTE 
    {
        public override int TierRangeBonus => 4;
        public override float TierSpeedMultiplier => 30f;
        public override int ValidTileType => ModContent.TileType<TitaniumBreaker>();
    }
}