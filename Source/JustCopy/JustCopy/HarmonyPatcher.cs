using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace JustCopy
{
    [StaticConstructorOnStartup]
    internal static class HarmonyPatcher
    {
        static HarmonyPatcher()
        {
            var harmony = new Harmony("iforgotmysocks." + Assembly.GetExecutingAssembly().GetName().Name);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(Dialog_ManageFoodRestrictions), "DoWindowContents")]
    public class Dialog_ManageFoodRestrictions_DoWindowContetents
    {
        public static readonly float RestrictionOffset = 450f + 30f;
        public static readonly float RestrictionWidth = 85f;
        public static readonly float DefaultOffset = RestrictionOffset + RestrictionWidth + 10f;
        public static readonly float DefaultWidth = 77f;

        public static void Postfix(Dialog_ManageFoodRestrictions __instance)
        {
            var copyRect = new Rect(RestrictionOffset, 0f, RestrictionWidth, 35f);
            if (Widgets.ButtonText(copyRect, "JC_NewFromExisting".Translate(), true, true, true))
            {
                var floatMenuList = new List<FloatMenuOption>();
                var resDB = Current.Game.foodRestrictionDatabase;
                foreach (var cur in resDB.AllFoodRestrictions)
                {
                    floatMenuList.Add(new FloatMenuOption(cur.label, delegate ()
                    {
                        var newRes = new FoodRestriction
                        {
                            id = !resDB.AllFoodRestrictions.Any() ? 1 : resDB.AllFoodRestrictions.Max(x => x.id) + 1,
                            label = cur.label + " copy",
                            filter = new ThingFilter(),
                        };
                        newRes.filter.CopyAllowancesFrom(cur.filter);
                        resDB.AllFoodRestrictions.Add(newRes);
                        typeof(Dialog_ManageFoodRestrictions).GetField("selFoodRestrictionInt", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, newRes);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null));
                }
                Find.WindowStack.Add(new FloatMenu(floatMenuList));
            }

            var defaultRect = new Rect(Dialog_ManageFoodRestrictions_DoWindowContetents.DefaultOffset, 0f, Dialog_ManageFoodRestrictions_DoWindowContetents.DefaultWidth, 35f);
            var currentRest = typeof(Dialog_ManageFoodRestrictions).GetField("selFoodRestrictionInt", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance) as FoodRestriction;
            if (currentRest == null) return;
            if (Widgets.ButtonText(defaultRect, "JC_Default".Translate(), true, true, true))
            {
                var restList = Current.Game.foodRestrictionDatabase.AllFoodRestrictions;
                if (restList.IndexOf(currentRest) == 0)
                {
                    SoundDefOf.ClickReject.PlayOneShot(null);
                    return;
                }
                restList.Remove(currentRest);
                restList.Insert(0, currentRest);
                SoundDefOf.Click.PlayOneShot(null);
            }
        }
    }

    [HarmonyPatch(typeof(Dialog_ManageOutfits), "DoWindowContents")]
    public class Dialog_ManageOutfits_DoWindowContetents
    {
        public static void Postfix(Dialog_ManageOutfits __instance)
        {
            var copyRect = new Rect(Dialog_ManageFoodRestrictions_DoWindowContetents.RestrictionOffset, 0f, Dialog_ManageFoodRestrictions_DoWindowContetents.RestrictionWidth, 35f);
            if (Widgets.ButtonText(copyRect, "JC_NewFromExisting".Translate(), true, true, true))
            {
                var floatMenuList = new List<FloatMenuOption>();
                var resDB = Current.Game.outfitDatabase;
                foreach (var cur in resDB.AllOutfits)
                {
                    floatMenuList.Add(new FloatMenuOption(cur.label, delegate ()
                    {
                        var newRes = new Outfit
                        {
                            uniqueId = !resDB.AllOutfits.Any() ? 1 : resDB.AllOutfits.Max(x => x.uniqueId) + 1,
                            label = cur.label + " copy",
                            filter = new ThingFilter()
                        };
                        newRes.filter.CopyAllowancesFrom(cur.filter);
                        resDB.AllOutfits.Add(newRes);
                        typeof(Dialog_ManageOutfits).GetField("selOutfitInt", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, newRes);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null));
                }
                Find.WindowStack.Add(new FloatMenu(floatMenuList));
            }

            var defaultRect = new Rect(Dialog_ManageFoodRestrictions_DoWindowContetents.DefaultOffset, 0f, Dialog_ManageFoodRestrictions_DoWindowContetents.DefaultWidth, 35f);
            var currentRest = typeof(Dialog_ManageOutfits).GetField("selOutfitInt", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance) as Outfit;
            if (currentRest == null) return;
            if (Widgets.ButtonText(defaultRect, "JC_Default".Translate(), true, true, true))
            {
                var restList = Current.Game.outfitDatabase.AllOutfits;
                if (restList.IndexOf(currentRest) == 0)
                {
                    SoundDefOf.ClickReject.PlayOneShot(null);
                    return;
                }
                restList.Remove(currentRest);
                restList.Insert(0, currentRest);
                SoundDefOf.Click.PlayOneShot(null);
            }
        }
    }

    [HarmonyPatch(typeof(Dialog_ManageDrugPolicies), "DoWindowContents")]
    public class Dialog_ManageDrugPolicies_DoWindowContetents
    {
        public static void Postfix(Dialog_ManageDrugPolicies __instance)
        {
            var copyRect = new Rect(Dialog_ManageFoodRestrictions_DoWindowContetents.RestrictionOffset, 0f, Dialog_ManageFoodRestrictions_DoWindowContetents.RestrictionWidth, 35f);
            if (Widgets.ButtonText(copyRect, "JC_NewFromExisting".Translate(), true, true, true))
            {
                var floatMenuList = new List<FloatMenuOption>();
                var resDB = Current.Game.drugPolicyDatabase;
                foreach (var cur in resDB.AllPolicies)
                {
                    floatMenuList.Add(new FloatMenuOption(cur.label, delegate ()
                    {
                        var newRes = new DrugPolicy
                        {
                            uniqueId = !resDB.AllPolicies.Any() ? 1 : resDB.AllPolicies.Max(x => x.uniqueId) + 1,
                            label = cur.label + " copy",
                        };

                        var curDrugEntries = (typeof(DrugPolicy).GetField("entriesInt", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(cur) as List<DrugPolicyEntry>);
                        var newDrugEntryList = new List<DrugPolicyEntry>();
                        foreach (var e in curDrugEntries)
                        {
                            var newPolE = new DrugPolicyEntry();
                            newPolE.CopyFrom(e);
                            newDrugEntryList.Add(newPolE);
                        }
                        typeof(DrugPolicy).GetField("entriesInt", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newRes, newDrugEntryList);

                        resDB.AllPolicies.Add(newRes);
                        typeof(Dialog_ManageDrugPolicies).GetField("selPolicy", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, newRes);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null));
                }
                Find.WindowStack.Add(new FloatMenu(floatMenuList));
            }

            var defaultRect = new Rect(Dialog_ManageFoodRestrictions_DoWindowContetents.DefaultOffset, 0f, Dialog_ManageFoodRestrictions_DoWindowContetents.DefaultWidth, 35f);
            var currentRest = typeof(Dialog_ManageDrugPolicies).GetField("selPolicy", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance) as DrugPolicy;
            if (currentRest == null) return;
            if (Widgets.ButtonText(defaultRect, "JC_Default".Translate(), true, true, true))
            {
                var restList = Current.Game.drugPolicyDatabase.AllPolicies;
                if (restList.IndexOf(currentRest) == 0)
                {
                    SoundDefOf.ClickReject.PlayOneShot(null);
                    return;
                }
                restList.Remove(currentRest);
                restList.Insert(0, currentRest);
                SoundDefOf.Click.PlayOneShot(null);
            }
        }
    }
}
