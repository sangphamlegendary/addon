﻿using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using LeagueSharp.Common;
using SharpDX;
using SharpDX.Direct3D9;
using Spell = LeagueSharp.Common.Spell;

namespace Mordekaiser
{
    internal class Utils
    {
        public static Font Text, TextBold, TextWarning;

        public Utils()
        {
            Text = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Segoe UI",
                    Height = 16,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.ClearTypeNatural
                });

            TextBold = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Segoe UI",
                    Height = 16,
                    Weight = FontWeight.Bold,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.ClearTypeNatural
                });

            TextWarning = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Malgun Gothic",
                    Height = 75,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.ClearTypeNatural
                });
        }

        private static bool MordekaiserHaveSlave
        {
            get { return Player.Self.Spellbook.GetSpell(SpellSlot.R).Name == "mordekaisercotgguide"; }
        }

        public static Obj_AI_Base HowToTrainYourDragon
        {
            get
            {
                if (!MordekaiserHaveSlave)
                    return null;

                return
                    ObjectManager
                        .Get<Obj_AI_Base>(
                        )
                        .FirstOrDefault(
                            m => m.LSDistance(Player.Self.Position) < 15000 && !m.Name.Contains("inion") && m.IsAlly &&
                                 m.HasBuff("mordekaisercotgpetbuff2"));
            }
        }

        public static string Tab
        {
            get { return "    "; }
        }

        public static void DrawText(Font vFont, string vText, float vPosX, float vPosY, ColorBGRA vColor)
        {
            vFont.DrawText(null, vText, (int) vPosX, (int) vPosY, vColor);
        }

        public class TargetSelector
        {
            internal class Ally
            {
                public static int GetPriority(string championName)
                {
                    string[] lowPriority =
                    {
                        "Alistar", "Amumu", "Bard", "Blitzcrank", "Braum", "Cho'Gath", "Dr. Mundo",
                        "Garen", "Gnar", "Hecarim", "Janna", "Jarvan IV", "Leona", "Lulu",
                        "Malphite", "Nami", "Nasus", "Nautilus", "Nunu", "Olaf", "Renekton",
                        "Sejuani", "Shen", "Shyvana", "Singed", "Sion", "Skarner", "Sona", "Soraka",
                        "Tahm", "Taric", "Thresh", "Volibear", "Warwick", "MonkeyKing", "Yorick",
                        "Zac", "Zyra", "Aatrox", "Akali", "Darius", "Elise", "Evelynn",
                        "Fiddlesticks", "Fiora", "Fizz", "Galio", "Gangplank", "Gragas",
                        "Heimerdinger", "Irelia", "Jax", "Jayce", "Kassadin", "Kayle", "Kha'Zix",
                        "Lee Sin", "Lissandra", "Maokai", "Morgana", "Nocturne", "Nidalee",
                        "Pantheon", "Poppy", "RekSai", "Rengar", "Riven", "Rumble", "Ryze", "Shaco",
                        "Swain", "Trundle", "Tryndamere", "Udyr", "Urgot", "Vladimir", "Vi",
                        "XinZhao", "Yasuo", "Zilean"
                    };

                    string[] highPriority =
                    {
                        "Mordekaiser", "Rammus", "Ekko", "Diana", "Dragon", "Ahri", "Anivia",
                        "Annie", "Ashe", "Azir", "Brand", "Caitlyn", "Cassiopeia", "Corki",
                        "Draven", "Ezreal", "Graves", "Jinx", "Kalista", "Karma", "Karthus",
                        "Katarina", "Kennen", "KogMaw", "Leblanc", "Lucian", "Lux", "Malzahar",
                        "MasterYi", "MissFortune", "Orianna", "Quinn", "Sivir", "Syndra", "Talon",
                        "Teemo", "Tristana", "TwistedFate", "Twitch", "Varus", "Vayne", "Veigar",
                        "VelKoz", "Viktor", "Xerath", "Zed", "Ziggs"
                    };

                    if (lowPriority.Contains(championName))
                    {
                        return 1;
                    }

                    return highPriority.Contains(championName) ? 2 : 1;
                }
            }

            internal class Enemy
            {

                public static int GetPriority(string championName)
                {
                    string[] lowPriority =
                    {
                        "Alistar", "Amumu", "Bard", "Blitzcrank", "Braum", "Cho'Gath", "Dr. Mundo", "Garen", "Gnar",
                        "Hecarim", "Janna", "Jarvan IV", "Leona", "Lulu", "Malphite", "Nami", "Nasus", "Nautilus",
                        "Nunu", "Olaf", "Rammus", "Renekton", "Sejuani", "Shen", "Shyvana", "Singed", "Sion", "Skarner",
                        "Sona",
                        "Soraka", "Tahm", "Taric", "Thresh", "Volibear", "Warwick", "MonkeyKing", "Yorick", "Zac",
                        "Zyra"
                    };

                    string[] mediumPriority =
                    {
                        "Cassiopeia", "Azir", "Brand", "Ahri", "Anivia", "Annie", "Aatrox", "Akali", "Darius", "Diana",
                        "Ekko", "Elise", "Evelynn", "Fiddlesticks", "Fiora", "Fizz", "Galio", "Gangplank", "Gragas",
                        "Heimerdinger", "Irelia", "Jax", "Jayce", "Kassadin", "Kayle", "Kha'Zix", "Lee Sin", "Lissandra",
                        "Maokai", "Mordekaiser", "Morgana", "Nocturne", "Nidalee", "Pantheon", "Poppy", "RekSai",
                        "Rengar",
                        "Riven", "Rumble", "Ryze", "Shaco", "Swain", "Trundle", "Tryndamere", "Karma", "Karthus", "Udyr",
                        "Urgot", "Vladimir", "Vi", "XinZhao", "Yasuo", "Zilean", "Katarina", "Leblanc", "Lux",
                        "Malzahar",
                        "Syndra", "Talon", "MasterYi", "TwistedFate", "Veigar", "VelKoz", "Viktor", "Xerath", "Zed",
                        "Ziggs"
                    };

                    string[] highPriority =
                    {
                        "Ashe", "Caitlyn", "Corki", "Draven", "Ezreal", "Graves", "Jinx", "Kalista", "Kennen", "KogMaw",
                        "Lucian", "MissFortune", "Orianna", "Quinn", "Sivir", "Teemo", "Tristana", "Twitch", "Varus",
                        "Vayne"
                    };

                    if (lowPriority.Contains(championName))
                    {
                        return 1;
                    }

                    if (mediumPriority.Contains(championName))
                    {
                        return 2;
                    }

                    return highPriority.Contains(championName) ? 3 : 1;
                }
            }
        }

        public class MinionManager
        {
            public enum MinionTypes
            {
                All,
                Siege
            }

            public enum MobTypes
            {
                All,
                BigBoys
            }

            public static Vector2 GetCircularFarmLocation(Spell spell, MobTypes mobTypes = MobTypes.All,
                int minMobCount = 1)
            {
                var cVector = new Vector2();
                var rangedMinionsE = LeagueSharp.Common.MinionManager.GetMinions(ObjectManager.Player.ServerPosition,
                    spell.Range);

                var minionsE = spell.GetCircularFarmLocation(rangedMinionsE, spell.Range);
                if (minionsE.MinionsHit > minMobCount && spell.IsInRange(minionsE.Position))
                {
                    cVector = minionsE.Position;
                }

                return cVector;
            }

            public static Obj_AI_Minion GetOneMinionObject(float range, MinionTypes mobTypes = MinionTypes.All,
                int minMobCount = 1)
            {
                Obj_AI_Minion oneMinion = null;
                var minionsQ = LeagueSharp.Common.MinionManager.GetMinions(Player.Self.ServerPosition,
                    range);

                if (mobTypes == MinionTypes.Siege)
                {
                    var oMob = (from fMobs in minionsQ
                        from fBigBoys in
                            new[]
                            {
                                "SRU_ChaosMinionSiege", "SRU_ChaosMinionSuper"
                            }
                        where fBigBoys == fMobs.BaseSkinName && fMobs.Health > Player.Self.TotalAttackDamage
                        select fMobs).FirstOrDefault();

                    if (oMob != null)
                    {
                        if (oMob.IsValidTarget(range))
                        {
                            oneMinion = (Obj_AI_Minion) oMob;
                        }
                    }
                }
                else if (minionsQ.Count > 0)
                {
                    oneMinion = (Obj_AI_Minion) minionsQ[0];
                }

                return oneMinion;
            }

            public static Vector2 Minions(Spell spell, MobTypes mobTypes = MobTypes.All, int minMobCount = 1)
            {
                var rangedMinionsE = LeagueSharp.Common.MinionManager.GetMinions(
                    ObjectManager.Player.ServerPosition,
                    Spells.E.Range);

                var minionsE = Spells.E.GetCircularFarmLocation(rangedMinionsE, spell.Range);
                if (minionsE.MinionsHit < minMobCount || !spell.IsInRange(minionsE.Position))
                {
                    return new Vector2();
                }

                return minionsE.Position;
            }


            public static Obj_AI_Base GetMobs(float spellRange, MobTypes mobTypes = MobTypes.All, int minMobCount = 1)
            {
                var mobs = LeagueSharp.Common.MinionManager.GetMinions(spellRange + 200,
                    LeagueSharp.Common.MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

                if (mobs == null) return null;

                if (mobTypes == MobTypes.BigBoys)
                {
                    var oMob = (from fMobs in mobs
                        from fBigBoys in
                            new[]
                            {
                                "SRU_Blue", "SRU_Gromp", "SRU_Murkwolf", "SRU_Razorbeak", "SRU_Red",
                                "SRU_Krug", "SRU_Dragon", "SRU_Baron", "Sru_Crab"
                            }
                        where fBigBoys == fMobs.BaseSkinName
                        select fMobs).FirstOrDefault();

                    if (oMob != null)
                    {
                        if (oMob.IsValidTarget(spellRange))
                        {
                            return oMob;
                        }
                    }
                }
                else if (mobs.Count >= minMobCount)
                {
                    return mobs[0];
                }

                return null;
            }
        }

        public class Player
        {
            public static AIHeroClient Self
            {
                get { return Program.Player; }
            }

            public static float GetQTotalDamage
            {
                get
                {
                    var PlayerAD = Self.TotalAttackDamage; // 122

                    var PlayerAP = Self.TotalMagicalDamage; // 40

                    var bonusAD = PlayerAD*new[] {0.25, 0.263, 0.275, 0.288, 0.3}[Spells.Q.Level - 1];
                    var bonusAP = PlayerAP*0.2f;

                    var totalBonusDamage = bonusAD + bonusAP + new[] {4, 8, 12, 16, 20}[Spells.Q.Level - 1];
                    //Game.PrintChat(totalBonusDamage.ToString());
                    double[] multiplierPerLevel = {2, 2.25, 2.50, 2.75, 3};


                    //Game.PrintChat(multiplierPerLevel[Spells.Q.Level - 1].ToString());

                    var y = (float) (PlayerAD*multiplierPerLevel[Spells.Q.Level - 1]*2);
                    var z = 0; //y * (float)multiplierPerLevel[Spells.Q.Level - 1];


                    var totalQDamage = PlayerAD + y + z + totalBonusDamage;

                    return (float) totalQDamage;
                }
            }

            public static float AutoAttackRange
            {
                get { return Orbwalking.GetRealAutoAttackRange(null) + 65; }
            }

            public static void CastItems()
            {
            }
        }
    }
}