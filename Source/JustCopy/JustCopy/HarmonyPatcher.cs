using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

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
        public static void Postfix(Dialog_ManageFoodRestrictions __instance, Rect inRect)
        {
            Rect copyRect = new Rect(RestrictionOffset, 0f, 150f, 35f);
            if (Widgets.ButtonText(copyRect, "JC_NewFromExisting".Translate(), true, true, true))
            {
                List<FloatMenuOption> floatMenuList = new List<FloatMenuOption>();
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
        }
    }

    [HarmonyPatch(typeof(Dialog_ManageOutfits), "DoWindowContents")]
    public class Dialog_ManageOutfits_DoWindowContetents
    {
        public static void Postfix(Dialog_ManageOutfits __instance, Rect inRect)
        {
            Rect copyRect = new Rect(Dialog_ManageFoodRestrictions_DoWindowContetents.RestrictionOffset, 0f, 150f, 35f);
            if (Widgets.ButtonText(copyRect, "JC_NewFromExisting".Translate(), true, true, true))
            {
                List<FloatMenuOption> floatMenuList = new List<FloatMenuOption>();
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
        }
    }

    [HarmonyPatch(typeof(Dialog_ManageDrugPolicies), "DoWindowContents")]
    public class Dialog_ManageDrugPolicies_DoWindowContetents
    {
        public static void Postfix(Dialog_ManageDrugPolicies __instance, Rect inRect)
        {
            Rect copyRect = new Rect(Dialog_ManageFoodRestrictions_DoWindowContetents.RestrictionOffset, 0f, 150f, 35f);
            if (Widgets.ButtonText(copyRect, "JC_NewFromExisting".Translate(), true, true, true))
            {
                List<FloatMenuOption> floatMenuList = new List<FloatMenuOption>();
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
        }
    }
}
