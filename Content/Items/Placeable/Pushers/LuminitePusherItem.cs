using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AutomationOverhaul.Content.Machines.Pushers;
using Microsoft.Xna.Framework;
using System;

namespace AutomationOverhaul.Content.Items.Placeable
{
    public class LuminitePusherItem : ModItem
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Pushers/LuminitePusherItem";
        public override void SetStaticDefaults() { }
        public override void SetDefaults() {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<LuminitePusher>();
            Item.placeStyle = 0;
            //editable:
            Item.rare = ItemRarityID.Red;
            Item.useAnimation = 14;
            Item.useTime = 14;
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

        public override bool CanUseItem(Player player) => true;

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ChlorophytePusherItem>())
                .AddIngredient(ItemID.LunarBar, 6)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}