
using Contracts;
using Contracts.Agents;
using KSP.Localization;
using KSP.UI;
using KSP.UI.Screens;
using KSP.UI.Screens.Editor;
using KSP.UI.Screens.Flight.Dialogs;
using KSP.UI.TooltipTypes;
using Strategies;
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
using UnityEngine.UI;
using Upgradeables;

namespace KerbalWitchery {

    //[KSPAddon(KSPAddon.Startup.MainMenu, false)]
    //public class KWLoader : MonoBehaviour {
    //    public void Start() {

    //        Debug.Log("loaded");

    //        KWUtil.UpdateParts();

    //        foreach (AvailablePart part in PartLoader.LoadedPartsList.Where(p => p.partPrefab.Modules.Contains<ModuleKWPressurisedCabin>())) {
    //            foreach (AvailablePart.ResourceInfo resInfo in part.resourceInfos.Where(r => "Nitro|Oxidizer|ToxicGas".Contains(r.resourceName)))
    //                resInfo.primaryInfo = "";
    //        }

            
    //    }

    //}

    [KSPAddon(KSPAddon.Startup.AllGameScenes, false)]
    public class KWEvents : MonoBehaviour {

        private float contractRep;
        public void Start() {
            // Debug.Log("BeginStart:" + HighLogic.LoadedScene);
            // if (KWUtil.FlightScene()) {
            GameEvents.onAttemptEva.Add(AttemptEVA);
            // GameEvents.onCrewOnEva.Add(CrewEVA);
            GameEvents.onCrewBoardVessel.Add(BoardVessel);
            GameEvents.OnExperimentDeployed.Add(DeployExperiment);
            // GameEvents.onKerbalPassedOutFromGeeForce.Add(KerbalPassedOut);
            GameEvents.OnVesselRollout.Add(RolloutVessel);
            GameEvents.onModuleInventoryChanged.Add(ChangeInventory);
            // GameEvents.onVesselCrewWasModified.Add(ModifiedVesselCrew);
            GameEvents.onCrewTransferred.Add(CrewTransferred);
            
            GameEvents.OnFlightGlobalsReady.Add(FlightReady);

            //    GameEvents.onPartResourceNonemptyEmpty.Add(EmptyResource);
            //    GameEvents.onPartResourceEmptyNonempty.Add(NonEmptyResource);
            //GameEvents.onTimeWarpRateChanged.Add(ChangeTimeWarpRate);
            // }
            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER) {
                // if (KWUtil.KSCScene()) {
                // GameEvents.OnTechnologyResearched.Add(ResearchTech);
                // }
                //if (!KWUtil.EditorScene()) {
                //    GameEvents.onKerbalStatusChanged.Add(ChangedKerbalStatus);
                //    GameEvents.onKerbalTypeChanged.Add(ChangedKerbalType);
                //}
                // if (KWUtil.EditorScene()) {
                //    GameEvents.Contract.onOffered.Add(OfferContract);
                // } else {
                GameEvents.Contract.onCancelled.Add(CancelContract);
                GameEvents.Contract.onDeclined.Add(DeclineContract);
                GameEvents.Contract.onParameterChange.Add(UpdateContract);
                GameEvents.Contract.onCompleted.Add(CompleteContract);
                GameEvents.Contract.onFailed.Add(FailContract);
                GameEvents.Modifiers.OnCurrencyModifierQuery.Add(QueryCurrency);
                GameEvents.OnFundsChanged.Add(ChangeFunds);
                // GameEvents.onLevelWasLoaded.Add(LoadLevel);
                GameEvents.OnReputationChanged.Add(ChangeRep);
                GameEvents.OnScienceRecieved.Add(ReceiveScience);
                GameEvents.OnVesselRecoveryRequested.Add(RecoverVessel);
                // }
                // GameEvents.Modifiers.OnCurrencyModified.Add(ModifiedCurrency);
            }
            if (!FindObjectOfType<ScenarioNewGameIntro>()?.kscComplete ?? false) SetupNewCareer();
            // Debug.Log("EndStart:" + HighLogic.LoadedScene);
        }
        public void OnDisable() {
            // Debug.Log("BeginDisable:" + HighLogic.LoadedScene);
            // if (KWUtil.FlightScene()) {
            GameEvents.onAttemptEva.Remove(AttemptEVA);
            // GameEvents.onCrewOnEva.Remove(CrewEVA);
            GameEvents.onCrewBoardVessel.Remove(BoardVessel);
            GameEvents.OnExperimentDeployed.Remove(DeployExperiment);
            // GameEvents.onKerbalPassedOutFromGeeForce.Remove(KerbalPassedOut);
            GameEvents.OnVesselRollout.Remove(RolloutVessel);
            GameEvents.onModuleInventoryChanged.Remove(ChangeInventory);
            // GameEvents.onVesselCrewWasModified.Remove(ModifiedVesselCrew);
            GameEvents.onCrewTransferred.Remove(CrewTransferred);
            GameEvents.OnFlightGlobalsReady.Remove(FlightReady);
            //    GameEvents.onPartResourceNonemptyEmpty.Add(EmptyResource);
            //    GameEvents.onPartResourceEmptyNonempty.Add(NonEmptyResource);
            // GameEvents.onTimeWarpRateChanged.Remove(ChangeTimeWarpRate);
            // }
            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER) {
                // if (KWUtil.KSCScene()) {
                // GameEvents.OnTechnologyResearched.Remove(ResearchTech);
                // }
                //if (!KWUtil.EditorScene()) {
                //    GameEvents.onKerbalStatusChanged.Remove(ChangedKerbalStatus);
                //    GameEvents.onKerbalTypeChanged.Remove(ChangedKerbalType);
                //}
                // if (KWUtil.EditorScene()) {
                //    GameEvents.Contract.onOffered.Remove(OfferContract);
                // } else {
                GameEvents.Contract.onCancelled.Remove(CancelContract);
                GameEvents.Contract.onCompleted.Remove(CompleteContract);
                GameEvents.Contract.onDeclined.Remove(DeclineContract);
                GameEvents.Contract.onFailed.Remove(FailContract);
                GameEvents.Contract.onParameterChange.Remove(UpdateContract);
                GameEvents.Modifiers.OnCurrencyModifierQuery.Remove(QueryCurrency);
                GameEvents.OnFundsChanged.Remove(ChangeFunds);
                // GameEvents.onLevelWasLoaded.Remove(LoadLevel);
                GameEvents.OnReputationChanged.Remove(ChangeRep);
                GameEvents.OnScienceRecieved.Remove(ReceiveScience);
                GameEvents.OnVesselRecoveryRequested.Remove(RecoverVessel);
                // }
                // GameEvents.Modifiers.OnCurrencyModified.Remove(ModifiedCurrency);

            }
            // Debug.Log("EndDisable:" + HighLogic.LoadedScene);
        }
        //private void ChangeTimeWarpRate() {
        //    if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready) {
        //        List<ModuleReactionWheelKW> rws = FlightGlobals.ActiveVessel.FindPartModulesImplementing<ModuleReactionWheelKW>();
        //        rws?.ForEach(r => r.warpTarget = FlightGlobals.ActiveVessel.transform.rotation); }
        //}
        
        private void FlightReady(bool ready) {
            StartCoroutine(CallbackUtil.DelayedCallback(10, delegate { KWUtil.UpdateHelmets(); }));
        }
        private void ChangeInventory(ModuleInventoryPart inv) {
            if (HighLogic.LoadedSceneIsEditor || !inv.part.Modules.Contains<KerbalEVA>()) return;
            ProtoCrewMember crew;
            if (inv.part.protoModuleCrew.Count > 0) {
                crew = inv.part.protoModuleCrew[0];
                KerbalEVA eva = inv.part.FindModuleImplementing<KerbalEVA>();
                if (crew.hasHelmetOn && !inv.ContainsPart("KWhelmet"))
                    KWUtil.ToggleHelmet(false, crew, eva);
                else if (!crew.hasHelmetOn && inv.ContainsPart("KWhelmet"))
                    KWUtil.ToggleHelmet(true, crew, eva);
            } else {
                crew = inv.kerbalReference;
                if (crew.hasHelmetOn && !inv.ContainsPart("KWhelmet")) 
                    KWUtil.ToggleHelmet(false, crew);
                else if (!crew.hasHelmetOn && inv.ContainsPart("KWhelmet")) 
                    KWUtil.ToggleHelmet(true, crew);
            }

            //if (inv.part.protoModuleCrew.Count > 0) {
            //    crew = inv.part.protoModuleCrew[0];
            //    if (crew.hasHelmetOn && !inv.ContainsPart("KWhelmet")) {
            //        crew.hasHelmetOn = false;
            //        crew.hasNeckRingOn = false;
            //        KerbalEVA eva = inv.part.FindModuleImplementing<KerbalEVA>();
            //        typeof(KerbalEVA).GetField("isHelmetEnabled", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(eva, false);
            //        typeof(KerbalEVA).GetField("isNeckRingEnabled", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(eva, false);
            //        eva.helmetTransform.gameObject.SetActive(false);
            //        eva.neckRingTransform.gameObject.SetActive(false);
            //        GameEvents.OnHelmetChanged.Fire(eva, false, false);
            //        typeof(KerbalEVA).GetMethod("UpdateVisorEventStates", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(eva, null);
            //    } else if (!crew.hasHelmetOn && inv.ContainsPart("KWhelmet")) {
            //        crew.hasHelmetOn = true;
            //        crew.hasNeckRingOn = true;
            //        KerbalEVA eva = inv.part.FindModuleImplementing<KerbalEVA>();
            //        typeof(KerbalEVA).GetField("isHelmetEnabled", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(eva, true);
            //        typeof(KerbalEVA).GetField("isNeckRingEnabled", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(eva, true);
            //        eva.helmetTransform.gameObject.SetActive(true);
            //        eva.neckRingTransform.gameObject.SetActive(true);
            //        GameEvents.OnHelmetChanged.Fire(eva, true, true);
            //    }
            //} else {
            //    crew = inv.kerbalReference;
            //    if (crew.hasHelmetOn && !inv.ContainsPart("KWhelmet")) {
            //        crew.hasHelmetOn = false;
            //        crew.KerbalRef.helmetTransform.gameObject.SetActive(false);
            //        crew.KerbalRef.showHelmet = false;
            //    } else if (!crew.hasHelmetOn && inv.ContainsPart("KWhelmet")) {
            //        crew.hasHelmetOn = true;
            //        crew.KerbalRef.helmetTransform.gameObject.SetActive(true);
            //        crew.KerbalRef.showHelmet = true;
            //    }
            //}

            // Debug.Log(inv.part);
            // crew = HighLogic.CurrentGame.CrewRoster.Crew.FirstOrDefault(c => c.KerbalInventoryModule == inv);
        }
            
        private void KerbalPassedOut(ProtoCrewMember crew) {
            if (TimeWarp.CurrentRateIndex > 0)
                AlarmClockScenario.AddAlarm(new AlarmTypeRaw {
                    title = $"{Localizer.Format("#autoLOC_236416")} {Localizer.Format("#autoLOC_283371", crew.displayName)}", ut = Planetarium.GetUniversalTime() + 1,
                    actions = { warp = AlarmActions.WarpEnum.KillWarp, message = AlarmActions.MessageEnum.Yes }});
        }
        private void DeployExperiment(ScienceData data) {
            if (HighLogic.LoadedSceneIsFlight)
                if (FlightGlobals.ActiveVessel.isEVA && KWUtil.evaKitConsumerIDs.Contains(data.subjectID.Split('@')[0]))
                    FlightGlobals.ActiveVessel.evaController.ModuleInventoryPartReference.RemoveNPartsFromInventory("evaScienceKit", 1, true);
        }
        private void AttemptEVA(ProtoCrewMember crew, Part part, Transform transform) {
            if (part.Modules.Contains<ModuleKWCabin>()) {
                ModuleKWCabin cabin = part.FindModuleImplementing<ModuleKWCabin>();
                cabin.UpdateValues();
                if (Math.Abs(cabin.cabPrsAtm * 101.325 - part.vessel.staticPressurekPa) > 5 || FlightGlobals.getAltitudeAtPos(transform.position, part.vessel.mainBody) < 0) {
                    FlightEVA.fetch.overrideEVA = true;
                    ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_252195"));
                } else cabin.OpenCabin(); }
        }
        private void BoardVessel(GameEvents.FromToAction<Part, Part> action) {
            // Debug.Log("BoardVessel: " + action.from.partInfo.name + " -> " + action.to.partInfo.name);
            action.to.FindModuleImplementing<ModuleKWCabin>()?.OpenCabin();
            StartCoroutine(CallbackUtil.DelayedCallback(10, delegate { KWUtil.UpdateHelmets(); }));
            // action.to.RequestResource("Oxygen", -action.from.Resources.Get("Oxygen").amount);
        }
        
        private void ModifiedVesselCrew(Vessel vessel) {
            // Debug.Log("vesselCrewMod");
            //List<Part> gooList = vessel.Parts.Where(p => p.partInfo.name == "GooExperiment").ToList();
            //gooList.ForEach(p => p.FindModuleImplementing<ModuleGenerator>().efficiency = Math.Max((float)vessel.GetCrewCount() / gooList.Count, 0.1f));
            // KWUI.updateHelmets = true;
        }

        private void CrewEVA(GameEvents.FromToAction<Part, Part> action) {
            Debug.Log("crewEVA: " + action.from.partInfo.name + " -> " + action.to.partInfo.name);
            //PartResource res = action.to.Resources.Get("Oxygen");
            //res.amount = action.from.RequestResource("Oxygen", res.maxAmount);
        }
        private void CrewTransferred(GameEvents.HostedFromToAction<ProtoCrewMember, Part> action) {
            StartCoroutine(CallbackUtil.DelayedCallback(10, delegate { KWUtil.UpdateHelmets(); }));
            
            // Debug.Log("crewtrans: " + action.from.partInfo.name + " -> " + action.to.partInfo.name);
            //if (!action.host.KerbalInventoryModule.ContainsPart("KWhelmet"))
            //    KWUtil.ToggleHelmet(false, action.host);
        }

        private void LoadLevel(GameScenes scene) {

            //if (scene == GameScenes.SPACECENTER && (!FindObjectOfType<ScenarioNewGameIntro>()?.kscComplete ?? false))
            //    SetupNewCareer();
            //if (!KWUtil.HeroReady()) {
            //    KWUtil.ToggleFacilityLock(true);
            //    if (KWAgencies.GetPlayer() != null)
            //        KWAgencies.GetPlayer().Quit();
            //}
            //if (scene == GameScenes.SPACECENTER || scene == GameScenes.FLIGHT)
            //    StartCoroutine(CallbackUtil.DelayedCallback(1, delegate {
            //        // if (KWUtil.SCParams().GetType().GetFields().Select(f => (bool)f.GetValue(KWUtil.SCParams())).Any(v => v == false))
            //        foreach (DestructibleBuilding db in FindObjectsOfType<DestructibleBuilding>()?.Where(d => Enum.TryParse(d.id.Split('/')[1], out SpaceCenterFacility fc)
            //            && !new SpaceCenterFacility[1] { SpaceCenterFacility.Runway }.Contains(fc))) {
            //            // && fc != SpaceCenterFacility.Administration  && KWUtil.IsTechLocked(fc + "")
            //            //!(bool)KWUtil.SCParams().GetType().GetField(KWUtil.fcSCParams[fc + ""]).GetValue(KWUtil.SCParams())))
            //            if (scene == GameScenes.SPACECENTER && (!FindObjectOfType<ScenarioNewGameIntro>()?.kscComplete ?? false)) {
            //                db.GetType().GetField("intact", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(db, false);
            //                db.GetType().GetField("destroyed", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(db, true);
            //            }
            //            foreach (DestructibleBuilding.CollapsibleObject co in db.CollapsibleObjects) {
            //                co.collapseObject.SetActive(false);
            //                co.replacementObject.SetActive(false);
            //            }
            //            // co.SetDestroyed(true);
            //        }
            //        // if (KWAgencies.NewPlayer()) GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
            //    }));

        }
        private void EmptyResource(PartResource res) {
            
            
        }
        private void NonEmptyResource(PartResource res) {
            
            
        }

        private void SetupNewCareer() {

            // new List<string> { "ksc", "Runway" }.ForEach(f => KWUtil.UnlockTech(f));

            // KWAdmin.AddFunds(KWUtil.GameOpts().adminStartFunds);
            //foreach (FieldInfo field in KWUtil.SCParams().GetType().GetFields().Where(f => f.Name != "CanLeaveToMainMenu"))
            //    field.SetValue(KWUtil.SCParams(), false);

            //foreach (SpaceCenterBuilding b in FindObjectsOfType<SpaceCenterBuilding>())
            //    b.OnClick.Add(delegate { Debug.Log("boop"); });
            // KWStartupOptions sOpts = KWUtil.StartOpts();
            // if (!KWUtil.GameOpts().kscPrograms)
            //StartCoroutine(CallbackUtil.DelayedCallback(5, delegate {
            //    foreach (DestructibleBuilding db in FindObjectsOfType<DestructibleBuilding>()?.Where(d => Enum.GetNames(typeof(SpaceCenterFacility)).Contains(d.id.Split('/')[1])))
            //        if (!(bool)sOpts.GetType().GetField(db.id.Split('/')[1]).GetValue(sOpts)) db.Demolish();
            //}));
            //foreach (FieldInfo field in sOpts.GetType().GetFields().Where(f => Enum.TryParse(f.Name, out SpaceCenterFacility fc)))
            //    if ((bool)field.GetValue(sOpts)) KWUtil.UnlockTech(field.Name);
            //    else if (KWUtil.GameOpts().kscPrograms)
            //            KWUtil.SCParams().GetType().GetField(KWUtil.fcSCParams[field.Name]).SetValue(KWUtil.SCParams(), false);

            //foreach (FieldInfo field in sOpts.GetType().GetFields().Where(f => f.Name.StartsWith("vet")))
            //    if (!(bool)field.GetValue(sOpts)) {
            //        GameParameters.CustomParameterUI at = (GameParameters.CustomParameterUI)Attribute.GetCustomAttribute(field, typeof(GameParameters.CustomParameterUI));
            //        HighLogic.CurrentGame.CrewRoster.Remove(Localizer.Format(at.title));
            //    }

            while (HighLogic.CurrentGame.CrewRoster.Count > 0)
                HighLogic.CurrentGame.CrewRoster.Remove(0);
            KWUtil.ToggleFacilityLock(true);
        }
        //private void ResearchTech(GameEvents.HostTargetAction<RDTech, RDTech.OperationResult> action) {
        //    if (action.target == RDTech.OperationResult.Successful) {
        //        if (HighLogic.CurrentGame.Parameters.Difficulty.BypassEntryPurchaseAfterResearch) KWUtil.LockAllParts();
        //        KWUtil.UnlockParts();
        //    }
        //}

        private void ModifiedCurrency(CurrencyModifierQuery query) {
            // Debug.Log(query.reason + "" + query.GetTotal(Currency.Funds));

        }
        private void ChangeFunds(double _, TransactionReasons __) {
            KWAgencies.GetPlayer()?.UpdateCurrencies();
            if (HighLogic.LoadedSceneIsEditor) KWUtil.UpdateEditorPartList();
        }
        private void ChangeRep(float _, TransactionReasons __) {
            Agency player = KWAgencies.GetPlayer();
            if (player == null) return;
            player.UpdateCurrencies();
            if (Reputation.CurrentRep < 0 && player.IsRnD())
                player.ToggleStrat(KWUI.Mode.Interns, false);
        }

        private void DeclineContract(Contract contract) => KWAgencies.AddStanding(contract.Agent, -HighLogic.CurrentGame.Parameters.Career.RepLossDeclined);
        private void QueryCurrency(CurrencyModifierQuery query) {
            if (query.reason == TransactionReasons.VesselRecovery && query.GetTotal(Currency.Funds) > 0) query.AddDelta(Currency.Funds, -query.GetTotal(Currency.Funds));
            else if (query.reason == TransactionReasons.ContractDecline) query.AddDelta(Currency.Reputation, -query.GetTotal(Currency.Reputation));
            else if (query.reason == TransactionReasons.Contracts && query.GetTotal(Currency.Reputation) != 0) contractRep = query.GetTotal(Currency.Reputation);
            else if (query.reason == TransactionReasons.StructureRepair && query.GetTotal(Currency.Funds) > 0) query.AddDelta(Currency.Funds, -query.GetTotal(Currency.Funds));
            if (KWAgencies.GetPlayer()?.StratActive(KWUI.Mode.Lead) ?? false) {
                if (query.reason == TransactionReasons.Progression && query.GetTotal(Currency.Reputation) > 0)
                    query.AddDelta(Currency.Reputation, query.GetTotal(Currency.Reputation));
                if (query.reason == TransactionReasons.ContractReward && query.GetTotal(Currency.Funds) > 0)
                    query.AddDelta(Currency.Funds, -query.GetTotal(Currency.Funds) * 0.15f);
            }
            // if (query.GetTotal(Currency.Funds) > 0) query.AddDelta(Currency.Funds, query.GetTotal(Currency.Funds) / 2);
            // if (query.GetTotal(Currency.Reputation) > 0) query.AddDelta(Currency.Reputation, -query.GetTotal(Currency.Reputation) * 1.5f);
        }
        private void UpdateContract(Contract contract, ContractParameter param) => KWAgencies.AddStanding(contract.Agent, contractRep);
        private void CompleteContract(Contract contract) {
            KWAgencies.AddStanding(contract.Agent, !KWAgencies.GetPlayer().StratActive(KWUI.Mode.Lead) ? contract.ReputationCompletion : contract.ReputationCompletion * 0.5f);
            KWAgencies.GetAgency(contract.Agent).AddRep(contract.ReputationCompletion);
            KWUtil.GetHero().stupidity = Mathf.Clamp(KWUtil.GetHero().stupidity - contract.ScienceCompletion * 0.002f, 0f, 1f);
        }
        private void CancelContract(Contract contract) => KWAgencies.AddStanding(contract.Agent, contract.ReputationFailure);
        private void FailContract(Contract contract) => KWAgencies.AddStanding(contract.Agent, contract.ReputationFailure);

        private void OfferContract(Contract contract) {
            // contract.ScienceCompletion = 0;
            // for (int i = 0; i < contract.ParameterCount; i++) contract.GetParameter(i).ScienceCompletion = 0;
                
        }
        private void ReceiveScience(float amount, ScienceSubject subject, ProtoVessel _, bool __) {
            KWUtil.GetHero().stupidity = Mathf.Clamp(KWUtil.GetHero().stupidity - amount * 0.001f, 0f, 1f);
            Agency player = KWAgencies.GetPlayer();
            if (player.IsRnD() && player.StratActive(KWUI.Mode.Interns))
                ResearchAndDevelopment.Instance.AddScience(amount * Reputation.CurrentRep * 0.001f, TransactionReasons.Strategies);
            //CelestialBody body = FlightGlobals.Bodies.Find(b => subject.id.Split('@')[1].StartsWith(b.name));
            //SciType type = SciType.Misc;
            //string id = subject.id.Split('@')[0];
            //if (id == "recovery") type = SciType.Milestones;
            //else if (id.Contains("Sample")) type = SciType.Samples;
            //else if (id.StartsWith("deployed")) type = SciType.Deployed;
            //else if (id == "crewReport" || id.StartsWith("eva")) type = SciType.Crew;
            //else if ("mysteryGoo|mobileMaterialsLab".Contains(id)) type = SciType.Materials;
            //else if (id.StartsWith("ROCScience") || "atmosphereAnalysis|infraredTelescope".Contains(id)) type = SciType.Scans;
            //else if ("temperatureScan|barometerScan|seismicScan|gravityScan|magnetometer".Contains(id)) type = SciType.Sensors;
            //if (body != null && type != SciType.Misc) KWAgencies.Player().AddSci(body, type, amount);
        }

        private void RolloutVessel(ShipConstruct ship) {
            // KWUtil.CareerOpts().adminFunds += (int)Math.Round(ship.GetShipCosts(out float d, out float f) * 0.5f * (KWUtil.CareerOpts().subsidyMod + 1f));
            // foreach (ProtoCrewMember crew in ship.Parts.SelectMany(p => p.protoModuleCrew)) crew.hasHelmetOn = true;
            if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER) return;
            HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().AllowNegativeCurrency = true;
            Agency player = KWAgencies.GetPlayer();
            foreach (AvailablePart part in ship.Parts.Select(p => p.partInfo)) {
                if (!player.RequestPart(part)) KWAgencies.OrderPart(part, false);
                KWAgencies.TryPayAgency(KWAgencies.GetManufacturer(part), part.cost * 0.5f);
                // KWAdmin.AddFunds(part.cost * 0.5f);
            }
            if (player.StratActive(KWUI.Mode.AppCmpn)) {
                Funding.Instance.AddFunds(-2500, TransactionReasons.Strategies);
                Reputation.Instance.AddReputation(1f + Math.Min(ship.GetShipCosts(out float _, out float _), 100000) * 0.0004f, TransactionReasons.Strategies);
            } else if (player.StratActive(KWUI.Mode.FRaise)) {
                Funding.Instance.AddFunds(2500 + Math.Min(ship.GetShipCosts(out float _, out float _), 250000) * 0.1f, TransactionReasons.Strategies);
                Reputation.Instance.AddReputation(-5, TransactionReasons.Strategies);
            }
        }

        private void RecoverVessel(Vessel vessel) {
            if (vessel.GetVesselCrew().Select(c => c.name).Contains(KWUtil.GetHero().name))
                KWUtil.GetHero().courage = Mathf.Clamp(KWUtil.GetHero().experience * 0.01f, 0f, 1f);
            foreach (AvailablePart part in vessel.Parts.Select(p => p.partInfo).Where(p => !p.name.Contains("kerbalEVA"))) // 
                KWAgencies.GetPlayer().StorePart(part);
            // Funding.Instance.AddFunds(-dialog.totalFunds, TransactionReasons.None);
            //float vesselMass = 0f;
            //foreach (ProtoPartSnapshot part in vessel.protoPartSnapshots) vesselMass += part.mass;
            //double amount = vesselMass * (1.05 - factor) * -10000;
            //amount += vessel.GetVesselCrew().Count * (1.05 - factor) * -10000;
            //if (vessel.situation == Vessel.Situations.SPLASHED) amount *= 1.25;
            //Funding.Instance.AddFunds(amount, TransactionReasons.None);
            //ScreenMessages.PostScreenMessage("recovery cost: " + amount);
            //Debug.Log("recovery cost: " + amount);
        }
        
    }

    [KSPAddon(KSPAddon.Startup.AllGameScenes, false)]
    public class KWUI : MonoBehaviour {
        // private readonly string[] labConverterIDs = new string[3] { "surfaceSample", "mysteryGoo", "mobileMaterialsLab" };
        private ApplicationLauncherButton agencyBtn;
        private ApplicationLauncherButton kerbBtn;
        private object expDialogObj;
        private FieldInfo labBtnField;
        private Button labBtn;
        private object vmInstance;
        // private MethodInfo vmClearMethod;
        private FieldInfo vmListField;
        private int vmCount;
        // public static bool updateHelmets = true;
        //private Button recoveryBtn;
        //private object erdInstance;
        //private FieldInfo erdBtnField;
        private bool inRnD;
        // private bool inAC;
        // private ListMode listMode;
        [Flags]
        public enum Mode {
            [Description("#autoLOC_6002637")] Main = 1,
            [Description("#autoLOC_900702")] Desc = 2,
            [Description("#KWLOC_takeoverBid")] Take = 4,
            [Description("#autoLOC_501150")] AppCmpn = 8,
            [Description("#autoLOC_501158")] OutSrc = 16,
            [Description("#autoLOC_501154")] OpenSrc = 32,
            [Description("#autoLOC_501160")] Patents = 64,
            [Description("#autoLOC_501168")] IPSell = 128,
            [Description("#autoLOC_501162")] Negs = 256,
            [Description("#autoLOC_501166")] Bail = 512,
            [Description("#autoLOC_501152")] FRaise = 1024,
            [Description("#autoLOC_501156")] Interns = 2048,
            [Description("#autoLOC_234195")] PartBrs = 4096,
            [Description("#autoLOC_234195")] PartBuy = 8192,
            [Description("#KWLOC_junkyardBuy")] JunkBuy = 16384,
            [Description("#KWLOC_junkyardSell")] JunkSell = 32768,
            [Description("#KWLOC_purchaseOrder")] PrsOrd = 65536,
            [Description("#autoLOC_501170")] Lead = 131072,
            [Description("#autoLOC_1900256")] Train = 262144,
            Parts = PartBrs | PartBuy,
            Junk = JunkBuy | JunkSell
        }
        
        public void Start() {
            // if (KWUtil.EditorScene() || KWUtil.FlightScene()) {
            GameEvents.onPartActionUICreate.Add(CreatePAW);
            // }
            GameEvents.onGUIApplicationLauncherReady.Add(AddAppBtns);
            GameEvents.onGUIApplicationLauncherDestroyed.Add(RemoveAppBtns);
            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER) {
                // if (KWUtil.KSCScene()) {
                RDNode.OnNodeSelected.Add(SelectRDNode);

                GameEvents.OnCrewmemberLeftForDead.Add(LostCrew);
                GameEvents.OnCrewmemberSacked.Add(LostCrew);
                // GameEvents.onFacilityContextMenuSpawn.Add(SpawnFacilityMenu);
                //GameEvents.onFacilityContextMenuDespawn.Add(DespawnFacilityMenu);
                GameEvents.OnTechnologyResearched.Add(ResearchTech);
                // GameEvents.onGUIAdministrationFacilitySpawn.Add(SpawnAdmin);
                // GameEvents.onGUIAdministrationFacilityDespawn.Add(DespawnAdmin);
                GameEvents.onGUIRnDComplexSpawn.Add(SpawnRnD);
                GameEvents.onGUIRnDComplexDespawn.Add(DespawnRnD);
                GameEvents.onTooltipSpawned.Add(SpawnTooltip);
                //GameEvents.onGUIAstronautComplexSpawn.Add(SpawnAC);
                //GameEvents.onGUIAstronautComplexDespawn.Add(DespawnAC);
                if (!KWUtil.HeroReady()) KerbPopup((ProtoCrewMember.Gender)UnityEngine.Random.Range(0, 2));
                if (HighLogic.LoadedScene == GameScenes.SPACECENTER) {
                    vmInstance = typeof(KSCVesselMarkers).GetField("fetch", BindingFlags.Static | BindingFlags.Public).GetValue(null);
                    // vmClearMethod = typeof(KSCVesselMarkers).GetMethod("ClearVesselMarkers", BindingFlags.Instance | BindingFlags.NonPublic);
                    vmListField = typeof(KSCVesselMarkers).GetField("markers", BindingFlags.Instance | BindingFlags.NonPublic);

                }
                // } else
                if (HighLogic.LoadedSceneIsEditor) {
                    StartCoroutine(CallbackUtil.DelayedCallback(1, delegate { KWUtil.UpdateEditorPartList(); }));
                } else if (HighLogic.LoadedScene == GameScenes.TRACKSTATION) {
                    StartCoroutine(CallbackUtil.DelayedCallback(1, delegate {
                        SpaceTracking.Instance.RecoverButton.onClick.RemoveAllListeners();
                        SpaceTracking.Instance.RecoverButton.onClick.AddListener(delegate {
                            if (KWUtil.VesselRecoveryCheck(SpaceTracking.Instance.SelectedVessel))
                                typeof(SpaceTracking).GetMethod("BtnOnclick_RecoverSelectedVessel", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(
                                    typeof(SpaceTracking).GetField("Instance", BindingFlags.Static | BindingFlags.Public).GetValue(null), null); }); }));
                } else if (HighLogic.LoadedSceneIsFlight) {
                    StartCoroutine(CallbackUtil.DelayedCallback(1, delegate {
                        AltimeterSliderButtons slBtns = FindObjectOfType<AltimeterSliderButtons>();
                        slBtns.vesselRecoveryButton.onClick.RemoveAllListeners();
                        slBtns.vesselRecoveryButton.onClick.AddListener(delegate {
                            if (KWUtil.VesselRecoveryCheck(FlightGlobals.ActiveVessel))
                                typeof(AltimeterSliderButtons).GetMethod("recoverVessel", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(slBtns, null); }); }));
                }
                // if (KWUtil.KSCScene() || KWUtil.EditorScene())
                //    if (flightScene) {
                //        FindObjectOfType<AltimeterSliderButtons>().vesselRecoveryButton.gameObject.SetActive(false);
                //    } else if (trackScene) {
                //        recoveryBtn = SpaceTracking.Instance.RecoverButton;
                //    }
            }

            if (HighLogic.LoadedSceneIsFlight) MusicLogic.SetVolume(GameSettings.MUSIC_VOLUME, GameSettings.AMBIENCE_VOLUME);
            // else if (KWUtil.KSCScene()) MusicLogic.SetVolume(KWUtil.GenOpts().volKSCMus, GameSettings.AMBIENCE_VOLUME);
            else if (HighLogic.LoadedScene == GameScenes.TRACKSTATION) MusicLogic.SetVolume(KWUtil.GenOpts().volTrackMus, GameSettings.AMBIENCE_VOLUME);
            else if (HighLogic.LoadedSceneIsEditor) MusicLogic.SetVolume(KWUtil.GenOpts().volEditorMus, GameSettings.AMBIENCE_VOLUME);

        }
        public void OnDisable() {
            // if (KWUtil.EditorScene() || KWUtil.FlightScene()) {
            GameEvents.onPartActionUICreate.Remove(CreatePAW);

            GameEvents.onGUIApplicationLauncherReady.Remove(AddAppBtns);
            GameEvents.onGUIApplicationLauncherDestroyed.Remove(RemoveAppBtns);
            RemoveAppBtns();
            // }
            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER) {
                // if (KWUtil.KSCScene()) {
                RDNode.OnNodeSelected.Remove(SelectRDNode);
                GameEvents.OnCrewmemberLeftForDead.Remove(LostCrew);
                GameEvents.OnCrewmemberSacked.Remove(LostCrew);
                GameEvents.OnTechnologyResearched.Remove(ResearchTech);

                // GameEvents.onFacilityContextMenuSpawn.Remove(SpawnFacilityMenu);
                //GameEvents.onFacilityContextMenuDespawn.Remove(DespawnFacilityMenu);
                // GameEvents.onGUIAdministrationFacilitySpawn.Remove(SpawnAdmin);
                // GameEvents.onGUIAdministrationFacilityDespawn.Remove(DespawnAdmin);
                GameEvents.onGUIRnDComplexSpawn.Remove(SpawnRnD);
                GameEvents.onGUIRnDComplexDespawn.Remove(DespawnRnD);
                    //GameEvents.onGUIAstronautComplexSpawn.Remove(SpawnAC);
                    //GameEvents.onGUIAstronautComplexDespawn.Remove(DespawnAC);
                // }
                // if (KWUtil.KSCScene() || KWUtil.EditorScene())
                GameEvents.onTooltipSpawned.Remove(SpawnTooltip);

            }

        }
        public void Update() {
            if (HighLogic.LoadedSceneIsFlight) {
                if (ExperimentsResultDialog.Instance != null) {
                    if (expDialogObj == null) {
                        expDialogObj = typeof(ExperimentsResultDialog).GetField("Instance", BindingFlags.Static | BindingFlags.Public).GetValue(null);
                        labBtnField = typeof(ExperimentsResultDialog).GetField("btnLab", BindingFlags.Instance | BindingFlags.NonPublic);
                        labBtn = (Button)labBtnField.GetValue(expDialogObj);
                    }
                    if (labBtn.gameObject.activeSelf && ExperimentsResultDialog.Instance.currentPage.labSearch.HasAnyLabs &&
                        !KWUtil.labConverterIDs.Contains(ExperimentsResultDialog.Instance.currentPage.pageData.subjectID.Split('@')[0])) {
                        //object instance = typeof(ExperimentsResultDialog).GetField("Instance", BindingFlags.Static | BindingFlags.Public).GetValue(null);
                        //FieldInfo field = typeof(ExperimentsResultDialog).GetField("btnLab", BindingFlags.Instance | BindingFlags.NonPublic);
                        //Button btn = (Button)field.GetValue(instance);
                        //btn.gameObject.SetActive(false);
                        //field.SetValue(instance, btn);
                        labBtn.gameObject.SetActive(false);
                        labBtnField.SetValue(expDialogObj, labBtn);
                    }
                } else if (expDialogObj != null)
                    expDialogObj = null;
                //if (FlightGlobals.ready && updateHelmets) {
                //    updateHelmets = false;
                //    foreach (ProtoCrewMember crew in FlightGlobals.ActiveVessel.GetVesselCrew())
                //        if (crew.KerbalRef.showHelmet && !crew.KerbalInventoryModule.ContainsPart("KWhelmet"))
                //            KWUtil.ToggleHelmet(false, crew);
                //}

            } // else if (KWUtil.KSCScene()) {
                // vmClearMethod.Invoke(vmInstance, null);
                //if (inAC) {
                //    FindObjectOfType<AstronautComplex>().ScrollListApplicants.GetUilistItemAt(4).GetComponent<CrewListItem>().MouseoverEnabled = false;
                //}
            // }

            //if (trackScene && SpaceTracking.Instance.SelectedVessel != null) {
            //    SpaceTracking.Instance.RecoverButton.gameObject.SetActive(SpaceTracking.Instance.SelectedVessel.LandedInStockLaunchSite); 
            //}
            //    //if (trackScene && SpaceTracking.Instance.RecoverButton.gameObject.activeSelf)
            //    //    SpaceTracking.Instance.RecoverButton.gameObject.SetActive(false);
            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER && HighLogic.LoadedScene == GameScenes.SPACECENTER) {
                List<KSCVesselMarker> vesselMarkers = (List<KSCVesselMarker>)vmListField.GetValue(vmInstance);
                if (vesselMarkers.Count > 0 && vesselMarkers.Count != vmCount) {
                    for (int i = vesselMarkers.Count - 1; i >= 0; i--) {
                        Vessel vessel = (Vessel)vesselMarkers[i].GetType().GetField("v", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(vesselMarkers[i]);
                        if (!KWUtil.VesselRecoveryCheck(vessel, false)) {
                            vesselMarkers[i].Terminate();
                            vesselMarkers.RemoveAt(i); }}
                    vmListField.SetValue(vmInstance, vesselMarkers);
                    vmCount = vesselMarkers.Count;
                }
                //if (vesselMarkers.Count > 0) {
                //    vesselMarkers.ForEach(m => m.Terminate());
                //    vesselMarkers.Clear();
                //    vmListField.SetValue(vmInstance, vesselMarkers);
                //}
            }
            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER && HighLogic.LoadedScene == GameScenes.TRACKSTATION)
                if (SpaceTracking.Instance?.SelectedVessel != null)
                    SpaceTracking.Instance.DeleteButton.Unlock();
            
        }
        private void SpawnAdmin() { }
        private void DespawnAdmin() { }
        private void SpawnRnD() { KWUtil.LockAllParts(); inRnD = true; }
        private void DespawnRnD() { KWUtil.UnlockParts(); inRnD = false; }
        // private void SpawnAC() { inAC = true; }
        // private void DespawnAC() { inAC = false; }
        private void LostCrew(ProtoCrewMember crew, int _) {
            if (!crew.isHero) return;
            KWAgencies.GetPlayer().Quit();
            agencyBtn.SetTexture(KWUtil.uiIcons[Mode.Take]);
        }
        private void CreatePAW(Part part) {

            //if (editorScene && part.Modules.Contains<ModuleKWPressurisedCabin>()) {
            //    // ModuleKWLifeSupport lifeSup = part.FindModuleImplementing<ModuleKWLifeSupport>();

            //    // UIPartActionWindow paw = UIPartActionController.Instance.GetItem(part);

            //    //for (int i = part.PartActionWindow.ListItems.Count; i >= 0; i--) {
            //    //    if (i.GetType() == typeof(UIPartActionResourceEditor)) {
            //    //        UIPartActionResourceEditor res = (UIPartActionResourceEditor)part.PartActionWindow.ListItems[i];
            //    //        if (new string[3] { "Air", "Ox", "Toxic Gas" }.Contains(res.resourceName.text))
            //    //            part.PartActionWindow.ListItems.Remove(part.PartActionWindow.ListItems[i]);
            //    //    }
            //    //}
            //    // UIPartActionWindow paw = UIPartActionController.Instance.GetItem(part);
            //    ModuleKWPressurisedCabin kwCabin = part.FindModuleImplementing<ModuleKWPressurisedCabin>();
            //    if (!kwCabin.showStockControls)
            //        StartCoroutine(CallbackUtil.DelayedCallback(1, delegate { kwCabin.HideStockControls(); }));
            //    // UIPartActionMinMaxRange item = (UIPartActionMinMaxRange)part.PartActionWindow.ListItems.Find(i => i.GetType() == typeof(UIPartActionMinMaxRange));
            //    // item.fieldAmount.text = "";
            //                                                                          // paw.displayDirty = true;

            //    //foreach (UIPartActionResourceEditor res in part.PartActionWindow.ListItems.Where(i => i.GetType() == typeof(UIPartActionResourceEditor)).ToList()) {

            //    //    // Debug.Log(res.resourceName.text);
            //    //    if (new string[3] { "Air", "Ox", "Toxic Gas" }.Contains(res.resourceName.text))
            //    //        part.PartActionWindow.ListItems.Remove(res);
            //    //}

            //    //foreach (PartResource res in part.Resources.Where(r => new string[3] { "IntakeAir", "Oxidizer", "ToxicGas" }.Contains(r.resourceName))) {
            //    //    paw.RemoveResourceControlEditor(res);
            //    //}
            //    //paw.CreatePartList(true);

            //    // 

            //    // lifeSup.UpdateCapPrs();
            //} else


            //if (part.Modules.Contains<ModuleResourceIntakeKWAddon>()) {
            //    StartCoroutine(CallbackUtil.DelayedCallback(1, delegate {
            //        foreach (PartResource res in part.Resources.Where(r => r.resourceName != part.FindModuleImplementing<ModuleResourceIntakeKWAddon>().resourceName))
            //            part.PartActionWindow.RemoveResourceControlEditor(part.Resources.Get(res.resourceName));
            //    }));
            //}

            if (part.Modules.Contains<ModuleKWResourceToggle>())
                StartCoroutine(CallbackUtil.DelayedCallback(1, delegate { part.FindModuleImplementing<ModuleKWResourceToggle>().UpdateUI(); }));
            if (part.Modules.Contains<ModuleKWCabin>()) {
                StartCoroutine(CallbackUtil.DelayedCallback(1, delegate { part.FindModuleImplementing<ModuleKWCabin>().UpdateUI(); }));
            }
            //if (part.Modules.Contains<ModuleKWEVA>())
            //    part.FindModuleImplementing<ModuleKWEVA>().UpdateUI();



            //if (part.Modules.Contains<ModuleKWIgnitors>())
            //    part.FindModuleImplementing<ModuleKWIgnitors>().UpdateUI();
            if (HighLogic.LoadedSceneIsFlight) {
                if (part.Modules.Contains<ModuleEnginesKW>())
                    part.FindModuleImplementing<ModuleEnginesKW>().UpdateUI();
                if (part.Modules.Contains<ModuleEnginesFXKW>())
                    part.FindModuleImplementing<ModuleEnginesFXKW>().UpdateUI();
                if (part.Modules.Contains<ModuleScienceExperimentKW>())
                    StartCoroutine(CallbackUtil.DelayedCallback(1, delegate { part.FindModuleImplementing<ModuleScienceExperimentKW>().UpdateUI(); }));
                
            }
        }
        private void SelectRDNode(RDNode node) { if (node.state == RDNode.State.RESEARCHED) HidePartPurchBtn(); }
        private void ResearchTech(GameEvents.HostTargetAction<RDTech, RDTech.OperationResult> action) { if (action.target == RDTech.OperationResult.Successful) HidePartPurchBtn(); }
        private void HidePartPurchBtn() => StartCoroutine(CallbackUtil.DelayedCallback(1, delegate { RDController.Instance.actionButton.gameObject.SetActive(false); }));
        private void SpawnFacilityMenu(KSCFacilityContextMenu menu) {
            SpaceCenterFacility facility = Enum.GetValues(typeof(SpaceCenterFacility)).Cast<SpaceCenterFacility>().FirstOrDefault(f => f.Description() ==
                (string)menu.GetType().GetField("facilityName", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(menu));

            if (facility == SpaceCenterFacility.AstronautComplex) {
                StartCoroutine(CallbackUtil.DelayedCallback(1, delegate {
                    FieldInfo tField = menu.GetType().GetField("RepairButtonText", BindingFlags.Instance | BindingFlags.NonPublic);
                    TextMeshProUGUI text = (TextMeshProUGUI)tField.GetValue(menu);
                    text.text = $"{Localizer.Format("#autoLOC_900174")} {Localizer.Format("#autoLOC_1900256")}";
                    text.color = Color.white; // new Color(212, 244, 253);
                    tField.SetValue(menu, text);
                }));
                // foreach (string btnName in new string[2] { "Repair", "Upgrade" }) {
                FieldInfo field = menu.GetType().GetField("RepairButton", BindingFlags.Instance | BindingFlags.NonPublic); // btnName + "Button"
                if (field == null) return; // continue;
                Button btn = (Button)field.GetValue(menu);
                // if (btnName == "Repair")
                StartCoroutine(CallbackUtil.DelayedCallback(1, delegate {
                    btn.interactable = true;
                    btn.gameObject.SetActive(true);
                    // if (facility == SpaceCenterFacility.AstronautComplex)
                    // btn.onClick.AddListener(delegate { CrewTrainingPopup(); });
                    field.SetValue(menu, btn);
                }));
                    //else {
                    //    btn.gameObject.SetActive(false);
                    //    field.SetValue(menu, btn);
                    //}

                // }
                //if (KWUtil.IsTechLocked(facility + "") && false)
                //    StartCoroutine(CallbackUtil.DelayedCallback(1, delegate {
                //        menu.statusText.text = Localizer.Format("#KWLOC_underConstruction");
                //        menu.descriptionText.text = "";
                //        menu.levelFieldText.text = "";
                //        menu.levelStatsText.text = ""; }));
            }
        }
        private void DespawnFacilityMenu(KSCFacilityContextMenu menu) {
            //SpaceCenterFacility facility = Enum.GetValues(typeof(SpaceCenterFacility)).Cast<SpaceCenterFacility>().FirstOrDefault(f => f.Description() ==
            //    (string)menu.GetType().GetField("facilityName", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(menu));
            //if (facility != SpaceCenterFacility.Administration) return;
            //FieldInfo field = menu.GetType().GetField("RepairButton", BindingFlags.Instance | BindingFlags.NonPublic);
            //if (field != null) {
            //    Button btn = (Button)field.GetValue(menu);
            //    btn.onClick.RemoveAllListeners();
            //    field.SetValue(menu, btn); }
        }
        private void SpawnTooltip(ITooltipController _, Tooltip tt) {
            if (inRnD && tt.GetType() == typeof(Tooltip_TitleAndText)) {
                //Tooltip_TitleAndText tooltip = (Tooltip_TitleAndText)tt;
                //tooltip.label.text = "";
            } else if (tt.GetType() == typeof(PartListTooltip)) {
                PartListTooltip tooltip = (PartListTooltip)tt;
                AvailablePart part = (AvailablePart)tooltip.GetType().GetField("partInfo", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(tooltip);
                if (ResearchAndDevelopment.PartTechAvailable(part) && !ResearchAndDevelopment.PartModelPurchased(part)) {
                    // toolTipPart = part;
                    // tooltip.extInfoListContainer.transform.GetChild(0).gameObject.SetActive(false);
                    tooltip.buttonPurchaseContainer.SetActive(false);
                    tooltip.costPanel.SetActive(true);
                }
                // if (tooltip.HasExtendedInfo && part.partPrefab.Modules.Contains<ModuleKWPressurisedCabin>()) {
                // hide resources
                // }
            }
        }
        private void AddAppBtns() {
            if (ApplicationLauncher.Ready && agencyBtn == null && HighLogic.CurrentGame.Mode == Game.Modes.CAREER) {
                agencyBtn = ApplicationLauncher.Instance.AddModApplication(
                    delegate {
                        if (!KWUtil.HeroReady()) ScreenMessages.PostScreenMessage($"{Localizer.Format("#autoLOC_6002290")}: {Localizer.Format("#autoLOC_900441")}"); // KerbPopup((ProtoCrewMember.Gender)UnityEngine.Random.Range(0, 2));
                        else if (KWAgencies.GetPlayer() == null) Popup(mode: Mode.Take);
                        else Popup(mode: HighLogic.LoadedSceneIsEditor ? Mode.PrsOrd : Mode.Main); }, null, null, null, null, null,
                    ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPH | 
                    ApplicationLauncher.AppScenes.MAPVIEW | ApplicationLauncher.AppScenes.TRACKSTATION, KWAgencies.GetPlayer()?.Agent().Logo ?? KWUtil.uiIcons[Mode.Take]); } // !KWUtil.HeroReady() ? KWUtil.adminIcons[ProgType.LifeSci] : (KWAgencies.GetPlayer()?.Agent().Logo ?? KWUtil.uiIcons[Mode.Take]));
            if (ApplicationLauncher.Ready && kerbBtn == null) {
                kerbBtn = ApplicationLauncher.Instance.AddModApplication( delegate { KerbPopup((ProtoCrewMember.Gender)UnityEngine.Random.Range(0, 2)); }, null, null, null, null, null, 
                    ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.MAPVIEW | ApplicationLauncher.AppScenes.TRACKSTATION, 
                    KWUtil.adminIcons[ProgType.LifeSci]); }
            //if (HighLogic.LoadedSceneIsEditor && ApplicationLauncher.Ready && editorBtn == null) {
            //    editorBtn = ApplicationLauncher.Instance.AddModApplication(delegate { EditorPopup(); }, delegate { EditorPopup(); }, null, null, null, null,
            //        ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPH, GameDatabase.Instance.GetTexture("Squad/Contracts/Icons/report", false));
            //}
        }
        private void RemoveAppBtns() {
            if (agencyBtn != null) { ApplicationLauncher.Instance.RemoveModApplication(agencyBtn); agencyBtn = null; }
            if (kerbBtn != null) { ApplicationLauncher.Instance.RemoveModApplication(kerbBtn); kerbBtn = null; }
            // if (editorBtn != null) { ApplicationLauncher.Instance.RemoveModApplication(editorBtn); editorBtn = null; }
        }
        //private void LaunchApp() {
        //    if (!KWUtil.HeroReady()) ScreenMessages.PostScreenMessage($"{Localizer.Format("#autoLOC_6002290")}: {Localizer.Format("#autoLOC_900441")}"); // KerbPopup((ProtoCrewMember.Gender)UnityEngine.Random.Range(0, 2));
        //    else if (KWAgencies.GetPlayer() == null) Popup(mode: Mode.Take);
        //    else Popup(mode: HighLogic.LoadedSceneIsEditor ? Mode.PrsOrd : Mode.Main);
        //}
        private void KerbPopup(ProtoCrewMember.Gender gend, string name = null) {
            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER && !KWUtil.HeroReady()) {
                if (name == null) name = CrewGenerator.GetRandomName(gend).Replace(" " + CrewGenerator.GetLastName(), "");
                PopupDialog.SpawnPopupDialog(
                new MultiOptionDialog("KWKerbPopup", "", Localizer.Format("#autoLOC_900441"), HighLogic.UISkin, new Rect(0.5f, 0.5f, 300, 1),
                    new DialogGUIBase[6] {
                    new DialogGUIBox("", 290, 3),
                    new DialogGUIHorizontalLayout(
                        new DialogGUIBox(Localizer.Format("#autoLOC_7000024"), 65, 32),
                        new DialogGUITextInput(name, false, 15,
                            (string n) => { name = n; return n; }, 80, 32),
                        new DialogGUIBox(CrewGenerator.GetLastName(), 65, 32),
                        new DialogGUIButton(Localizer.Format("#autoLOC_900432"), delegate { KerbPopup((ProtoCrewMember.Gender)UnityEngine.Random.Range(0, 2)); }, 65, 32, true)),
                    new DialogGUIBox("", 290, 3),
                    new DialogGUIHorizontalLayout(
                        new DialogGUIBox(Localizer.Format("#autoLOC_900447") + ":", 65, 32),
                        new DialogGUIToggleButton(gend == ProtoCrewMember.Gender.Female, Localizer.Format("#autoLOC_900444"),
                            delegate { PopupDialog.DismissPopup("KWHeroPopup"); KerbPopup(ProtoCrewMember.Gender.Female); }, 108, 32),
                        new DialogGUIToggleButton(gend == ProtoCrewMember.Gender.Male, Localizer.Format("#autoLOC_900434"),
                            delegate { PopupDialog.DismissPopup("KWHeroPopup"); KerbPopup(ProtoCrewMember.Gender.Male); }, 107, 32)),
                    new DialogGUIBox("", 290, 3),
                    new DialogGUIButton(Localizer.Format("#autoLOC_900341"), delegate {
                        ProtoCrewMember hero = new ProtoCrewMember(ProtoCrewMember.KerbalType.Applicant, $"{name} {CrewGenerator.GetLastName()}") {
                            gender = gend, stupidity = 1f, trait = KerbalRoster.touristTrait };
                        hero.GetType().GetProperty("isHero").SetMethod.Invoke(hero, new object[] { true });
                        if (HighLogic.CurrentGame.CrewRoster.AddCrewMember(hero)) {
                            // if (!KWUtil.CareerOpts().takeoverBids && KWAgencies.NewPlayer())
                                // KWAgencies.Takeover(KWAgencies.AList.Find(a => a.Name == "Research & Development Department"));
                            // agencyBtn.SetTexture(KWUtil.uiIcons[Mode.Take]);
                            // KWUtil.SpawnHero();
                            Funding.Instance.SetFunds(HighLogic.CurrentGame.Parameters.Career.StartingFunds, TransactionReasons.None);
                            Reputation.Instance.SetReputation(HighLogic.CurrentGame.Parameters.Career.StartingReputation, TransactionReasons.None);
                            ResearchAndDevelopment.Instance.SetScience(HighLogic.CurrentGame.Parameters.Career.StartingScience, TransactionReasons.None);
                            kerbBtn.SetFalse(false);
                        } else ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8002103", $"{name} {CrewGenerator.GetLastName()}"));
                    }, 290, 32, true) }), false, HighLogic.UISkin);
            } else {
                List<DialogGUIBase> box = new List<DialogGUIBase> {
                new DialogGUIHorizontalLayout(
                    new DialogGUIBox($"<i>{Localizer.Format("#autoLOC_218513")} {Localizer.Format("#autoLOC_900174")} {Localizer.Format("#autoLOC_6001352")}</i>", 266, 32),
                    new DialogGUIButton("x", delegate { kerbBtn.SetFalse(false); }, 32, 32, true)) };
                box.Add(new DialogGUIHorizontalLayout(new DialogGUIBox(Localizer.Format("#autoLOC_901054"), 149, 32), new DialogGUIBox(Localizer.Format("#autoLOC_6001352"), 149, 32)));
                foreach (ProtoCrewMember crew in HighLogic.CurrentGame.CrewRoster.Crew.Where(c => c.rosterStatus == ProtoCrewMember.RosterStatus.Assigned)) {
                    box.Add(new DialogGUIHorizontalLayout(new DialogGUIBox($"<color=white>{crew.displayName}</color>", 149, 32), new DialogGUIBox(
                        KWKerbs.GetKerb(crew).OxLoss > 0 ? $"<color=red>{Localizer.Format("#autoLOC_456381", "#KWLOC_oxygen")}</color>" :
                        $"<color=green>{Localizer.Format("#autoLOC_219034")}</color>", 149, 32)));
                    if (crew.hasHelmetOn) {
                        ProtoPartResourceSnapshot[] ox = KWUtil.GetInvResources("Oxygen", crew.KerbalInventoryModule); 
                        box.Add(new DialogGUIHorizontalLayout(
                            new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.adminIcons[ProgType.PhysSci]),
                            new DialogGUIBox($"<color=#D4F4FD>{Localizer.Format("#KWLOC_oxygen")}</color>", 112, 32),
                            new DialogGUIBox($"<color=#D4F4FD>{KSPUtil.PrintTime(ox.Length > 0 ? ox.Select(r => r.amount).Sum() / 0.0000014 : 0, 4, true)}</color>", 149, 32)));  }
                }
                PopupDialog.SpawnPopupDialog(new MultiOptionDialog("KWKerbPopup", "", "", HighLogic.UISkin, new Rect(0.5f, 0.5f, 313, 1), box.ToArray()), false, HighLogic.UISkin);
            }
        }
        //private DialogGUIHorizontalLayout TitleBar(Agency agency, bool mainMode) => new DialogGUIHorizontalLayout(
        //    new DialogGUIButton(mainMode ? "^" : "<", delegate { Popup(mainMode ? KWAgencies.Player() : agency, Mode.Main); }, 32, 32, true),
        //    new DialogGUIToggleButton(true, $"<i>{agency.Agent().Title}</i>", delegate { Dismiss(); Popup(agency, Mode.Desc); }, 256, 32), //  mainMode ? 292 : 
        //    new DialogGUIButton("x", delegate { }, 32, 32, true));
            //if (!mainMode) bar.Insert(0, new DialogGUIButton("◄", delegate { Popup(agency, Mode.Main); }, 32, 32, true));
            //return new DialogGUIHorizontalLayout(bar.ToArray());
        private DialogGUIHorizontalLayout NavBar(Agency agency, Mode mode) { // , ListMode lMode = ListMode.All
            List<DialogGUIBase> bar = new List<DialogGUIBase>();
            bool navActive = (Mode.Main | Mode.Desc | Mode.PartBuy | Mode.OutSrc | Mode.Take | Mode.Negs).HasFlag(mode);
            bool isPlayer = agency.IsPlayer();
            if (navActive) {
                //List<Agency> aList = PartLoader.LoadedPartsList.Where(p => ResearchAndDevelopment.PartTechAvailable(p)).Select(p =>
                //    KWAgencies.GetAgency(AgentList.Instance.GetAgentbyTitle(p.manufacturer))).Append(KWAgencies.Player()).Append(KWAgencies.Junkyard()).Distinct().OrderBy(a => a.Name).ToList();
                List<Agency> aList = KWAgencies.AList.Where(a => mode == Mode.Desc || a.Name == agency.Name ||
                    (mode == Mode.Main && (a.Name == KWAgencies.GetJunkyard().Name || a.IsPlayer())) ||
                    ((Mode.Main | Mode.PartBuy).HasFlag(mode) && KWAgencies.GetAvailableParts(a).Count > 0) ||
                    (mode == Mode.Take && a.CanTakeover()) ||
                    (mode == Mode.Negs && !a.IsPlayer()) ||
                    (mode == Mode.OutSrc && a.Labs.Count > 0 && a.IsPlayer())).ToList();
                int i = aList.FindIndex(a => a.Name == agency.Name);
                bar.AddRange(new DialogGUIBase[] {
                    new DialogGUIButton("<<", delegate { Popup(aList[i - 1 > -1 ? i - 1 : aList.Count - 1], mode); }, 32, 32, true),
                    new DialogGUIButton(">>", delegate { Popup(aList[i + 1 < aList.Count ? i + 1 : 0], mode); }, 32, 32, true) });
            }
            // if (mode == Mode.Main) //  && !KWAgencies.NewPlayer()) || (mode == Mode.Desc && isPlayer) bar.Insert(1, new DialogGUIBox("mode selector", 256, 32));
            // bar.Insert(1, new DialogGUIBox(Localizer.Format(Mode.Main.Description()), 256, 32)); } // #autoLOC_6001393
            if (KWAgencies.GetPlayer() == null && mode == Mode.Desc)
                //if (!navActive) bar.Add(new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.Lead]));
                bar.InsertRange(1, new DialogGUIBase[] { GetIcon(Mode.Take),
                    new DialogGUIButton(Localizer.Format(Mode.Take.Description()), delegate { Dismiss(); Popup(agency, Mode.Take); }, 184, 32, true), GetIcon(Mode.Take) }); // 257, 221,  

            //else if ((Mode.FRaise | Mode.Negs | Mode.IPSell).HasFlag(mode))
            //    bar.InsertRange(bar.Count > 1 ? 1 : 0, new DialogGUIBase[] {
            //        new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[mode]),
            //        new DialogGUIToggleButton(true, Localizer.Format(mode.Description()),
            //            delegate { Dismiss(); Popup(agency, Mode.Main); }, bar.Count > 1 ? 184 : 256, 32),
            //        new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[mode]) });
            //else if ((Mode.OpenSrc | Mode.Patents).HasFlag(mode))
            //    bar.InsertRange(bar.Count > 1 ? 1 : 0, new DialogGUIBase[] {
            //        new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[mode]),
            //        new DialogGUIToggleButton(true, Localizer.Format(mode.Description()),
            //            delegate { Dismiss(); Popup(agency, isPlayer ? Mode.Main : Mode.OutSrc); }, bar.Count > 1 ? 184 : 256, 32),
            //        new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[mode]) });
            else if ((Mode.Parts | Mode.Junk | Mode.PrsOrd).HasFlag(mode)) {
                bar.InsertRange(navActive ? 1 : 0, new DialogGUIBase[] {
                    new DialogGUIBox($"{Localizer.Format("#autoLOC_6001017")} {Localizer.Format("#autoLOC_901054")}",
                        mode == Mode.PartBrs ? 271 /*220*/ : (!navActive ? 187 : 151), 32), // !navActive ? 271 : 202 
                    GetIcon((Mode.PrsOrd | Mode.JunkBuy).HasFlag(mode) ? Mode.PartBuy : Mode.PartBrs) });
                //new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white,
                //        (Mode.PrsOrd | Mode.JunkBuy).HasFlag(mode) ? KWUtil.uiIcons[Mode.PartBuy] : KWUtil.uiIcons[Mode.PartBrs])
                if (!navActive && mode != Mode.PartBrs) bar.Insert(2, new DialogGUIBox(Localizer.Format(mode == Mode.JunkSell ? "#autoLOC_900337" : "#autoLOC_223622"), 80, 32));
                else if (!isPlayer) bar.Insert(3, new DialogGUIBox(Localizer.Format("#autoLOC_223622"), 65, 32)); // 
                //} else if ((Mode.Parts | Mode.Junk | Mode.PrsOrd).HasFlag(mode)) {
                //    bar.InsertRange(bar.Count > 1 ? 1 : 0, new DialogGUIBase[] {
                //        new DialogGUIBox($"{Localizer.Format("#autoLOC_6001017")} {Localizer.Format("#autoLOC_901054")}", Mode.Junk.HasFlag(mode) ? 187 : 151, 32), // !navActive ? 187 : 118
                //        new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, mode == Mode.JunkBuy ? KWUtil.uiIcons[Mode.PartBuy] : KWUtil.uiIcons[Mode.PartBrs]) });
                //    if (Mode.Junk.HasFlag(mode))
                //        bar.Insert(2, new DialogGUIBox(Localizer.Format("#autoLOC_223622"), 80, 32));
                //    else bar.Insert(bar.Count > 2 ? 3 : 2, new DialogGUIBox(Localizer.Format("#autoLOC_223622"), 65, 32));
                    //bar.Insert(3, new DialogGUIToggleButton(mode == Mode.PartBuy, Localizer.Format("#autoLOC_223622"),
                    //            delegate { Dismiss(); Popup(agency, mode == Mode.PartBrs ? Mode.PartBuy : Mode.PartBrs); }, 65, 32));
            } else if ((Mode.Main | Mode.Desc).HasFlag(mode)) bar.Insert(1, new DialogGUIBox(Localizer.Format(Mode.Main.Description()), 256, 32));
            else bar.InsertRange(navActive ? 1 : 0, new DialogGUIBase[] {
                GetIcon(mode), new DialogGUIBox(Localizer.Format(mode.Description()), navActive ? 184 : 256, 32), GetIcon(mode) });
            return new DialogGUIHorizontalLayout(bar.ToArray());
        }
        private DialogGUIHorizontalLayout ModeNavBtn(Agency agency, Mode mode, bool isActive = true) {
            List<DialogGUIBase> bar = new List<DialogGUIBase> { GetIcon(mode), GetIcon(mode) };
            if (isActive) bar.Insert(1, new DialogGUIButton(Localizer.Format(mode.Description()), delegate { Popup(agency, mode); }, 229, 32, true)); // 265
            else bar.Insert(1, new DialogGUIBox($"<color=orange>{Localizer.Format(mode.Description())}</color>", 229, 32));
            return new DialogGUIHorizontalLayout(bar.ToArray());
        }
        private DialogGUIHorizontalLayout ModeToggleBtn(Agency agency, Mode mode, bool canToggle = true) {
            List<DialogGUIBase> bar = new List<DialogGUIBase> { GetIcon(mode), new DialogGUIButton("?", delegate { Popup(agency, mode); }, 32, 32, true)};
            if (canToggle) bar.Insert(1, new DialogGUIToggleButton(agency.StratActive(mode), Localizer.Format(mode.Description()), delegate { agency.ToggleStrat(mode); }, 229, 32));
            else bar.Insert(1, new DialogGUIBox($"<color=orange>{Localizer.Format(mode.Description())}</color>", 229, 32));
            return new DialogGUIHorizontalLayout(bar.ToArray());
        }
        private DialogGUIBase[] LabStatusBox(Agency agency) {
            List<DialogGUIBase> icons = new List<DialogGUIBase>();
            foreach (var pair in agency.Labs)
                icons.Add(new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), pair.Value ? Color.white : new Color(255, 128, 0), KWUtil.sciIcons[pair.Key]));
            return new DialogGUIBase[] { new DialogGUIBox($"<i>{Localizer.Format("#autoLOC_6001440")}</i>", 301, 20), new DialogGUIHorizontalLayout(icons.ToArray()) };
        }
        private DialogGUIImage GetIcon(Mode mode) => new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[mode]);
        private DialogGUIScrollList ScrollBox(Agency agency, Mode mode) {
            // bool isPlayer = agency.IsPlayer();
            List<DialogGUIBase> box = new List<DialogGUIBase> {
                new DialogGUIContentSizer(ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.PreferredSize, true) };
            if (mode == Mode.Main) {
                // box.Add(new DialogGUIBox("", 303, 32));
                //if (!KWAgencies.NewPlayer()) 
                //    box.Add(new DialogGUIHorizontalLayout(KWAdmin.GetLobbyProgs(agency).Select(p => 
                //        new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.adminIcons[p])).ToArray()));
                //else box.AddRange(KWAdmin.GetLobbyProgs(agency).Select(p =>
                //        new DialogGUIHorizontalLayout(
                //            new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.adminIcons[p]),
                //            new DialogGUIBox(Localizer.Format(p.Description()), 266, 32))).ToArray());
                //if (KWAgencies.Player() == null)
                //    box.Add(ModeNavBtn(agency, Mode.Take));
                if (agency.IsPlayer() && HighLogic.LoadedSceneIsEditor)
                    box.Add(ModeNavBtn(agency, Mode.PrsOrd));
                if (agency.IsPlayer())
                    box.Add(ModeNavBtn(agency, Mode.PartBrs, KWAgencies.GetPlayer().StoredParts().Count > 0));
                else box.Add(ModeNavBtn(agency, Mode.PartBuy, KWAgencies.GetAvailableParts(agency).Count > 0));
                if (agency.IsRnD()) box.Add(ModeNavBtn(agency, Mode.Train, KWAgencies.GetPlayer().IsRnD() || KWAgencies.GetStanding(KWAgencies.GetPlayer().IsRnD() ? KWAgencies.GetKWF() : KWAgencies.GetRnD()) > 0));
                if (agency.Name == KWAgencies.GetJunkyard().Name) {
                    box.Add(new DialogGUIBox($"<i>{Localizer.Format("#KWLOC_usedPartsJunkyard")}</i>", 301, 20));
                    box.Add(ModeNavBtn(agency, Mode.JunkBuy, KWAgencies.JunkParts().Count > 0));
                    box.Add(ModeNavBtn(agency, Mode.JunkSell, KWAgencies.GetPlayer().StoredParts().Count > 0)); }
                if (agency.IsPlayer() && (agency.IsKWF() || agency.IsRnD())) {
                    box.Add(new DialogGUIBox($"<i>{Localizer.Format("#autoLOC_6001741")}</i>", 301, 20));
                    if (agency.IsKWF()) {
                        box.Add(ModeToggleBtn(agency, Mode.FRaise));
                        box.Add(ModeToggleBtn(agency, Mode.Lead));
                    } else {
                        box.Add(ModeToggleBtn(agency, Mode.AppCmpn));
                        box.Add(ModeNavBtn(agency, Mode.Bail, Funding.Instance.Funds < 0));
                        box.Add(ModeToggleBtn(agency, Mode.Interns, Reputation.CurrentRep > 0)); }}
                if (!agency.IsPlayer() && !KWAgencies.GetPlayer().IsRnD() && !KWAgencies.GetPlayer().IsKWF())
                    box.Add(ModeToggleBtn(agency, Mode.Negs));
                if (agency.Labs.Count > 0 && false) {
                    box.Add(new DialogGUIBox($"<i>{Localizer.Format("#autoLOC_6001742")}</i>", 301, 20));
                    if (!agency.IsPlayer())
                        box.Add(new DialogGUIHorizontalLayout(
                            new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.OutSrc]),
                            new DialogGUIBox(Localizer.Format(Mode.OutSrc.Description()), 229, 32),
                            new DialogGUIButton("?", delegate { Popup(agency, Mode.OutSrc); }, 32, 32, true)));
                    box.Add(ModeNavBtn(agency, Mode.OpenSrc, KWAgencies.PlayerCanOutSrc(agency)));
                    box.Add(ModeNavBtn(agency, Mode.Patents, KWAgencies.PlayerCanOutSrc(agency)));
                    box.AddRange(LabStatusBox(agency)); }
            } else if (mode == Mode.PartBrs) {
                foreach (var pair in KWAgencies.GetPlayer().StoredParts())
                    box.Add(new DialogGUIHorizontalLayout(
                        new DialogGUIBox($"<color=white>{pair.Key.title}</color>", 266, 32),
                        new DialogGUIBox(pair.Value + "", 32, 32)));
            } else if (mode == Mode.PartBuy) {
                foreach (AvailablePart part in KWAgencies.GetAvailableParts(agency)) {
                    DialogGUIBase boxOrBtn;
                    if (Funding.Instance.Funds >= KWAgencies.PricePart(part))
                        boxOrBtn = new DialogGUIButton($"{KWUtil.cSprite[Currency.Funds]} <color=#B4D455>{KWAgencies.PricePart(part):N0}</color>",
                        delegate { KWAgencies.OrderPart(part); Popup(agency, mode); }, 80, 32, true);
                    else boxOrBtn = new DialogGUIBox($"{KWUtil.cSprite[Currency.Funds]} <color=orange>{KWAgencies.PricePart(part):N0}</color>", 80, 32);
                    box.Add(new DialogGUIHorizontalLayout(
                        new DialogGUIBox($"<color=white>{part.title}</color>", 182, 32), new DialogGUIBox(KWAgencies.GetPlayer().GetPartQty(part) + "", 32, 32), boxOrBtn));
                }
            } else if (mode == Mode.JunkBuy) {
                foreach (var pair in KWAgencies.JunkParts()) {
                    DialogGUIBase boxOrBtn;
                    if (Funding.Instance.Funds >= KWAgencies.PriceJunk(pair.Key))
                        boxOrBtn = new DialogGUIButton($"{KWUtil.cSprite[Currency.Funds]} <color=#B4D455>{KWAgencies.PriceJunk(pair.Key):N0}</color>",
                        delegate { KWAgencies.BuyJunk(pair.Key); Popup(agency, mode); }, 80, 32, true);
                    else boxOrBtn = new DialogGUIBox($"{KWUtil.cSprite[Currency.Funds]} <color=orange>{KWAgencies.PriceJunk(pair.Key):N0}</color>", 80, 32);
                    box.Add(new DialogGUIHorizontalLayout(new DialogGUIBox(pair.Key.title, 182, 32), new DialogGUIBox(pair.Value + "", 32, 32), boxOrBtn));
                }
            } else if (mode == Mode.JunkSell) {
                foreach (var pair in KWAgencies.GetPlayer().StoredParts())
                    box.Add(new DialogGUIHorizontalLayout(
                    new DialogGUIBox(pair.Key.title, 182, 32),
                    new DialogGUIBox(pair.Value + "", 32, 32),
                    new DialogGUIButton($"{KWUtil.cSprite[Currency.Funds]} <color=#B4D455>{KWAgencies.PriceJunk(pair.Key, false):N0}</color>",
                        delegate { KWAgencies.SellJunk(pair.Key); Popup(agency, mode); }, 80, 32, true)));
            } else if (mode == Mode.OutSrc) {
                box.Add(new DialogGUIBox($"<color=#D4F4FD>{Localizer.Format("#autoLOC_501159")}</color>", 303, 140));
                box.Add(ModeNavBtn(agency, Mode.OpenSrc, KWAgencies.PlayerCanOutSrc(agency)));
                box.Add(ModeNavBtn(agency, Mode.Patents, KWAgencies.PlayerCanOutSrc(agency)));
                box.AddRange(LabStatusBox(agency));
            } else if (mode == Mode.AppCmpn) {
                bool isActive = agency.StratActive(mode);
                box.AddRange(new DialogGUIBase[] {
                    new DialogGUIBox($"<color=#D4F4FD>{Localizer.Format("#autoLOC_501151")}</color>", 303, 110),
                    new DialogGUIBox($"{Localizer.Format("#autoLOC_6001074")} {Localizer.Format("#autoLOC_901039")}:\n\n<color=#B4D455>{Localizer.Format("#autoLOC_8005414")} " +
                        $"{KWUtil.cSprite[Currency.Funds]} {2500:N0} {Localizer.Format(Currency.Funds.Description())}</color>\n\n<color=#E0D503>" + 
                        Localizer.Format("#autoLOC_303661", $"{KWUtil.cSprite[Currency.Reputation]} {Localizer.Format(Currency.Reputation.Description())}" +
                        $"</color>\n\n<color=#B4D455>", $"{KWUtil.cSprite[Currency.Funds]} {2500:N0}",
                        $"{Localizer.Format("#autoLOC_900523")} {Localizer.Format("#autoLOC_223622")}") + $"</color>\n\n<color=#E0D503><i>({Localizer.Format("#autoLOC_900348")} " +
                        $"{KWUtil.cSprite[Currency.Reputation]} {40:N0} {Localizer.Format(Currency.Reputation.Description())} " +
                        $"{Localizer.Format("#autoLOC_502101")})</i></color>", 303, 166),
                    new DialogGUIToggleButton(isActive, $"{Localizer.Format(isActive ? "#autoLOC_6001957" : "#autoLOC_6001485")} {Localizer.Format(mode.Description())}",
                        delegate { agency.ToggleStrat(mode); Popup(agency, mode); }, 302, 32)});
            } else if (mode == Mode.IPSell) {
                box.Add(new DialogGUIBox($"<color=#D4F4FD>{Localizer.Format("#autoLOC_501169")}</color>", 303, 140));

            } else if (mode == Mode.Negs) {
                // box.Add(new DialogGUIBox($"<color=#D4F4FD>{Localizer.Format("#autoLOC_501163")}</color>", 303, 140));
                bool isActive = agency.StratActive(mode);
                box.AddRange(new DialogGUIBase[] {
                    new DialogGUIBox($"<color=#D4F4FD>{Localizer.Format("#autoLOC_501163")}</color>", 303, 110),
                    new DialogGUIBox($"{Localizer.Format("#autoLOC_6001074")} {Localizer.Format("#autoLOC_901039")}:\n\n<color=#B4D455>{Localizer.Format("#autoLOC_8005414")} " +
                        $"{KWUtil.cSprite[Currency.Funds]} {2500:N0} {Localizer.Format(Currency.Funds.Description())}</color>\n\n<color=#E0D503>" +
                        Localizer.Format("#autoLOC_303661", $"{KWUtil.cSprite[Currency.Reputation]} {Localizer.Format(Currency.Reputation.Description())}" +
                        $"</color>\n\n<color=#B4D455>", $"{KWUtil.cSprite[Currency.Funds]} {2500:N0}",
                        $"{Localizer.Format("#autoLOC_900523")} {Localizer.Format("#autoLOC_223622")}") + $"</color>\n\n<color=#E0D503><i>({Localizer.Format("#autoLOC_900348")} " +
                        $"{KWUtil.cSprite[Currency.Reputation]} {40:N0} {Localizer.Format(Currency.Reputation.Description())} " +
                        $"{Localizer.Format("#autoLOC_502101")})</i></color>", 303, 166),
                    new DialogGUIToggleButton(isActive, $"{Localizer.Format(isActive ? "#autoLOC_6001957" : "#autoLOC_6001485")} {Localizer.Format(mode.Description())}",
                        delegate { agency.ToggleStrat(mode); Popup(agency, mode); }, 302, 32)});
            } else if (mode == Mode.Desc) {
                box.Add(new DialogGUIBox($"<color=#D4F4FD>{agency.Agent().Description}</color>", 303, 317));
            } else if (mode == Mode.FRaise) {
                bool isActive = agency.StratActive(mode);
                box.AddRange(new DialogGUIBase[] {
                    new DialogGUIBox($"<color=#D4F4FD>{Localizer.Format("#autoLOC_501153")}</color>", 303, 120),
                    new DialogGUIBox($"{Localizer.Format("#autoLOC_6001074")} {Localizer.Format("#autoLOC_901039")}:\n\n<color=#E0D503>{Localizer.Format("#autoLOC_8005414")} " +
                        $"{KWUtil.cSprite[Currency.Reputation]} {5:N0} {Localizer.Format(Currency.Reputation.Description())}</color>\n\n<color=#B4D455>" +
                        Localizer.Format("#autoLOC_303666", $"{KWUtil.cSprite[Currency.Funds]} {2500:N0} {Localizer.Format(Currency.Funds.Description())}\n\n",
                        $"+ {KWUtil.cSprite[Currency.Funds]} {0.1:N1} {Localizer.Format(Currency.Funds.Description())}",
                        $"{Localizer.Format("#autoLOC_900523")} {Localizer.Format("#autoLOC_223622")}") + $"\n\n<i>({Localizer.Format("#autoLOC_900348")} " +
                        $"{KWUtil.cSprite[Currency.Funds]} {25000:N0} {Localizer.Format(Currency.Funds.Description())} " +
                        $"{Localizer.Format("#autoLOC_502101")})</i></color>", 303, 156),
                    new DialogGUIToggleButton(isActive, $"{Localizer.Format(isActive ? "#autoLOC_6001957" : "#autoLOC_6001485")} {Localizer.Format(mode.Description())}",
                        delegate { agency.ToggleStrat(mode); Popup(agency, mode); }, 302, 32)});
            } else if (mode == Mode.Lead) {
                bool isActive = agency.StratActive(mode);
                box.AddRange(new DialogGUIBase[] {
                    new DialogGUIBox($"<color=#D4F4FD>{Localizer.Format("#autoLOC_501171")}</color>", 303, 140),
                    new DialogGUIBox($"<color=#E0D503>{Localizer.Format("#autoLOC_8004446")} {KWUtil.cSprite[Currency.Reputation]} " +
                        $"{Localizer.Format(Currency.Reputation.Description())} {Localizer.Format("#autoLOC_502109")}</color>\n\n<color=#B4D455>" + 
                        Localizer.Format("#autoLOC_303660", $"{15:N0}", $"{KWUtil.cSprite[Currency.Funds]} {Localizer.Format(Currency.Funds.Description())}", "#autoLOC_502111") + 
                        "</color>\n\n<color=white>" + Localizer.Format("#autoLOC_303660", $"{50:N0}", "#KWLOC_standing", "#autoLOC_502111") + "</color>", 303, 136),
                    new DialogGUIToggleButton(isActive, $"{Localizer.Format(isActive ? "#autoLOC_6001957" : "#autoLOC_6001485")} {Localizer.Format(mode.Description())}",
                        delegate { agency.ToggleStrat(mode); Popup(agency, mode); }, 302, 32)});
            } else if (mode == Mode.OpenSrc) {
                box.Add(new DialogGUIBox($"<color=#D4F4FD>{Localizer.Format("#autoLOC_501155")}</color>", 303, 140));
                foreach (var pair1 in KWAgencies.GetPlayer().Sci.Where(p1 => p1.Value.Any(p2 => p2.Value != 0f))) {
                    box.Add(new DialogGUIBox(pair1.Key, 303, 32));
                    foreach (var pair2 in pair1.Value.Where(p => p.Value != 0))
                        box.Add(new DialogGUIHorizontalLayout(
                            new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.sciIcons[pair2.Key]),
                            new DialogGUIBox($"{pair2.Value:N1}", 32, 32),
                            new DialogGUIButton("25%", delegate { }, 32, 32, true),
                            new DialogGUIButton("50%", delegate { }, 32, 32, true),
                            new DialogGUIButton("100%", delegate { }, 32, 32, true))); }
            } else if (mode == Mode.Patents) {
                box.Add(new DialogGUIBox($"<color=#D4F4FD>{Localizer.Format("#autoLOC_501161")}</color>", 303, 140));

            } else if (mode == Mode.Bail) {
                bool fundsLow = Funding.Instance.Funds < 0;
                float repLoss = fundsLow ? (HighLogic.CurrentGame.Parameters.Career.StartingFunds - (float)Funding.Instance.Funds) / 500 : 0;
                box.AddRange(new DialogGUIBase[] {
                    new DialogGUIBox($"<color=#D4F4FD>{Localizer.Format("#autoLOC_501167")}</color>", 303, 140),
                    new DialogGUIBox($"<color={(fundsLow ? "green>" : "red>") + Localizer.Format("#autoLOC_244332")} {Localizer.Format(Currency.Funds.Description())} " +
                    $"{Localizer.Format("#autoLOC_7003013", 0)}\n\n</color><color=#B4D455>" + Localizer.Format("#autoLOC_303952", Currency.Funds.Description(), "#autoLOC_8005038",
                        $": {KWUtil.cSprite[Currency.Funds]} {HighLogic.CurrentGame.Parameters.Career.StartingFunds:N0}</color>") + "\n\n<color=#E0D503>" +
                        Localizer.Format("#autoLOC_303660", $"{KWUtil.cSprite[Currency.Reputation]} {repLoss:N1}",
                        Currency.Reputation.Description(), "</color>").Replace("%",""), 303, 136)});
                if (fundsLow)
                    box.Add(new DialogGUIButton($"{Localizer.Format("#autoLOC_900384")} {Localizer.Format("#autoLOC_501166")}", delegate {
                        Funding.Instance.SetFunds(HighLogic.CurrentGame.Parameters.Career.StartingFunds, TransactionReasons.Strategies);
                        Reputation.Instance.SetReputation(Reputation.CurrentRep - repLoss, TransactionReasons.Strategies); Popup(agency); }, 303, 32, true));
                else box.Add(new DialogGUIBox($"<color=red>{Localizer.Format("#autoLOC_501166")} {Localizer.Format("#autoLOC_260121")}</color>", 303, 32));
            } else if (mode == Mode.Interns) {
                bool isActive = agency.StratActive(mode);
                bool canActivate = Reputation.CurrentRep > 0;
                box.AddRange(new DialogGUIBase[] {
                    new DialogGUIBox($"<color=#D4F4FD>{Localizer.Format("#autoLOC_501157")}</color>", 303, 140),
                    new DialogGUIBox($"<color={(canActivate ? "green>" : "red>") + Localizer.Format("#autoLOC_244332")} {KWUtil.cSprite[Currency.Reputation]} " +
                    $"{Localizer.Format(Currency.Reputation.Description())} {Localizer.Format("#autoLOC_7003012", 0)}\n\n<color=#6DCFF6>" + Localizer.Format("#autoLOC_303666",
                        "+0.1% " + KWUtil.cSprite[Currency.Science], Localizer.Format(Currency.Science.Description()) + "\n\n</color><color=#E0D503>",
                        $"{KWUtil.cSprite[Currency.Reputation]} {Localizer.Format(Currency.Reputation.Description())}") + "</color>", 303, 136)});
                if (canActivate)
                    box.Add(new DialogGUIToggleButton(isActive, $"{Localizer.Format(isActive ? "#autoLOC_6001957" : "#autoLOC_6001485")} {Localizer.Format(mode.Description())}",
                        delegate { agency.ToggleStrat(mode); Popup(agency, mode); }, 302, 32));
                else box.Add(new DialogGUIBox($"<color=red>{Localizer.Format(mode.Description())} {Localizer.Format("#autoLOC_260121")}</color>", 303, 32));
            } else if (mode == Mode.Take) {
                // $"{(Funding.Instance.Funds < agency.Value ? Localizer.Format("#autoLOC_7003246") : "")}"
                // Funding.Instance.Funds >= agency.Value
                // bool storageEmpty = KWAgencies.Player() == null || (!KWAgencies.Player()?.StoredParts().Values.Any(i => i > 0) ?? true);
                // bool canTakeover = Funding.Instance.Funds >= agency.Value && Reputation.CurrentRep >= agency.Rep;
                //(!KWUtil.CareerOpts().takeoverBids && KWAgencies.IsRnD(agency)) || (
                //&& ResearchAndDevelopment.Instance.Science == 0 && storageEmpty && FlightGlobals.Vessels.Count == 0 && KWUtil.CareerOpts().takeoverBids);
                box.AddRange(new DialogGUIBase[] {
                    new DialogGUIBox($"<color=#D4F4FD>{Localizer.Format("#KWLOC_lead_desc", agency.Agent().Title)}</color>", 303, 110), // 70
                    new DialogGUIBox($"<color=white>{agency.Agent().Title}</color>\n\n<color=#B4D455>" +
                        $"{Localizer.Format("#autoLOC_900729")}: {KWUtil.cSprite[Currency.Funds]} {agency.Value:N0}</color>\n<color=#E0D503>" +
                        $"{Localizer.Format("#autoLOC_464661", $"{KWUtil.cSprite[Currency.Reputation]} {agency.Rep:N0}").Replace("%","")}</color>\n\n<color=" +
                        $"{(agency.CanTakeover() ? "green>" : "red>") + Localizer.Format("#autoLOC_211272")}</color>\n\n<color={(Funding.Instance.Funds >= agency.Value ? "#B4D455>" : "red>")}" +
                        $"{Localizer.Format("#autoLOC_419441", $"{KWUtil.cSprite[Currency.Funds]} {Funding.Instance.Funds:N0}")}</color>\n<color=" +
                        (Reputation.CurrentRep >= agency.Rep ? "#E0D503>" : "red>") + Localizer.Format("#autoLOC_464661", KWUtil.cSprite[Currency.Reputation] +
                        $" {Reputation.CurrentRep:N0}").Replace("%","") + "</color>", 303, 166) }); // 206
                //  +$"</color>\n<color={(ResearchAndDevelopment.Instance.Science == 0 ? "#6DCFF6>" : "red>")}" +
                //  Localizer.Format("#autoLOC_419420", $"<sprite=\"CurrencySpriteAsset\" name=\"Science\" color=#6DCFF6> {ResearchAndDevelopment.Instance.Science:N0}") +
                //  $"</color>\n<color={(storageEmpty ? "#D4F4FD>" : "red>") + Localizer.Format("#KWLOC_partsStorage")}: " + (storageEmpty ? Localizer.Format("#autoLOC_6002404") :
                //  Localizer.Format("#autoLOC_244028")) + "</color>\n<color=" + (FlightGlobals.Vessels.Count == 0 ? "#D4F4FD>" : "red>") + Localizer.Format("#autoLOC_8003014",
                //  FlightGlobals.Vessels.Count == 0 ? Localizer.Format("#autoLOC_6003000") : Localizer.Format("#autoLOC_145786"))
                // + (canTakeover ? $"\n\n<color=green>{Localizer.Format("#KWLOC_takeoverBid")} {Localizer.Format("#autoLOC_238176")}</color>" : "")
                if (agency.CanTakeover())
                    box.Add(new DialogGUIButton($"{Localizer.Format("#autoLOC_900523")} {Localizer.Format(Mode.Take.Description())}", // KWUtil.CareerOpts().takeoverBids ? 
                        delegate { agency.Takeover(); agencyBtn.SetTexture(KWAgencies.GetPlayer().Agent().Logo); agencyBtn.SetFalse(false); }, 303, 32, true)); // $"{Localizer.Format("#autoLOC_6001485")} {Localizer.Format(Mode.Take.Description())}"
                else box.Add(new DialogGUIBox($"<color=red>{Localizer.Format(Mode.Take.Description())} " +
                    Localizer.Format("#autoLOC_234994", Localizer.Format("#autoLOC_7003010").Replace(":", "")) + "</color>", 303, 32));
            } else if (mode == Mode.PrsOrd) {
                List<AvailablePart> parts = EditorLogic.fetch.ship.Parts.Select(p => p.partInfo).ToList();
                int totalCost = 0;
                foreach (AvailablePart part in parts.Distinct()) {
                    int count = parts.Count(p => p == part) - KWAgencies.GetPlayer().GetPartQty(part);
                    if (count <= 0) continue;
                    totalCost += count * part.entryCost;
                    box.Add(new DialogGUIHorizontalLayout(
                        new DialogGUIBox($"<color=white>{part.title}</color>", 182, 32),
                        new DialogGUIBox(count + "", 32, 32),
                        new DialogGUIBox($"{KWUtil.cSprite[Currency.Funds]} <color=#B4D455>{count * part.entryCost:N0}</color>", 80, 32))); }
                string colour = totalCost + EditorLogic.fetch.ship.GetShipCosts(out float _, out float __) > Funding.Instance.Funds ? "yellow" : "green";
                box.Add(new DialogGUIHorizontalLayout(
                    new DialogGUIBox($"<color={colour}>{Localizer.Format(Mode.PrsOrd.Description())} {Localizer.Format("#autoLOC_8100136")}</color>", 218, 32),
                    new DialogGUIBox($"{KWUtil.cSprite[Currency.Funds]} <color={colour}>{totalCost:N0}</color>", 80, 32)));
            } else if (mode == Mode.Train) {
                DialogGUIBase boxOrBtn = new DialogGUIBase();
                ProtoCrewMember hero = KWUtil.GetHero();
                bool available = hero.rosterStatus == ProtoCrewMember.RosterStatus.Available;
                bool hasCourage = (int)Math.Round(hero.courage * 100) >= new int[] { 1, 5, 12, 24, 48, 88 }[hero.experienceLevel];
                bool notStupid = hero.stupidity < new float[] { 0.95f, 0.85f, 0.7f, 0.5f, 0.25f, 0.1f }[hero.experienceLevel];
                bool hasFunds = Funding.Instance.Funds >= (hero.experienceLevel + 1) * 15000;
                box.Add(new DialogGUIBox($"<color=white>{KWUtil.GetHero().displayName}</color>\n\n" +
                    $"<color=#D4F4FD>{Localizer.Format("#autoLOC_900431")}: {hero.GetLocalizedTrait()}\n" +
                    $"{Localizer.Format("#autoLOC_900345")}: {hero.experienceLevel}</color>\n\n" +
                    $"<color={(available ? "green>" : "red>")}{Localizer.Format("#autoLOC_475347")} {Localizer.Format(hero.rosterStatus.Description())}</color>\n" +
                    $"<color={(hasCourage ? "green>" : "red>")}{Localizer.Format("#autoLOC_900297")}: {hero.courage:P1}</color>\n" +
                    $"<color={(notStupid ? "green>" : "red>")}{Localizer.Format("#autoLOC_900298")}: {hero.stupidity:P1}</color>\n\n" +
                    $"<color={(hasFunds ? "#B4D455>" : "red>")}{Localizer.Format("#autoLOC_1900256")} {Localizer.Format("#autoLOC_223622")}: " +
                    $"<sprite=\"CurrencySpriteAsset\" name=\"Funds\" color=#B4D455> {(hero.experienceLevel + 1) * 15000:N0}</color>", 303, 207));
                if (available && notStupid && hasFunds && hero.trait != KerbalRoster.engineerTrait)
                    boxOrBtn = new DialogGUIButton($"{Localizer.Format("#autoLOC_501020")} {Localizer.Format("#autoLOC_1900256")}", delegate {
                        Funding.Instance.AddFunds((hero.experienceLevel + 1) * -15000, TransactionReasons.None);
                        KerbalRoster.SetExperienceTrait(hero, KerbalRoster.engineerTrait);
                        kerbBtn.SetFalse(false);
                    }, 180, 32, true);
                else boxOrBtn = new DialogGUIBox($"<color=red>{Localizer.Format("#autoLOC_1900256")} {Localizer.Format("#autoLOC_190802")}</color>", 180, 32);
                box.Add(new DialogGUIHorizontalLayout(new DialogGUIBox($"<color=white>{Localizer.Format("#autoLOC_500103")}</color>", 118, 32), boxOrBtn));
                if (available && hasCourage && hasFunds && hero.trait != KerbalRoster.pilotTrait)
                    boxOrBtn = new DialogGUIButton($"{Localizer.Format("#autoLOC_501020")} {Localizer.Format("#autoLOC_1900256")}", delegate {
                        Funding.Instance.AddFunds((hero.experienceLevel + 1) * -15000, TransactionReasons.None);
                        KerbalRoster.SetExperienceTrait(hero, KerbalRoster.pilotTrait);
                        kerbBtn.SetFalse(false);
                    }, 180, 32, true);
                else boxOrBtn = new DialogGUIBox($"<color=red>{Localizer.Format("#autoLOC_1900256")} {Localizer.Format("#autoLOC_190802")}</color>", 180, 32);
                box.Add(new DialogGUIHorizontalLayout(new DialogGUIBox($"<color=white>{Localizer.Format("#autoLOC_500101")}</color>", 118, 32), boxOrBtn));
                if (available && notStupid && hasFunds && hero.trait != KerbalRoster.scientistTrait)
                    boxOrBtn = new DialogGUIButton($"{Localizer.Format("#autoLOC_501020")} {Localizer.Format("#autoLOC_1900256")}", delegate {
                        Funding.Instance.AddFunds((hero.experienceLevel + 1) * -15000, TransactionReasons.None);
                        KerbalRoster.SetExperienceTrait(hero, KerbalRoster.scientistTrait);
                        kerbBtn.SetFalse(false);
                    }, 180, 32, true);
                else boxOrBtn = new DialogGUIBox($"<color=red>{Localizer.Format("#autoLOC_1900256")} {Localizer.Format("#autoLOC_190802")}</color>", 180, 32);
                box.Add(new DialogGUIHorizontalLayout(new DialogGUIBox($"<color=white>{Localizer.Format("#autoLOC_500105")}</color>", 118, 32), boxOrBtn));
            }
            return new DialogGUIScrollList(Vector2.one, false, true, new DialogGUIVerticalLayout(10, 100, 4, new RectOffset(5, 24, 5, 5), TextAnchor.MiddleLeft, box.ToArray()));
        }
        private void Popup(Agency agency = null, Mode mode = Mode.Main) { // , ListMode lMode = ListMode.All
            if (agency == null) agency = KWAgencies.GetPlayer() ?? KWAgencies.GetRnD();
            if (KWAgencies.GetPlayer() == null && mode == Mode.Main) mode = Mode.Desc;
            DialogGUIBase[] content = new DialogGUIBase[] {
                new DialogGUIHorizontalLayout(
                    new DialogGUIButton(mode == Mode.Main ? "^" : "<",
                        delegate { Popup(mode == Mode.Main ? (KWAgencies.GetPlayer() ?? KWAgencies.AList.Find(a => a.IsRnD())) : agency, Mode.Main); }, 32, 32, true),
                    new DialogGUIToggleButton(true, $"<i>{agency.Agent().Title}</i>", delegate { Dismiss(); Popup(agency, Mode.Desc); }, 256, 32), //  mainMode ? 292 : 
                    new DialogGUIButton("x", delegate { agencyBtn.SetFalse(false); }, 32, 32, true)),
                // TitleBar(agency, mode == Mode.Main),
                new DialogGUIBox("", 330, 3),
                new DialogGUIImage(new Vector2(330, 165), new Vector2(0, 0), Color.white, agency.Agent().Logo),
                new DialogGUIBox("", 330, 3),
                new DialogGUIHorizontalLayout(
                    new DialogGUIBox($"<sprite=\"CurrencySpriteAsset\" name=\"Funds\" color=#B4D455> <color=#B4D455>{agency.Value:N0}</color>", 85, 32),
                    new DialogGUIBox($"<sprite=\"CurrencySpriteAsset\" name=\"Reputation\" color=#E0D503> <color=#E0D503>{agency.Rep:N0}</color>", 50, 32),
                    new DialogGUIBox(KWAgencies.GetPlayer() == null || agency.IsPlayer() ? Localizer.Format("#KWLOC_leader", agency.Leader) : KWAgencies.GetStandingDesc(agency), 185, 32)),
                NavBar(agency, mode),
                ScrollBox(agency, mode) };
            PopupDialog.SpawnPopupDialog(new MultiOptionDialog("KWPopup", "", "", HighLogic.UISkin, new Rect(0.88f, 0.35f, 340, 636), content), false, HighLogic.UISkin); // 0.5f - content.Count / 58f
        }
        private void Dismiss() => PopupDialog.DismissPopup("KWPopup");
        private void CrewTrainingPopup() {
            List<DialogGUIBase> box = new List<DialogGUIBase> {
                new DialogGUIHorizontalLayout(
                    new DialogGUIBox($"<i>{Localizer.Format("#autoLOC_900174")} {Localizer.Format("#autoLOC_1900256")}</i>", 266, 32),
                    new DialogGUIButton("x", delegate { }, 32, 32, true)) };
            DialogGUIBase boxOrBtn = new DialogGUIBase();
            ProtoCrewMember hero = KWUtil.GetHero();
            bool available = hero.rosterStatus == ProtoCrewMember.RosterStatus.Available;
            bool hasCourage = (int)Math.Round(hero.courage * 100) >= new int[] { 1, 5, 12, 24, 48, 88 }[hero.experienceLevel];
            bool notStupid = hero.stupidity < new float[] { 0.95f, 0.85f, 0.7f, 0.5f, 0.25f, 0.1f }[hero.experienceLevel];
            bool hasFunds = Funding.Instance.Funds >= (hero.experienceLevel + 1) * 15000;
            box.Add(new DialogGUIBox($"<color=white>{KWUtil.GetHero().displayName}</color>\n\n" +
                $"<color=#D4F4FD>{Localizer.Format("#autoLOC_900431")}: {hero.GetLocalizedTrait()}\n" +
                $"{Localizer.Format("#autoLOC_900345")}: {hero.experienceLevel}</color>\n\n" +
                $"<color={(available ? "green>" : "red>")}{Localizer.Format("#autoLOC_475347")} {Localizer.Format(hero.rosterStatus.Description())}</color>\n" +
                $"<color={(hasCourage ? "green>" : "red>")}{Localizer.Format("#autoLOC_900297")}: {hero.courage:P1}</color>\n" +
                $"<color={(notStupid ? "green>" : "red>")}{Localizer.Format("#autoLOC_900298")}: {hero.stupidity:P1}</color>\n\n" +
                $"<color={(hasFunds ? "#B4D455>" : "red>")}{Localizer.Format("#autoLOC_1900256")} {Localizer.Format("#autoLOC_223622")}: " +
                $"<sprite=\"CurrencySpriteAsset\" name=\"Funds\" color=#B4D455> {(hero.experienceLevel + 1) * 15000:N0}</color>", 303, 170));
            if (available && notStupid && hasFunds && hero.trait != KerbalRoster.engineerTrait)
                boxOrBtn = new DialogGUIButton($"{Localizer.Format("#autoLOC_501020")} {Localizer.Format("#autoLOC_1900256")}", delegate {
                    Funding.Instance.AddFunds((hero.experienceLevel + 1) * -15000, TransactionReasons.None);
                    KerbalRoster.SetExperienceTrait(hero, KerbalRoster.engineerTrait);
                }, 180, 32, true);
            else boxOrBtn = new DialogGUIBox($"<color=red>{Localizer.Format("#autoLOC_1900256")} {Localizer.Format("#autoLOC_190802")}</color>", 180, 32);
            box.Add(new DialogGUIHorizontalLayout(new DialogGUIBox(Localizer.Format("#autoLOC_500103"), 118, 32), boxOrBtn));
            if (available && hasCourage && hasFunds && hero.trait != KerbalRoster.pilotTrait)
                boxOrBtn = new DialogGUIButton($"{Localizer.Format("#autoLOC_501020")} {Localizer.Format("#autoLOC_1900256")}", delegate {
                    Funding.Instance.AddFunds((hero.experienceLevel + 1) * -15000, TransactionReasons.None);
                    KerbalRoster.SetExperienceTrait(hero, KerbalRoster.pilotTrait);
                }, 180, 32, true);
            else boxOrBtn = new DialogGUIBox($"<color=red>{Localizer.Format("#autoLOC_1900256")} {Localizer.Format("#autoLOC_190802")}</color>", 180, 32);
            box.Add(new DialogGUIHorizontalLayout(new DialogGUIBox(Localizer.Format("#autoLOC_500101"), 118, 32), boxOrBtn));
            if (available && notStupid && hasFunds && hero.trait != KerbalRoster.scientistTrait)
                boxOrBtn = new DialogGUIButton($"{Localizer.Format("#autoLOC_501020")} {Localizer.Format("#autoLOC_1900256")}", delegate {
                    Funding.Instance.AddFunds((hero.experienceLevel + 1) * -15000, TransactionReasons.None);
                    KerbalRoster.SetExperienceTrait(hero, KerbalRoster.scientistTrait);
                }, 180, 32, true);
            else boxOrBtn = new DialogGUIBox($"<color=red>{Localizer.Format("#autoLOC_1900256")} {Localizer.Format("#autoLOC_190802")}</color>", 180, 32);
            box.Add(new DialogGUIHorizontalLayout(new DialogGUIBox(Localizer.Format("#autoLOC_500105"), 118, 32), boxOrBtn));
            PopupDialog.SpawnPopupDialog(new MultiOptionDialog("KWCrewTrainPopup", "", "", HighLogic.UISkin, new Rect(0.5f, 0.5f, 313, 1), box.ToArray()), false, HighLogic.UISkin);
        }
        //internal static void AdminPopup() {
        //    List<DialogGUIBase> content = new List<DialogGUIBase> {
        //    new DialogGUIHorizontalLayout(
        //        new DialogGUIToggleButton(true, "<i>Donors</i>", delegate { }, 392, 32),
        //        new DialogGUIButton("x", delegate { }, 32, 32, true)) };

        //    content.Add(new DialogGUIHorizontalLayout(
        //        new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.adminIcons[ProgType.PhysSci]),
        //        new DialogGUIBox(Localizer.Format(ProgType.PhysSci.Description()), 103, 32),
        //        new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.adminIcons[ProgType.Pilots]),
        //        new DialogGUIBox(Localizer.Format(ProgType.Crew.Description()), 103, 32),
        //        new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.adminIcons[ProgType.Robotics]),
        //        new DialogGUIBox(Localizer.Format(ProgType.Robotics.Description()), 103, 32)));
            
        //    content.Add(new DialogGUIHorizontalLayout(
        //        new DialogGUIVerticalLayout(KWAdmin.GetLobbyProgs(ProgType.PhysSci).Select(a => 
        //            new DialogGUIHorizontalLayout(
        //                new DialogGUIImage(new Vector2(64, 40), new Vector2(0, 0), Color.white, a.Agent().LogoScaled),
        //                new DialogGUIBox("<sprite=\"CurrencySpriteAsset\" name=\"Funds\" color=#B4D455> <color=#B4D455>1,000,000</color>", 75, 40))).ToArray()),
        //        new DialogGUIVerticalLayout(KWAdmin.GetLobbyProgs(ProgType.Pilots).Select(a =>
        //            new DialogGUIHorizontalLayout(
        //                new DialogGUIImage(new Vector2(64, 40), new Vector2(0, 0), Color.white, a.Agent().LogoScaled),
        //                new DialogGUIBox("0", 75, 40))).ToArray()),
        //        new DialogGUIVerticalLayout(KWAdmin.GetLobbyProgs(ProgType.Robotics).Select(a =>
        //            new DialogGUIHorizontalLayout(
        //                new DialogGUIImage(new Vector2(64, 40), new Vector2(0, 0), Color.white, a.Agent().LogoScaled),
        //                new DialogGUIBox("0", 75, 40))).ToArray())));

        //    PopupDialog.SpawnPopupDialog(new MultiOptionDialog("KWAdminPopup", "", "", HighLogic.UISkin, new Rect(0.5f, 0.5f, 440, 340), content.ToArray()), false, HighLogic.UISkin);
        //}
        //private void EditorPopup() {
        //    List<DialogGUIBase> content = new List<DialogGUIBase> {
        //        new DialogGUIHorizontalLayout(
        //            new DialogGUIToggleButton(true, Localizer.Format("#KWLOC_purchaseOrder"), delegate { Dismiss(); Popup(); }, 262, 32),
        //            new DialogGUIButton("x", delegate { }, 32, 32, true)),
        //        new DialogGUIHorizontalLayout(
        //            new DialogGUIBox($"{Localizer.Format("#autoLOC_6001017")} {Localizer.Format("#autoLOC_901054")}", 178, 32),
        //            new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/number3", false)),
        //            new DialogGUIBox(Localizer.Format("#autoLOC_223622"), 80, 32)) };
        //    List<AvailablePart> parts = EditorLogic.fetch.ship.Parts.Select(p => p.partInfo).ToList();
        //    int totalCost = 0;
        //    foreach (AvailablePart part in parts.Distinct()) {
        //        int count = parts.Count(p => p == part) - KWAgencies.Player().GetPartQty(part);
        //        if (count <= 0) continue;
        //        totalCost += count * part.entryCost;
        //        content.Add(new DialogGUIHorizontalLayout(
        //            new DialogGUIBox($"<color=white>{part.title}</color>", 178, 32),
        //            new DialogGUIBox(count + "", 32, 32),
        //            new DialogGUIBox($"<sprite=\"CurrencySpriteAsset\" name=\"Funds\" color=#B4D455> <color=#B4D455>{count * part.entryCost:N0}</color>", 80, 32))); }
        //    content.Add(new DialogGUIHorizontalLayout(
        //        new DialogGUIBox($"<color={(totalCost > Funding.Instance.Funds ? "yellow" : "green")}>{Localizer.Format("#KWLOC_total")}</color>", 214, 32),
        //        new DialogGUIBox($"<sprite=\"CurrencySpriteAsset\" name=\"Funds\" color=#B4D455> <color=#B4D455>{totalCost:N0}</color>", 80, 32)));
        //    PopupDialog.SpawnPopupDialog(new MultiOptionDialog("KWPopup", "", "",
        //        HighLogic.UISkin, new Rect(0.8f, 0.5f - content.Count / 58f, 310, 1), content.ToArray()), false, HighLogic.UISkin);
        //}
        //public static void GameOverPopup() => PopupDialog.SpawnPopupDialog(
        //    new MultiOptionDialog("GameOverPopup", "", Localizer.Format("#autoLOC_8014052"), HighLogic.UISkin, new Rect(0.5f, 0.5f, 250, 1),
        //        new DialogGUIBase[2] { // PauseMenu / QuickSaveLoadDialog
        //            new DialogGUIButton(Localizer.Format("#autoLOC_360553"), delegate {
        //                LoadGameDialog.Create(new LoadGameDialog.FinishedCallback(delegate { }), HighLogic.SaveFolder, false, UISkinManager.GetSkin("MainMenuSkin")); }, true),
        //            new DialogGUIButton(Localizer.Format("#autoLOC_360837"), delegate { HighLogic.LoadScene(GameScenes.MAINMENU); }, true)
        //        }), false, HighLogic.UISkin);

    }
    
}