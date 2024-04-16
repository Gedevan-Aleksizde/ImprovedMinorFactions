﻿using HarmonyLib;
using TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace ImprovedMinorFactions.Patches
{
    [HarmonyPatch(typeof(AiVisitSettlementBehavior), "IsSettlementSuitableForVisitingCondition")]
    public class IsSettlementSuitableForVisitingConditionPatch
    {
        static void Postfix(ref bool __result, MobileParty mobileParty, Settlement settlement)
        {
            if (__result == true || !Helpers.isMFHideout(settlement))
                return;
            var mfHideout = Helpers.GetSettlementMFHideout(settlement);
            bool hideoutIsMercenaryOfParty = settlement.OwnerClan.IsUnderMercenaryService && settlement.OwnerClan.Kingdom == mobileParty.ActualClan.Kingdom;
            __result = settlement.Party.MapEvent == null && mfHideout.IsActive && (mobileParty.Party.Owner.MapFaction == settlement.MapFaction || hideoutIsMercenaryOfParty);
        }
    }

    // make minor faction lords more likely to patrol their hideouts
    [HarmonyPatch(typeof(DefaultTargetScoreCalculatingModel), "CalculatePatrollingScoreForSettlement")]
    public class CalculatePatrollingScoreForSettlementPatch
    {
        static void Postfix(ref float __result, Settlement settlement, MobileParty mobileParty)
        {
            if (!Helpers.isMFHideout(settlement) || !mobileParty.ActualClan.IsMinorFaction)
                return;
            __result *= 3;
        }
    }

    
}
