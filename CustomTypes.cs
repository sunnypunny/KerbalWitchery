
using Contracts.Agents;
using KSP.Localization;
using KSP.UI.Screens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Upgradeables;

namespace KerbalWitchery {

    public enum SciType {
        [Description("#KWLOC_misc")] Misc,
        [Description("#KWLOC_milestones")] Milestones,
        [Description("#KWLOC_sensors")] Sensors,
        [Description("#KWLOC_scans")] Scans,
        [Description("#KWLOC_materials")] Materials,
        [Description("#autoLOC_900174")] Crew,
        [Description("#autoLOC_6002269")] Deployed,
        [Description("#KWLOC_samples")] Samples
    }
    
    [Flags]
    public enum ProgType {
        [Description("#KWLOC_lobbySci_title")] LobbySci = 1,
        [Description("#KWLOC_lobbyCrew_title")] LobbyCrew = 2,
        [Description("#KWLOC_lobbyEng_title")] LobbyEng = 4,
        [Description("#KWLOC_physSci_title")] PhysSci = 8,
        [Description("#KWLOC_lifeSci_title")] LifeSci = 16,
        [Description("#KWLOC_earthSci_title")] EarthSci = 32,
        [Description("#KWLOC_pilots_title")] Pilots = 64,
        [Description("#KWLOC_astronauts_title")] Astronauts = 128,
        [Description("#KWLOC_spaceplanes_title")] Spaceplanes = 256,
        [Description("#KWLOC_robotics_title")] Robotics = 512,
        [Description("#KWLOC_probes_title")] Probes = 1024,
        [Description("#KWLOC_deepSpace_title")] DeepSpace = 2048,
        Lobbying = LobbySci | LobbyCrew | LobbyEng,
        Sci = PhysSci | LifeSci | EarthSci,
        Crew = Pilots | Astronauts | Spaceplanes,
        Eng = Robotics | Probes | DeepSpace,
        Lvl1 = PhysSci | Pilots | Robotics,
        Lvl2 = LifeSci | Astronauts | Probes,
        Lvl3 = EarthSci | Spaceplanes | DeepSpace
    }
    
