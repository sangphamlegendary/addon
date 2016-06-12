using System.Reflection;
using System.Linq;
using LeagueSharp.Common;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK;
using Valvrave_Sharp.Core;
using Valvrave_Sharp.Evade;

namespace YasuoPro
{
    internal static class YasuoMenu
    {
        internal static Menu Config, ComboM, HarassM, KillstealM, FarmingM, WaveclearM, MiscM, DrawingsM;
        internal static Yasuo Yas;

        public static void Init(Yasuo yas)
        {
            Yas = yas;

            Config = MainMenu.AddMenu("YasuoPro", "YasuoPro");

            ComboM = Config.AddSubMenu("Combo");
            YasuoMenu.Combo.Attach(ComboM);

            HarassM = Config.AddSubMenu("Harass");
            YasuoMenu.Harass.Attach(HarassM);

            KillstealM = Config.AddSubMenu("Killsteal");
            YasuoMenu.Killsteal.Attach(KillstealM);

            FarmingM = Config.AddSubMenu("LastHitting");
            YasuoMenu.Farm.Attach(FarmingM);

            WaveclearM = Config.AddSubMenu("Waveclear");
            YasuoMenu.Waveclear.Attach(WaveclearM);

            MiscM = Config.AddSubMenu("Misc");
            YasuoMenu.Misc.Attach(MiscM);

            DrawingsM = Config.AddSubMenu("Drawings");
            YasuoMenu.Drawings.Attach(DrawingsM);
        }

        struct Combo
        {
            internal static void Attach(Menu menu)
            {
                menu.AddGroupLabel("Items");
                menu.AddBool("Items.Enabled", "Use Items");
                menu.AddBool("Items.UseTIA", "Use Tiamat");
                menu.AddBool("Items.UseHDR", "Use Hydra");
                menu.AddBool("Items.UseBRK", "Use BORK");
                menu.AddBool("Items.UseBLG", "Use Bilgewater");
                menu.AddBool("Items.UseYMU", "Use Youmu");
                menu.AddSeparator();
                menu.AddGroupLabel("Combo :");
                menu.AddBool("Combo.UseQ", "Use Q");
                menu.AddBool("Combo.UseQ2", "Use Q2");
                menu.AddBool("Combo.StackQ", "Stack Q if not in Range");
                menu.AddBool("Combo.UseW", "Use W");
                menu.AddBool("Combo.UseE", "Use E");
                menu.AddBool("Combo.UseEQ", "Use EQ");
                menu.AddBool("Combo.ETower", "Use E under Tower", false);
                menu.AddBool("Combo.EAdvanced", "Predict E position with Waypoints");
                menu.AddBool("Combo.NoQ2Dash", "Dont Q2 while dashing", false);
                menu.AddBool("Combo.UseIgnite", "Use Ignite");
                menu.AddSeparator();
                menu.AddGroupLabel("Ult Settings ");
                foreach (var hero in HeroManager.Enemies)
                {
                    menu.AddBool("ult" + hero.ChampionName, "Ult " + hero.ChampionName);
                }
                menu.AddSeparator();
                menu.AddSList("Combo.UltMode", "Ult Prioritization", new string[] { "Lowest Health", "TS Priority", "Most enemies" }, 0);
                menu.AddSlider("Combo.knockupremainingpct", "Knockup Remaining % for Ult", 95, 40, 100);
                menu.AddBool("Combo.UseR", "Use R");
                menu.AddBool("Combo.UltTower", "Ult under Tower", false);
                menu.AddBool("Combo.UltOnlyKillable", "Ult only Killable", false);
                menu.AddBool("Combo.RPriority", "Ult if priority 5 target is knocked up", true);
                menu.AddSeparator();
                menu.AddSlider("Combo.RMinHit", "Min Enemies for Ult", 1, 1, 5);
                menu.AddBool("Combo.OnlyifMin", "Only Ult if minimum enemies met", false);
                menu.AddSeparator();
                menu.AddSlider("Combo.MinHealthUlt", "Minimum health to Ult %", 0, 0, 100);
            }
        }

        struct Harass
        {
            internal static void Attach(Menu menu)
            {
                menu.AddKeyBind("Harass.KB", "Harass Key", KeyCode("H"), EloBuddy.SDK.Menu.Values.KeyBind.BindTypes.PressToggle);
                menu.AddSeparator();
                menu.AddBool("Harass.InMixed", "Harass in Mixed Mode", false);
                menu.AddBool("Harass.UseQ", "Use Q");
                menu.AddBool("Harass.UseQ2", "Use Q2");
                menu.AddBool("Harass.UseE", "Use E", false);
                menu.AddBool("Harass.UseEMinion", "Use E Minions", false);
            }
        }

