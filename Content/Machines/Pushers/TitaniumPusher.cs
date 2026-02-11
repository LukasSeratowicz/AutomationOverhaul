using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using AutomationOverhaul.Content.Items.Placeable;

namespace AutomationOverhaul.Content.Machines.Pushers
{
    public class TitaniumPusher : ModTile
    {
        public override string Texture => "AutomationOverhaul/Assets/Tiles/Pushers/TitaniumPusher";

        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileNoAttach[Type] = false;
            Main.tileBlockLight[Type] = true;

            MineResist = 4.5f;
            MinPick = 150;
            DustType = DustID.Titanium;
            HitSound = SoundID.Tink;

            RegisterItemDrop(ModContent.ItemType<TitaniumPusherItem>());

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);
        }

        public override void PlaceInWorld(int i, int j, Item item) {
            Point16 pos = new Point16(i, j);
            if (TileEntity.ByPosition.TryGetValue(pos, out TileEntity existingTE)) {
                TileEntity.ByID.Remove(existingTE.ID);
                TileEntity.ByPosition.Remove(pos);
            }

            int id = ModContent.GetInstance<TitaniumPusherTE>().Place(i, j);
            if (id != -1 && TileEntity.ByID.TryGetValue(id, out TileEntity te) && te is TitaniumPusherTE pusher) {
                pusher.CooldownTimer = pusher.MaxCooldown;
            }
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem) {
            if (!fail) {
                 ModContent.GetInstance<TitaniumPusherTE>().Kill(i, j);
            }
        }

        public override bool Slope(int i, int j) => false;

        public static void Rotate(int i, int j) {
            Tile tile = Main.tile[i, j];
            tile.TileFrameX += 18;
            if (tile.TileFrameX >= 72) tile.TileFrameX = 0;
            if (Main.netMode != NetmodeID.SinglePlayer) {
                NetMessage.SendTileSquare(-1, i, j, 1);
            }
        }

        public override bool CanPlace(int i, int j) {
            Tile tile = Main.tile[i, j];
            if (tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileCut[tile.TileType]) return false;
            return HasSolidNeighbor(i, j) || Main.tile[i, j].WallType > WallID.None;
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