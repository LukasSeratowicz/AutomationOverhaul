using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Items.Tools
{
    // TIER 0: Wood (1x1)
    public class WoodenStateFlipper : BaseStateFlipper
    {
        //public override string Texture => "Terraria/Images/Item_" + ItemID.WireKite; 
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/WoodenStateFlipper";
        public override int MaxRange => 1; // 1x1
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.White;
        }
        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 50)
                .AddIngredient(ItemID.Cobweb, 10)
                .AddIngredient(ItemID.Gel, 5)
                .AddIngredient(ItemID.FallenStar, 1)
                .AddTile(TileID.WorkBenches).Register();
        }
    }

    // TIER 1: Copper
    public class CopperStateFlipper : BaseStateFlipper
    {
        //public override string Texture => "Terraria/Images/Item_" + ItemID.WireKite; 
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/CopperStateFlipper";
        public override int MaxRange => 2;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.Green;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<WoodenStateFlipper>()).AddIngredient(ItemID.CopperBar, 3).AddTile(TileID.WorkBenches).Register();
        }
    }
    // TIER 1: Tin
    public class TinStateFlipper : BaseStateFlipper
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/TinStateFlipper";
        public override int MaxRange => 2;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.Green;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<WoodenStateFlipper>()).AddIngredient(ItemID.TinBar, 3).AddTile(TileID.WorkBenches).Register();
        }
    }

    // TIER 2: Iron
    public class IronStateFlipper : BaseStateFlipper
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/IronStateFlipper";
        public override int MaxRange => 3;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.Blue;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<WoodenStateFlipper>()).AddIngredient(ItemID.IronBar, 3).AddTile(TileID.Anvils).Register();
        }
    }
    // TIER 2: Lead
    public class LeadStateFlipper : BaseStateFlipper
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/LeadStateFlipper";
        public override int MaxRange => 3;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.Blue;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<WoodenStateFlipper>()).AddIngredient(ItemID.LeadBar, 3).AddTile(TileID.Anvils).Register();
        }
    }

    // TIER 3: Silver
    public class SilverStateFlipper : BaseStateFlipper
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/SilverStateFlipper";
        public override int MaxRange => 4;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.Blue;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<TinStateFlipper>()).AddIngredient(ItemID.SilverBar, 3).AddTile(TileID.Anvils).Register();
            CreateRecipe().AddIngredient(ModContent.ItemType<CopperStateFlipper>()).AddIngredient(ItemID.SilverBar, 3).AddTile(TileID.Anvils).Register();
        }
    }
    // TIER 3: Tungsten
    public class TungstenStateFlipper : BaseStateFlipper
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/TungstenStateFlipper";
        public override int MaxRange => 4;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.Blue;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<TinStateFlipper>()).AddIngredient(ItemID.TungstenBar, 3).AddTile(TileID.Anvils).Register();
            CreateRecipe().AddIngredient(ModContent.ItemType<CopperStateFlipper>()).AddIngredient(ItemID.TungstenBar, 3).AddTile(TileID.Anvils).Register();
        }
    }
    
    // TIER 4: Gold
    public class GoldStateFlipper : BaseStateFlipper
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/GoldStateFlipper";
        public override int MaxRange => 5;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<IronStateFlipper>()).AddIngredient(ItemID.GoldBar, 3).AddTile(TileID.Anvils).Register();
            CreateRecipe().AddIngredient(ModContent.ItemType<LeadStateFlipper>()).AddIngredient(ItemID.GoldBar, 3).AddTile(TileID.Anvils).Register();
        }
    }
    // TIER 4: Platinum
    public class PlatinumStateFlipper : BaseStateFlipper
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/PlatinumStateFlipper";
        public override int MaxRange => 5;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<IronStateFlipper>()).AddIngredient(ItemID.PlatinumBar, 3).AddTile(TileID.Anvils).Register();
            CreateRecipe().AddIngredient(ModContent.ItemType<LeadStateFlipper>()).AddIngredient(ItemID.PlatinumBar, 3).AddTile(TileID.Anvils).Register();
        }
    }
    // TIER 5: Demonite
    public class DemoniteStateFlipper : BaseStateFlipper
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/DemoniteStateFlipper";
        public override int MaxRange => 6;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<GoldStateFlipper>()).AddIngredient(ItemID.DemoniteBar, 3).AddTile(TileID.Anvils).Register();
            CreateRecipe().AddIngredient(ModContent.ItemType<PlatinumStateFlipper>()).AddIngredient(ItemID.DemoniteBar, 3).AddTile(TileID.Anvils).Register();
        }
    }
    // TIER 5: Crimtane
    public class CrimtaneStateFlipper : BaseStateFlipper
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/CrimtaneStateFlipper";
        public override int MaxRange => 6;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<GoldStateFlipper>()).AddIngredient(ItemID.CrimtaneBar, 3).AddTile(TileID.Anvils).Register();
            CreateRecipe().AddIngredient(ModContent.ItemType<PlatinumStateFlipper>()).AddIngredient(ItemID.CrimtaneBar, 3).AddTile(TileID.Anvils).Register();
        }
    }
    // TIER 6: Meteorite
    public class MeteoriteStateFlipper : BaseStateFlipper
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/MeteoriteStateFlipper";
        public override int MaxRange => 7;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.LightRed;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<DemoniteStateFlipper>()).AddIngredient(ItemID.MeteoriteBar, 3).AddTile(TileID.Anvils).Register();
            CreateRecipe().AddIngredient(ModContent.ItemType<CrimtaneStateFlipper>()).AddIngredient(ItemID.MeteoriteBar, 3).AddTile(TileID.Anvils).Register();
        }
    }
    // TIER 7: Hellstone
    public class HellstoneStateFlipper : BaseStateFlipper
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/HellstoneStateFlipper";
        public override int MaxRange => 8;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.LightRed;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<MeteoriteStateFlipper>()).AddIngredient(ItemID.HellstoneBar, 3).AddTile(TileID.Anvils).Register();
        }
    }

    // TIER 8: Cobalt
    public class CobaltStateFlipper : BaseStateFlipper
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/CobaltStateFlipper";
        public override int MaxRange => 9;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.Pink;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<HellstoneStateFlipper>()).AddIngredient(ItemID.CobaltBar, 3).AddTile(TileID.Anvils).Register();
        }
    }
    // TIER 8: Palladium
    public class PalladiumStateFlipper : BaseStateFlipper
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/PalladiumStateFlipper";
        public override int MaxRange => 9;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.Pink;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<HellstoneStateFlipper>()).AddIngredient(ItemID.PalladiumBar, 3).AddTile(TileID.Anvils).Register();
        }
    }

    // TIER 9: Mythril
    public class MythrilStateFlipper : BaseStateFlipper
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/MythrilStateFlipper";
        public override int MaxRange => 10;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.LightPurple;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<CobaltStateFlipper>()).AddIngredient(ItemID.MythrilBar, 3).AddTile(TileID.MythrilAnvil).Register();
            CreateRecipe().AddIngredient(ModContent.ItemType<PalladiumStateFlipper>()).AddIngredient(ItemID.MythrilBar, 3).AddTile(TileID.MythrilAnvil).Register();
        }
    }
    // TIER 9: Orichalcum
    public class OrichalcumStateFlipper : BaseStateFlipper
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/OrichalcumStateFlipper";
        public override int MaxRange => 10;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.LightPurple;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<CobaltStateFlipper>()).AddIngredient(ItemID.OrichalcumBar, 3).AddTile(TileID.MythrilAnvil).Register();
            CreateRecipe().AddIngredient(ModContent.ItemType<PalladiumStateFlipper>()).AddIngredient(ItemID.OrichalcumBar, 3).AddTile(TileID.MythrilAnvil).Register();
        }
    }

    // TIER 10: Adamantite
    public class AdamantiteStateFlipper : BaseStateFlipper
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/AdamantiteStateFlipper";
        public override int MaxRange => 11;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.Lime;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<MythrilStateFlipper>()).AddIngredient(ItemID.AdamantiteBar, 3).AddTile(TileID.MythrilAnvil).Register();
            CreateRecipe().AddIngredient(ModContent.ItemType<OrichalcumStateFlipper>()).AddIngredient(ItemID.AdamantiteBar, 3).AddTile(TileID.MythrilAnvil).Register();
        }
    }
    // TIER 10: Titanium
    public class TitaniumStateFlipper : BaseStateFlipper
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/TitaniumStateFlipper";
        public override int MaxRange => 11;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.Lime;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<MythrilStateFlipper>()).AddIngredient(ItemID.TitaniumBar, 3).AddTile(TileID.MythrilAnvil).Register();
            CreateRecipe().AddIngredient(ModContent.ItemType<OrichalcumStateFlipper>()).AddIngredient(ItemID.TitaniumBar, 3).AddTile(TileID.MythrilAnvil).Register();
        }
    }

    // TIER 11: Hallowed
    public class HallowedStateFlipper : BaseStateFlipper
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/HallowedStateFlipper";
        public override int MaxRange => 13;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<AdamantiteStateFlipper>()).AddIngredient(ItemID.HallowedBar, 3).AddTile(TileID.MythrilAnvil).Register();
            CreateRecipe().AddIngredient(ModContent.ItemType<TitaniumStateFlipper>()).AddIngredient(ItemID.HallowedBar, 3).AddTile(TileID.MythrilAnvil).Register();
        }
    }

    // TIER 12: Chlorophyte
    public class ChlorophyteStateFlipper : BaseStateFlipper
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/ChlorophyteStateFlipper";
        public override int MaxRange => 16;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.Cyan;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<HallowedStateFlipper>()).AddIngredient(ItemID.ChlorophyteBar, 3).AddTile(TileID.MythrilAnvil).Register();
        }
    }

    // TIER 13: Luminite
    public class LuminiteStateFlipper : BaseStateFlipper
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/StateFlippers/LuminiteStateFlipper";
        public override int MaxRange => 25;
        public override void SetDefaults() {
            base.SetDefaults();
            Item.rare = ItemRarityID.Red;
        }
        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ModContent.ItemType<ChlorophyteStateFlipper>()).AddIngredient(ItemID.LunarBar, 3).AddTile(TileID.LunarCraftingStation).Register();
        }
    }
}