        struct Farm
        {
            internal static void Attach(Menu menu)
            {
                menu.AddBool("Farm.UseQ", "Use Q");
                menu.AddBool("Farm.UseQ2", "Use Q - Tornado");
                menu.AddSlider("Farm.Qcount", "Minions for Q (Tornado)", 1, 1, 10);
                menu.AddBool("Farm.UseE", "Use E");
            }
        }


        struct Waveclear
        {
            internal static void Attach(Menu menu)
            {
                menu.AddGroupLabel("Items");
                menu.AddBool("Waveclear.UseItems", "Use Items");
                menu.AddSlider("Waveclear.MinCountHDR", "Minion count for Cleave", 2, 1, 10);
                menu.AddSlider("Waveclear.MinCountYOU", "Minion count for Youmu", 2, 1, 10);
                menu.AddBool("Waveclear.UseTIA", "Use Tiamat");
                menu.AddBool("Waveclear.UseHDR", "Use Hydra");
                menu.AddBool("Waveclear.UseYMU", "Use Youmu", false);
                menu.AddSeparator();
                menu.AddGroupLabel("Wave Clear :");
                menu.AddBool("Waveclear.UseQ", "Use Q");
                menu.AddBool("Waveclear.UseQ2", "Use Q - Tornado");
                menu.AddSlider("Waveclear.Qcount", "Minions for Q (Tornado)", 1, 1, 10);
                menu.AddSeparator();
                menu.AddBool("Waveclear.UseE", "Use E");
                menu.AddSlider("Waveclear.Edelay", "Delay for E", 0, 0, 400);
                menu.AddBool("Waveclear.ETower", "Use E under Tower", false);
                menu.AddBool("Waveclear.UseENK", "Use E even if not killable");
                menu.AddSeparator();
                menu.AddBool("Waveclear.Smart", "Smart Waveclear");
            }
        }

        struct Killsteal
        {
            internal static void Attach(Menu menu)
            {
                menu.AddBool("Killsteal.Enabled", "KillSteal");
                menu.AddBool("Killsteal.UseQ", "Use Q");
                menu.AddBool("Killsteal.UseE", "Use E");
                menu.AddBool("Killsteal.UseR", "Use R");
                menu.AddBool("Killsteal.UseIgnite", "Use Ignite");
                menu.AddBool("Killsteal.UseItems", "Use Items");
            }
        }

        struct Misc
        {
            internal static void Attach(Menu menu)
            {
                menu.AddGroupLabel("Flee");
                menu.AddSList("Flee.Mode", "Flee Mode", new[] { "To Nexus", "To Allies", "To Cursor" }, 2);
                menu.AddBool("Flee.Smart", "Smart Flee", true);
                menu.AddBool("Flee.StackQ", "Stack Q during Flee");
                menu.AddBool("Flee.UseQ2", "Use Tornado", false);
                menu.AddSeparator();
                menu.AddGroupLabel("Misc :");
                menu.AddBool("Misc.SafeE", "Safety Check for E");
                menu.AddBool("Misc.AutoStackQ", "Auto Stack Q", false);
                menu.AddBool("Misc.AutoR", "Auto Ultimate");
                menu.AddSlider("Misc.RMinHit", "Min Enemies for Autoult", 1, 1, 5);
                menu.AddSeparator();
                menu.AddKeyBind("Misc.TowerDive", "Tower Dive Key", KeyCode("T"), EloBuddy.SDK.Menu.Values.KeyBind.BindTypes.HoldActive);
                menu.AddSeparator();
                menu.AddSList("Hitchance.Q", "Q Hitchance", new[] { HitChance.Low.ToString(), HitChance.Medium.ToString(), HitChance.High.ToString(), HitChance.VeryHigh.ToString() }, 2);
                menu.AddSlider("Misc.Healthy", "Healthy Amount HP", 5, 0, 100);
                menu.AddSeparator();
                menu.AddBool("Misc.AG", "Use Q (Tornado) on Gapcloser");
                menu.AddBool("Misc.Interrupter", "Use Q (Tornado) to Interrupt");
                menu.AddBool("Misc.Walljump", "Use Walljump", false);
                menu.AddBool("Misc.Debug", "Debug", false);
            }
        }

        struct Drawings
        {
            internal static void Attach(Menu menu)
            {
                menu.AddBool("Drawing.Disable", "Disable Drawings", true);
                menu.AddCircle("Drawing.DrawQ", "Draw Q");//, Yas.Qrange, System.Drawing.Color.Red);
                menu.AddCircle("Drawing.DrawE", "Draw E");//, Yas.Erange, System.Drawing.Color.CornflowerBlue);
                menu.AddCircle("Drawing.DrawR", "Draw R");//, Yas.Rrange, System.Drawing.Color.DarkOrange);
                menu.AddBool("Drawing.SS", "Draw Skillshot Drawings", false);
            }
        }

        internal static uint KeyCode(string s)
        {
            return s.ToCharArray()[0];
        }
    }
}
