using System;
using System.Collections.Generic;
using ClipperLib;
using Color = System.Drawing.Color;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK;
using EloBuddy;
using LeagueSharp.Common.Data;
using LeagueSharp.Common;
using SharpDX;
using System.Linq;
using FioraProject.Evade;

namespace FioraProject
{
    using static FioraPassive;
    using static GetTargets;
    using static Combos;
    public static class Program
    {
        public static AIHeroClient Player { get { return ObjectManager.Player; } }

        public static LeagueSharp.Common.Spell Q, W, E, R;

        public static Menu Menu, Harass, Combo, Target, PriorityMode, OptionalMode, SelectedMode, LaneClear, JungClear, Draw, Misc;

        public static void Game_OnGameLoad()
        {
            if (Player.ChampionName != "Fiora")
                return;
            Q = new LeagueSharp.Common.Spell(SpellSlot.Q, 400);
            W = new LeagueSharp.Common.Spell(SpellSlot.W, 750);
            E = new LeagueSharp.Common.Spell(SpellSlot.E);
            R = new LeagueSharp.Common.Spell(SpellSlot.R);
            W.SetSkillshot(0.75f, 80, 2000, false, SkillshotType.SkillshotLine);
            W.MinHitChance = LeagueSharp.Common.HitChance.High;


            Menu = MainMenu.AddMenu("Project" + Player.ChampionName, Player.ChampionName);

            Harass = Menu.AddSubMenu("Harass", "Harass");
            Harass.Add("Use Q Harass", new CheckBox("Q Enable"));
            Harass.Add("Use Q Harass Gap", new CheckBox("Use Q to gapclose"));
            Harass.Add("Use Q Harass Pre Pass", new CheckBox("Use Q to hit pre-passive spot"));
            Harass.Add("Use Q Harass Pass", new CheckBox("Use Q to hit passive"));
            Harass.Add("Use E Harass", new CheckBox("E Enable"));
            Harass.Add("Mana Harass", new Slider("Mana Harass", 40, 0, 100));

            Combo = Menu.AddSubMenu("Combo", "Combo");
            Combo.Add("Orbwalker2Mouse", new KeyBind("Combo Orbwalk to Passive", false, KeyBind.BindTypes.HoldActive, 'Z'));
            Combo.Add("Use Q Combo", new CheckBox("Q Enable"));
            Combo.Add("Use Q Combo Gap", new CheckBox("Use Q to gapclose"));
            Combo.Add("Use Q Combo Pre Pass", new CheckBox("Use Q to hit pre-passive spot"));
            Combo.Add("Use Q Combo Pass", new CheckBox("Use Q to hit passive"));
            Combo.Add("Use Q Combo Gap Minion", new CheckBox("Use Q minion to gapclose", false));
            Combo.Add("Use Q Combo Gap Minion Value", new Slider("Q minion gapclose if % cdr >=", 25, 0, 40));
            Combo.Add("Use E Combo", new CheckBox("E Enable"));
            Combo.Add("Use R Combo", new CheckBox("R Enable"));
            Combo.Add("Use R Combo LowHP", new CheckBox("Use R LowHP"));
            Combo.Add("Use R Combo LowHP Value", new Slider("R LowHP if player hp <", 40, 0, 100));
            Combo.Add("Use R Combo Killable", new CheckBox("Use R Killable"));
            Combo.Add("Use R Combo On Tap", new CheckBox("Use R on Tap"));
            Combo.Add("Use R Combo On Tap Key", new KeyBind("R on Tap key", false, KeyBind.BindTypes.HoldActive, 'G'));
            Combo.Add("Use R Combo Always", new CheckBox("Use R Always", false));

            Target = Menu.AddSubMenu("Targeting Modes", "Targeting Modes");
            Target.Add("Targeting Mode", new ComboBox("Targeting Mode", 0, "Optional", "Selected", "Priority", "Normal"));
            Target.Add("Orbwalk To Passive Range", new Slider("Orbwalk To Passive Range", 300, 250, 500));
            Target.Add("Focus Ulted Target", new CheckBox("Focus Ulted Target", false));
            Target.AddLabel("Go in each Mode menu to customize what you want!");
            Target.AddLabel("Please remember Orbwalk to Passive spot only works");
            Target.AddLabel("in \" Combo Orbwalk to Passive\" mode can be found");
            Target.AddLabel("in misc menu!");

            PriorityMode = Menu.AddSubMenu("Priority", "Priority Mode");
            PriorityMode.Add("Priority Range", new Slider("Priority Range", 1000, 300, 1000));
            PriorityMode.Add("Priority Orbwalk to Passive", new CheckBox("Orbwalk to Passive"));
            PriorityMode.Add("Priority Under Tower", new CheckBox("Under Tower"));
            foreach (var hero in HeroManager.Enemies)
            {
                PriorityMode.Add("Priority" + hero.ChampionName, new Slider(hero.ChampionName, 2, 1, 5));
            }

            OptionalMode = Menu.AddSubMenu("Optional", "Optional Mode");
            OptionalMode.Add("Optional Range", new Slider("Optional Range", 1000, 300, 1000));
            OptionalMode.Add("Optional Orbwalk to Passive", new CheckBox("Orbwalk to Passive"));
            OptionalMode.Add("Optional Under Tower", new CheckBox("Under Tower", false));
            OptionalMode.Add("Optional Switch Target Key", new KeyBind("Switch Target Key", false, KeyBind.BindTypes.HoldActive, 'T'));
            OptionalMode.AddLabel("Also Can Left-click the target to switch!");

            SelectedMode = Menu.AddSubMenu("Selected", "Selected Mode");
            SelectedMode.Add("Selected Range", new Slider("Selected Range", 1000, 300, 1000));
            SelectedMode.Add("Selected Orbwalk to Passive", new CheckBox("Orbwalk to Passive"));
            SelectedMode.Add("Selected Under Tower", new CheckBox("Under Tower", false));
            SelectedMode.Add("Selected Switch If No Selected", new CheckBox("Switch to Optional if no target"));

            LaneClear = Menu.AddSubMenu("Lane Clear", "Lane Clear");
            LaneClear.Add("Use E LClear", new CheckBox("E Enable"));
            LaneClear.Add("Use Timat LClear", new CheckBox("Tiamat Enable"));
            LaneClear.Add("minimum Mana LC", new Slider("minimum Mana", 40, 0, 100));

            JungClear = Menu.AddSubMenu("Jungle Clear", "Jungle Clear");
            JungClear.Add("Use E JClear", new CheckBox("E Enable"));
            JungClear.Add("Use Timat JClear", new CheckBox("Tiamat Enable"));
            JungClear.Add("minimum Mana JC", new Slider("minimum Mana", 40, 0, 100));

            Misc = Menu.AddSubMenu("Misc", "Misc");
            Misc.Add("WallJump", new KeyBind("WallJump", false, KeyBind.BindTypes.HoldActive, 'H'));
            Misc.Add("Orbwalk Last Right Click", new KeyBind("Orbwalk Last Right Click", false, KeyBind.BindTypes.HoldActive, 'Y')).OnValueChange += OrbwalkLastClick.OrbwalkLRCLK_ValueChanged;

            Draw = Menu.AddSubMenu("Draw", "Draw");
            Draw.Add("Draw Q", new CheckBox("Draw Q", false));
            Draw.Add("Draw W", new CheckBox("Draw W", false));
            Draw.Add("Draw Optional Range", new CheckBox("Draw Optional Range"));
            Draw.Add("Draw Selected Range", new CheckBox("Draw Selected Range"));
            Draw.Add("Draw Priority Range", new CheckBox("Draw Priority Range"));
            Draw.Add("Draw Target", new CheckBox("Draw Target"));
            Draw.Add("Draw Vitals", new CheckBox("Draw Vitals", false));
            Draw.Add("Draw Fast Damage", new CheckBox("Draw Fast Damage", false)).OnValueChange += DrawHP_ValueChanged;

            if (HeroManager.Enemies.Any())
            {
                Evade.Evade.Init();
                EvadeTarget.Init();
                TargetedNoMissile.Init();
                OtherSkill.Init();
            }

            OrbwalkLastClick.Init();
            Drawing.OnDraw += Drawing_OnDraw;
            Drawing.OnEndScene += Drawing_OnEndScene;

            //GameObject.OnCreate += GameObject_OnCreate;
            Game.OnUpdate += Game_OnGameUpdate;
            Orbwalker.OnPostAttack += AfterAttack;
            Orbwalker.OnPostAttack += Orbwalking_AfterAttackNoTarget;
            Orbwalker.OnAttack += OnAttack;
            Obj_AI_Base.OnProcessSpellCast += oncast;
            Game.OnWndProc += Game_OnWndProc;
            CustomDamageIndicator.Initialize(GetFastDamage);
            CustomDamageIndicator.Enabled = DrawHP;

            //evade
            FioraProject.Evade.Evade.Evading += EvadeSkillShots.Evading;
            Chat.Print("Welcome to FioraWorld");
        }

