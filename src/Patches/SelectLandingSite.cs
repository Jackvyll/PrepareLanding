﻿using PrepareLanding.Core.Gui.Window;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace PrepareLanding.Patches
{
    /// <summary>
    ///     This class is used in replacement of the RimWorld.Page_SelectLandingSite. We patch GetFirstConfigPage() to use our
    ///     class rather than the RimWorld one.
    /// </summary>
    public class SelectLandingSite : Page_SelectStartingSite
    {
        public override void ExtraOnGUI()
        {
            Text.Anchor = TextAnchor.UpperCenter;
            DrawPageTitle(new Rect(0f, 5f, UI.screenWidth, 300f));
            Text.Anchor = TextAnchor.UpperLeft;
            DoCustomBottomButtons();
        }

        /// <summary>
        ///     This is a rip of the DoCustomBottomButtons() function in RimWorld.Page_SelectLandingSite with a new button.
        /// </summary>
        private void DoCustomBottomButtons()
        {
            // HACK : changed the number of buttons from '5 : 4' to '6 : 5' as we add a button
            var num = (!TutorSystem.TutorialMode) ? 6 : 5;
            int num2;
            if (num >= 4 && UI.screenWidth < 1340f)
            {
                num2 = 2;
            }
            else
            {
                num2 = 1;
            }
            var num3 = Mathf.CeilToInt(num / (float)num2);
            var num4 = BottomButSize.x * num3 + 10f * (num3 + 1);
            var num5 = num2 * BottomButSize.y + 10f * (num2 + 1);
            var rect = new Rect((UI.screenWidth - num4) / 2f, UI.screenHeight - num5 - 4f, num4, num5);
            var worldInspectPane = Find.WindowStack.WindowOfType<WorldInspectPane>();
            if (worldInspectPane != null && rect.x < InspectPaneUtility.PaneWidthFor(worldInspectPane) + 4f)
            {
                rect.x = InspectPaneUtility.PaneWidthFor(worldInspectPane) + 4f;
            }
            Widgets.DrawWindowBackground(rect);
            var num6 = rect.xMin + 10f;
            var num7 = rect.yMin + 10f;
            Text.Font = GameFont.Small;
            if (Widgets.ButtonText(new Rect(num6, num7, BottomButSize.x, BottomButSize.y), "Back".Translate()) && CanDoBack())
            {
                /* ADDED CODE */

                #region INSERTED_CODE

                // make sure the prepare landing window (or its minimized window) is closed when we go back
                if (Find.WindowStack.IsOpen<MainWindow>() || Find.WindowStack.IsOpen<MinimizedWindow>())
                {
                    if (PrepareLanding.Instance.MainWindow != null)
                        PrepareLanding.Instance.MainWindow.ForceClose();
                }

                #endregion

                /* END ADDED CODE*/

                DoBack();
            }
            num6 += BottomButSize.x + 10f;
            if (!TutorSystem.TutorialMode)
            {
                if (Widgets.ButtonText(new Rect(num6, num7, BottomButSize.x, BottomButSize.y), "Advanced".Translate()))
                {
                    Find.WindowStack.Add(new Dialog_AdvancedGameConfig(Find.WorldInterface.SelectedTile));
                }
                num6 += BottomButSize.x + 10f;
            }
            if (Widgets.ButtonText(new Rect(num6, num7, BottomButSize.x, BottomButSize.y), "SelectRandomSite".Translate()))
            {
                SoundDefOf.Click.PlayOneShotOnCamera();
                Find.WorldInterface.SelectedTile = TileFinder.RandomStartingTile();
                Find.WorldCameraDriver.JumpTo(Find.WorldGrid.GetTileCenter(Find.WorldInterface.SelectedTile));
            }
            num6 += BottomButSize.x + 10f;
            if (num2 == 2)
            {
                num6 = rect.xMin + 10f;
                num7 += BottomButSize.y + 10f;
            }
            if (Widgets.ButtonText(new Rect(num6, num7, BottomButSize.x, BottomButSize.y), "WorldFactionsTab".Translate()))
            {
                Find.WindowStack.Add(new Dialog_FactionDuringLanding());
            }
            num6 += BottomButSize.x + 10f;

            #region INSERTED_CODE

            /* ADDED CODE */

            if (Widgets.ButtonText(new Rect(num6, num7, BottomButSize.x, BottomButSize.y), "Prepare Landing"))
            {
                Log.Message("[PrepareLanding] Page button pressed!");

                // don't add a new window if the window is already there
                if (PrepareLanding.Instance.MainWindow == null)
                    PrepareLanding.Instance.MainWindow = new MainWindow(PrepareLanding.Instance.GameData);

                PrepareLanding.Instance.MainWindow.Show();
            }
            num6 += BottomButSize.x + 10f;

            /* END ADDED CODE*/

            #endregion INSERTED_CODE


            if (Widgets.ButtonText(new Rect(num6, num7, BottomButSize.x, BottomButSize.y), "Next".Translate()) && CanDoNext())
            {
                /* ADDED CODE */
                #region INSERTED_CODE

                // make sure the prepare landing window (or its minimized window) is closed when we go to the next window / game stage
                if (Find.WindowStack.IsOpen<MainWindow>() || Find.WindowStack.IsOpen<MinimizedWindow>())
                {
                    if (PrepareLanding.Instance.MainWindow != null)
                        PrepareLanding.Instance.MainWindow.ForceClose();
                }

                #endregion
                /* END ADDED CODE*/

                DoNext();
            }
            num6 += BottomButSize.x + 10f;
            GenUI.AbsorbClicksInRect(rect);
        }
    }
}