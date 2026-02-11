using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Breakers
{
    public class DemoniteBreakerTE : BaseBreakerTE 
    {
        public override int TierRangeBonus => 1;
        public override float TierSpeedMultiplier => 80f;
        public override int ValidTileType => ModContent.TileType<DemoniteBreaker>();
    }
}