        // events 
        public static void AfterAttack(AttackableUnit target, EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) || OrbwalkerPassive || OrbwalkLastClickActive)
            {
                if (Ecombo && E.IsReady())
                {
                    E.Cast();
                }
                else if (HasItem())
                {
                    CastItem();
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                if (Eharass && E.IsReady() && Player.ManaPercent >= Manaharass)
                {
                    E.Cast();
                }
                else if (HasItem())
                {
                    CastItem();
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                // jungclear
                if (EJclear && E.IsReady() && Player.Mana * 100 / Player.MaxMana >= ManaJclear && !Orbwalker.ShouldWait
                    && Player.Position.CountMinionsInRange(Player.AttackRange + 200, true) >= 1)
                {
                    E.Cast();
                }
                else if (TimatJClear && HasItem() && !Orbwalker.ShouldWait
                    && Player.Position.CountMinionsInRange(Player.AttackRange + 200, true) >= 1)
                {
                    CastItem();
                }
                // laneclear
                if (ELclear && E.IsReady() && Player.Mana * 100 / Player.MaxMana >= ManaLclear && !Orbwalker.ShouldWait
                    && Player.Position.CountMinionsInRange(Player.AttackRange + 200, false) >= 1)
                {
                    E.Cast();
                }
                else if (TimatLClear && HasItem() && !Orbwalker.ShouldWait
                    && Player.Position.CountMinionsInRange(Player.AttackRange + 200, false) >= 1)
                {
                    CastItem();
                }
            }

        }
        private static void Orbwalking_AfterAttackNoTarget(AttackableUnit target, EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) || OrbwalkerPassive || OrbwalkLastClickActive)
            {
                if (Ecombo && E.IsReady() && Player.CountEnemiesInRange(Player.AttackRange + 200) >= 1)
                {
                    E.Cast();
                }
                else if (HasItem() && Player.CountEnemiesInRange(Player.AttackRange + 200) >= 1)
                {
                    CastItem();
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                if (Eharass && E.IsReady() && Player.ManaPercent >= Manaharass
                    && Player.CountEnemiesInRange(Player.AttackRange + 200) >= 1)
                {
                    E.Cast();
                }
                else if (HasItem() && Player.CountEnemiesInRange(Player.AttackRange + 200) >= 1)
                {
                    CastItem();
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                // jungclear
                if (EJclear && E.IsReady() && Player.Mana * 100 / Player.MaxMana >= ManaJclear && !Orbwalker.ShouldWait
                    && Player.Position.CountMinionsInRange(Player.AttackRange + 200, true) >= 1)
                {
                    E.Cast();
                }
                else if (TimatJClear && HasItem() && !Orbwalker.ShouldWait
                    && Player.Position.CountMinionsInRange(Player.AttackRange + 200, true) >= 1)
                {
                    CastItem();
                }
                // laneclear
                if (ELclear && E.IsReady() && Player.Mana * 100 / Player.MaxMana >= ManaLclear && !Orbwalker.ShouldWait
                    && Player.Position.CountMinionsInRange(Player.AttackRange + 200, false) >= 1)
                {
                    E.Cast();
                }
                else if (TimatLClear && HasItem() && !Orbwalker.ShouldWait
                    && Player.Position.CountMinionsInRange(Player.AttackRange + 200, false) >= 1)
                {
                    CastItem();
                }
            }
        }

        public static void Game_OnGameUpdate(EventArgs args)
        {
            if (Player.IsDead)
                return;
            FioraPassiveUpdate();
            OrbwalkToPassive();
            WallJump();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) || OrbwalkerPassive)
            {
                Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                Harass();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {

            }
        }
        public static void oncast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var spell = args.SData;
            if (!sender.IsMe)
                return;
            if (spell.Name.Contains("ItemTiamatCleave"))
            {

            }
            if (spell.Name.Contains("FioraQ"))
            {

            }
            if (spell.Name == "FioraE")
            {
                Orbwalker.ResetAutoAttack();
            }
            if (spell.Name == "ItemTitanicHydraCleave")
            {
                Orbwalker.ResetAutoAttack();
            }
            if (spell.Name.ToLower().Contains("fiorabasicattack"))
            {
            }

        }
        public static void OnAttack(AttackableUnit target, EventArgs args)
        {
            var item = new Item(ItemId.Youmuus_Ghostblade, 0);
            if ((Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) || OrbwalkerPassive || OrbwalkLastClickActive))
            {
                if (item.IsReady())
                    item.Cast();
            }

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


        //harass
        public static bool Qharass { get { return getCheckBoxItem(Harass, "Use Q Harass"); } }
        private static bool Eharass { get { return getCheckBoxItem(Harass, "Use E Harass"); } }
        public static bool CastQGapCloseHarass { get { return getCheckBoxItem(Harass, "Use Q Harass Gap"); } }
        public static bool CastQPrePassiveHarass { get { return getCheckBoxItem(Harass, "Use Q Harass Pre Pass"); } }
        public static bool CastQPassiveHarasss { get { return getCheckBoxItem(Harass, "Use Q Harass Pass"); } }
        public static int Manaharass { get { return getSliderItem(Harass, "Mana Harass"); } }

        //combo
        public static bool Qcombo { get { return getCheckBoxItem(Combo, "Use Q Combo"); } }
        private static bool Ecombo { get { return getCheckBoxItem(Combo, "Use E Combo"); } }
        public static bool CastQGapCloseCombo { get { return getCheckBoxItem(Combo, "Use Q Combo Gap"); } }
        public static bool CastQPrePassiveCombo { get { return getCheckBoxItem(Combo, "Use Q Combo Pre Pass"); } }
        public static bool CastQPassiveCombo { get { return getCheckBoxItem(Combo, "Use Q Combo Pass"); } }
        public static bool CastQMinionGapCloseCombo { get { return getCheckBoxItem(Combo, "Use Q Combo Gap Minion"); } }
        public static int ValueQMinionGapCloseCombo { get { return getSliderItem(Combo, "Use Q Combo Gap Minion Value"); } }
        public static bool Rcombo { get { return getCheckBoxItem(Combo, "Use R Combo"); } }
        public static bool UseRComboLowHP { get { return getCheckBoxItem(Combo, "Use R Combo LowHP"); } }
        public static int ValueRComboLowHP { get { return getSliderItem(Combo, "Use R Combo LowHP Value"); } }
        public static bool UseRComboKillable { get { return getCheckBoxItem(Combo, "Use R Combo Killable"); } }
        public static bool UseRComboOnTap { get { return getCheckBoxItem(Combo, "Use R Combo On Tap"); } }
        public static bool RTapKeyActive { get { return getKeyBindItem(Combo, "Use R Combo On Tap Key"); } }
        public static bool UseRComboAlways { get { return getCheckBoxItem(Combo, "Use R Combo Always"); } }

        //jclear && lclear
        private static bool ELclear { get { return getCheckBoxItem(LaneClear, "Use E LClear"); } }
        private static bool TimatLClear { get { return getCheckBoxItem(LaneClear, "Use Timat LClear"); } }
        private static bool EJclear { get { return getCheckBoxItem(JungClear, "Use E JClear"); } }
        private static bool TimatJClear { get { return getCheckBoxItem(JungClear, "Use Timat JClear"); } }
        public static int ManaJclear { get { return getSliderItem(JungClear, "minimum Mana JC"); } }
        public static int ManaLclear { get { return getSliderItem(LaneClear, "minimum Mana LC"); } }

        //orbwalkpassive
        private static float OrbwalkToPassiveRange { get { return getSliderItem(Target, "Orbwalk To Passive Range"); } }
        private static bool OrbwalkToPassiveTargeted { get { return getCheckBoxItem(SelectedMode, "Selected Orbwalk to Passive"); } }
        private static bool OrbwalkToPassiveOptional { get { return getCheckBoxItem(OptionalMode, "Optional Orbwalk to Passive"); } }
        private static bool OrbwalkToPassivePriority { get { return getCheckBoxItem(PriorityMode, "Priority Orbwalk to Passive"); } }
        private static bool OrbwalkTargetedUnderTower { get { return getCheckBoxItem(SelectedMode, "Selected Under Tower"); } }
        private static bool OrbwalkOptionalUnderTower { get { return getCheckBoxItem(OptionalMode, "Optional Under Tower"); } }
        private static bool OrbwalkPriorityUnderTower { get { return getCheckBoxItem(PriorityMode, "Priority Under Tower"); } }

        // orbwalklastclick
        public static bool OrbwalkLastClickActive { get { return getKeyBindItem(Misc, "Orbwalk Last Right Click"); } }
        public static bool OrbwalkerPassive { get { return getKeyBindItem(Combo, "Orbwalker2Mouse"); } }

        #region Drawing
        private static bool DrawQ { get { return getCheckBoxItem(Draw, "Draw Q"); } }
        private static bool DrawW { get { return getCheckBoxItem(Draw, "Draw W"); } }
        private static bool DrawOptionalRange { get { return getCheckBoxItem(Draw, "Draw Optional Range"); } }
        private static bool DrawSelectedRange { get { return getCheckBoxItem(Draw, "Draw Selected Range"); } }
        private static bool DrawPriorityRange { get { return getCheckBoxItem(Draw, "Draw Priority Range"); } }
        private static bool DrawTarget { get { return getCheckBoxItem(Draw, "Draw Target"); } }
        private static bool DrawHP { get { return getCheckBoxItem(Draw, "Draw Fast Damage"); } }
        private static bool DrawVitals { get { return getCheckBoxItem(Draw, "Draw Vitals"); } }

        private static void DrawHP_ValueChanged(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            if (sender != null)
            {
                CustomDamageIndicator.Enabled = args.NewValue;
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead)
                return;
            if (DrawQ)
                Render.Circle.DrawCircle(Player.Position, 400, Color.Green);
            if (DrawW)
            {
                Render.Circle.DrawCircle(Player.Position, W.Range, Color.Green);
            }
            if (DrawOptionalRange && TargetingMode == TargetMode.Optional)
            {
                Render.Circle.DrawCircle(Player.Position, OptionalRange, Color.DeepPink);
            }
            if (DrawSelectedRange && TargetingMode == TargetMode.Selected)
            {
                Render.Circle.DrawCircle(Player.Position, SelectedRange, Color.DeepPink);
            }
            if (DrawPriorityRange && TargetingMode == TargetMode.Priority)
            {
                Render.Circle.DrawCircle(Player.Position, PriorityRange, Color.DeepPink);
            }
            if (DrawTarget && TargetingMode != TargetMode.Normal)
            {
                var hero = GetTarget();
                if (hero != null)
                    Render.Circle.DrawCircle(hero.Position, 75, Color.Yellow, 5);
            }
            if (DrawVitals && TargetingMode != TargetMode.Normal)
            {
                var hero = GetTarget();
                if (hero != null)
                {
                    var status = hero.GetPassiveStatus(0f);
                    if (status.HasPassive && status.PassivePredictedPositions.Any())
                    {
                        foreach (var x in status.PassivePredictedPositions)
                        {
                            Render.Circle.DrawCircle(x.To3D(), 50, Color.Yellow);
                        }
                    }
                }
            }
            if (activewalljump)
            {
                var Fstwall = GetFirstWallPoint(Player.Position.To2D(), Game.CursorPos.To2D());
                if (Fstwall != null)
                {
                    var firstwall = ((Vector2)Fstwall);
                    var pos = firstwall.Extend(Game.CursorPos.To2D(), 100);
                    var Lstwall = GetLastWallPoint(firstwall, Game.CursorPos.To2D());
                    if (Lstwall != null)
                    {
                        var lastwall = ((Vector2)Lstwall);
                        if (InMiddileWall(firstwall, lastwall))
                        {
                            for (int i = 0; i <= 359; i++)
                            {
                                var pos1 = pos.RotateAround(firstwall, i);
                                var pos2 = firstwall.Extend(pos1, 400);
                                if (pos1.InTheCone(firstwall, Game.CursorPos.To2D(), 60) && pos1.IsWall() && !pos2.IsWall())
                                {
                                    Render.Circle.DrawCircle(firstwall.To3D(), 50, Color.Green);
                                    goto Finish;
                                }
                            }

                            Render.Circle.DrawCircle(firstwall.To3D(), 50, Color.Red);
                        }
                    }
                }
                Finish:;
            }

        }
        private static void Drawing_OnEndScene(EventArgs args)
        {
        }

        #endregion Drawing

        #region WallJump
        public static bool usewalljump = true;
        public static bool activewalljump { get { return getKeyBindItem(Misc, "WallJump"); } }
        public static int movetick;
        public static void WallJump()
        {
            if (usewalljump && activewalljump)
            {
                var Fstwall = GetFirstWallPoint(Player.Position.To2D(), Game.CursorPos.To2D());
                if (Fstwall != null)
                {
                    var firstwall = ((Vector2)Fstwall);
                    var Lstwall = GetLastWallPoint(firstwall, Game.CursorPos.To2D());
                    if (Lstwall != null)
                    {
                        var lastwall = ((Vector2)Lstwall);
                        if (InMiddileWall(firstwall, lastwall))
                        {
                            var y = Player.Position.Extend(Game.CursorPos, 30);
                            for (int i = 20; i <= 300; i = i + 20)
                            {
                                if (Utils.GameTimeTickCount - movetick < (70 + Math.Min(60, Game.Ping)))
                                    break;
                                if (Player.LSDistance(Game.CursorPos) <= 1200 && Player.Position.To2D().Extend(Game.CursorPos.To2D(), i).IsWall())
                                {
                                    EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, Player.Position.To2D().Extend(Game.CursorPos.To2D(), i - 20).To3D());
                                    movetick = Utils.GameTimeTickCount;
                                    break;
                                }
                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo,
                                    Player.LSDistance(Game.CursorPos) <= 1200 ?
                                    Player.Position.To2D().Extend(Game.CursorPos.To2D(), 200).To3D() :
                                    Game.CursorPos);
                            }
                            if (y.IsWall() && LeagueSharp.Common.Prediction.GetPrediction(Player, 500).UnitPosition.LSDistance(Player.Position) <= 10 && Q.IsReady())
                            {
                                var pos = Player.Position.To2D().Extend(Game.CursorPos.To2D(), 100);
                                for (int i = 0; i <= 359; i++)
                                {
                                    var pos1 = pos.RotateAround(Player.Position.To2D(), i);
                                    var pos2 = Player.Position.To2D().Extend(pos1, 400);
                                    if (pos1.InTheCone(Player.Position.To2D(), Game.CursorPos.To2D(), 60) && pos1.IsWall() && !pos2.IsWall())
                                    {
                                        Q.Cast(pos2);
                                    }

                                }
                            }
                        }
                        else if (Utils.GameTimeTickCount - movetick >= (70 + Math.Min(60, Game.Ping)))
                        {
                            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                            movetick = Utils.GameTimeTickCount;
                        }
                    }
                    else if (Utils.GameTimeTickCount - movetick >= (70 + Math.Min(60, Game.Ping)))
                    {
                        EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                        movetick = Utils.GameTimeTickCount;
                    }
                }
                else if (Utils.GameTimeTickCount - movetick >= (70 + Math.Min(60, Game.Ping)))
                {
                    EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                    movetick = Utils.GameTimeTickCount;
                }
            }
        }
        public static Vector2? GetFirstWallPoint(Vector2 from, Vector2 to, float step = 25)
        {
            var direction = (to - from).Normalized();

            for (float d = 0; d < from.LSDistance(to); d = d + step)
            {
                var testPoint = from + d * direction;
                var flags = NavMesh.GetCollisionFlags(testPoint.X, testPoint.Y);
                if (flags.HasFlag(CollisionFlags.Wall) || flags.HasFlag(CollisionFlags.Building))
                {
                    return from + (d - step) * direction;
                }
            }

            return null;
        }
        public static Vector2? GetLastWallPoint(Vector2 from, Vector2 to, float step = 25)
        {
            var direction = (to - from).Normalized();
            var Fstwall = GetFirstWallPoint(from, to);
            if (Fstwall != null)
            {
                var firstwall = ((Vector2)Fstwall);
                for (float d = step; d < firstwall.LSDistance(to) + 1000; d = d + step)
                {
                    var testPoint = firstwall + d * direction;
                    var flags = NavMesh.GetCollisionFlags(testPoint.X, testPoint.Y);
                    if (!flags.HasFlag(CollisionFlags.Wall) && !flags.HasFlag(CollisionFlags.Building))
                    //if (!testPoint.IsWall())
                    {
                        return firstwall + d * direction;
                    }
                }
            }

            return null;
        }
        public static bool InMiddileWall(Vector2 firstwall, Vector2 lastwall)
        {
            var midwall = new Vector2((firstwall.X + lastwall.X) / 2, (firstwall.Y + lastwall.Y) / 2);
            var point = midwall.Extend(Game.CursorPos.To2D(), 50);
            for (int i = 0; i <= 350; i = i + 10)
            {
                var testpoint = point.RotateAround(midwall, i);
                var flags = NavMesh.GetCollisionFlags(testpoint.X, testpoint.Y);
                if (!flags.HasFlag(CollisionFlags.Wall) && !flags.HasFlag(CollisionFlags.Building))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion WallJump

        #region OrbwalkToPassive
        private static void OrbwalkToPassive()
        {
            if (OrbwalkerPassive)
            {
                var target = GetTarget(OrbwalkToPassiveRange);
                if (target.IsValidTarget(OrbwalkToPassiveRange) && !target.IsZombie)
                {
                    var status = target.GetPassiveStatus(0);
                    if (Player.Position.To2D().LSDistance(target.Position.To2D()) <= OrbwalkToPassiveRange && status.HasPassive
                        && ((TargetingMode == TargetMode.Selected && OrbwalkToPassiveTargeted && (OrbwalkTargetedUnderTower || !Player.UnderTurret(true)))
                        || (TargetingMode == TargetMode.Optional && OrbwalkToPassiveOptional && (OrbwalkOptionalUnderTower || !Player.UnderTurret(true)))
                        || (TargetingMode == TargetMode.Priority && OrbwalkToPassivePriority && (OrbwalkPriorityUnderTower || !Player.UnderTurret(true)))))
                    {
                        var point = status.PassivePredictedPositions.OrderBy(x => x.LSDistance(Player.Position.To2D())).FirstOrDefault();
                        point = point.IsValid() ? point : Game.CursorPos.To2D();
                        Orbwalker.OrbwalkTo(point.To3D());
                    }
                    else Orbwalker.OrbwalkTo(Game.CursorPos);
                }
                else Orbwalker.OrbwalkTo(Game.CursorPos);
            }
        }
        #endregion OrbwalkToPassive
    }
}
