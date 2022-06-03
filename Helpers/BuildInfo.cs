using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.Composition;

namespace UltraTextEdit_UWP.Helpers
{
    internal class BuildInfo
    {
        private static BuildInfo _buildInfo;

        private BuildInfo()
        {
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 15))
            {
                Build = Build.Win11Anniversary;
            }
            else if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 14))
            {
                Build = Build.Win11;
            }
            else if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 13))
            {
                Build = Build.Nov2021;
            }
            else if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 12))
            {
                Build = Build.May2021;
            }
            else if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 11))
            {
                Build = Build.Oct2020;
            }
            else if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 10))
            {
                Build = Build.May2020;
            }
            else if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 9))
            {
                Build = Build.Nov2019;
            }
            else if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
            {
                Build = Build.May2019;
            }
            else if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
            {
                Build = Build.Oct2018;
            }
            else if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 6))
            {
                Build = Build.Apr2018;
            }
            else if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 5))
            {
                Build = Build.FallCreators;
            }
            else if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 4))
            {
                Build = Build.Creators;
            }
            else if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3))
            {
                Build = Build.Anniversary;
            }
            else if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 2))
            {
                Build = Build.Threshold2;
            }
            else if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 1))
            {
                Build = Build.Threshold1;
            }
            else
            {
                Build = Build.Unknown;
            }

            if (!BeforeCreatorsUpdate)
            {
                var capabilities = CompositionCapabilities.GetForCurrentView();
                capabilities.Changed += (s, e) => UpdateCapabilities(capabilities);
                UpdateCapabilities(capabilities);
            }

            void UpdateCapabilities(CompositionCapabilities capabilities)
            {
                AreEffectsSupported = capabilities.AreEffectsSupported();
                AreEffectsFast = capabilities.AreEffectsFast();
            }
        }

        public static Build Build { get; private set; }
        public static bool AreEffectsFast { get; private set; }
        public static bool AreEffectsSupported { get; private set; }
        public static bool BeforeCreatorsUpdate => Build < Build.Creators;

        public static bool BeforeWin11 => Build < Build.Win11;

        public static bool BeforeWin1122H2 => Build < Build.Win11Anniversary;

        public static BuildInfo RetrieveApiInfo() => _buildInfo ??= new BuildInfo();
    }

    public enum Build
    {
        Unknown = 0,
        Threshold1 = 1507,   // 10240
        Threshold2 = 1511,   // 10586
        Anniversary = 1607,  // 14393 Redstone 1
        Creators = 1703,     // 15063 Redstone 2
        FallCreators = 1709,  // 16299 Redstone 3
        Apr2018 = 1803,       // 17134 Redsone 4
        Oct2018 = 1809,       // 17763 Redstone 5
        May2019 = 1903,       // 18362 19H1
        Nov2019 = 1909,       // 18363 19H2
        May2020 = 2004,       // 19041 20H1
        Oct2020 = 2009,       // 19042 20H2
        May2021 = 2104,       // 19043 21H1
        Nov2021 = 2110,       // 19044 21H2 (Win10)
        Win11 = 2200,          // 22000 21H2 (Win11)
        Win11Anniversary = 2262    //22621 22H2 (Win11)
    }
}
