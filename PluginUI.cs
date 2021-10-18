﻿// <copyright file="PluginUI.cs" company="lokinmodar">
// Copyright (c) lokinmodar. All rights reserved.
// Licensed under the Creative Commons Attribution-NonCommercial-NoDerivatives 4.0 International Public License license.
// </copyright>

using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

using Dalamud.Logging;
using Echoglossian.Properties;
using ImGuiNET;

namespace Echoglossian
{
  public partial class Echoglossian
  {
    public string[] FontSizes = Array.ConvertAll(Enumerable.Range(4, 72).ToArray(), x => x.ToString());

    internal void ReloadFont()
    {
      this.pluginInterface.UiBuilder.RebuildFonts();
#if DEBUG
      PluginLog.LogVerbose("Font atlas rebuilt!");
#endif
    }

    private void EchoglossianConfigUi()
    {
      bool languageNotSupported = this.configuration.Lang is 3 or 5 or 11 or 13 or 114 or 115;
      bool languageOnlySupportedThruOverlay =
        this.configuration.Lang is 2 or 4 or 6 or 8 or 9 or 10 or 12 or 14 or 15 or 16 or 18 or 19 or 21 or 22 or 24 or 25 or 30 or 36 or 38 or 39 or 41 or 42 or 43 or 44 or 45 or 47 or 53 or 54 or 55 or 57 or 58 or 59 or 60 or 62 or 63 or 66 or 69 or 72 or 73 or 74 or 78 or 79 or 80 or 82 or 84 or 85 or 88 or 91 or 92 or 93 or 94 or 98 or 102 or 103 or 104 or 105 or 106 or 107 or 108 or 109 or 110 or 111 or 112 or 113 or 119;
      ImGui.SetNextWindowSizeConstraints(new Vector2(600, 600), new Vector2(1920, 1080));
      ImGui.Begin(Resources.ConfigWindowTitle, ref this.config);
      if (ImGui.BeginTabBar("TabBar", ImGuiTabBarFlags.None))
      {
        if (ImGui.BeginTabItem(Resources.ConfigTab0Name))
        {
          if (ImGui.Combo(Resources.LanguageSelectLabelText, ref languageInt, this.languages, this.languages.Length))
          {
            this.configuration.Lang = languageInt;
            this.SaveConfig();
          }

          ImGui.SameLine();
          ImGui.Text(Resources.HoverTooltipIndicator);
          if (ImGui.IsItemHovered())
          {
            ImGui.SetTooltip(Resources.LanguageSelectionTooltip);
          }

          if (languageNotSupported)
          {
            ImGui.Text(Resources.LanguageNotSupportedText);
            bool translationsDisabled = this.DisableAllTranslations();

            if (translationsDisabled)
            {
              this.SaveConfig();
            }
          }
          else
          {
            if (languageOnlySupportedThruOverlay)
            {
              ImGui.Text(Resources.LanguageOnlySupportedUsingOverlay);
              this.configuration.UseImGui = true;
              this.configuration.DoNotUseImGuiForToasts = false;

              this.SaveConfig();
            }
            else
            {

              ImGui.Text(Resources.WhatToTranslateText);
              if (ImGui.Checkbox(Resources.TranslateTalkToggleLabel, ref this.configuration.TranslateTalk))
              {
                this.SaveConfig();
              }

              if (ImGui.Checkbox(Resources.TransLateBattletalkToggle, ref this.configuration.TranslateBattleTalk))
              {
                this.SaveConfig();
              }

              if (ImGui.Checkbox(Resources.TranslateToastToggleText, ref this.configuration.TranslateToast))
              {
                if (!this.configuration.TranslateToast)
                {
                  this.configuration.TranslateAreaToast = false;
                  this.configuration.TranslateClassChangeToast = false;
                  this.configuration.TranslateErrorToast = false;
                  this.configuration.TranslateQuestToast = false;
                  this.configuration.TranslateWideTextToast = false;
                }

                this.SaveConfig();
              }

              ImGui.Text(Resources.TranslationsEnabled);

            }
          }

          ImGui.EndTabItem();
          this.SaveConfig();

        }

        if (ImGui.BeginTabItem(Resources.ConfigTab1Name, ref this.configuration.TranslateTalk))
        {
          if (!languageOnlySupportedThruOverlay && this.configuration.UseImGui)
          {
            if (ImGui.Checkbox(
              Resources.OverlayToggleLabel,
              ref this.configuration.UseImGui))
            {
              this.SaveConfig();
            }
          }

          if (languageOnlySupportedThruOverlay || this.configuration.UseImGui)
          {
            if (ImGui.Checkbox(Resources.TranslateNpcNamesToggle, ref this.configuration.TranslateNPCNames))
            {
              this.SaveConfig();
            }

            ImGui.Spacing();
            ImGui.Separator();

            if (ImGui.SliderFloat(
              Resources.OverlayFontScaleLabel,
              ref this.configuration.FontScale,
              -3f,
              3f,
              "%.2f"))
            {
              this.configuration.FontChangeTime = DateTime.Now.Ticks;
              this.SaveConfig();
            }

            ImGui.SameLine();
            ImGui.Text(Resources.HoverTooltipIndicator);
            if (ImGui.IsItemHovered())
            {
              ImGui.SetTooltip(Resources.OverlayFontSizeOrientations);
            }

            ImGui.Separator();
            ImGui.Spacing();
            ImGui.SameLine();
            ImGui.Text(Resources.FontColorSelectLabel);
            ImGui.SameLine();
            if (ImGui.ColorEdit3(
              Resources.OverlayColorSelectName,
              ref this.configuration.OverlayTextColor,
              ImGuiColorEditFlags.NoInputs | ImGuiColorEditFlags.NoLabel))
            {
              this.SaveConfig();
            }

            ImGui.SameLine();
            ImGui.Text(Resources.HoverTooltipIndicator);
            if (ImGui.IsItemHovered())
            {
              ImGui.SetTooltip(Resources.OverlayFontColorOrientations);
            }

            ImGui.Spacing();
            ImGui.Separator();
            if (ImGui.DragFloat(
              Resources.OverlayWidthScrollLabel,
              ref this.configuration.ImGuiTalkWindowWidthMult,
              0.001f, 0.01f, 3f))
            {
              this.SaveConfig();
            }

            ImGui.Separator();
            if (ImGui.DragFloat(
              Resources.OverlayHeightScrollLabel,
              ref this.configuration.ImGuiTalkWindowHeightMult,
              0.001f, 0.01f, 3f))
            {
              this.SaveConfig();
            }

            ImGui.Separator();
            ImGui.Spacing();
            if (ImGui.DragFloat2(
              Resources.OverlayPositionAdjustmentLabel,
              ref this.configuration.ImGuiWindowPosCorrection))
            {
              this.SaveConfig();
            }

            ImGui.SameLine();
            ImGui.Text(Resources.HoverTooltipIndicator);
            if (ImGui.IsItemHovered())
            {
              ImGui.SetTooltip(Resources.OverlayAdjustmentOrientations);
            }
          }

          ImGui.Spacing();
          ImGui.Separator();
          if (!languageOnlySupportedThruOverlay && this.configuration.UseImGui)
          {
            if (ImGui.Checkbox(Resources.SwapTranslationTextToggle, ref this.configuration.SwapTextsUsingImGui))
            {
              this.SaveConfig();
            }
          }

          ImGui.EndTabItem();
        }

        if (ImGui.BeginTabItem(Resources.ConfigTab2Name, ref this.configuration.TranslateBattleTalk))
        {
          if (languageOnlySupportedThruOverlay)
          {
            ImGui.Spacing();
            ImGui.Separator();
          }

          if (this.configuration.UseImGui)
          {
            /*ImGui.Separator();
            if (ImGui.Combo(Resources.OverlayFontSizeLabel, ref this.configuration.FontSize, this.FontSizes, this.FontSizes.Length))
            {
              this.SaveConfig();
              //this.LoadFont();
              if (this.FontLoaded)
              {
                this.pluginInterface.UiBuilder.RebuildFonts();
              }
            }*/

            // TODO: Fix this to BattleTalk
            ImGui.Separator();
            if (ImGui.DragFloat(
              Resources.OverlayWidthScrollLabel,
              ref this.configuration.ImGuiBattleTalkWindowWidthMult,
              0.001f, 0.01f, 3f))
            {
              this.SaveConfig();
            }

            ImGui.SameLine();
            ImGui.Text(Resources.HoverTooltipIndicator);
            if (ImGui.IsItemHovered())
            {
              ImGui.SetTooltip(Resources.OverlayWidthMultiplierOrientations);
            }
          }

          ImGui.EndTabItem();
        }

        if (ImGui.BeginTabItem(Resources.ConfigTab3Name, ref this.configuration.TranslateToast))
        {
          if (this.configuration.TranslateToast)
          {
            if (!languageOnlySupportedThruOverlay)
            {
              ImGui.Spacing();
              ImGui.Separator();
            }
            else
            {
              ImGui.Checkbox(
                Resources.OverlayToggleLabel,
                ref this.configuration.UseImGui);
            }

            ImGui.Separator();
            ImGui.Text(Resources.WhichToastsToTranslateText);
            ImGui.Checkbox(Resources.TranslateErrorToastToggleText, ref this.configuration.TranslateErrorToast);
            ImGui.Checkbox(Resources.TranslateQuestToastToggleText, ref this.configuration.TranslateQuestToast);
            ImGui.Checkbox(Resources.TranslateAreaToastToggleText, ref this.configuration.TranslateAreaToast);
            ImGui.Checkbox(
              Resources.TranslateClassChangeToastToggleText,
              ref this.configuration.TranslateClassChangeToast);
            ImGui.Checkbox(
              Resources.TranslateScreenInfoToastToggleText,
              ref this.configuration.TranslateWideTextToast);
            ImGui.Separator();
            if (ImGui.Checkbox(
              Resources.DoNotUseImGuiForToastsToggle,
              ref this.configuration.DoNotUseImGuiForToasts))
            {
              this.SaveConfig();
            }

            ImGui.Separator();
            if (this.configuration.UseImGui && !this.configuration.DoNotUseImGuiForToasts)
            {
              ImGui.Separator();
              if (ImGui.DragFloat(
                Resources.ToastOverlayWidthScrollLabel,
                ref this.configuration.ImGuiToastWindowWidthMult, 0.001f, 0.01f, 3f))
              {
                this.SaveConfig();
              }

              ImGui.SameLine();
              ImGui.Text(Resources.HoverTooltipIndicator);
              if (ImGui.IsItemHovered())
              {
                ImGui.SetTooltip(Resources.ToastOverlayWidthMultiplierOrientations);
              }
            }
          }

          ImGui.EndTabItem();
        }

        if (ImGui.BeginTabItem(Resources.ConfigTab4Name, ref this.configuration.TranslateJournal))
        {
          ImGui.Text("This is the Cucumber tab!\nblah blah blah blah blah");
          ImGui.EndTabItem();
        }

        if (ImGui.BeginTabItem(Resources.ConfigTab5Name, ref this.configuration.TranslateTooltips))
        {
          ImGui.Text("This is the Cucumber tab!\nblah blah blah blah blah");
          ImGui.EndTabItem();
        }

        if (ImGui.BeginTabItem(Resources.ConfigTab6Name, ref this.configuration.TranslateToDoList))
        {
          ImGui.Text("This is the Cucumber tab!\nblah blah blah blah blah");
          ImGui.EndTabItem();
        }

        if (ImGui.BeginTabItem(Resources.ConfigTabAbout))
        {
          ImGui.Text("This is the Cucumber tab!\nblah blah blah blah blah");
          ImGui.EndTabItem();
        }
        ImGui.EndTabBar();
      }

      var pos = new Vector2(ImGui.GetWindowContentRegionMin().X, ImGui.GetWindowContentRegionMax().Y - 30);
      ImGui.Separator();
      ImGui.SetCursorPos(pos);
      ImGui.BeginGroup();
      if (ImGui.Button(Resources.SaveCloseButtonLabel))
      {
        this.SaveConfig();
        this.config = false;
      }

      ImGui.SameLine();
      ImGui.PushStyleColor(ImGuiCol.Button, 0xFF000000 | 0x005E5BFF);
      ImGui.PushStyleColor(ImGuiCol.ButtonActive, 0xDD000000 | 0x005E5BFF);
      ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 0xAA000000 | 0x005E5BFF);

