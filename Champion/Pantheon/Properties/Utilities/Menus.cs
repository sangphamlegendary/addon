using EloBuddy.SDK.Menu.Values;
using ExorSDK.Utilities;

namespace ExorSDK.Champions.Pantheon
{
    /// <summary>
    ///     The menu class.
    /// </summary>
    internal class Menus
    {
        /// <summary>
        ///     Sets the menu.
        /// </summary>
        public static void Initialize()
        {
            /// <summary>
            ///     Sets the menu for the Q.
            /// </summary>
            Vars.QMenu = Vars.Menu.AddSubMenu("Use Q to:");
            {
                Vars.QMenu.Add("combo", new CheckBox("Combo"));
                Vars.QMenu.Add("killsteal", new CheckBox("KillSteal"));
                Vars.QMenu.Add("harass", new Slider("Harass / if Mana >= x%", 50, 10, 101));
                Vars.QMenu.Add("jungleclear", new Slider("JungleClear / if Mana >= x%", 50, 10, 101));
            }

            /// <summary>
            ///     Sets the menu for the W.
            /// </summary>
            Vars.WMenu = Vars.Menu.AddSubMenu("Use W to:");
            {
                Vars.WMenu.Add("combo", new CheckBox("Combo"));
                Vars.WMenu.Add("killsteal", new CheckBox("KillSteal"));
                Vars.WMenu.Add("interrupter", new CheckBox("Interrupt Enemy Channels"));
            }

            /// <summary>
            ///     Sets the menu for the E.
            /// </summary>
            Vars.EMenu = Vars.Menu.AddSubMenu("Use E to:");
            {
                Vars.EMenu.Add("combo", new CheckBox("Combo"));
                Vars.EMenu.Add("clear", new Slider("Clear / if Mana >= x%", 50, 10, 101));
            }

            /// <summary>
            ///     Sets the drawings menu.
            /// </summary>
            Vars.DrawingsMenu = Vars.Menu.AddSubMenu("Drawings");
            {
                Vars.DrawingsMenu.Add("q", new CheckBox("Q Range"));
                Vars.DrawingsMenu.Add("w", new CheckBox("W Range"));
                Vars.DrawingsMenu.Add("e", new CheckBox("E Range"));
            }
        }
    }
}