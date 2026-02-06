using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AutomationOverhaul.Content.Items.Placeable; 

namespace AutomationOverhaul.Content.Systems
{
    public class RecipeSystem : ModSystem
    {
        public override void AddRecipeGroups() {
            RecipeGroup groupT1 = new RecipeGroup(() => "Any Copper/Tin Piston", new int[] {
                ModContent.ItemType<CopperPistonItem>(),
                ModContent.ItemType<TinPistonItem>()
            });
            RecipeGroup.RegisterGroup("AutomationOverhaul:PistonsT1", groupT1);

            RecipeGroup groupT2 = new RecipeGroup(() => "Any Iron/Lead Piston", new int[] {
                ModContent.ItemType<IronPistonItem>(),
                ModContent.ItemType<LeadPistonItem>()
            });
            RecipeGroup.RegisterGroup("AutomationOverhaul:PistonsT2", groupT2);
            
        }
    }
}