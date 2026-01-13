using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;

namespace PinHoard.view.style
{
    /// <summary>
    /// Represents a colour palette for the applications UI elements where all strings are hex colour codes
    /// </summary>
    public class AppStyle
    {
        public string StyleName { get; set; }

        // background of all windows, as well as widgets placed on secondary background
        public string PrimaryBackground { get; set; } 

        // background of scrollviewer panels including the pin area in the board window
        public string SecondaryBackground { get; set; }

        // toolbar background
        public string LightAccent { get; set; }

        // toolbar popout containers background
        public string DarkAccent { get; set; }

        // background for all primary action buttons
        public string PrimaryButtonBackground { get; set; }

        // background for all buttons in a window that are not primary
        public string SecondaryButtonBackground { get; set; }

        // background for delete buttons
        public string DangerButtonBackground { get; set; }

        // majority text colour
        public string PrimaryTextColour { get; set; }

        // secondary text colour for less important information
        public string SecondaryTextColour { get; set; }

        // text colour for question text in quizzes
        public string HighlightTextColour { get; set; }
        public AppStyle(
            string styleName,
            string primaryBackground, string secondaryBackground,
            string lightAccent, string darkAccent,
            string primaryButtonBackground, string secondaryButtonBackground,
            string dangerButtonBackground,
            string primaryTextColour, string secondaryTextColour, string highlightTextColour)
        {
            StyleName = styleName;
            PrimaryBackground = primaryBackground;
            SecondaryBackground = secondaryBackground;
            LightAccent = lightAccent;
            DarkAccent = darkAccent;
            PrimaryButtonBackground = primaryButtonBackground;
            SecondaryButtonBackground = secondaryButtonBackground;
            DangerButtonBackground = dangerButtonBackground;
            PrimaryTextColour = primaryTextColour;
            SecondaryTextColour = secondaryTextColour;
            HighlightTextColour = highlightTextColour;
        }
    }

    internal static class Styles
    {
        public static AppStyle PinHoardClassic = new AppStyle(
            styleName: "PinHoard Classic",
            primaryBackground: "#FFFCF8F3",    
            secondaryBackground: "#FFF2E7D3", 
            primaryButtonBackground: "#FFF69176", 
            secondaryButtonBackground: "#FFFDC6B7",
            primaryTextColour: "#FF223E5B",
            dangerButtonBackground: "#FFF69176",
            lightAccent: "#FFF69176",
            darkAccent: "#FFF1623C",
            secondaryTextColour: "#FF223E5B",
            highlightTextColour: "#FF0000FF"
        );

        public static AppStyle PinHoardModernLight = new AppStyle(
            styleName: "Modern Light",
            primaryBackground: "#FFF7F8FA",  
            secondaryBackground: "#FFE9ECEF",
            primaryButtonBackground: "#FF5F8DB8",
            secondaryButtonBackground: "#FFCEDBE8",
            primaryTextColour: "#FF1F2933",
            dangerButtonBackground: "#FFCF3F3F",
            lightAccent: "#FFE3EBF4",
            darkAccent: "#FFD2E0EE",
            secondaryTextColour: "#FF6B7280",
            highlightTextColour: "#FF1D4ED8"
        );

        public static AppStyle PinHoardModernDark = new AppStyle(
            styleName: "Modern Dark",
            primaryBackground: "#FF121826",
            secondaryBackground: "#FF1E2636",
            primaryButtonBackground: "#FF4FA3D1",
            secondaryButtonBackground: "#FF2E3B4E",
            primaryTextColour: "#FFE5E7EB",
            dangerButtonBackground: "#FFD05353",
            lightAccent: "#FF24324A",
            darkAccent: "#FF2F3F5C",
            secondaryTextColour: "#FF9CA3AF",
            highlightTextColour: "#FF7DD3FC"
        );

        public static AppStyle LightDefault = new AppStyle(
            styleName: "Default (Light)",
            primaryBackground: "#FFFFFFFF",
            secondaryBackground: "#FFF4F5F7",
            primaryButtonBackground: "#FF374151",
            secondaryButtonBackground: "#FFE5E7EB",
            primaryTextColour: "#FF111827", 
            dangerButtonBackground: "#FFDC2626",
            lightAccent: "#FFF3F4F6",
            darkAccent: "#FFE5E7EB",
            secondaryTextColour: "#FF6B7280",
            highlightTextColour: "#FF2563EB"
        );
    }

    public static class StyleManager
    {
        public const string PrimaryBackgroundKey = "PrimaryBackgroundBrush";
        public const string SecondaryBackgroundKey = "SecondaryBackgroundBrush";
        public const string LightAccentKey = "LightAccentBrush";
        public const string DarkAccentKey = "DarkAccentBrush";
        public const string PrimaryButtonBackgroundKey = "PrimaryButtonBackgroundBrush";
        public const string SecondaryButtonBackgroundKey = "SecondaryButtonBackgroundBrush";
        public const string DangerButtonBackgroundKey = "DangerButtonBackgroundBrush";
        public const string PrimaryTextKey = "PrimaryTextBrush";
        public const string SecondaryTextKey = "SecondaryTextBrush";
        public const string HighlightTextKey = "HighlightTextBrush";

        public static readonly Dictionary<int, AppStyle> StyleLibrary = new Dictionary<int, AppStyle>()
        {
            { 0, Styles.PinHoardClassic },
            { 1, Styles.PinHoardModernLight },
            { 2, Styles.PinHoardModernDark },
            { 3, Styles.LightDefault }
        };

        public static AppStyle GetStyle(int index)
        {
            if (StyleLibrary.TryGetValue(index, out AppStyle style))
            {
                return style;
            }
            else return StyleLibrary[0];
        }

        public static void ApplyStyle(int styleIndex)
        {
            AppStyle style = GetStyle(styleIndex);

            Application.Current.Dispatcher.Invoke(() =>
            {
                SetBrush(PrimaryBackgroundKey, style.PrimaryBackground);
                SetBrush(SecondaryBackgroundKey, style.SecondaryBackground);
                SetBrush(LightAccentKey, style.LightAccent);
                SetBrush(DarkAccentKey, style.DarkAccent);
                SetBrush(PrimaryButtonBackgroundKey, style.PrimaryButtonBackground);
                SetBrush(SecondaryButtonBackgroundKey, style.SecondaryButtonBackground);
                SetBrush(DangerButtonBackgroundKey, style.DangerButtonBackground);
                SetBrush(PrimaryTextKey, style.PrimaryTextColour);
                SetBrush(SecondaryTextKey, style.SecondaryTextColour);
                SetBrush(HighlightTextKey, style.HighlightTextColour);
            });
        }
        static void SetBrush(string key, string hex)
        {
            var color = (Color)ColorConverter.ConvertFromString(hex);
            Application.Current.Resources[key] = new SolidColorBrush(color);
        }
    }
}
