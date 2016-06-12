﻿// This file is part of LeagueSharp.Common.
// 
// LeagueSharp.Common is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// LeagueSharp.Common is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with LeagueSharp.Common.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using LeagueSharp.Common;
using SharpDX;
using SharpDX.Direct3D9;

namespace iDZed.Utils
{
    internal class AssassinManager
    {
        private static Font _text;
        private static Font _textBold;
        public static Menu assMenu;

        public AssassinManager()
        {
            Load();
        }

        private static void Load()
        {
            _textBold = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Tahoma",
                    Height = 13,
                    Weight = FontWeight.Bold,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.ClearType
                });
            _text = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Tahoma",
                    Height = 13,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.ClearType
                });

            assMenu = Zed.Menu.AddSubMenu(":: Deathmark Priority Targets", "MenuAssassin");

            assMenu.Add("AssassinActive", new CheckBox("Active"));
            assMenu.Add("AssassinSearchRange", new Slider(" Search Range", 1400, 1400, 2000));
            assMenu.Add("AssassinSelectOption", new ComboBox(" Set:", 0, "Single Select", "Multi Select"));

            assMenu.AddGroupLabel("Enemies :");
            foreach (
                var enemy in ObjectManager.Get<AIHeroClient>().Where(enemy => enemy.Team != ObjectManager.Player.Team))
            {
                assMenu.Add("Assassin" + enemy.ChampionName,
                    new CheckBox(" " + enemy.ChampionName, TargetSelector.GetPriority(enemy) > 3));
            }

            assMenu.AddGroupLabel("Other Settings :");
            assMenu.Add("AssassinSetClick", new CheckBox(" Add/Remove with click"));
            assMenu.Add("AssassinReset", new KeyBind(" Reset List", false, KeyBind.BindTypes.HoldActive, 'T'));

            assMenu.AddGroupLabel("Draw :");
            assMenu.Add("DrawSearch", new CheckBox("Search Range"));
            assMenu.Add("DrawActive", new CheckBox("Active Enemy"));
            assMenu.Add("DrawNearest", new CheckBox("Nearest Enemy"));
            assMenu.Add("DrawStatus", new CheckBox("Show status on the screen"));

            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnWndProc += Game_OnWndProc;
        }

        public static bool getCheckBoxItem(string item)
        {
            return assMenu[item].Cast<CheckBox>().CurrentValue;
        }

        public static int getSliderItem(string item)
        {
            return assMenu[item].Cast<Slider>().CurrentValue;
        }

        public static bool getKeyBindItem(string item)
        {
            return assMenu[item].Cast<KeyBind>().CurrentValue;
        }

        public static int getBoxItem(string item)
        {
            return assMenu[item].Cast<ComboBox>().CurrentValue;
        }

        private static void ClearAssassinList()
        {
            foreach (
                var enemy in ObjectManager.Get<AIHeroClient>().Where(enemy => enemy.Team != ObjectManager.Player.Team))
            {
                //assMenu.Item("Assassin" + enemy.ChampionName).SetValue(false);
                assMenu["Assassin" + enemy.ChampionName].Cast<CheckBox>().CurrentValue = false;
            }
        }

        private static void OnUpdate(EventArgs args)
        {
        }

        public static void DrawText(Font vFont, string vText, float vPosX, float vPosY, ColorBGRA vColor)
        {
            vFont.DrawText(null, vText, (int) vPosX, (int) vPosY, vColor);
        }

        public static void DrawTextBold(Font vFont, string vText, float vPosX, float vPosY, ColorBGRA vColor)
        {
            vFont.DrawText(null, vText, (int) vPosX, (int) vPosY, vColor);
        }

        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (getKeyBindItem("AssassinReset") && args.Msg == 257)
            {
                ClearAssassinList();
                Chat.Print(
                    "<font color='#FFFFFF'>Reset Assassin List is Complete! Click on the enemy for Add/Remove.</font>");
            }
            if (args.Msg != (uint) WindowsMessages.WM_LBUTTONDOWN)
            {
                return;
            }
            if (getCheckBoxItem("AssassinSetClick"))
            {
                foreach (var objAiHero in from hero in ObjectManager.Get<AIHeroClient>()
                    where hero.IsValidTarget()
                    select hero
                    into h
                    orderby h.LSDistance(Game.CursorPos) descending
                    select h
                    into enemy
                    where enemy.LSDistance(Game.CursorPos) < 150f
                    select enemy)
                {
                    if (objAiHero != null && objAiHero.IsVisible && !objAiHero.IsDead)
                    {
                        var xSelect = getBoxItem("AssassinSelectOption");
                        switch (xSelect)
                        {
                            case 0:
                                ClearAssassinList();
                                assMenu["Assassin" + objAiHero.ChampionName].Cast<CheckBox>().CurrentValue = true;
                                Chat.Print(
                                    string.Format(
                                        "<font color='FFFFFF'>Added to Assassin List</font> <font color='#09F000'>{0} ({1})</font>",
                                        objAiHero.Name, objAiHero.ChampionName));
                                break;
                            case 1:
                                var menuStatus = getCheckBoxItem("Assassin" + objAiHero.ChampionName);
                                assMenu["Assassin" + objAiHero.ChampionName].Cast<CheckBox>().CurrentValue = !menuStatus;
                                Chat.Print(
                                    string.Format(
                                        "<font color='{0}'>{1}</font> <font color='#09F000'>{2} ({3})</font>",
                                        !menuStatus ? "#FFFFFF" : "#FF8877",
                                        !menuStatus ? "Added to Assassin List:" : "Removed from Assassin List:",
                                        objAiHero.Name, objAiHero.ChampionName));
                                break;
                        }
                    }
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (!getCheckBoxItem("AssassinActive"))
            {
                return;
            }
            if (getCheckBoxItem("DrawStatus"))
            {
                var enemies = ObjectManager.Get<AIHeroClient>().Where(xEnemy => xEnemy.IsEnemy);
                var objAiHeroes = enemies as AIHeroClient[] ?? enemies.ToArray();
                DrawText(_textBold, "Target Mode:", Drawing.Width*0.89f, Drawing.Height*0.55f, Color.White);
                var xSelect = getBoxItem("AssassinSelectOption");
                DrawText(
                    _text, xSelect == 0 ? "Single Target" : "Multi Targets", Drawing.Width*0.94f,
                    Drawing.Height*0.55f, Color.White);
                DrawText(
                    _textBold, "Priority Targets", Drawing.Width*0.89f, Drawing.Height*0.58f, Color.White);
                DrawText(_textBold, "_____________", Drawing.Width*0.89f, Drawing.Height*0.58f, Color.White);
                for (var i = 0; i < objAiHeroes.Count(); i++)
                {
                    var xValue = getCheckBoxItem("Assassin" + objAiHeroes[i].ChampionName);
                    DrawTextBold(
                        xValue ? _textBold : _text, objAiHeroes[i].ChampionName, Drawing.Width*0.895f,
                        Drawing.Height*0.58f + (float) (i + 1)*15,
                        xValue ? Color.GreenYellow : Color.DarkGray);
                }
            }
            var drawSearch = getCheckBoxItem("DrawSearch");
            var drawActive = getCheckBoxItem("DrawActive");
            var drawNearest = getCheckBoxItem("DrawNearest");
            var drawSearchRange = getSliderItem("AssassinSearchRange");
            if (drawSearch)
            {
                Render.Circle.DrawCircle(ObjectManager.Player.Position, drawSearchRange,
                    System.Drawing.Color.GreenYellow, 1);
            }
            foreach (var enemy in
                ObjectManager.Get<AIHeroClient>()
                    .Where(enemy => enemy.Team != ObjectManager.Player.Team)
                    .Where(
                        enemy =>
                            enemy.IsVisible && assMenu["Assassin" + enemy.ChampionName] != null && !enemy.IsDead)
                    .Where(enemy => getCheckBoxItem("Assassin" + enemy.ChampionName)))
            {
                if (ObjectManager.Player.LSDistance(enemy) < drawSearchRange)
                {
                    if (drawActive)
                    {
                        Render.Circle.DrawCircle(enemy.Position, 115f, System.Drawing.Color.GreenYellow, 1);
                    }
                }
                else if (ObjectManager.Player.LSDistance(enemy) > drawSearchRange &&
                         ObjectManager.Player.LSDistance(enemy) < drawSearchRange + 400)
                {
                    if (drawNearest)
                    {
                        Render.Circle.DrawCircle(enemy.Position, 115f, System.Drawing.Color.DarkSeaGreen, 1);
                    }
                }
            }
        }
    }
}