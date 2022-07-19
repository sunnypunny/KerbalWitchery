
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
            [KWUI.Mode.Negs] = GameDatabase.Instance.GetTexture("Squad/Strategies/Icons/AggressiveNegotiations", false),
            [KWUI.Mode.AppCmpn] = GameDatabase.Instance.GetTexture("Squad/Strategies/Icons/AppreciationCampaign", false),
            [KWUI.Mode.Bail] = GameDatabase.Instance.GetTexture("Squad/Strategies/Icons/BailOutGrant", false),
            [KWUI.Mode.FRaise] = GameDatabase.Instance.GetTexture("Squad/Strategies/Icons/FundraisingCampaign", false),
            [KWUI.Mode.IPSell] = GameDatabase.Instance.GetTexture("Squad/Strategies/Icons/ResearchRightsSellOut", false),
            [KWUI.Mode.JunkBuy] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_robotics", false),
            [KWUI.Mode.JunkSell] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_specializedelectrics", false),
            [KWUI.Mode.Lead] = GameDatabase.Instance.GetTexture("Squad/Strategies/Icons/LeadershipInitiative", false),
            [KWUI.Mode.OpenSrc] = GameDatabase.Instance.GetTexture("Squad/Strategies/Icons/OpenSourceTechProgram", false),
            [KWUI.Mode.OutSrc] = GameDatabase.Instance.GetTexture("Squad/Strategies/Icons/OutsourcedResearch", false),
            [KWUI.Mode.PartBrs] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/inventory_part", false),
            [KWUI.Mode.PartBuy] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/cargo_part", false),
            [KWUI.Mode.Patents] = GameDatabase.Instance.GetTexture("Squad/Strategies/Icons/PatentsLicensing", false),
            [KWUI.Mode.Interns] = GameDatabase.Instance.GetTexture("Squad/Strategies/Icons/UnpaidResearchProgram", false),
            [KWUI.Mode.PrsOrd] = GameDatabase.Instance.GetTexture("Squad/Contracts/Icons/report", false),
            [KWUI.Mode.Train] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/deployable_ground_part", false),
            [KWUI.Mode.Take] = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/probe_control_unit", false)
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
        // public static readonly bool lifeSupport = System.IO.File.Exists(KSPUtil.ApplicationRootPath + "GameData/KerbalWitchery/CFGs/LifeSupport.cfg");
        public static readonly Dictionary<string, ConfigNode.ValueList> bodyResources = 
            GameDatabase.Instance.GetConfigNode("KerbalWitchery/KWBodyResources").GetNodes().ToDictionary(n => n.name, n => n.values);
        //public static readonly Dictionary<TankType, string[]> tankResources = GameDatabase.Instance.GetConfigNode("KerbalWitchery/KWTanks").GetNodes().ToDictionary(
        //    n => (TankType)Enum.Parse(typeof(TankType), n.name), n => n.GetValues());
        public static readonly Dictionary<Currency, string> cSprite = new Dictionary<Currency, string> {
            [Currency.Funds] = "<sprite=\"CurrencySpriteAsset\" name=\"Funds\" color=#B4D455>",
            [Currency.Reputation] = "<sprite=\"CurrencySpriteAsset\" name=\"Reputation\" color=#E0D503>",
            [Currency.Science] = "<sprite=\"CurrencySpriteAsset\" name=\"Science\" color=#6DCFF6>"
        };
        public static readonly string labConverterIDs = GameDatabase.Instance.GetConfigNode("KerbalWitchery/KWScience").GetValue("LabConverterIDs");
        public static readonly string evaKitConsumerIDs = GameDatabase.Instance.GetConfigNode("KerbalWitchery/KWScience").GetValue("EVAKitConsumerIDs");
        // public static bool CareerMode() => HighLogic.CurrentGame.Mode == Game.Modes.CAREER;
        // public static bool KSCScene() => HighLogic.LoadedScene == GameScenes.SPACECENTER;
        // public static bool EditorScene() => HighLogic.LoadedSceneIsEditor;
        // public static bool FlightScene() => HighLogic.LoadedSceneIsFlight;
        // public static bool TrackScene() => HighLogic.LoadedScene == GameScenes.TRACKSTATION;
        // public static KWKerbalOptions KerbalOpts() => HighLogic.CurrentGame.Parameters.CustomParams<KWKerbalOptions>();
        public static KWCareerOptions CareerOpts() => HighLogic.CurrentGame.Parameters.CustomParams<KWCareerOptions>();
        public static KWGeneralOptions GenOpts() => HighLogic.CurrentGame.Parameters.CustomParams<KWGeneralOptions>();
        // public static GameParameters.SpaceCenterParams SCParams() => HighLogic.CurrentGame.Parameters.SpaceCenter;
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

        public static bool VesselRecoveryCheck(Vessel vessel, bool feedback = true) {
            if (vessel.LandedInKSC || ScienceUtil.GetExperimentBiome(vessel.mainBody, vessel.latitude, vessel.longitude) == "KSC") return true;
            else if (vessel.LandedInStockLaunchSite && vessel.Parts.Count == 1 && (vessel.isEVA || vessel.Parts[0].partInfo.name == "ScienceBox")) return true;
            else if (feedback) ScreenMessages.PostScreenMessage(Localizer.Format("#KWLOC_recoveryFailMsg"));
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
        public static void SpawnHero() {
            ProtoCrewMember hero = GetHero();
            ConfigNode[] parts = new ConfigNode[1] { ProtoVessel.CreatePartNode(hero.gender == ProtoCrewMember.Gender.Female ? "kerbalEVAfemale" : "kerbalEVA",
                ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState), hero) };
            CelestialBody cb = Planetarium.fetch.Home;
            Vector3d pos = cb.GetWorldSurfacePosition(-0.08311596808409738, -74.611498206188756, 68.2);
            Orbit orbit = new Orbit(0, 0, 0, 0, 0, 0, 0, cb);
            orbit.UpdateFromStateVectors(pos, cb.getRFrmVel(pos), cb, Planetarium.GetUniversalTime());
            ConfigNode vesselNode = ProtoVessel.CreateVesselNode(hero.name, VesselType.EVA, orbit, 0, parts);
            vesselNode.SetValue("sit", Vessel.Situations.LANDED.ToString());
            vesselNode.SetValue("landed", true);
            vesselNode.SetValue("splashed", false);
            vesselNode.SetValue("lat", -0.08311596808409738);
            vesselNode.SetValue("lon", -74.611498206188756);
            vesselNode.SetValue("alt", 68.2);
            vesselNode.SetValue("landedAt", cb.name);
            GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
            KSCVesselMarkers.fetch.RefreshMarkers();
        }

        public static void ToggleHelmet(bool enabled, ProtoCrewMember crew, KerbalEVA eva = null) {
            crew.hasHelmetOn = enabled;
            if (eva == null) {
                crew.KerbalRef.helmetTransform.gameObject.SetActive(enabled);
                crew.KerbalRef.showHelmet = enabled;
            } else {
                crew.hasNeckRingOn = enabled;
                typeof(KerbalEVA).GetField("isHelmetEnabled", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(eva, enabled);
                typeof(KerbalEVA).GetField("isNeckRingEnabled", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(eva, enabled);
                eva.helmetTransform.gameObject.SetActive(enabled);
                eva.neckRingTransform.gameObject.SetActive(enabled);
                GameEvents.OnHelmetChanged.Fire(eva, enabled, enabled);
                if (!enabled) typeof(KerbalEVA).GetMethod("UpdateVisorEventStates", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(eva, null);
            }
        }

        public static void UpdateHelmets() {
            foreach (ProtoCrewMember crew in FlightGlobals.ActiveVessel.GetVesselCrew().Where(c => !c.KerbalInventoryModule.ContainsPart("KWhelmet")))
                ToggleHelmet(false, crew);
        }

        public static ProtoPartResourceSnapshot[] GetInvResources(string resourceName, ModuleInventoryPart inv) {
            return inv.storedParts.Values.Where(p => p.snapshot.resources[0]?.resourceName == resourceName).OrderByDescending(p =>
                p.snapshot.resourcePriorityOffset).Select(p => p.snapshot.resources[0]).ToArray();
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
        public void Takeover() {
            HighLogic.CurrentGame.flagURL = Agent().LogoURL;
            ProtoCrewMember leader = KWUtil.GetHero();
            Leader = leader.displayName;
            leader.type = ProtoCrewMember.KerbalType.Crew;
            UpdateCurrencies();
            KWUtil.ToggleFacilityLock(false);
            // KWUtil.UpgradeFacility(SpaceCenterFacility.MissionControl);
        }
        public void Quit() {
            Leader = "#KWLOC_aiControlled";
            strats.Clear();
            KWUtil.ToggleFacilityLock(true);
            List<Vessel> vList = FlightGlobals.Vessels.Where(v => v.vesselType != VesselType.Unknown && v.vesselType != VesselType.SpaceObject).ToList();
            for (int i = vList.Count - 1; i >= 0; i--) {
                vList[i].GetVesselCrew()?.ForEach(c => c.StartRespawnPeriod());
                UnityEngine.Object.DestroyImmediate(vList[i]); }
        }
        public bool IsPlayer() => Leader != "#KWLOC_aiControlled";
        public bool IsJunkyard() => Name == "Jebediah Kerman's Junkyard and Spacecraft Parts Co";
        public bool IsRnD() => Name == "Research & Development Department";
        public bool IsKWF() => Name == "Kerbin World-Firsts Record-Keeping Society";
        public void UnsetPlayer() { Leader = "#KWLOC_aiControlled"; strats.Clear(); }
        public bool CanTakeover() => IsRnD(); // Funding.Instance.Funds >= Value && Reputation.CurrentRep >= Rep;
        public void AddValue(double amount) => Value += amount;
        public void SetValue(double amount) => Value = amount;
        public void AddRep(float amount) => Rep += amount;
        public void SetRep(float amount) => Rep = amount;
        public void UpdateCurrencies() {
            Rep = Reputation.CurrentRep;
            Value = Funding.Instance.Funds;
        }
        public void AddSci(CelestialBody body, SciType type, float amount) => Sci[body.name][type] += amount;
        public void ToggleStrat(KWUI.Mode strat, bool? active = null) => strats[strat] = active ?? (strats.ContainsKey(strat) ? !strats[strat] : true);
        public bool StratActive(KWUI.Mode strat) => strats.ContainsKey(strat) ? strats[strat] : false;
        public bool RequestPart(AvailablePart part) { if (parts.ContainsKey(part.name)) if (parts[part.name] > 0) { parts[part.name]--; return true; } return false; }
        public void StorePart(AvailablePart part) { if (!parts.ContainsKey(part.name)) parts[part.name] = 0; parts[part.name]++; }
        public int GetPartQty(AvailablePart part) => parts.ContainsKey(part.name) ? parts[part.name] : 0;
        public Dictionary<AvailablePart, int> StoredParts() => parts.Where(p => p.Value > 0).ToDictionary(p => PartLoader.getPartInfoByName(p.Key), p => p.Value);

    }
    
    public class Kerb {
        public ProtoCrewMember Crew { get; private set; }
        public double OxLoss { get; private set; }
        // public double EdLoss { get; private set; }
        // public double AqLoss { get; private set; }
        public Kerb(ProtoCrewMember crew) => Crew = crew;
        public Kerb(ConfigNode node) {
            Crew = HighLogic.CurrentGame.CrewRoster[node.GetValue("name")];
            OxLoss = double.Parse(node.GetValue("oxLoss"));
            // EdLoss = double.Parse(node.GetValue("edLoss"));
            // AqLoss = double.Parse(node.GetValue("aqLoss"));
        }
        public void Save(ConfigNode node) {
            ConfigNode kNode = node.AddNode("Kerb");
            kNode.AddValue("name", Crew.name);
            kNode.AddValue("oxLoss", OxLoss);
            // kNode.AddValue("edLoss", OxLoss);
            // kNode.AddValue("aqLoss", OxLoss);
        }
        public void AddOxLoss(double amount) {
            if (OxLoss == 0 && amount > 0)
                if (TimeWarp.CurrentRateIndex > 0) AlarmClockScenario.AddAlarm(new AlarmTypeRaw {
                    title = $"{Localizer.Format("#autoLOC_236416")} {Crew.displayName}: {Localizer.Format("#autoLOC_244419", PartResourceLibrary.Instance.GetDefinition("Oxygen").displayName)}",
                    ut = Planetarium.GetUniversalTime() + 1, actions = { warp = AlarmActions.WarpEnum.KillWarp, message = AlarmActions.MessageEnum.Yes }});
                else ScreenMessages.PostScreenMessage($"{Localizer.Format("#autoLOC_236416")} {Crew.displayName}: {Localizer.Format("#autoLOC_244419", PartResourceLibrary.Instance.GetDefinition("Oxygen").displayName)}");
            OxLoss += amount;
            if (OxLoss < 0) OxLoss = 0;
        }
        //public void AddEdLoss(double amount) {
        //    if (EdLoss == 0 && amount > 0)
        //        if (TimeWarp.CurrentRateIndex > 0) AlarmClockScenario.AddAlarm(new AlarmTypeRaw {
        //            title = $"{Localizer.Format("#autoLOC_236416")} {Crew.displayName}: {Localizer.Format("#autoLOC_244419", PartResourceLibrary.Instance.GetDefinition("Edibles").displayName)}",
        //            ut = Planetarium.GetUniversalTime() + 1, actions = { warp = AlarmActions.WarpEnum.KillWarp, message = AlarmActions.MessageEnum.Yes }
        //        });
        //        else ScreenMessages.PostScreenMessage($"{Localizer.Format("#autoLOC_236416")} {Crew.displayName}: {Localizer.Format("#autoLOC_244419", PartResourceLibrary.Instance.GetDefinition("Edibles").displayName)}");
        //    EdLoss += amount;
        //    if (EdLoss < 0) EdLoss = 0;
        //}
        //public void AddAqLoss(double amount) {
        //    if (AqLoss == 0 && amount > 0)
        //        if (TimeWarp.CurrentRateIndex > 0) AlarmClockScenario.AddAlarm(new AlarmTypeRaw {
        //            title = $"{Localizer.Format("#autoLOC_236416")} {Crew.displayName}: {Localizer.Format("#autoLOC_244419", PartResourceLibrary.Instance.GetDefinition("Aqua").displayName)}",
        //            ut = Planetarium.GetUniversalTime() + 1, actions = { warp = AlarmActions.WarpEnum.KillWarp, message = AlarmActions.MessageEnum.Yes }
        //        });
        //        else ScreenMessages.PostScreenMessage($"{Localizer.Format("#autoLOC_236416")} {Crew.displayName}: {Localizer.Format("#autoLOC_244419", PartResourceLibrary.Instance.GetDefinition("Aqua").displayName)}");
        //    AqLoss += amount;
        //    if (AqLoss < 0) AqLoss = 0;
        //}
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
