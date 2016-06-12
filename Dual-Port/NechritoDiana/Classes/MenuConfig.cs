using ClipperLib;
using Color = System.Drawing.Color;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK;
using EloBuddy;
using Font = SharpDX.Direct3D9.Font;
using LeagueSharp.Common.Data;
using LeagueSharp.Common;
using SharpDX.Direct3D9;
using SharpDX;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Security.AccessControl;
using System;
using System.Speech.Synthesis;
namespace Nechrito_Diana
{
    class MenuConfig
    {
        public static Menu Config, combo,lane, jungle, killsteal, misc, draw, flee;
        public static string menuName = "Nechrito Diana";
        public static void LoadMenu()
        {
            Config = MainMenu.AddMenu(menuName, menuName);
            
            combo = Config.AddSubMenu("Combo", "Combo");
            combo.Add("ComboR", new CheckBox("Only R if hit by Q", false));
            combo.Add("ComboE", new CheckBox("Smart E", false));
            combo.Add("Misaya", new CheckBox("Force Misaya", false));
            combo.Add("AutoSmite", new CheckBox("Use Smite"));
            
            lane = Config.AddSubMenu("Lane", "Lane");
            lane.Add("LaneQ", new CheckBox("Use Q", false));
            lane.Add("LaneW", new CheckBox("Use W"));

            jungle = Config.AddSubMenu("Jungle", "Jungle");
            jungle.Add("jnglQR", new CheckBox("QR Gapclose"));
            jungle.Add("jnglE", new Slider("E Interrupt", 15, 0, 50));
            jungle.Add("jnglW", new CheckBox("Use W"));

            killsteal = Config.AddSubMenu("Killsteal", "Killsteal");
            killsteal.Add("ksQ", new CheckBox("Killsteal Q"));
            killsteal.Add("ksR", new CheckBox("Killsteal R"));
            killsteal.Add("ignite", new CheckBox("Auto Ignite"));
            killsteal.Add("ksSmite", new CheckBox("Auto Smite"));

            misc = Config.AddSubMenu("Misc", "Misc");
            misc.Add("Gapcloser", new CheckBox("Gapcloser"));
            misc.Add("Interrupt", new CheckBox("Interrupt"));

            draw = Config.AddSubMenu("Draw", "Draw");
            draw.Add("EngageDraw", new CheckBox("Engage Range"));
            draw.Add("EscapeSpot", new CheckBox("Escape Spot"));

            flee = Config.AddSubMenu("Flee", "Flee");
            flee.Add("FleeMouse", new KeyBind("Flee", false, KeyBind.BindTypes.HoldActive, 'A'));

            SPrediction.Prediction.Initialize(Config);
        }

        public static bool ComboR => getCheckBoxItem(combo, "ComboR");
        public static bool ComboE => getCheckBoxItem(combo, "ComboE");
        public static bool EscapeSpot => getCheckBoxItem(draw, "EscapeSpot");
        public static bool EngageDraw => getCheckBoxItem(draw, "EngageDraw");
        public static bool Misaya => getCheckBoxItem(combo, "Misaya");
        public static bool LaneQ => getCheckBoxItem(lane, "LaneQ");
        public static bool LaneW => getCheckBoxItem(lane, "LaneW");
        public static bool ksSmite => getCheckBoxItem(killsteal, "ksSmite");
        public static bool ksQ => getCheckBoxItem(killsteal, "ksQ");
        public static bool ksR => getCheckBoxItem(killsteal, "ksR");
        public static bool Interrupt => getCheckBoxItem(misc, "Interrupt");
        public static bool Gapcloser => getCheckBoxItem(misc, "Gapcloser");
        public static bool jnglW => getCheckBoxItem(jungle, "jnglW");
        public static bool jnglQR => getCheckBoxItem(jungle, "jnglQR");
        public static bool ignite => getCheckBoxItem(killsteal, "ignite");
        public static bool AutoSmite => getCheckBoxItem(combo, "AutoSmite");
        public static bool FleeMouse => getKeyBindItem(flee, "FleeMouse");
        public static int jnglE => getSliderItem(jungle, "jnglE");

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
    }
}
