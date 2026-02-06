using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using AutomationOverhaul.Content.Items.Placeable;

namespace AutomationOverhaul.Content.Machines.Pistons
{
    public class WoodenPiston : ModTile
    {
        public override string Texture => "AutomationOverhaul/Assets/Tiles/WoodenPiston";

        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileNoAttach[Type] = false;
            Main.tileBlockLight[Type] = true;

            MineResist = 3.0f; 
            MinPick = 35;
            
            HitSound = SoundID.Tink;

            RegisterItemDrop(ModContent.ItemType<WoodenPistonItem>());

            // Object Data
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);
        }

        public override void PlaceInWorld(int i, int j, Item item) {
            ModContent.GetInstance<WoodenPistonTE>().Place(i, j);

            if (TileEntity.ByPosition.TryGetValue(new Terraria.DataStructures.Point16(i, j), out TileEntity te)) {
                if (te is WoodenPistonTE piston) {
                    piston.CooldownTimer = piston.MaxCooldown;
                }
            }
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem) {
            ModContent.GetInstance<WoodenPistonTE>().Kill(i, j);
        }

        public static void Rotate(int i, int j) {
            Tile tile = Main.tile[i, j];
            tile.TileFrameX += 18;
            if (tile.TileFrameX >= 72) {
                tile.TileFrameX = 0;
            }
            if (Main.netMode != NetmodeID.SinglePlayer) {
                NetMessage.SendTileSquare(-1, i, j, 1);
            }
        }

        public override bool CanPlace(int i, int j) {
            Tile tile = Main.tile[i, j];
            if (tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileCut[tile.TileType]) {
                return false;
            }
            if (HasSolidNeighbor(i, j) || Main.tile[i, j].WallType > 0) {
                return true;
            }
            return false;
        }

        private bool HasSolidNeighbor(int x, int y) {
            if (IsSolid(x, y - 1)) return true;
            if (IsSolid(x, y + 1)) return true;
            if (IsSolid(x - 1, y)) return true;
            if (IsSolid(x + 1, y)) return true;
            return false;
        }

        private bool IsSolid(int x, int y) {
            if (!WorldGen.InWorld(x, y)) return false;
            Tile t = Main.tile[x, y];
            return t.HasTile && Main.tileSolid[t.TileType] && !Main.tileCut[t.TileType];
        }
    }
}