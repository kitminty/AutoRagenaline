using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameInput;
using CalamityMod.CalPlayer;

namespace AutoRagenaline
{
    public class AutoRagenalinePlayer : ModPlayer
    {
        public SoundStyle RageBoom = new("AutoRagenaline/vineboom");
        public SoundStyle AdrenalinePipe = new("AutoRagenaline/metalpipe");
        public SoundStyle RageActivationSound = new("CalamityMod/Sounds/Custom/AbilitySounds/RageActivate");
        public SoundStyle AdrenalineActivationSound = new("CalamityMod/Sounds/Custom/AbilitySounds/AdrenalineActivate");
        public SoundStyle NanomachinesActivationSound = new("CalamityMod/Sounds/Custom/AbilitySounds/NanomachinesActivate");
        
        public int startrage = 0;
        public int startadrenaline = 1;
        public int startsecretmode = 0;
        public int forcehiddensecretmode = 0;

        public async override void OnEnterWorld() {
            while (Main.menuMode != 0) {
                var autoRagenalineServerConfig = new AutoRagenalineServerConfig();
                var CalamityPlayer = Player.GetModPlayer<CalamityPlayer>();
                if (AutoRagenalineServerConfig.Instance.Secret == Player.name && AutoRagenalineServerConfig.Instance.EnableOrDisableSecret == true) {
                    forcehiddensecretmode = 1;
                    startsecretmode = 1;
                } else {
                    forcehiddensecretmode = 0;
                }
                if (CalamityPlayer.rage >= CalamityPlayer.rageMax && !CalamityPlayer.rageModeActive && startrage == 1)
                    {
                        Player.AddBuff(ModContent.BuffType<CalamityMod.Buffs.StatBuffs.RageMode>(), 2);
                        if (Player.whoAmI == Main.myPlayer)
                            if (startsecretmode == 0) {
                                SoundEngine.PlaySound(RageActivationSound);
                            } else {
                                SoundEngine.PlaySound(RageBoom);
                            }
                        int rageDustID = 235;
                        int dustCount = 132;
                        float minSpeed = 4f;
                        float maxSpeed = 11f;
                        for (int i = 0; i < dustCount; ++i)
                        {
                            float speed = (float)Math.Sqrt(Main.rand.NextFloat(minSpeed * minSpeed, maxSpeed * maxSpeed));
                            Vector2 dustVel = Main.rand.NextVector2Unit() * speed;
                            Dust d = Dust.NewDustPerfect(Player.Center, rageDustID, dustVel);
                            d.noGravity = !Main.rand.NextBool(4);
                            d.noLight = false;
                            d.scale = Main.rand.NextFloat(0.9f, 2.1f);
                        }
                    }
                if (CalamityPlayer.adrenaline == CalamityPlayer.adrenalineMax && !CalamityPlayer.adrenalineModeActive && startadrenaline == 1)
                    {
                        Player.AddBuff(ModContent.BuffType<CalamityMod.Buffs.StatBuffs.AdrenalineMode>(), CalamityPlayer.AdrenalineDuration);
                        SoundStyle ActivationSound = CalamityPlayer.draedonsHeart ? NanomachinesActivationSound : AdrenalineActivationSound;
                        if (Player.whoAmI == Main.myPlayer)
                            if (startsecretmode == 0) {
                                SoundEngine.PlaySound(ActivationSound);
                            } else {
                                SoundEngine.PlaySound(AdrenalinePipe);
                            }
                        int dustPerSegment = 96;
                        Vector2 segmentOneStart = new Vector2(0f, -120f);
                        Vector2 segmentOneEnd = new Vector2(-48f, 24f);
                        Vector2 segmentOneIncrement = (segmentOneEnd - segmentOneStart) / dustPerSegment;
                        Vector2 segmentTwoStart = segmentOneEnd;
                        Vector2 segmentTwoEnd = new Vector2(48f, -24f);
                        Vector2 segmentTwoIncrement = (segmentTwoEnd - segmentTwoStart) / dustPerSegment;
                        Vector2 segmentThreeStart = segmentTwoEnd;
                        Vector2 segmentThreeEnd = new Vector2(0f, 120f);
                        Vector2 segmentThreeIncrement = (segmentThreeEnd - segmentThreeStart) / dustPerSegment;
                        float maxDustVelSpread = 1.2f;
                        for (int i = 0; i < dustPerSegment; ++i)
                        {
                            bool electricity = Main.rand.NextBool(4);
                            int dustID = electricity ? (Main.rand.NextBool() ? 132 : 131) : ModContent.DustType<CalamityMod.Dusts.AdrenDust>();
                            float interpolant = i + 0.5f;
                            float spreadSpeed = Main.rand.NextFloat(0.5f, maxDustVelSpread);
                            if (electricity)
                                spreadSpeed *= 4f;
                            Vector2 segmentOnePos = Player.Center + segmentOneStart + segmentOneIncrement * interpolant;
                            Dust d = Dust.NewDustPerfect(segmentOnePos, dustID, Vector2.Zero);
                            if (electricity)
                                d.noGravity = false;
                            d.scale = Main.rand.NextFloat(1.2f, 1.8f);
                            d.velocity = Main.rand.NextVector2Unit() * spreadSpeed;
                            Vector2 segmentTwoPos = Player.Center + segmentTwoStart + segmentTwoIncrement * interpolant;
                            d = Dust.CloneDust(d);
                            d.position = segmentTwoPos;
                            d.scale = Main.rand.NextFloat(1.2f, 1.8f);
                            d.velocity = Main.rand.NextVector2Unit() * spreadSpeed;
                            Vector2 segmentThreePos = Player.Center + segmentThreeStart + segmentThreeIncrement * interpolant;
                            d = Dust.CloneDust(d);
                            d.position = segmentThreePos;
                            d.scale = Main.rand.NextFloat(1.2f, 1.8f);
                            d.velocity = Main.rand.NextVector2Unit() * spreadSpeed;
                        }
                    }
                await Task.Delay(1000);
            }
        }
        //done

        public async override void ProcessTriggers(TriggersSet triggersSet) {
            if (AutoRagenaline.ragee.JustPressed) {
                startrage = 1 - startrage;
                if (startadrenaline == 1 && startrage == 1) {
                    Main.NewText("Automatic Rage and Adrenaline"); 
                } else if (startadrenaline == 0 && startrage == 1) {
                    Main.NewText("Automatic Rage");
                } else if (startadrenaline == 1 && startrage == 0) {
                    Main.NewText("Only Automatic Adrenaline");
                } else {
                    Main.NewText("Disabling Automatic Rage");
                }
            }
            if (AutoRagenaline.adrenalinee.JustPressed) {
                startadrenaline = 1 - startadrenaline;
                if (startrage == 1 && startadrenaline == 1) {
                    Main.NewText("Automatic Rage and Adrenaline"); 
                } else if (startrage == 0 && startadrenaline == 1) {
                    Main.NewText("Automatic Adrenaline");
                } else if (startrage == 1 && startadrenaline == 0) {
                    Main.NewText("Only Automatic Rage");
                } else {
                    Main.NewText("Disabling Automatic Adrenaline");
                }
            }
            if (AutoRagenaline.secretmode.JustPressed && forcehiddensecretmode == 0) {
                startsecretmode = 1 - startsecretmode;
                if (startsecretmode == 1) {
                    Main.NewText("Enabled");
                    SoundEngine.PlaySound(RageBoom);
                } else {
                    Main.NewText("Disabled");
                }
            }
        }
    }
}