      if (ImGui.Button(Resources.PatronButtonLabel))
      {
        Process.Start(new ProcessStartInfo
        {
          FileName = "https://ko-fi.com/lokinmodar",
          UseShellExecute = true,
        });
        this.SaveConfig();
        this.config = false;
      }

      ImGui.PopStyleColor(3);
      ImGui.SameLine();
      ImGui.PushID(4);
      ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(4, 7.0f, 0.6f, 0.6f));
      ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(4, 7.0f, 0.7f, 0.7f));
      ImGui.PushStyleColor(ImGuiCol.ButtonActive, new Vector4(4, 7.0f, 0.8f, 0.8f));
      if (ImGui.Button(Resources.SendPixButton))
      {
        ImGui.OpenPopup(Resources.PixQrWindowLabel);
        this.SaveConfig();
      }

      // Always center this window when appearing
      var center = ImGui.GetMainViewport().GetCenter();
      ImGui.SetNextWindowPos(center, ImGuiCond.Appearing, new Vector2(0.5f, 0.5f));
      if (ImGui.BeginPopupModal(Resources.PixQrWindowLabel))
      {
        ImGui.Text(Resources.QRCodeInstructionsText);
        ImGui.Image(this.pixImage.ImGuiHandle, new Vector2(512, 512));
        if (ImGui.Button(Resources.CloseButtonLabel))
        {
          ImGui.CloseCurrentPopup();
        }

        ImGui.EndPopup();
        ImGui.SetItemDefaultFocus();
      }

      ImGui.PopStyleColor(3);
      ImGui.PopID();
      ImGui.EndGroup();
      ImGui.End();
    }

    private bool DisableAllTranslations()
    {
      this.configuration.TranslateTalk = false;
      this.configuration.TranslateBattleTalk = false;
      this.configuration.TranslateToast = false;
      this.configuration.TranslateNPCNames = false;
      this.configuration.TranslateErrorToast = false;
      this.configuration.TranslateQuestToast = false;
      this.configuration.TranslateAreaToast = false;
      this.configuration.TranslateClassChangeToast = false;
      this.configuration.TranslateWideTextToast = false;
      this.configuration.TranslateYesNoScreen = false;
      this.configuration.TranslateCutSceneSelectString = false;
      this.configuration.TranslateSelectString = false;
      this.configuration.TranslateSelectOk = false;
      this.configuration.TranslateToDoList = false;
      this.configuration.TranslateScenarioTree = false;

      return true;
    }
  }
}