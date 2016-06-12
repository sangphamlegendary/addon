using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Common;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace iLucian.MenuHelper
{
    class MenuGenerator
    {

        public static Menu comboOptions, harassOptions, laneclearOptions, miscOptions, jungleclearOptions;

        public static void Generate()
        {
            Variables.Menu = MainMenu.AddMenu("iLucian", "com.ilucian");

            comboOptions = Variables.Menu.AddSubMenu(":: iLucian - Combo Options", "com.ilucian.combo");
            comboOptions.Add("com.ilucian.combo.q", new CheckBox("Use Q", true));
            comboOptions.Add("com.ilucian.combo.qExtended", new CheckBox("Use Extended Q", true));
            comboOptions.Add("com.ilucian.combo.w", new CheckBox("Use W", true));
            comboOptions.Add("com.ilucian.combo.e", new CheckBox("Use E", true));
            comboOptions.Add("com.ilucian.combo.startE", new CheckBox("Start Combo With E", true));
            comboOptions.Add("com.ilucian.combo.eRange", new Slider("E Dash Range", 65, 50, 475));
            comboOptions.Add("com.ilucian.combo.eMode", new ComboBox("E Mode", 5, "Kite", "Side", "Cursor", "Enemy", "Fast Mode", "Smart E"));
            comboOptions.Add("com.ilucian.combo.forceR", new KeyBind("Semi Ult Key", false, KeyBind.BindTypes.HoldActive, 'T'));

            harassOptions = Variables.Menu.AddSubMenu(":: iLucian - Harass Options", "com.ilucian.harass");
            harassOptions.AddGroupLabel("Auto Harass : ");
            harassOptions.Add("com.ilucian.harass.auto.autoharass", new KeyBind("Enabled", false, KeyBind.BindTypes.PressToggle, 'Z'));
            harassOptions.Add("com.ilucian.harass.auto.q", new CheckBox("Use Q", true));
            harassOptions.Add("com.ilucian.harass.auto.qExtended", new CheckBox("Use Extended Q", true));
            harassOptions.Add("com.ilucian.harass.auto.autoharass.mana", new Slider("Min Mana % for auto harasss", 70, 0, 100));
            harassOptions.AddSeparator();
            harassOptions.Add("com.ilucian.harass.whitelist", new CheckBox("Extended Q Whitelist", true));            
                foreach (var hero in HeroManager.Enemies)
                {
                  harassOptions.Add("com.ilucian.harass.whitelist." + hero.NetworkId, new CheckBox("Don't Q: " + hero.ChampionName));
                }
            
            harassOptions.AddGroupLabel("Harass : ");
            harassOptions.Add("com.ilucian.harass.q", new CheckBox("Use Q", true));
            harassOptions.Add("com.ilucian.harass.qExtended", new CheckBox("Use Extended Q", true));
            harassOptions.Add("com.ilucian.harass.w", new CheckBox("Use W", true));
            harassOptions.Add("com.ilucian.harass.mana", new Slider("Min Mana % for harass", 70, 0, 100));

            laneclearOptions = Variables.Menu.AddSubMenu(":: iLucian - Laneclear Options", "com.ilucian.laneclear");
            laneclearOptions.Add("com.ilucian.laneclear.q", new CheckBox("Use Q", true));
            laneclearOptions.Add("com.ilucian.laneclear.mana", new Slider("Min Mana % for laneclear", 70, 0, 100));
            laneclearOptions.Add("com.ilucian.laneclear.qMinions", new Slider("Cast Q on x minions", 3, 1, 10));

            jungleclearOptions = Variables.Menu.AddSubMenu(":: iLucian - Jungleclear Options", "com.ilucian.jungleclear");
            jungleclearOptions.Add("com.ilucian.jungleclear.q", new CheckBox("Use Q", true));
            jungleclearOptions.Add("com.ilucian.jungleclear.w", new CheckBox("Use W", true));
            jungleclearOptions.Add("com.ilucian.jungleclear.e", new CheckBox("Use E", true));
            jungleclearOptions.Add("com.ilucian.jungleclear.mana", new Slider("Min Mana % for jungleclear", 70, 0, 100));


            miscOptions = Variables.Menu.AddSubMenu(":: iLucian - Misc Options", "com.ilucian.misc");
            miscOptions.Add("com.ilucian.misc.antiVayne", new CheckBox("Anti Vayne Condemn", true));
            miscOptions.Add("com.ilucian.misc.antiMelee", new CheckBox("Anti Melee (E)", true));
            miscOptions.Add("com.ilucian.misc.usePrediction", new CheckBox("Use W Pred", true));
            miscOptions.Add("com.ilucian.misc.forcePassive", new CheckBox("Force Passive Target", true));
            miscOptions.Add("com.ilucian.misc.gapcloser", new CheckBox("Use E For Gapcloser", true));
            miscOptions.Add("com.ilucian.misc.eqKs", new CheckBox("EQ - Killsteal", true));
            miscOptions.Add("com.ilucian.misc.useChampions", new CheckBox("Use EQ on Champions", true));
            miscOptions.Add("com.ilucian.misc.extendChamps", new CheckBox("Use Ext Q on Champions", true));
            miscOptions.Add("com.ilucian.misc.drawQ", new CheckBox("Draw Ext Q Range", true));
        }
    }
}