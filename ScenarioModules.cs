
using Contracts;
using Contracts.Agents;
using KSP.Localization;
using KSP.UI;
using KSP.UI.Screens;
using KSP.UI.Screens.Editor;
using KSP.UI.Screens.SpaceCenter.MissionSummaryDialog;
using KSP.UI.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace KerbalWitchery {
    // ScenarioCreationOptions.AddToNewCareerGames | ScenarioCreationOptions.AddToExistingCareerGames
    [KSPScenario(ScenarioCreationOptions.None, new GameScenes[1] { GameScenes.SPACECENTER })]
    public class KWAdmin : ScenarioModule {
        
        private static readonly Dictionary<ProgType, string[]> goals = GameDatabase.Instance.GetConfigNode("KerbalWitchery/KWAdminPrograms").GetNodes().ToDictionary(n =>
            (ProgType)Enum.Parse(typeof(ProgType), n.name), n => n.GetValues().ToArray());
        private static List<Program> programs;

        public static string hero;
        public void Start() {
            

        }
        public void OnDisable() {

        }
        public override void OnLoad(ConfigNode node) {
            // programs = node.HasNode("PROGRAM") ? node.GetNodes("PROGRAM").Select(n => new Program(n)).ToList() : goals.Keys.Select(t => new Program(t)).ToList();

        }
        public override void OnSave(ConfigNode node) {

            // programs.ForEach(p => p.Save(node));

        }

        public static Program GetProgram(ProgType type) => programs.FirstOrDefault(p => p.Type == type);
        public static void AddFunds(float amount) {
            
            programs.Where(p => p.active).ToList().ForEach(p => p.AddFunds(amount / programs.Count(pr => pr.active)));

        }
        private static void UpdateProgram(ProgType type) {
            
            int i = goals[type].IndexOf(goals[type].First(t => KWUtil.IsTechLocked(t)));
            //if (Global.techList.Contains(targets[i - 1])) program.UpdateTarget();
            //else if (Enum.TryParse(targets[i - 1], out ProgType t)) ChangeProgram(t);
            //else {
            //    ProgType[] types = (ProgType[])targets[i - 1].Split('|').Cast<ProgType>();
            //    ChangeProgram(types[UnityEngine.Random.Range(0, types.Length)]);
            //}
        }
        public static ProgType[] GetLobbyProgs(Agency agency) => goals.Keys.Where(pT => goals[pT].Any(t => agency.Labs.Keys.Select(s => s.ToString()).Contains(t) ||
            KWAgencies.MfPartTechs(agency).Contains(t))).ToArray();
        public static Agency[] GetLobbyProgs(ProgType type) => KWAgencies.AList.Where(a => a.Labs.Keys.Select(s => s.ToString()).Intersect(goals[type]).Any() ||
            KWAgencies.MfPartTechs(a).Intersect(goals[type]).Any()).ToArray();
            
        public static bool ProgContainsLab(ProgType type, SciType sciType) => goals[type]?.Contains(sciType + "") ?? false;
        public static bool ProgContainsMfParts(ProgType type, Agency agency) => goals[type]?.Any(t =>
            PartLoader.LoadedPartsList.Where(p => p.manufacturer == agency.Agent().Title).Select(p => p.TechRequired).Contains(t)) ?? false;
        


        public class Program {
            public ProgType Type { get; private set; }
            public bool active { get; private set; }
            public float Funds { get; private set; }
            public Dictionary<string, float> Donors { get; private set; }
            public Program(ProgType type) {
                Type = type;
                Donors = AgentList.Instance.Agencies.OrderBy(a => a.Name).ToDictionary(a => a.Name, a => 0f);
            }
            public Program(ConfigNode node) {
                Type = (ProgType)Enum.Parse(typeof(ProgType), node.GetValue("type"));
                active = bool.Parse(node.GetValue("active"));
                Funds = float.Parse(node.GetValue("funds"));
                Donors = AgentList.Instance.Agencies.OrderBy(a => a.Name).ToDictionary(a => a.Name, a => 0f);
                foreach (ConfigNode dNode in node.GetNodes("DONOR"))
                    Donors[dNode.GetValue("name")] = float.Parse(dNode.GetValue("amount"));
            }
            public void Save(ConfigNode node) {
                ConfigNode pNode = node.AddNode("PROGRAM");
                pNode.AddValue("type", Type);
                pNode.AddValue("active", active);
                pNode.AddValue("funds", Funds);
                foreach (KeyValuePair<string, float> pair in Donors.Where(p => p.Value > 0)) {
                    ConfigNode dNode = pNode.AddNode("DONOR");
                    dNode.AddValue("name", pair.Key);
                    dNode.AddValue("amount", pair.Value);
                }
            }

            public void AddFunds(float amount) {
                Funds += amount;

            }
            public void Donate(float amount) {
                Donors[KWAgencies.Player().Name] += amount;

            }

        }
    }

    [KSPScenario(ScenarioCreationOptions.AddToNewCareerGames | ScenarioCreationOptions.AddToExistingCareerGames,
        new GameScenes[4] { GameScenes.SPACECENTER, GameScenes.EDITOR, GameScenes.FLIGHT, GameScenes.TRACKSTATION })]
    public class KWAgencies : ScenarioModule {
        public static List<Agency> AList { get; private set; }
        private static Dictionary<string[], float> standings;
        private static Dictionary<string, int> junkParts;

        private static readonly Dictionary<SciType, string[]> sciLabs = GameDatabase.Instance.GetConfigNode("KerbalWitchery/KWSciLabs").GetNodes().ToDictionary(n =>
            (SciType)Enum.Parse(typeof(SciType), n.name), n => n.GetValues());
        public void Start() {

        }
        public void OnDisable() {

        }
        public override void OnLoad(ConfigNode node) {
            AList = node.HasNode("AGENCY") ? node.GetNodes("AGENCY").Select(n => new Agency(n)).ToList() :
                AgentList.Instance.Agencies.OrderBy(a => a.Name).Select(a => new Agency(a)).ToList();
            standings = AList.SelectMany((fst, i) => AList.Skip(i + 1).Select(snd => (fst, snd))).ToDictionary(p => new string[2] { p.fst.Name, p.snd.Name }, p => 0f);
            foreach (string value in node.GetNode("STANDINGS")?.GetValues() ?? new string[0])
                AddStanding(GetAgency(value.Split('|')[0]), GetAgency(value.Split('|')[1]), float.Parse(value.Split('|')[2]));
            junkParts = new Dictionary<string, int>();
            if (node.GetNode("JUNKYARD")?.HasValues() ?? false)
                foreach (ConfigNode.Value pair in node.GetNode("JUNKYARD").values)
                    junkParts[pair.name] = int.Parse(pair.value);
        }
        public override void OnSave(ConfigNode node) {
            AList.ForEach(a => a.Save(node));
            ConfigNode sNode = node.AddNode("STANDINGS");
            standings.Where(s => s.Value != 0f).ToList().ForEach(s => sNode.AddValue("value", $"{s.Key[0]}|{s.Key[1]}|{s.Value}"));
            ConfigNode jNode = node.AddNode("JUNKYARD");
            junkParts.Where(p => p.Value > 0).ToList().ForEach(p => jNode.AddValue(p.Key, p.Value));
        }
        public static Agency Player() => AList.FirstOrDefault(a => a.Leader != "#KWLOC_aiControlled");
        public static bool NewPlayer() => Player() == null;
        public static bool PlayerIsAgency(Agency agency) => agency.Name == Player()?.Name;
        public static bool IsRnD(Agency agency) => agency.Name == "Research & Development Department";
        public static Agency GetAgency(Agent agent) => AList.FirstOrDefault(a => a.Name == agent.Name);
        public static Agency GetAgency(string name) => AList.FirstOrDefault(a => a.Name == name);
        public static Agency GetManufacturer(AvailablePart part) => AList.FirstOrDefault(a => a.Agent().Title == part.manufacturer);
        public static string[] MfPartTechs(Agency agency) => PartLoader.LoadedPartsList.Where(p => p.manufacturer == agency.Agent().Title).Select(p => p.TechRequired).Distinct().ToArray();
        public static Agency Junkyard() => AList.FirstOrDefault(a => a.Name.Contains("Junkyard"));
        public static Agency RandomAgency() => AList.ElementAt(UnityEngine.Random.Range(0, AList.Count));
        public static void AddStanding(Agency agency, float amt) => standings[GetStKey(agency, Player())] += amt;
        public static void AddStanding(Agent agent, float amt) => standings[GetStKey(GetAgency(agent), Player())] += amt;
        public static void AddStanding(Agency agency1, Agency agency2, float amt) => standings[GetStKey(agency1, agency2)] += amt;
        public static float GetStanding(Agent agent) => standings[GetStKey(GetAgency(agent), Player())];
        public static float GetStanding(Agency agency) => standings[GetStKey(agency, Player())];
        public static List<AvailablePart> GetAvailableParts(Agency agency) => PartLoader.LoadedPartsList.Where(p => p.manufacturer == agency.Agent().Title && 
            ResearchAndDevelopment.PartTechAvailable(p) && PartThresholdMet(p)).ToList();
        public static Dictionary<SciType, int> LabCosts() => KWUtil.TechTree().Where(t => Enum.TryParse(t.techID.Replace("sci", ""), out SciType sci)).ToDictionary(t =>
            (SciType)Enum.Parse(typeof(SciType), t.techID.Replace("sci", "")), t => t.scienceCost);
        public static SciType[] Labs(Agency agency) => sciLabs.Where(p => p.Value.Contains(agency.Name))?.Select(p => p.Key).ToArray();
        public static bool TryPayAgency(Agency agency, float amount) {
            if (agency == null || PlayerIsAgency(agency)) return false;
            agency.AddValue(amount);
            AddStanding(agency, amount / 10000);
            return true;
        }
        public static void UpdatePlayerCurrencies() {
            Player().SetRep(Reputation.CurrentRep);
            Player().SetValue(Funding.Instance.Funds);
        }
        public static void ResetPlayer() {
            Player().UnsetPlayer();
            Funding.Instance.SetFunds(HighLogic.CurrentGame.Parameters.Career.StartingFunds, TransactionReasons.None);
            Reputation.Instance.SetReputation(HighLogic.CurrentGame.Parameters.Career.StartingReputation, TransactionReasons.None);
            ResearchAndDevelopment.Instance.SetScience(HighLogic.CurrentGame.Parameters.Career.StartingScience, TransactionReasons.None);
            foreach (Vessel vessel in FlightGlobals.Vessels.Where(v => (int)v.vesselType > 2)) vessel.DestroyVesselComponents();
        }
        public static void Takeover(Agency agency) {
            HighLogic.CurrentGame.flagURL = agency.Agent().LogoURL;
            KWUtil.GetHero().type = ProtoCrewMember.KerbalType.Crew;
            if (NewPlayer()) {
                agency.SetPlayer();
                UpdatePlayerCurrencies();
                KWUtil.ToggleFacilityLock(false);
                // KWUtil.UnlockTech("Administration");
                // KWUtil.UpgradeFacility(SpaceCenterFacility.Administration);
            } else {
                Agency prev = Player();
                prev.UnsetPlayer();
                agency.SetPlayer();
                // Reputation.Instance.SetReputation(agency.Rep, TransactionReasons.None);
                if (agency.Value < 0) Funding.Instance.AddFunds(agency.Value, TransactionReasons.None);
                // agency.SetValue(Funding.Instance.Funds);
                prev.AddValue(-Math.Abs(agency.Value));
            }
        }
        // public static float GetStanding(Agency agency1, Agency agency2) => Standings[GetStKey(agency1, agency2)];
        private static string[] GetStKey(Agency agency1, Agency agency2) => standings.FirstOrDefault(s => s.Key.Contains(agency1.Name) && s.Key.Contains(agency2.Name)).Key;

        public static string GetStandingDesc(Agency agency) {
            // if (Player() == null) return $"{Localizer.Format("#autoLOC_475347")} <color=orange></color>";
            string[] descs = new string[5] {
                $"red>{Localizer.Format("#autoLOC_184746")}",
                $"orange>{Localizer.Format("#autoLOC_184750")}",
                $"yellow>{Localizer.Format("#autoLOC_184754")}",
                $"green>{Localizer.Format("#autoLOC_184758")}",
                $"#00ff00ff>{Localizer.Format("#autoLOC_184762")}" };
            return $"{Localizer.Format("#KWLOC_standing")} <color={descs[Mathf.Clamp((int)Math.Floor(((80 + GetStanding(agency)) / 200) * 5), 0, 4)]} ({GetStanding(agency):N0})</color>";
        }
        public static bool PartThresholdMet(AvailablePart part) {
            if (!KWUtil.CareerOpts().partsReqSts) return true;
            Agency agency = GetManufacturer(part);
            if (agency == null) return true;
            if (GetStanding(agency) < KWUtil.CareerOpts().minStAgency) return false;
            return Mathf.Max(GetStanding(agency) * 10000, KWUtil.CareerOpts().partStThrs) >= part.entryCost;
        }
        public static int PricePart(AvailablePart part) => PlayerIsAgency(GetManufacturer(part)) ? (int)Math.Round(part.entryCost * 0.666) : part.entryCost;
        public static void OrderPart(AvailablePart part, bool store = true) {
            HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().AllowNegativeCurrency = true;
            if (store) Player().StorePart(part);
            Funding.Instance.AddFunds(-PricePart(part), TransactionReasons.RnDPartPurchase);
            TryPayAgency(GetManufacturer(part), PricePart(part));
        }

        public static Dictionary<AvailablePart, int> JunkParts() => junkParts.Where(p => p.Value > 0).ToDictionary(p => PartLoader.getPartInfoByName(p.Key), p => p.Value);
        public static void BuyJunk(AvailablePart part) {
            if (junkParts.ContainsKey(part.name))
                if (junkParts[part.name] > 0) {
                    if (!PlayerIsAgency(Junkyard())) {
                        Funding.Instance.AddFunds(-PriceJunk(part), TransactionReasons.RnDPartPurchase);
                        TryPayAgency(Junkyard(), PriceJunk(part)); }
                    junkParts[part.name]--;
                    Player().StorePart(part); }
        }
        public static void SellJunk(AvailablePart part) {
            if (Player().RequestPart(part)) {
                if (!PlayerIsAgency(Junkyard())) {
                    Funding.Instance.AddFunds(PriceJunk(part, false), TransactionReasons.RnDPartPurchase);
                    Junkyard().AddValue(-PriceJunk(part, false)); }
                if (!junkParts.ContainsKey(part.name)) junkParts[part.name] = 0;
                junkParts[part.name]++;
            }
        }
        public static int PriceJunk(AvailablePart part, bool buy = true) => (int)Math.Round(part.entryCost * (buy ? 0.75 : 0.5));
    }
    
    [KSPScenario(ScenarioCreationOptions.None,
        new GameScenes[4] { GameScenes.SPACECENTER, GameScenes.EDITOR, GameScenes.FLIGHT, GameScenes.TRACKSTATION })]
    public class KWSci : ScenarioModule {

        public void Start() {
            
        }
        public void OnDisable() {
            
        }
        public override void OnLoad(ConfigNode node) {
            
        }
        public override void OnSave(ConfigNode node) {

        }

    }
    
    // ScenarioCreationOptions.AddToAllGames
    [KSPScenario(ScenarioCreationOptions.None, new GameScenes[1] { GameScenes.FLIGHT })]
    public class KWHealth : ScenarioModule {
        private static Dictionary<string, float> hp;
        public void Start() {


        }
        public void OnDisable() {

        }

        public override void OnLoad(ConfigNode node) {
            hp = new Dictionary<string, float>();
            foreach (ConfigNode kNode in node.GetNodes())
                hp[kNode.GetValue("name")] = float.Parse(kNode.GetValue("hp"));

        }
        public override void OnSave(ConfigNode node) {
            foreach (var pair in hp) {
                ConfigNode kNode = node.AddNode("KERBAL");
                kNode.AddValue("name", pair.Key);
                kNode.AddValue("hp", pair.Value);
            }
        }

        public static float GetHP(string kerbal) => hp.ContainsKey(kerbal) ? Mathf.Clamp(hp[kerbal], 0f, 1f) : 0f;
        public static void ModifyHP(string kerbal, float amount) {
            if (hp.ContainsKey(kerbal)) {
                if (hp[kerbal] < 1) hp[kerbal] += amount;
                if (hp[kerbal] <= 0)
                    HighLogic.CurrentGame.CrewRoster[kerbal].Die();
            }
        }

    }

}
