
using Contracts.Agents;
using KSP.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KerbalWitchery {

    public abstract class KWParams : GameParameters.CustomParameterNode {
        public override string Section => "KerbalWitchery";
        public override string DisplaySection => "#KWLOC_mod_title";
    }

    public class KWGeneralOptions : KWParams {
        public override GameParameters.GameMode GameMode => GameParameters.GameMode.ANY;
        public override bool HasPresets => false;
        public override int SectionOrder => 2;
        public override string Title => "#autoLOC_189661";

        [GameParameters.CustomIntParameterUI("#KWLOC_stockThrottle_title", toolTip = "#KWLOC_stockThrottle_toolTip")]
        public bool stockThrottle = true;

        [GameParameters.CustomStringParameterUI("title0", title = "#autoLOC_900885", autoPersistance = false, lines = 2)]
        public string title0 = "";
        [GameParameters.CustomFloatParameterUI("#autoLOC_148167", displayFormat = "P0")]
        public float volEditorMus = GameSettings.MUSIC_VOLUME;
        // [GameParameters.CustomFloatParameterUI("#autoLOC_300900", displayFormat = "P0")]
        // public float volKSCMus = GameSettings.MUSIC_VOLUME;
        [GameParameters.CustomFloatParameterUI("#autoLOC_360600", displayFormat = "P0")]
        public float volTrackMus = GameSettings.MUSIC_VOLUME;

    }

    //public class KWKerbalOptions : KWParams {
    //    public override GameParameters.GameMode GameMode => GameParameters.GameMode.ANY;
    //    public override bool HasPresets => true;
    //    public override int SectionOrder => 0;
    //    public override string Title => "#autoLOC_900441";

    //    [GameParameters.CustomParameterUI("#KWLOC_useSaveName", newGameOnly = true)]
    //    public bool useSaveName;
    //    [GameParameters.CustomParameterUI("#autoLOC_901054", newGameOnly = true)]
    //    public string name;
    //    [GameParameters.CustomParameterUI("#autoLOC_900447", newGameOnly = true)]
    //    public string gender;
    //    [GameParameters.CustomFloatParameterUI("#autoLOC_900436", displayFormat = "P0", newGameOnly = true)]
    //    public float courage;
    //    [GameParameters.CustomFloatParameterUI("#autoLOC_900438", displayFormat = "P0", newGameOnly = true)]
    //    public float stupidity;
    //    [GameParameters.CustomParameterUI("#autoLOC_900440", newGameOnly = true)]
    //    public bool badS;

    //    public readonly string[] genders = new string[2] { Localizer.Format("#autoLOC_900434"), Localizer.Format("#autoLOC_900444") };

    //    public override bool Interactible(MemberInfo member, GameParameters parameters) => new string[2] { nameof(gender), nameof(useSaveName) }.Contains(member.Name) ||
    //        (member.Name == nameof(name) && !parameters.CustomParams<KWKerbalOptions>().useSaveName);

    //    public override void SetDifficultyPreset(GameParameters.Preset preset) {
    //        ProtoCrewMember.Gender gend = (ProtoCrewMember.Gender)UnityEngine.Random.Range(0, 2);
    //        name = CrewGenerator.GetRandomName(gend, new KSPRandom(UnityEngine.Random.Range(1, 999999) + DateTime.Now.Millisecond));
    //        gender = genders[(int)gend];
    //        courage = 0.12f + (int)preset * 0.25f + UnityEngine.Random.Range(-0.12f, 0.13f);
    //        stupidity = 0.12f + (3 - (int)preset) * 0.25f + UnityEngine.Random.Range(-0.12f, 0.13f);
    //        badS = (int)preset * 10 > UnityEngine.Random.Range(0, 100);
    //    }

    //    public override IList ValidValues(MemberInfo member) {
    //        if (member.Name == nameof(name)) {
    //            List<string> names = new List<string>();
    //            for (int i = 0; i < 100; i++)
    //                names.Add(CrewGenerator.GetRandomName(i < 51 ? ProtoCrewMember.Gender.Female : ProtoCrewMember.Gender.Male, new KSPRandom(i + DateTime.Now.Millisecond)));
    //            return names;
    //        } else if (member.Name == nameof(gender)) return genders;
    //        return null;
    //    }

    //}

    public class KWCareerOptions : KWParams {
        public override GameParameters.GameMode GameMode => GameParameters.GameMode.CAREER;
        public override bool HasPresets => true;
        public override int SectionOrder => 1;
        public override string Title => "#autoLOC_189717";

        // agency takeovers (if disabled, lock to r&d)
        // r&d controls research & facility upgrades (if takeovers disabled)

        //[GameParameters.CustomStringParameterUI("title0", title = "#autoLOC_6002637", autoPersistance = false, lines = 2)]
        //public string title0 = "";
        [GameParameters.CustomParameterUI("#KWLOC_partsReqSts_title", toolTip = "#KWLOC_partsReqSts_toolTip")]
        public bool partsReqSts;
        [GameParameters.CustomIntParameterUI("#KWLOC_minStAgency_title", minValue = -50, maxValue = 50, toolTip = "#KWLOC_minStAgency_toolTip")]
        public int minStAgency;
        [GameParameters.CustomIntParameterUI("#KWLOC_partStThrs_title", maxValue = 100000, stepSize = 1000, toolTip = "#KWLOC_partStThrs_toolTip")]
        public int partStThrs;

        [GameParameters.CustomParameterUI("#KWLOC_takeoverBids_title", toolTip = "#KWLOC_takeoverBids_toolTip", newGameOnly = true)]
        public bool takeoverBids = false;
        //[GameParameters.CustomParameterUI("#KWLOC_kscPrograms_title", toolTip = "#KWLOC_kscPrograms_toolTip", newGameOnly = true)]
        //public bool kscPrograms;

        //[GameParameters.CustomParameterUI("starting program", newGameOnly = true)]
        //public string startProg = Localizer.Format("#autoLOC_900432");
        //[GameParameters.CustomIntParameterUI("Program starting funds", newGameOnly = true, maxValue = 1000000, stepSize = 10000, toolTip = "")]
        //public int adminFunds;
        //[GameParameters.CustomFloatParameterUI("subsidy modifier", displayFormat = "F2", maxValue = 5f, toolTip = "")]
        //public float subsidyMod;


        //[GameParameters.CustomStringParameterUI("title1", title = "#KWLOC_lifeSupport", autoPersistance = false, lines = 2)]
        //public string title1 = "";
        //[GameParameters.CustomParameterUI("#autoLOC_6002101")]
        //public bool air;


        public override bool Interactible(MemberInfo member, GameParameters parameters) {
            Dictionary<string, bool> interactReqs = new Dictionary<string, bool> {
                [nameof(minStAgency)] = parameters.CustomParams<KWCareerOptions>().partsReqSts,
                [nameof(partStThrs)] = parameters.CustomParams<KWCareerOptions>().partsReqSts,
                [nameof(takeoverBids)] = false,
                //[nameof(kscPrograms)] = false, // !parameters.CustomParams<KWCareerOptions>().takeoverBids
                //[nameof(startProg)] = parameters.CustomParams<KWCareerOptions>().kscPrograms,
                //[nameof(adminFunds)] = parameters.CustomParams<KWCareerOptions>().kscPrograms,
                //[nameof(subsidyMod)] = parameters.CustomParams<KWCareerOptions>().kscPrograms,
            };
            if (interactReqs.Keys.Contains(member.Name))
                return interactReqs[member.Name];
            return true;
        }
        
        public override void SetDifficultyPreset(GameParameters.Preset preset) {
            //adminFunds = (5 - (int)preset) * 50000;
            //subsidyMod = 3f - (int)preset;
            partsReqSts = preset > 0;
            minStAgency = (int)preset * 10 - 30;
            partStThrs = Mathf.Clamp((3 - (int)preset) * 10000, 1000, 30000);
        }
        
        public override IList ValidValues(MemberInfo member) {
            //if (member.Name == nameof(startProg)) return new string[4] { Localizer.Format("#autoLOC_900432"),
            //    ProgType.PhysSci.Description(), ProgType.Pilots.Description(), ProgType.Robotics.Description() };
            return null;
        }

    }

}
