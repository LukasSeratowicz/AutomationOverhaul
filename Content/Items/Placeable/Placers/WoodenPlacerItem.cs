using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AutomationOverhaul.Content.Machines.Placers;
using Microsoft.Xna.Framework;
using System;

namespace AutomationOverhaul.Content.Items.Placeable
{
    public class WoodenPlacerItem : ModItem
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Placers/WoodenPlacerItem";

        public override void SetStaticDefaults() { }

        public override void SetDefaults() {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<WoodenPlacer>();
            Item.placeStyle = 0; // Default to Up
            Item.rare = ItemRarityID.Gray;
        }

        public override void UpdateInventory(Player player) {
            // Only run if holding this specific item
            if (player.HeldItem.type != Item.type) return;

            Vector2 dir = Main.MouseWorld - player.Center;
            int style = 0;

            // Logic copied 1:1 from WoodenPistonItem
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
                .AddIngredient(ItemID.Wood, 30)
                .AddIngredient(ItemID.Cobweb, 10)
                .AddIngredient(ItemID.Gel, 5)
                .AddIngredient(ItemID.Mushroom, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}