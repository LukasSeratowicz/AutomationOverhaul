using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using AutomationOverhaul.Content.Machines.Pistons; 
using AutomationOverhaul.Content.Machines.Placers;

namespace AutomationOverhaul.Content.Items.Tools
{
    public class Rotator : ModItem
    {
        public override string Texture => "AutomationOverhaul/Assets/Items/Tools/Rotator";

        public override void SetDefaults() {
            Item.width = 16; 
            Item.height = 16;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Thrust; 
            Item.value = 2000;
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = true; 
        }

        public override bool? UseItem(Player player) {
            if (player.whoAmI != Main.myPlayer) return true;

            int i = Player.tileTargetX;
            int j = Player.tileTargetY;

            if (!player.IsInTileInteractionRange(i, j, TileReachCheckSettings.Simple)) return false;

            // Check if the targeted tile is of a type that can be rotated (Machines)
            if (TileEntity.ByPosition.TryGetValue(new Point16(i, j), out TileEntity te)) {
                if (te is BasePistonTE || te is BasePlacerTE) {
                    RotateTile(i, j);
                    return true;
                }
            }

            return true;
        }

        private void RotateTile(int i, int j) {
            Tile tile = Main.tile[i, j];
            
            // Cycle Frames: 0 -> 18 -> 36 -> 54 -> 0
            tile.TileFrameX += 18;
            if (tile.TileFrameX >= 72) {
                tile.TileFrameX = 0;
            }

            // Audio (Local)
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Unlock, new Microsoft.Xna.Framework.Vector2(i * 16, j * 16));

            // Network Sync
            if (Main.netMode != NetmodeID.SinglePlayer) {
                NetMessage.SendTileSquare(-1, i, j, 1);
            }
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 25)
                .AddIngredient(ItemID.Gel, 5)
                .AddIngredient(ItemID.IronBar, 4)
                .AddIngredient(ItemID.Chain, 2)
                .AddIngredient(ItemID.RopeCoil, 1)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 25)
                .AddIngredient(ItemID.Gel, 5)
                .AddIngredient(ItemID.LeadBar, 4)
                .AddIngredient(ItemID.Chain, 2)
                .AddIngredient(ItemID.RopeCoil, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}