using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using LeagueSharp;
using LeagueSharp.Common;

namespace SephKhazix
{
    class KhazixMenu
    {
        public static Menu menu, harass, combo, farm, ks, safety, djump, draw, debug;

        public KhazixMenu()
        {
            menu = MainMenu.AddMenu("SephKhazix", "SephKhazix");

            //Harass
            harass = menu.AddSubMenu("Harass");
            harass.Add("UseQHarass", new CheckBox("Use Q"));
            harass.Add("UseWHarass", new CheckBox("Use W"));
            harass.Add("Harass.AutoWI", new CheckBox("Auto-W immobile"));
            harass.Add("Harass.AutoWD", new CheckBox("Auto W"));
            harass.Add("Harass.Key", new KeyBind("Harass key", false, KeyBind.BindTypes.PressToggle, 'H'));
            harass.Add("Harass.InMixed", new CheckBox("Harass in Mixed Mode", false));
            harass.Add("Harass.WHitchance", new ComboBox("W Hit Chance", 1, HitChance.Low.ToString(), HitChance.Medium.ToString(), HitChance.High.ToString()));


            //Combo
            combo = menu.AddSubMenu("Combo");
            combo.Add("UseQCombo", new CheckBox("Use Q"));
            combo.Add("UseWCombo", new CheckBox("Use W"));
            combo.Add("WHitchance", new ComboBox("W Hit Chance", 1, HitChance.Low.ToString(), HitChance.Medium.ToString(), HitChance.High.ToString()));
            combo.Add("UseECombo", new CheckBox("Use E"));
            combo.Add("UseEGapclose", new CheckBox("Use E To Gapclose for Q"));
            combo.Add("UseEGapcloseW", new CheckBox("Use E To Gapclose For W"));
            combo.Add("UseRGapcloseW", new CheckBox("Use R after long gapcloses"));
            combo.Add("UseRCombo", new CheckBox("Use R"));
            combo.Add("UseItems", new CheckBox("Use Items"));

            //Farm
            farm = menu.AddSubMenu("Farm");
            farm.Add("UseQFarm", new CheckBox("Use Q"));
            farm.Add("UseEFarm", new CheckBox("Use E"));
            farm.Add("UseWFarm", new CheckBox("Use W"));
            farm.Add("Farm.WHealth", new Slider("Health % to use W", 80, 0, 100));
            farm.Add("UseItemsFarm", new CheckBox("Use Items"));

            //Kill Steal
            ks = menu.AddSubMenu("KillSteal");
            ks.Add("Kson", new CheckBox("Use KillSteal"));
            ks.Add("UseQKs", new CheckBox("Use Q"));
            ks.Add("UseWKs", new CheckBox("Use W"));
            ks.Add("UseEKs", new CheckBox("Use E"));
            ks.Add("Ksbypass", new CheckBox("Bypass safety checks for E KS", false));
            ks.Add("UseEQKs", new CheckBox("Use EQ in KS"));
            ks.Add("UseEWKs", new CheckBox("Use EW in KS"));
            ks.Add("UseTiamatKs", new CheckBox("Use items"));
            ks.Add("Edelay", new Slider("E Delay (ms)", 0, 0, 300));

            // safety
            safety = menu.AddSubMenu("Safety Menu");
            safety.Add("Safety.Enabled", new CheckBox("Enable Safety Checks"));
            safety.Add("Safety.Override", new KeyBind("Safety Override Key", false, KeyBind.BindTypes.HoldActive, 'T'));
            safety.Add("Safety.autoescape", new CheckBox("Use E to get out when low"));
            safety.Add("Safety.CountCheck", new CheckBox("Min Ally ratio to Enemies to jump"));
            safety.Add("Safety.Ratio", new Slider("Ally:Enemy Ratio (/5)", 1, 0, 5));
            safety.Add("Safety.TowerJump", new CheckBox("Avoid Tower Diving"));
            safety.Add("Safety.MinHealth", new Slider("Healthy %", 15, 0, 100));
            safety.Add("Safety.noaainult", new CheckBox("No Autos while Stealth", false));

            //Double Jump
            djump = menu.AddSubMenu("Double Jumping");
            djump.Add("djumpenabled", new CheckBox("Enabled"));
            djump.Add("JEDelay", new Slider("Delay between jumps", 250, 250, 500));
            djump.Add("jumpmode", new ComboBox("Jump Mode", 0, "Default (jumps towards your nexus)", "Custom - Settings below"));
            djump.Add("save", new CheckBox("Save Double Jump Abilities", false));
            djump.Add("noauto", new CheckBox("Wait for Q instead of autos", false));
            djump.Add("jcursor", new CheckBox("Jump to Cursor (true) or false for script logic"));
            djump.Add("secondjump", new CheckBox("Do second Jump"));
            djump.Add("jcursor2", new CheckBox("Second Jump to Cursor (true) or false for script logic"));
            djump.Add("jumpdrawings", new CheckBox("Enable Jump Drawinsg"));


            //Drawings
            draw = menu.AddSubMenu("Drawings");
            draw.Add("Drawings.Disable", new CheckBox("Disable all", true));
            draw.Add("DrawQ", new CheckBox("Draw Q"));//, 0, System.Drawing.Color.White);
            draw.Add("DrawW", new CheckBox("Draw W"));//, 0, System.Drawing.Color.Red);
            draw.Add("DrawE", new CheckBox("Draw E"));//, 0, System.Drawing.Color.Green);

            //Debug
            debug = menu.AddSubMenu("Debug");
            debug.Add("Debugon", new CheckBox("Enable Debugging"));
        }

        public static bool getCheckBoxItem(Menu m, string item)
        {
            return m[item].Cast<CheckBox>().CurrentValue;
        }

        public static int getSliderItem(Menu m, string item)
        {
            return m[item].Cast<Slider>().CurrentValue;
        }

        public static bool getKeyBindItem(Menu m, string item)
        {
            return m[item].Cast<KeyBind>().CurrentValue;
        }

        public static int getBoxItem(Menu m, string item)
        {
            return m[item].Cast<ComboBox>().CurrentValue;
        }

        internal HitChance GetHitChance(string search)
        {
            var hitchance = getBoxItem(combo, "WHitchance");
            switch (hitchance)
            {
                case 0:
                    return HitChance.Low;
                case 1:
                    return HitChance.Medium;
                case 2:
                    return HitChance.High;
            }
            return HitChance.Medium;
        }
    }
}
