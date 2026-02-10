using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AutomationOverhaul.Content.Machines.Placers;
using Microsoft.Xna.Framework;
using System;

namespace AutomationOverhaul.Content.Items.Placeable
{
    public class AdamantitePlacerItem : ModItem
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Placers/AdamantitePlacerItem";

        public override void SetStaticDefaults() { }

        public override void SetDefaults() {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<AdamantitePlacer>();
            Item.placeStyle = 0; // Default to Up
            // editable:
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.rare = ItemRarityID.Lime;
        }

        public override void UpdateInventory(Player player) {
            // Only run if holding this specific item
            if (player.HeldItem.type != Item.type) return;

            Vector2 dir = Main.MouseWorld - player.Center;
            int style = 0;

            // Logic copied 1:1 from AdamantitePistonItem
            if (Math.Abs(dir.Y) > Math.Abs(dir.X) * 1.5f) {
                // Vertical (Mouse Angle)
                style = dir.Y > 0 ? 2 : 0; // Down : Up
            } 
            else {
                // Horizontal (Player Face)
                style = player.direction == 1 ? 1 : 3; // Right : Left
            }

            // Force update the style for the preview ghost and placement
            if (Item.placeStyle != style) {
                Item.placeStyle = style;
            }
        }

        public override bool CanUseItem(Player player) {
            UpdateInventory(player);
            return true;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<OrichalcumPlacerItem>())
                .AddIngredient(ItemID.AdamantiteBar, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AdamantitePlacerItem>())
                .AddIngredient(ItemID.AdamantiteBar, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}