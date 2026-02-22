using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using static PeteTimesSix.CompactHediffs.ModCompat.EBF;

namespace PeteTimesSix.CompactHediffs.ModCompat
{
	[StaticConstructorOnStartup]
    public static class SmartMedicine
    {
        public static bool active = false;

		public static Texture2D[] careTextures;

		public delegate List<FloatMenuOption> _CreateCareMenuOptionsWithList(List<Hediff> affectedHediffs, Hediff primaryHediff = null);
        public delegate List<Action<Rect>> _GetElements(Hediff hediff);
		public delegate Dictionary<Hediff, MedicalCareCategory> _PriorityCareCompGet();
		public delegate MedicalCareCategory _GetCare(Pawn pawn);

        public static _CreateCareMenuOptionsWithList CreateCareMenuOptionsWithList;
        public static _GetElements GetElements;
		public static _PriorityCareCompGet PriorityCareCompGet;
		public static _GetCare GetCare;

		static SmartMedicine()
		{
			active = ModLister.AnyModActiveNoSuffix(new[] {"Memegoddess.SmartMedicine" });
            if (!active) return;

            try
            {
                careTextures = AccessTools.StaticFieldRefAccess<Texture2D[]>(AccessTools.Field(typeof(MedicalCareUtility), "careTextures")).Invoke();

                PriorityCareCompGet = AccessTools.MethodDelegate<_PriorityCareCompGet>(AccessTools.Method("SmartMedicine.PriorityCareSettingsComp:Get"));
                GetCare = AccessTools.MethodDelegate<_GetCare>(AccessTools.Method("SmartMedicine.GetPawnMedicalCareCategory:GetCare"));

                CreateCareMenuOptionsWithList = AccessTools.MethodDelegate<_CreateCareMenuOptionsWithList>(
                    AccessTools.Method("SmartMedicine.HediffRowPriorityCare:CreateCareMenuOptionsWithList"));

                GetElements = AccessTools.MethodDelegate<_GetElements>(
                    AccessTools.Method("SmartMedicine.HediffRowPriorityCare:GetElements"));
            }
            catch (Exception ex)
            {
                Log.Error($"Compact hediffs - SmartMedicine patch failed." +
                          $"\n{nameof(careTextures)}: {careTextures != null}" +
                          $"\n{nameof(PriorityCareCompGet)}: {PriorityCareCompGet != null}" +
                          $"\n{nameof(GetCare)}: {GetCare != null}" +
                          $"\n{nameof(CreateCareMenuOptionsWithList)}: {CreateCareMenuOptionsWithList != null}" +
                          $"\n{nameof(GetElements)}: {GetElements != null}" +
                          $"\n{ex.Message}" +
                          $"\n{ex.StackTrace}");
                active = false;
            }
        }
    }
}
