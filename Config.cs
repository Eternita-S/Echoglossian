﻿// <copyright file="Config.cs" company="lokinmodar">
// Copyright (c) lokinmodar. All rights reserved.
// Licensed under the Creative Commons Attribution-NonCommercial-NoDerivatives 4.0 International Public License license.
// </copyright>

using System;
using System.Numerics;

using Dalamud.Configuration;

namespace Echoglossian
{
  public class Config : IPluginConfiguration
  {
    public string PluginCulture = "en";
    public int PluginCultureInt;

    public int Lang { get; set; } = 16;

    public int FontSize = 24;

    public bool ShowInCutscenes = true;

    public bool TranslateBattleTalk = true;
    public bool TranslateTalk = true;
    public bool TranslateToast = false;
    public bool TranslateNPCNames = true;
    public bool TranslateErrorToast = false;
    public bool TranslateQuestToast = false;
    public bool TranslateAreaToast = false;
    public bool TranslateClassChangeToast = false;
    public bool TranslateWideTextToast = false;
    public bool TranslateYesNoScreen = true;
    public bool TranslateCutSceneSelectString = true;
    public bool TranslateSelectString = true;
    public bool TranslateSelectOk = true;
    public bool TranslateToDoList = true;
    public bool TranslateScenarioTree = true;

    public bool UseImGui = true;
    public bool DoNotUseImGuiForToasts = false;
    public bool SwapTextsUsingImGui = false;

    public int ChosenTransEngine = 0;

    public Vector2 ImGuiWindowPosCorrection = new Vector2(0.0f, 0.0f);
    public Vector2 ImGuiToastWindowPosCorrection = new Vector2(0.0f, 0.0f);
    public float ImGuiTalkWindowWidthMult = 1f;
    public float ImGuiTalkWindowHeightMult = 1f;
    public float ImGuiBattleTalkWindowWidthMult = 1f;
    public float ImGuiBattleTalkWindowHeightMult = 1f;
    public float ImGuiToastWindowWidthMult = 1f;
    public Vector3 OverlayTextColor = new Vector3(1.0f, 1.0f, 1.0f);
    public float FontScale = 1;
    [NonSerialized]
    public long FontChangeTime = DateTime.Now.Ticks;

    public int Version { get; set; } = 0;
  }
}