    public class KWUtil {
        public static readonly Dictionary<KWUI.Mode, Texture2D> uiIcons = new Dictionary<KWUI.Mode, Texture2D> {
            [KWUI.Mode.Take] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/probe_control_unit", false),
            [KWUI.Mode.Negs] = GameDatabase.Instance.GetTexture("Squad/Strategies/Icons/AggressiveNegotiations", false),
            [KWUI.Mode.Bail] = GameDatabase.Instance.GetTexture("Squad/Strategies/Icons/BailOutGrant", false),
            [KWUI.Mode.FRaise] = GameDatabase.Instance.GetTexture("Squad/Strategies/Icons/FundraisingCampaign", false),
            [KWUI.Mode.IPSell] = GameDatabase.Instance.GetTexture("Squad/Strategies/Icons/ResearchRightsSellOut", false),
            [KWUI.Mode.JunkBuy] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_generic", false),
            [KWUI.Mode.JunkSell] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_robotics", false),
            [KWUI.Mode.Lead] = GameDatabase.Instance.GetTexture("Squad/Strategies/Icons/LeadershipInitiative", false),
            [KWUI.Mode.OpenSrc] = GameDatabase.Instance.GetTexture("Squad/Strategies/Icons/OpenSourceTechProgram", false),
            [KWUI.Mode.OutSrc] = GameDatabase.Instance.GetTexture("Squad/Strategies/Icons/OutsourcedResearch", false),
            [KWUI.Mode.PartBrs] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/inventory_part", false),
            [KWUI.Mode.PartBuy] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/cargo_part", false),
            [KWUI.Mode.Patents] = GameDatabase.Instance.GetTexture("Squad/Strategies/Icons/PatentsLicensing", false),
            [KWUI.Mode.Interns] = GameDatabase.Instance.GetTexture("Squad/Strategies/Icons/UnpaidResearchProgram", false),
            [KWUI.Mode.PrsOrd] = GameDatabase.Instance.GetTexture("Squad/Contracts/Icons/report", false),
            [KWUI.Mode.Train] = GameDatabase.Instance.GetTexture("Squad/Contracts/Icons/ksc", false)
        };
        public static readonly Dictionary<SciType, Texture2D> sciIcons = new Dictionary<SciType, Texture2D> {
            [SciType.Misc] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_nanolathing", false),
            [SciType.Milestones] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_experimentalrocketry", false),
            [SciType.Sensors] = GameDatabase.Instance.GetTexture("Squad/Contracts/Icons/thermometer", false),
            [SciType.Scans] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/kerbnet_access", false),
            [SciType.Materials] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_composites", false),
            [SciType.Crew] = GameDatabase.Instance.GetTexture("Squad/Contracts/Icons/eva", false),
            [SciType.Deployed] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/deployed_science_part", false),
            [SciType.Samples] = GameDatabase.Instance.GetTexture("Squad/Contracts/Icons/sample", false) };
        public static readonly Dictionary<ProgType, Texture2D> adminIcons = new Dictionary<ProgType, Texture2D> {
            [ProgType.LobbySci] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_advsciencetech", false),
            [ProgType.LobbyCrew] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_evatech", false),
            [ProgType.LobbyEng] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_precisionengineering", false),
            [ProgType.PhysSci] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/enviro_sensor", false),
            [ProgType.LifeSci] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/experience_management", false),
            [ProgType.EarthSci] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/fuels_ore", false),
            [ProgType.Pilots] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_advexploration", false),
            [ProgType.Astronauts] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_start", false),
            [ProgType.Spaceplanes] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_highaltitudeflight", false),
            [ProgType.Robotics] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_robotics", false),
            [ProgType.Probes] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_advunmanned", false),
            [ProgType.DeepSpace] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_veryheavyrocketry", false) };
        public static readonly bool lifeSupport = System.IO.File.Exists(KSPUtil.ApplicationRootPath + "GameData/KerbalWitchery/CFGs/LifeSupport.cfg");
        public static readonly Dictionary<string, ConfigNode.ValueList> bodyResources = 
            GameDatabase.Instance.GetConfigNode("KerbalWitchery/KWBodyResources").GetNodes().ToDictionary(n => n.name, n => n.values);
        //public static readonly Dictionary<TankType, string[]> tankResources = GameDatabase.Instance.GetConfigNode("KerbalWitchery/KWTanks").GetNodes().ToDictionary(
        //    n => (TankType)Enum.Parse(typeof(TankType), n.name), n => n.GetValues());
        public static readonly string labConverterIDs = GameDatabase.Instance.GetConfigNode("KerbalWitchery/KWScience").GetValue("LabConverterIDs");
        public static readonly string evaKitConsumerIDs = GameDatabase.Instance.GetConfigNode("KerbalWitchery/KWScience").GetValue("EVAKitConsumerIDs");
        public static bool CareerMode() => HighLogic.CurrentGame.Mode == Game.Modes.CAREER;
        public static bool KSCScene() => HighLogic.LoadedScene == GameScenes.SPACECENTER;
        public static bool EditorScene() => HighLogic.LoadedSceneIsEditor;
        public static bool FlightScene() => HighLogic.LoadedSceneIsFlight;
        public static bool TrackScene() => HighLogic.LoadedScene == GameScenes.TRACKSTATION;
        // public static KWKerbalOptions KerbalOpts() => HighLogic.CurrentGame.Parameters.CustomParams<KWKerbalOptions>();
        public static KWCareerOptions CareerOpts() => HighLogic.CurrentGame.Parameters.CustomParams<KWCareerOptions>();
        public static KWGeneralOptions GenOpts() => HighLogic.CurrentGame.Parameters.CustomParams<KWGeneralOptions>();
        public static GameParameters.SpaceCenterParams SCParams() => HighLogic.CurrentGame.Parameters.SpaceCenter;
        // public static bool IsNewGame() => KWAgencies.Player() == null; // change to => IsTechLocked("FlagPole");
        public static ProtoTechNode[] TechTree() => AssetBase.RnDTechTree.GetTreeTechs();
        public static bool IsTechLocked(string techID) => TechTree().Select(t => t.techID).Contains(techID) && ResearchAndDevelopment.Instance.GetTechState(techID) == null;
        public static void UnlockTech(string techID) { if (IsTechLocked(techID)) ResearchAndDevelopment.Instance.UnlockProtoTechNode(new ProtoTechNode { techID = techID, scienceCost = 1 }); }
        // public static Agent PlayerAgent() => AgentList.Instance.GetAgentbyTitle(GameOpts().agency);
        // public static bool PlayerIsAgent(Agent agent) => agent.Title == GameOpts().agency;
        public static void LockAllParts() {
            foreach (string techID in TechTree().Select(t => t.techID)) {
                ProtoTechNode node = ResearchAndDevelopment.Instance.GetTechState(techID);
                if (node != null) {
                    node.partsPurchased = new List<AvailablePart>();
                    ResearchAndDevelopment.Instance.SetTechState(node.techID, node); }}
        }
        public static void UnlockParts() {
            foreach (AvailablePart part in PartLoader.LoadedPartsList.Where(p => ResearchAndDevelopment.PartTechAvailable(p) && KWAgencies.PartThresholdMet(p))) {
                ProtoTechNode node = ResearchAndDevelopment.Instance.GetTechState(part.TechRequired);
                node.partsPurchased.Add(part);
                ResearchAndDevelopment.Instance.SetTechState(node.techID, node); }
        }
        public static void UpgradeFacility(SpaceCenterFacility facility, int lvl = 1) {
            if (lvl == 1)
                foreach (DestructibleBuilding db in UnityEngine.Object.FindObjectsOfType<DestructibleBuilding>()?.Where(d =>
                    Enum.TryParse(d.id.Split('/')[1], out SpaceCenterFacility fc) && fc == facility)) {
                    db.Repair();
                    foreach (DestructibleBuilding.CollapsibleObject co in db.CollapsibleObjects)
                        co.collapseObject.SetActive(true); }
            else if (ScenarioUpgradeableFacilities.protoUpgradeables.ContainsKey("SpaceCenter/" + facility))
                foreach (UpgradeableFacility uf in ScenarioUpgradeableFacilities.protoUpgradeables["SpaceCenter/" + facility].facilityRefs)
                    uf.SetLevel(lvl);
        }
        public static void UpdateParts() {
            // if (ResearchAndDevelopment.Instance.GetTechState("found").partsPurchased.Count == 0)
                foreach (AvailablePart part in PartLoader.LoadedPartsList.Where(p =>
                    p.TechRequired != null && !p.TechRequired.Contains("Unresearcheable") && !p.TechHidden && !p.name.StartsWith("kerbalEVA") && p.name != "flag"))
                    part.TechRequired = KWAgencies.GetManufacturer(part)?.Agent().LogoURL.Split('/').Last() ?? "found";
        }
        public static double GetCabPrskPa(Part part) => (part.Resources.Get("IntakeAir").amount / part.Resources.Get("IntakeAir").maxAmount) * 101.325;
        public static void OpenCapsule(Part part) {
            part.Resources.Get("IntakeAir").amount = (part.vessel.mainBody.atmosphereContainsOxygen ? part.vessel.staticPressurekPa / 101.325 : 0) * part.Resources.Get("IntakeAir").maxAmount;

            // int airID = PartResourceLibrary.Instance.GetDefinition("IntakeAir").id;
            // int igID = PartResourceLibrary.Instance.GetDefinition("InertGas").id;
            // int oxID = PartResourceLibrary.Instance.GetDefinition("Oxidizer").id;
            // int toxID = PartResourceLibrary.Instance.GetDefinition("ToxicGas").id;
            // double cabAir = part.Resources.Get(airID).amount;
            // double cabIG = part.Resources.Get(igID).amount;
            // double cabOx = part.Resources.Get(oxID).amount;
            // double cabTox = part.Resources.Get(toxID).amount;
            //double atmPrs = part.vessel.staticPressurekPa;
            //double oneAtmCab = part.Resources.Get("Oxidizer").maxAmount * 0.2;
            //part.Resources.Get("InertGas").amount = part.vessel.mainBody.atmosphereContainsOxygen ? (atmPrs / 101.325) * oneAtmCab * 0.79 : 0;
            //part.Resources.Get("Oxidizer").amount = part.vessel.mainBody.atmosphereContainsOxygen ? (atmPrs / 101.325) * oneAtmCab * 0.21 : 0;
            //part.Resources.Get("ToxicGas").amount = ("Eve|Duna".Contains(part.vessel.mainBody.name) && atmPrs > 0) ? (atmPrs / 101.325) * oneAtmCab : 0;
            // part.Resources.Get("Oxidizer").amount = 0;
            // part.Resources.Get("InertGas").amount = 0;
            // if (cabOx > 0) part.RequestResource(oxID, cabOx, ResourceFlowMode.NO_FLOW);
            // if (cabIG > 0) part.RequestResource(igID, cabIG, ResourceFlowMode.NO_FLOW);
            //if ("Eve|Duna".Contains(part.vessel.mainBody.name) && atmPrs > 0) {
            //    part.Resources.Get("InertGas").amount = 0;
            //    part.Resources.Get("Oxidizer").amount = 0;
            //    part.Resources.Get("ToxicGas").amount = (atmPrs / 101.325) * oneAtmCab;
            //    // if (cabAir > 0) part.RequestResource(airID, cabAir, ResourceFlowMode.NO_FLOW);
            //    //if (GetCabPrskPa(part) > atmPrs)
            //    //    part.RequestResource(toxID, (GetCabPrskPa(part) - atmPrs) / 100 * cabTox, ResourceFlowMode.NO_FLOW);
            //    //else part.TransferResource(toxID, atmPrs / 101.325 * oneAtmCab - cabTox);
            //} else {
            //    part.Resources.Get("InertGas").amount = toxicAtmo ? 0 : (atmPrs / 101.325) * oneAtmCab * 0.79;
            //    part.Resources.Get("Oxidizer").amount = toxicAtmo ? 0 : (atmPrs / 101.325) * oneAtmCab * 0.21;
            //    part.Resources.Get("ToxicGas").amount = toxicAtmo ? (atmPrs / 101.325) * oneAtmCab : 0;
            //    //if (cabTox > 0) part.RequestResource(toxID, cabTox, ResourceFlowMode.NO_FLOW);
            //    //if (atmPrs == 0 || !part.vessel.mainBody.atmosphereContainsOxygen) 
            //    //    if (cabAir > 0) part.RequestResource(airID, cabAir, ResourceFlowMode.NO_FLOW);
            //    //else if (GetCabPrskPa(part) > atmPrs)
            //    //    part.RequestResource(airID, (GetCabPrskPa(part) - atmPrs) / 100 * cabAir, ResourceFlowMode.NO_FLOW);
            //    //else part.TransferResource(airID, atmPrs / 101.325 * oneAtmCab - cabAir);
            //}
            // part.AddForce(part.airlock.forward * ((float)(cabinPrs - atmPrs) * part.mass));
        }

        public static string FormatResource(ModuleResource res) {
            if (res.rate >= 0.1) return Localizer.Format("#autoLOC_244197", new string[2] { res.resourceDef.displayName, res.rate.ToString("N2") });
            else if (res.rate >= 1 / 600f)
                return Localizer.Format("#autoLOC_244201", new string[2] { res.resourceDef.displayName, (res.rate * KSPUtil.dateTimeFormatter.Minute).ToString("N2") });
            else if (res.rate >= 1 / 36000f)
                return $"- <b>{Localizer.Format(res.resourceDef.displayName)}: </b>{Localizer.Format("#autoLOC_6001047", (res.rate * KSPUtil.dateTimeFormatter.Hour).ToString("N2"))}\n";
            else return $"- <b>{Localizer.Format(res.resourceDef.displayName)}: </b>{Localizer.Format("#autoLOC_6001046", (res.rate * KSPUtil.dateTimeFormatter.Day).ToString("N2"))}\n";
        }
        
        // may need
        public static string TimeFormatRate(double input) {
            if (input >= 0.1) return input.ToString("N2");
            else if (input >= 1 / 600f) return (input * KSPUtil.dateTimeFormatter.Minute).ToString("N2");
            else if (input >= 1 / 36000f) return (input * KSPUtil.dateTimeFormatter.Hour).ToString("N2");
            else return (input * KSPUtil.dateTimeFormatter.Day).ToString("N2");
        }

        public static bool VesselRecoveryCheck(Vessel vessel) {
            if (vessel.LandedInKSC) return true;
            if (vessel.LandedInStockLaunchSite && vessel.Parts.Count == 1 && (vessel.isEVA || vessel.Parts[0].partInfo.name == "ScienceBox")) return true;
            ScreenMessages.PostScreenMessage(Localizer.Format("#KWLOC_recoveryFailMsg"));
            return false;
        }
        public static void UpdateEditorPartList() {
            EditorPartListFilter<AvailablePart> stFilter = new EditorPartListFilter<AvailablePart>("AgencyStandings", p => KWAgencies.PartThresholdMet(p));
            EditorPartList.Instance.ExcludeFilters.AddFilter(stFilter);
            EditorPartList.Instance.Refresh();
        }
        public static ProtoCrewMember GetHero() => HighLogic.CurrentGame.CrewRoster.Applicants.Concat(HighLogic.CurrentGame.CrewRoster.Crew).FirstOrDefault(c => c.isHero);
        public static bool HeroReady() => HighLogic.CurrentGame.CrewRoster.Applicants.Concat(HighLogic.CurrentGame.CrewRoster.Crew).Any(c => c.isHero
            && c.rosterStatus != ProtoCrewMember.RosterStatus.Dead && (HighLogic.CurrentGame.Parameters.Difficulty.MissingCrewsRespawn || c.rosterStatus != ProtoCrewMember.RosterStatus.Missing));
        public static void ToggleFacilityLock(bool locked) {
            foreach (FieldInfo field in HighLogic.CurrentGame.Parameters.SpaceCenter.GetType().GetFields().Where(f => f.Name != "CanLeaveToMainMenu"))
                field.SetValue(HighLogic.CurrentGame.Parameters.SpaceCenter, !locked);
        }

    }

