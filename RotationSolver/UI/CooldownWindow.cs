﻿using Dalamud.Interface.Windowing;
using RotationSolver.Basic.Configuration;
using RotationSolver.Localization;
using RotationSolver.Updaters;
using System.Drawing;

namespace RotationSolver.UI;

internal class CooldownWindow : Window
{
    public CooldownWindow()
        :base(nameof(CooldownWindow))
    {
        
    }

    public override void Draw()
    {
        if (RotationUpdater.RightNowRotation != null)
        {
            var width = Service.Config.GetValue(PluginConfigFloat.CooldownWindowIconSize);
            var count = Math.Max(1, (int)MathF.Floor(ImGui.GetColumnWidth() / (width * (1 + 6 / 82) + ImGui.GetStyle().ItemSpacing.X)));

            foreach (var pair in RotationUpdater.AllGroupedActions)
            {
                var showItems = pair.OrderBy(a => a.SortKey).Where(a => a.IsInCooldown);
                if (!Service.Config.GetValue(PluginConfigBool.ShowGCDCooldown)) showItems = showItems.Where(i => !(i is IBaseAction a && a.IsGeneralGCD));

                if (!showItems.Any()) continue;
                if (!Service.Config.GetValue(PluginConfigBool.ShowItemsCooldown) && showItems.Any(i => i is IBaseItem)) continue;

                ImGui.Text(pair.Key);

                uint started = 0;
                foreach (var item in showItems)
                {
                    if (started++ % count != 0)
                    {
                        ImGui.SameLine();
                    }
                    DrawActionCooldown(item, width);
                }
            }
        }
    }

    static readonly uint progressCol = ImGui.ColorConvertFloat4ToU32(new Vector4(0.6f, 0.6f, 0.6f, 0.7f));

    private static void DrawActionCooldown(IAction act, float width)
    {
        var recast = act.RecastTimeOneChargeRaw;
        var elapsed = act.RecastTimeElapsedRaw;
        var shouldSkip = recast < 3 && act is IBaseAction a && !a.IsRealGCD;

        ImGui.BeginGroup();
        var winPos = ImGui.GetWindowPos();

        var r = -1f;
        if (Service.Config.GetValue(PluginConfigBool.UseOriginalCooldown))
        {
            r = !act.EnoughLevel ? 0: recast == 0 || !act.IsCoolingDown || shouldSkip ? 1 : elapsed / recast;
        }
        var pair = ControlWindow.DrawIAction(act, width, r, false);
        var pos = pair.Item1;
        var size = pair.Item2;
        ImguiTooltips.HoveredTooltip(act.Name + "\n" + LocalizationManager.RightLang.ConfigWindow_Control_ClickToUse);

        if (!act.EnoughLevel)
        {
            if (!Service.Config.GetValue(PluginConfigBool.UseOriginalCooldown))
            {
                ImGui.GetWindowDrawList().AddRectFilled(new Vector2(pos.X, pos.Y) + winPos,
                    new Vector2(pos.X + size.X, pos.Y + size.Y) + winPos, progressCol);
            }
        }
        else if (act.IsCoolingDown && !shouldSkip)
        {
            if (!Service.Config.GetValue(PluginConfigBool.UseOriginalCooldown))
            {
                var ratio = recast == 0 || !act.EnoughLevel ? 0 : elapsed % recast / recast;
                var startPos = new Vector2(pos.X + size.X * ratio, pos.Y) + winPos;
                ImGui.GetWindowDrawList().AddRectFilled(startPos,
                    new Vector2(pos.X + size.X, pos.Y + size.Y) + winPos, progressCol);

                ImGui.GetWindowDrawList().AddLine(startPos, startPos + new Vector2(0, size.Y), black);
            }

            ImGui.PushFont(ImGuiHelper.GetFont(Service.Config.GetValue(PluginConfigFloat.CooldownFontSize)));
            string time = recast == 0  ? "0" : ((int)(recast - elapsed % recast) + 1).ToString();
            var strSize = ImGui.CalcTextSize(time);
            var fontPos = new Vector2(pos.X + size.X / 2 - strSize.X / 2, pos.Y + size.Y / 2 - strSize.Y / 2) + winPos;

            TextShade(fontPos, time);
            ImGui.PopFont();
        }

        if (act.EnoughLevel && act is IBaseAction bAct && bAct.MaxCharges > 1)
        {
            for (int i = 0; i < bAct.CurrentCharges; i++)
            {
                ImGui.GetWindowDrawList().AddCircleFilled(winPos + pos + (i + 0.5f) * new Vector2(width / 5, 0), width / 12, white);
            }
        }

        ImGui.EndGroup();
    }

    static readonly uint black = ImGui.ColorConvertFloat4ToU32(new Vector4(0, 0, 0, 1));
    static readonly uint white = ImGui.ColorConvertFloat4ToU32(new Vector4(1, 1, 1, 1));

    public static void TextShade(Vector2 pos, string text, float width = 1.5f)
    {
        ImGui.GetWindowDrawList().AddText(pos - new Vector2(0, width), black, text);
        ImGui.GetWindowDrawList().AddText(pos - new Vector2(0, -width), black, text);
        ImGui.GetWindowDrawList().AddText(pos - new Vector2(width, 0), black, text);
        ImGui.GetWindowDrawList().AddText(pos - new Vector2(-width, 0), black, text);
        ImGui.GetWindowDrawList().AddText(pos, white, text);
    }
}
