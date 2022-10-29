using System;
using UnityEngine;
using Verse;

namespace JustCopy
{
    class ModSettings : Verse.ModSettings
    {
        public static int xOffset = 0;
        public static int yOffset = 0;
        private const float buttonWidth = 140f;
        private const float labelOffset = 160f;
        public override void ExposeData()
        {
            Scribe_Values.Look(ref xOffset, "xOffset", 0);
            Scribe_Values.Look(ref yOffset, "yOffset", 0);
        }

        public void DoWindowContents(Rect wrect)
        {
            var options = new Listing_Standard();
            options.Begin(wrect);
            var labelWidth = options.ColumnWidth - labelOffset - 20;
            var btnlblHeight = Text.LineHeight * 1.5f;
            var lblYOffset = Text.LineHeight * 0.25f;
            options.Label($@"0 is default for both options. To enter negative values first enter a number that isn't 0, then add the ""-"" infront.");
            options.Gap();
            options.Label($"Use presets or adjust manually below. Presets may not fit for all resolutions.");
            options.Gap();
            var rectdefault = options.GetRect(btnlblHeight);
            rectdefault.width = buttonWidth;
            if (Widgets.ButtonText(rectdefault, "Default"))
            {
                xOffset = 0;
                yOffset = 0;
            }
            Widgets.Label(new Rect(labelOffset, rectdefault.y + lblYOffset, labelWidth, btnlblHeight), "0/0, default position.");

            var rectb1 = options.GetRect(btnlblHeight);
            rectb1.width = buttonWidth;
            if (Widgets.ButtonText(rectb1, "Next line, left"))
            {
                xOffset = -255;
                yOffset = 48;
            }
            Widgets.Label(new Rect(labelOffset, rectb1.y + lblYOffset, labelWidth, btnlblHeight), "-255/48, places the buttons one line below to the left.");
            
            var rectb2 = options.GetRect(btnlblHeight);
            rectb2.width = buttonWidth;
            if (Widgets.ButtonText(rectb2, "Window bottom, left"))
            {
                xOffset = -480;
                yOffset = 628;
            }
            Widgets.Label(new Rect(labelOffset, rectb2.y + lblYOffset, labelWidth, btnlblHeight), "-480/628, places the buttons on the window bottom to the left.");

            options.Gap();
            options.Gap();

            var rect = options.GetRect(Text.LineHeight);
            rect.width = options.ColumnWidth * 0.92f;
            Widgets.Label(rect, "xOffset: ");
            var xOffsetInput = Widgets.TextField(new Rect(70, rect.y, 60, Text.LineHeight), ModSettings.xOffset.ToString());
            if (double.TryParse(xOffsetInput, out var xOffsetDouble)) ModSettings.xOffset = Convert.ToInt32(xOffsetDouble);

            options.Gap();

            var rect2 = options.GetRect(Text.LineHeight);
            rect2.width = options.ColumnWidth * 0.92f;
            Widgets.Label(rect2, "yOffset: ");
            var yOffsetInput = Widgets.TextField(new Rect(70, rect2.y, 60, Text.LineHeight), ModSettings.yOffset.ToString());
            if (double.TryParse(yOffsetInput, out var yOffsetDouble)) ModSettings.yOffset = Convert.ToInt32(yOffsetDouble);

            options.End(); 
            this.Write();
        }
    }
}
