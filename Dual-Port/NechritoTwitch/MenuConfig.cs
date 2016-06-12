using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using LeagueSharp;
using LeagueSharp.Common;

namespace Nechrito_Twitch
{
    class MenuConfig
    {
        public static Menu Config, combo, harass, lane, draw;
        public static string menuName = "Nechrito Twitch";

        public static void LoadMenu()
        {
            Config = MainMenu.AddMenu(menuName, menuName);

            combo = Config.AddSubMenu("Combo", "Combo");
            combo.Add("UseW", new CheckBox("Use W"));
            combo.Add("KsE", new CheckBox("Ks E"));

            harass = Config.AddSubMenu("Harass", "Harass");
            harass.Add("harassW", new CheckBox("Use W"));
            harass.Add("ESlider", new Slider("E Stack When Out Of AA Range", 0, 0, 6));

            lane = Config.AddSubMenu("Lane", "Lane");
            lane.Add("laneW", new CheckBox("Use W"));

            draw = Config.AddSubMenu("Draw", "Draw");
            draw.Add("dind", new CheckBox("Dmg Indicator"));
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

        // Menu Items
        public static bool UseW => getCheckBoxItem(combo, "UseW");
        public static bool KsE => getCheckBoxItem(combo, "KsE");
        public static bool laneW => getCheckBoxItem(lane, "laneW");
        public static bool harassW => getCheckBoxItem(harass, "harassW");
        public static bool dind => getCheckBoxItem(draw, "dind");
        public static int ESlider => getSliderItem(harass, "ESlider");
    }
}
