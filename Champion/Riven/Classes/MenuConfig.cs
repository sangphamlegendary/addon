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

namespace NechritoRiven
{
    class MenuConfig
    {
        public static Menu Config, animation, combo, lane, jngl, misc, draw, flee;

        public static string menuName = "Nechrito Riven";

        public static void LoadMenu()
        {
            Config = MainMenu.AddMenu(menuName, menuName);

            animation = Config.AddSubMenu("Animation", "Animation");
            animation.Add("qReset", new CheckBox("Fast & Legit Q"));
            animation.AddSeparator();
            animation.Add("Qstrange", new CheckBox("Enable", false));
            animation.Add("animLaugh", new CheckBox("Laugh", false));
            animation.Add("animTaunt", new CheckBox("Taunt", false));
            animation.Add("animTalk", new CheckBox("Joke", false));
            animation.Add("animDance", new CheckBox("Dance", false));

            combo = Config.AddSubMenu("Combo", "Combo");
            combo.Add("ignite", new CheckBox("Auto Ignite"));
            combo.Add("Burst", new KeyBind("Force Burst", false, KeyBind.BindTypes.PressToggle, 'T'));
            combo.Add("AlwaysR", new KeyBind("Force R", false, KeyBind.BindTypes.PressToggle, 'G'));
            combo.Add("AlwaysF", new KeyBind("Force Flash", false, KeyBind.BindTypes.PressToggle, 'L'));

            lane = Config.AddSubMenu("Lane", "Lane");
            lane.Add("FastC", new CheckBox("Fast Laneclear", false));
            lane.Add("LaneQ", new CheckBox("Use Q"));
            lane.Add("LaneW", new CheckBox("Use W"));
            lane.Add("LaneE", new CheckBox("Use E"));

            jngl = Config.AddSubMenu("Jungle", "Jungle");
            jngl.Add("JungleQ", new CheckBox("Use Q"));
            jngl.Add("JungleW", new CheckBox("Use W"));
            jngl.Add("JungleE", new CheckBox("Use E"));

            misc = Config.AddSubMenu("Misc", "Misc");
            misc.Add("KeepQ", new CheckBox("Keep Q Alive"));
            misc.Add("FHarass", new KeyBind("Fast Harass", false, KeyBind.BindTypes.PressToggle, 'J'));
            misc.Add("QD", new Slider("Ping Delay", 56, 20, 300));
            misc.Add("QLD", new Slider("Spell Delay", 56, 20, 300));

            draw = Config.AddSubMenu("Draw", "Draw");
            draw.Add("FleeSpot", new CheckBox("Draw Flee Spots"));
            draw.Add("Dind", new CheckBox("Damage Indicator"));
            draw.Add("DrawForceFlash", new CheckBox("Flash Status"));
            draw.Add("DrawAlwaysR", new CheckBox("R Status"));
            draw.Add("DrawTimer1", new CheckBox("Draw Q Expiry Time", false));
            draw.Add("DrawTimer2", new CheckBox("Draw R Expiry Time", false));
            draw.Add("DrawCB", new CheckBox("Combo Engage", false));
            draw.Add("DrawBT", new CheckBox("Burst Engage", false));
            draw.Add("DrawFH", new CheckBox("FastHarass Engage", false));
            draw.Add("DrawHS", new CheckBox("Harass Engage", false));

            flee = Config.AddSubMenu("Flee", "Flee");
            flee.Add("WallFlee", new CheckBox("WallJump in Flee"));
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

        public static bool fastHar => getKeyBindItem(misc, "FHarass");
        public static bool burst => getKeyBindItem(combo, "Burst");
        public static bool FastC => getCheckBoxItem(lane, "FastC");
        public static bool FleeSpot => getCheckBoxItem(draw, "FleeSpot");
        public static bool WallFlee => getCheckBoxItem(flee, "WallFlee");
        public static bool jnglQ => getCheckBoxItem(jngl, "JungleQ");
        public static bool jnglW => getCheckBoxItem(jngl, "JungleW");
        public static bool jnglE => getCheckBoxItem(jngl, "JungleE");
        public static bool AlwaysF => getKeyBindItem(combo, "AlwaysF");
        public static bool ignite => getCheckBoxItem(combo, "ignite");
        public static bool ForceFlash => getCheckBoxItem(draw, "DrawForceFlash");
        public static bool QReset => getCheckBoxItem(animation, "qReset");
        public static bool Dind => getCheckBoxItem(draw, "Dind");
        public static bool DrawCb => getCheckBoxItem(draw, "DrawCB");
        public static bool AnimLaugh => getCheckBoxItem(animation, "animLaugh");
        public static bool AnimTaunt => getCheckBoxItem(animation, "animTaunt");
        public static bool AnimDance => getCheckBoxItem(animation, "animDance");
        public static bool AnimTalk => getCheckBoxItem(animation, "animTalk");
        public static bool DrawAlwaysR => getCheckBoxItem(draw, "DrawAlwaysR");
        public static bool KeepQ => getCheckBoxItem(misc, "KeepQ");
        public static bool DrawFh => getCheckBoxItem(draw, "DrawFH");
        public static bool DrawTimer1 => getCheckBoxItem(draw, "DrawTimer1");
        public static bool DrawTimer2 => getCheckBoxItem(draw, "DrawTimer2");
        public static bool DrawHs => getCheckBoxItem(draw, "DrawHS");
        public static bool DrawBt => getCheckBoxItem(draw, "DrawBT");
        public static bool AlwaysR => getKeyBindItem(combo, "AlwaysR");
        public static int Qd => getSliderItem(misc, "QD");
        public static int Qld => getSliderItem(misc, "QLD");
        public static bool LaneW => getCheckBoxItem(lane, "LaneW");
        public static bool LaneE => getCheckBoxItem(lane, "LaneE");
        public static bool Qstrange => getCheckBoxItem(animation, "Qstrange");
        public static bool LaneQ => getCheckBoxItem(lane, "LaneQ");
    }
}