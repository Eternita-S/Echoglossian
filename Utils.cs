﻿// <copyright file="Utils.cs" company="lokinmodar">
// Copyright (c) lokinmodar. All rights reserved.
// Licensed under the Creative Commons Attribution-NonCommercial-NoDerivatives 4.0 International Public License license.
// </copyright>

using System;
using System.Globalization;
using System.IO;
using System.Numerics;

using Dalamud.Logging;
using Echoglossian.Properties;
using ImGuiNET;

namespace Echoglossian
{
  public partial class Echoglossian
  {
    private Tuple<string, int> LoadFontHelper(int chosenLanguage)
    {
      return new Tuple<string, int>("a", chosenLanguage);
    }

#if DEBUG
    public void ListCultureInfos()
    {
      using StreamWriter logStream = new(this.ConfigDir + "CultureInfos.txt", append: true);

      var cus = CultureInfo.GetCultures(CultureTypes.AllCultures);
      foreach (var cu in cus)
      {
        logStream.WriteLine(cu.ToString());
      }
    }
#endif

    public string MovePathUp(string path, int noOfLevels)
    {
      string parentPath = path.TrimEnd(new[] { '/', '\\' });
      for (int i = 0; i < noOfLevels; i++)
      {
        if (parentPath != null)
        {
          parentPath = Directory.GetParent(parentPath)?.ToString();
        }
      }

      return parentPath;
    }


    private static readonly string[] Codes =
    {
      "af", "an", "ar", "az", "be_x_old",
      "bg", "bn", "br", "bs",
      "ca", "ceb", "cs", "cy", "da",
      "de", "el", "en", "eo", "es",
      "et", "eu", "fa", "fi", "fr",
      "gl", "he", "hi", "hr", "ht",
      "hu", "hy", "id", "is", "it",
      "ja", "jv", "ka", "kk", "ko",
      "la", "lb", "lt", "lv",
      "mg", "mk", "ml", "mr", "ms",
      "new", "nl", "nn", "no", "oc",
      "pl", "pt", "ro", "roa_rup",
      "ru", "sk", "sl",
      "sq", "sr", "sv", "sw", "ta",
      "te", "th", "tl", "tr", "uk",
      "ur", "vi", "vo", "war", "zh",
      "zh_classical", "zh_yue",
    };

    private readonly string[] languages =
    {
      "Afrikaans", "Aragonese", "Arabic", "Azerbaijani", "Belarusian",
      "Bulgarian", "Bengali", "Breton", "Bosnian",
      "Catalan; Valencian", "Cebuano", "Czech", "Welsh", "Danish",
      "German", "Greek, Modern", "English", "Esperanto", "Spanish; Castilian",
      "Estonian", "Basque", "Persian", "Finnish", "French",
      "Galician", "Hebrew", "Hindi", "Croatian", "Haitian; Haitian Creole",
      "Hungarian", "Armenian", "Indonesian", "Icelandic", "Italian",
      "Japanese", "Javanese", "Georgian", "Kazakh", "Korean",
      "Latin", "Luxembourgish; Letzeburgesch", "Lithuanian", "Latvian",
      "Malagasy", "Macedonian", "Malayalam", "Marathi", "Malay",
      "Nepal Bhasa; Newari", "Dutch; Flemish", "Norwegian Nynorsk; Nynorsk, Norwegian", "Norwegian",
      "Occitan (post 1500)",
      "Polish", "Portuguese", "Romanian; Moldavian; Moldovan", "Romance languages",
      "Russian", "Slovak", "Slovenian",
      "Albanian", "Serbian", "Swedish", "Swahili", "Tamil",
      "Telugu", "Thai", "Tagalog", "Turkish", "Ukrainian",
      "Urdu", "Vietnamese", "Volapük", "Waray", "Chinese",
      "Chinese Classical", "Chinese yue",
    };

    private void FixConfig()
    {
      if (this.configuration.Version == 0)
      {
        return;
      }

      if (this.pluginInterface.ConfigFile.Exists)
      {
        this.pluginInterface.ConfigFile.Delete();
        if (this.pluginInterface.ConfigDirectory.Exists)
        {
          this.pluginInterface.ConfigDirectory.Delete(true);
        }
      }

      this.SaveConfig();
    }

    private void SaveConfig()
    {
      this.configuration.Lang = languageInt;

      this.pluginInterface.SavePluginConfig(this.configuration);
    }

    [Flags]
    public enum TransEngines
    {
      Google = 1 << 0, // Google Translator (free engine)
      Deepl = 1 << 1, // DeepL Translator
      Bing = 1 << 2, // Microsoft Bing Translator (free engine)
      Yandex = 1 << 3, // Yandex Translator
      GTranslate = 1 << 4, // Uses Google, Bing and Yandex (free engines)
      Amazon = 1 << 5, // Amazon Translate
      Azure = 1 << 6, // Microsoft Azure Translate
    }
  }
}