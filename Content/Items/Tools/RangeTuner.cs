using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using AutomationOverhaul.Content.Machines.Breakers;

namespace AutomationOverhaul.Content.Items.Tools
{
    public class RangeTuner : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.MechanicalLens;

        public override void SetDefaults() {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 1;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.value = 2000;
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = false;
        }

        public override bool? UseItem(Player player) {
            if (player.whoAmI != Main.myPlayer) return true;

            int i = Player.tileTargetX;
            int j = Player.tileTargetY;

            if (!player.IsInTileInteractionRange(i, j, TileReachCheckSettings.Simple)) return false;

            if (TileEntity.ByPosition.TryGetValue(new Point16(i, j), out TileEntity te)) {
                if (te is BaseBreakerTE breaker) {
                    TuneBreaker(breaker, i, j, player);
                    return true;
                }
            }
            return true;
        }

        private void TuneBreaker(BaseBreakerTE breaker, int i, int j, Player player) {
            int maxCap = breaker.GetMaxPossibleRange();

            int oldRange = breaker.CurrentRangeSetting;

            breaker.CurrentRangeSetting++;
            if (breaker.CurrentRangeSetting > maxCap) {
                breaker.CurrentRangeSetting = 1;
            }
            int newRange = breaker.CurrentRangeSetting;

            breaker.CalculateSpeed();
            breaker.CooldownTimer = breaker.MaxCooldown;

            PopupText.NewText(new AdvancedPopupRequest() {
                Text = $"New Range: {newRange}",
                Color = Color.Cyan,
                DurationInFrames = 60,
                Velocity = new Vector2(0, -3)
            }, player.Center - new Vector2(0, 40));

            ShowVisualization(i, j, oldRange, newRange, maxCap);

            Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuTick, new Vector2(i * 16, j * 16));

            if (Main.netMode != NetmodeID.SinglePlayer) {
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, breaker.ID, i, j);
            }
        }

        private void ShowVisualization(int x, int y, int oldRange, int newRange, int maxCap) {
            Tile tile = Main.tile[x, y];
            int style = (tile.TileFrameX / 18) % 4;
            int dx = 0; int dy = 0;

            // 0=Up, 1=Right, 2=Down, 3=Left
            if (style == 0) dy = -1;
            else if (style == 1) dx = 1;
            else if (style == 2) dy = 1;
            else if (style == 3) dx = -1;

            Vector2 origin = new Vector2(x, y);

            Vector2 startStrip = origin + new Vector2(dx, dy);
            Vector2 endStrip = origin + new Vector2(dx * maxCap, dy * maxCap);
            
            Vector2 stripTL = new Vector2(Math.Min(startStrip.X, endStrip.X), Math.Min(startStrip.Y, endStrip.Y));
            int stripW = Math.Abs((int)(endStrip.X - startStrip.X)) + 1;
            int stripH = Math.Abs((int)(endStrip.Y - startStrip.Y)) + 1;

            DrawSelectionBox(stripTL, stripW, stripH, Color.White * 0.4f);

            Vector2 oldPos = origin + new Vector2(dx * oldRange, dy * oldRange);
            DrawSelectionBox(oldPos, 1, 1, Color.Red, fastFade: true);

            Vector2 newPos = origin + new Vector2(dx * newRange, dy * newRange);
            DrawSelectionBox(newPos, 1, 1, Color.Lime);
        }

        private void DrawSelectionBox(Vector2 topLeftTile, int width, int height, Color color, bool fastFade = false) {
            Vector2 pos1 = topLeftTile * 16;
            Vector2 pos2 = (topLeftTile + new Vector2(width, height)) * 16;

            float pxWidth = pos2.X - pos1.X;
            float pxHeight = pos2.Y - pos1.Y;

            int step = 4;

            // Top & Bottom
            for (float k = 0; k <= pxWidth; k += step) {
                SpawnSafeDust(pos1 + new Vector2(k, 0), color, fastFade);
                SpawnSafeDust(pos1 + new Vector2(k, pxHeight), color, fastFade);
            }
            // Left & Right
            for (float k = 0; k <= pxHeight; k += step) {
                SpawnSafeDust(pos1 + new Vector2(0, k), color, fastFade);
                SpawnSafeDust(pos1 + new Vector2(pxWidth, k), color, fastFade);
            }
        }

        private void SpawnSafeDust(Vector2 pos, Color color, bool fastFade) {
            int dustType = ModContent.DustType<Content.Dusts.RangeDust>();
            Dust d = Dust.NewDustPerfect(pos, dustType, Vector2.Zero, 0, color, 1f);
            d.noGravity = true;
            d.noLight = true;
            if (fastFade) {
                //d.velocity = Main.rand.NextVector2Circular(1, 1);
                d.scale = 0.8f; 
                d.fadeIn = 0;
            }
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 4)
                .AddIngredient(ItemID.Wire, 2)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}