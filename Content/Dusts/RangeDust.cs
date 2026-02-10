using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Dusts
{
    public class RangeDust : ModDust
    {
        public override string Texture => "AutomationOverhaul/Assets/Dusts/SelectionDust"; // todo: change in the future if we want a different texture
        
        public override void OnSpawn(Dust dust) {
            dust.noGravity = true;
            dust.noLight = true; 
            dust.frame = new Rectangle(0, 0, 8, 8);
        }

        public override bool Update(Dust dust) {
            dust.position += dust.velocity;
            dust.rotation = 0f;
            dust.scale -= 0.01f; 
            if (dust.scale < 0.1f) dust.active = false;
            return false;
        }

        // We ignore 'lightColor' (the cave darkness) and return our own color.
        // This makes it glow in the dark without lighting up blocks.
        public override Color? GetAlpha(Dust dust, Color lightColor) {
            return dust.color; 
        }
    }
}