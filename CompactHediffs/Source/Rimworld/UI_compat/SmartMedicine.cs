using PeteTimesSix.CompactHediffs.ModCompat;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace PeteTimesSix.CompactHediffs.Rimworld.UI_compat
{
    public static class UI_SmartMedicine
	{

		public static void DrawSmartMedicineIcon(Rect rowRect, List<Hediff> hediffs, ref float widthAccumulator, int iconWidth)
		{
			var maxCareHediff = HighestCarePriority(hediffs);
			if(maxCareHediff == null)
				return;

            var icons = SmartMedicine.GetElements(maxCareHediff);

            foreach (var icon in icons)
            {
				var iconRect = new Rect(rowRect.width - widthAccumulator - iconWidth, rowRect.y, iconWidth, rowRect.height).Rounded();
				icon.Invoke(iconRect);
				widthAccumulator += iconWidth;
			}
        }

		public static void AddSmartMedicineFloatMenuButton(Rect buttonRect, List<Hediff> hediffs, MedicalCareCategory defaultCare)
        {
            if (Event.current.button != 1 || !Widgets.ButtonInvisible(buttonRect) || !hediffs.Any(h => h.TendableNow(true)))
                return;

            var list = SmartMedicine.CreateCareMenuOptionsWithList(hediffs.ToList());
            Find.WindowStack.Add(new FloatMenu(list));
        }

        private static Hediff HighestCarePriority(List<Hediff> hediffs) //heck if I know how to get an out parameter through traverse
		{
			var care = MedicalCareCategory.NoCare;
			Hediff maxCareHediff = null;
			var hediffCares =  SmartMedicine.PriorityCareCompGet();
			foreach (Hediff h in hediffs)
			{
				if (h.TendableNow(true) && hediffCares.TryGetValue(h, out MedicalCareCategory heCare))
				{
					care = heCare > care ? heCare : care;
					maxCareHediff = h;
				}
			}
			return maxCareHediff;
		}
	}
}

