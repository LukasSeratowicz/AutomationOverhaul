using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Items.Tools
{
    // TIER 0: Wood (1x1)
    public class WoodenStateFlipper : BaseStateFlipper
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.WireKite; 
        //public override string Texture => "AutomationOverhaul/Assets/Items/Tools/WoodenStateFlipper";
        public override int MaxRange => 1; // 1x1
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.White;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ItemID.Wood, 10).AddTile(TileID.WorkBenches).Register();
        }
    }

    // TIER 1: Copper (3x3) - Radius 1
    public class CopperStateFlipper : BaseStateFlipper
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.WireKite; 
        //public override string Texture => "AutomationOverhaul/Assets/Items/Tools/CopperStateFlipper";
        public override int MaxRange => 3; // 3x3
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.Green;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ItemID.Wood, 10).AddTile(TileID.WorkBenches).Register();
        }
    }

    // TIER 2: Iron (5x5) - Radius 2
    public class IronStateFlipper : BaseStateFlipper
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.WireKite; 
        //public override string Texture => "AutomationOverhaul/Assets/Items/Tools/IronStateFlipper";
        public override int MaxRange => 5; // 5x5
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.Blue;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ItemID.Wood, 10).AddTile(TileID.WorkBenches).Register();
        }
    }


    // TIER 13: Luminite (25x25)
    public class LuminiteStateFlipper : BaseStateFlipper
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.WireKite; 
        //public override string Texture => "AutomationOverhaul/Assets/Items/Tools/LuminiteStateFlipper";
        public override int MaxRange => 25; // 25x25
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.Red;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ItemID.Wood, 10).AddTile(TileID.WorkBenches).Register();
        }
    }
}