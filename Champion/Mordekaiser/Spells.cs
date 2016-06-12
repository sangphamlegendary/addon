﻿using System;
using EloBuddy;
using LeagueSharp.Common;

namespace Mordekaiser
{
    public class Spells
    {
        public static Spell Q, W, E, R;

        //public static float WDamageRadius = 400f;
        public static int WCastedTime;

        public static float WDamageRadius
        {
            get { return Menu.getSliderItem(Menu.MenuW, "UseW.DamageRadius"); }
        }

        public static void Initiate()
        {
            Q = new Spell(SpellSlot.Q);

            W = new Spell(SpellSlot.W, 1000);
            W.SetTargetted(0.5f, 1500f);

            E = new Spell(SpellSlot.E, 670);
            E.SetSkillshot(0.25f, 12f*2*(float) Math.PI/180, 2000f, false, SkillshotType.SkillshotCone);

            R = new Spell(SpellSlot.R, 650);
            R.SetTargetted(0.5f, 1500f);
        }
    }
}