    public class Agency {
        public string Name { get; private set; }
        public string Leader { get; private set; }
        public double Value { get; private set; }
        public float Rep { get; private set; }
        public Dictionary<string, Dictionary<SciType, float>> Sci { get; private set; }
        public Dictionary<SciType, bool> Labs { get; private set; }
        private Dictionary<KWUI.Mode, bool> strats;
        private Dictionary<string, int> parts;
        public Agency(Agent agent) {
            Name = agent.Name;
            Leader = "#KWLOC_aiControlled";
            strats = new Dictionary<KWUI.Mode, bool>();
            Sci = PSystemManager.Instance.localBodies.ToDictionary(b => b.name, b => Enum.GetValues(typeof(SciType)).Cast<SciType>().ToDictionary(s => s, s => 0f));
            Labs = KWAgencies.Labs(this).ToDictionary(p => p, p => true) ?? new Dictionary<SciType, bool>();
            parts = new Dictionary<string, int>();
            Value = PartLoader.LoadedPartsList.Where(p => p.manufacturer == Agent().Title).Select(p => p.entryCost).Sum() + 
                KWAgencies.LabCosts().Where(l => Labs.ContainsKey(l.Key)).Select(l => l.Value).Sum();
            Rep = (float)Math.Floor(Value / 50000);
        }
        public Agency(ConfigNode node) {
            Name = node.GetValue("name");
            Leader = node.GetValue("leader");
            Value = float.Parse(node.GetValue("value"));
            Rep = float.Parse(node.GetValue("rep"));
            strats = new Dictionary<KWUI.Mode, bool>();
            foreach (ConfigNode.Value pair in node.GetNode("STRATEGIES").values)
                strats[(KWUI.Mode)Enum.Parse(typeof(KWUI.Mode), pair.name)] = bool.Parse(pair.value);
            Sci = PSystemManager.Instance.localBodies.ToDictionary(b => b.name, b => Enum.GetValues(typeof(SciType)).Cast<SciType>().ToDictionary(s => s, s => 0f));
            foreach (string body in PSystemManager.Instance.localBodies.Select(b => b.name))
                foreach (SciType type in Enum.GetValues(typeof(SciType)))
                    if (float.TryParse(node.GetNode("SCI").GetNode(body)?.GetValue("" + type), out float val)) Sci[body][type] = val;
            Labs = new Dictionary<SciType, bool>();
            foreach (ConfigNode.Value pair in node.GetNode("LABS").values)
                Labs[(SciType)Enum.Parse(typeof(SciType), pair.name)] = bool.Parse(pair.value);
            parts = new Dictionary<string, int>();
            foreach (ConfigNode.Value pair in node.GetNode("PARTS").values)
                parts[pair.name] = int.Parse(pair.value);
        }
        public void Save(ConfigNode node) {
            ConfigNode aNode = node.AddNode("AGENCY");
            aNode.AddValue("name", Name);
            aNode.AddValue("leader", Leader);
            aNode.AddValue("value", Value);
            aNode.AddValue("rep", Rep);
            ConfigNode stNode = aNode.AddNode("STRATEGIES");
            strats.ToList().ForEach(s => stNode.AddValue(s.Key + "", s.Value));
            ConfigNode sNode = aNode.AddNode("SCI");
            foreach (var pair1 in Sci.Where(p1 => p1.Value.Any(p2 => p2.Value != 0f))) {
                ConfigNode bodyNode = sNode.AddNode(pair1.Key);
                foreach (var pair2 in pair1.Value.Where(p => p.Value != 0f))
                    bodyNode.AddValue("" + pair2.Key, pair2.Value); }
            ConfigNode lNode = aNode.AddNode("LABS");
            Labs.ToList().ForEach(l => lNode.AddValue(l.Key + "", l.Value));
            ConfigNode pNode = aNode.AddNode("PARTS");
            parts.ToList().ForEach(p => pNode.AddValue(p.Key, p.Value));
        }
        public Agent Agent() => AgentList.Instance.GetAgent(Name);
        public void SetPlayer() => Leader = KWUtil.GetHero().displayName; // "#autoLOC_8001049";
        public void UnsetPlayer() => Leader = "#KWLOC_aiControlled";
        public void AddValue(double amount) => Value += amount;
        public void SetValue(double amount) => Value = amount;
        public void AddRep(float amount) => Rep += amount;
        public void SetRep(float amount) => Rep = amount;
        public void AddSci(CelestialBody body, SciType type, float amount) => Sci[body.name][type] += amount;
        public void ToggleStrat(KWUI.Mode strat, bool? active = null) => strats[strat] = active ?? (strats.ContainsKey(strat) ? !strats[strat] : true);
        public bool StratActive(KWUI.Mode strat) => strats.ContainsKey(strat) ? strats[strat] : false;
        public bool RequestPart(AvailablePart part) { if (parts.ContainsKey(part.name)) if (parts[part.name] > 0) { parts[part.name]--; return true; } return false; }
        public void StorePart(AvailablePart part) { if (!parts.ContainsKey(part.name)) parts[part.name] = 0; parts[part.name]++; }
        public int GetPartQty(AvailablePart part) => parts.ContainsKey(part.name) ? parts[part.name] : 0;
        public Dictionary<AvailablePart, int> StoredParts() => parts.Where(p => p.Value > 0).ToDictionary(p => PartLoader.getPartInfoByName(p.Key), p => p.Value);

    }
    
    public class FacilityAddon {
        public string TechID { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int Cost { get; private set; }
        public SpaceCenterFacility Facility { get; private set; }
        public FacilityAddon(ConfigNode node) {
            TechID = node.GetValue("id");
            Title = node.GetValue("title");
            Description = node.GetValue("description");
            Cost = int.Parse(node.GetValue("cost"));
            Facility = Enum.TryParse(node.GetNode("Parent").GetValue("parentID"), out SpaceCenterFacility f) ? f : SpaceCenterFacility.Administration;
        }

    }

}
