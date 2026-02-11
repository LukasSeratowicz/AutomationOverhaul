using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AutomationOverhaul.Content.Machines.Pushers;
using Microsoft.Xna.Framework;
using System;

namespace AutomationOverhaul.Content.Items.Placeable
{
    public class WoodenPusherItem : ModItem
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Pushers/WoodenPusherItem";
        public override void SetStaticDefaults() { }
        public override void SetDefaults() {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<WoodenPusher>();
            Item.placeStyle = 0;
            //editable:
            Item.rare = ItemRarityID.White;
            Item.useAnimation = 40;
            Item.useTime = 40;
        }

        public override void UpdateInventory(Player player) {
            if (player.HeldItem.type != Item.type) return;

            Vector2 dir = Main.MouseWorld - player.Center;
            int style = 0;

            if (Math.Abs(dir.Y) > Math.Abs(dir.X) * 1.5f) {
                style = dir.Y > 0 ? 2 : 0; // Down : Up
            } 
            else {
                style = player.direction == 1 ? 1 : 3; // Right : Left
            }

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
                .AddIngredient(ItemID.Wood, 50)
                .AddIngredient(ItemID.StoneBlock, 25)
                .AddIngredient(ItemID.Acorn, 5)
                .AddIngredient(ItemID.Gel, 5)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}