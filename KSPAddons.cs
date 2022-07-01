
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
            if (KWUtil.FlightScene()) {
                // GameEvents.onAttemptEva.Add(AttemptEVA);
                // GameEvents.onCrewBoardVessel.Add(BoardVessel);
                GameEvents.OnExperimentDeployed.Add(DeployExperiment);
                //    GameEvents.onVesselCrewWasModified.Add(ModifiedVesselCrew);
                //    GameEvents.onCrewOnEva.Add(CrewEVA);
                //    GameEvents.onCrewTransferred.Add(CrewTransferred);
                //    GameEvents.onPartResourceNonemptyEmpty.Add(EmptyResource);
                //    GameEvents.onPartResourceEmptyNonempty.Add(NonEmptyResource);
                //GameEvents.onTimeWarpRateChanged.Add(ChangeTimeWarpRate);
            }
            if (KWUtil.CareerMode()) {
                if (KWUtil.KSCScene()) {
                    //GameEvents.Contract.onCancelled.Add(CancelContract);
                    //GameEvents.Contract.onDeclined.Add(DeclineContract);
                    // GameEvents.onLevelWasLoaded.Add(LoadLevel);
                    // GameEvents.OnTechnologyResearched.Add(ResearchTech);
                    //GameEvents.onFacilityContextMenuSpawn.Add(SpawnFacilityMenu);
                    //GameEvents.onFacilityContextMenuDespawn.Add(DespawnFacilityMenu);
                    // if (KWAgencies.NewPlayer()) StartNewGame();
                }
                //if (!KWUtil.EditorScene()) {
                //    GameEvents.onKerbalStatusChanged.Add(ChangedKerbalStatus);
                //    GameEvents.onKerbalTypeChanged.Add(ChangedKerbalType);
                //}
                //if (editorScene) {
                //    GameEvents.OnVesselRollout.Add(RolloutVessel);
                //    GameEvents.Contract.onOffered.Add(OfferContract);
                //} else {
                //    GameEvents.Contract.onParameterChange.Add(UpdateContract);
                //    GameEvents.Contract.onCompleted.Add(CompleteContract);
                //    GameEvents.Contract.onFailed.Add(FailContract);
                //    GameEvents.Modifiers.OnCurrencyModifierQuery.Add(QueryCurrency);
                //    GameEvents.OnScienceRecieved.Add(ReceiveScience);
                //    GameEvents.onVesselRecoveryProcessing.Add(RecoverVessel);
                //}
                // GameEvents.Modifiers.OnCurrencyModified.Add(ModifyCurrency);
                //GameEvents.OnFundsChanged.Add(ChangeFunds);
                //GameEvents.OnReputationChanged.Add(ChangeRep);
            }
        }
        public void OnDisable() {
            if (KWUtil.FlightScene()) {
                // GameEvents.onAttemptEva.Remove(AttemptEVA);
                // GameEvents.onCrewBoardVessel.Remove(BoardVessel);
                 GameEvents.OnExperimentDeployed.Remove(DeployExperiment);
                //    GameEvents.onVesselCrewWasModified.Remove(ModifiedVesselCrew);
                //    GameEvents.onCrewOnEva.Remove(CrewEVA);
                //    GameEvents.onCrewTransferred.Remove(CrewTransferred);
                //    GameEvents.onPartResourceNonemptyEmpty.Add(EmptyResource);
                //    GameEvents.onPartResourceEmptyNonempty.Add(NonEmptyResource);
                // GameEvents.onTimeWarpRateChanged.Remove(ChangeTimeWarpRate);
            }
            if (KWUtil.CareerMode()) {
                if (KWUtil.KSCScene()) {
                    //GameEvents.Contract.onCancelled.Remove(CancelContract);
                    //GameEvents.Contract.onDeclined.Remove(DeclineContract);
                    // GameEvents.onLevelWasLoaded.Remove(LoadLevel);
                    // GameEvents.OnTechnologyResearched.Remove(ResearchTech);
                    //GameEvents.onFacilityContextMenuSpawn.Remove(SpawnFacilityMenu);
                    //GameEvents.onFacilityContextMenuDespawn.Remove(DespawnFacilityMenu);
                }
                //if (!KWUtil.EditorScene()) {
                //    GameEvents.onKerbalStatusChanged.Remove(ChangedKerbalStatus);
                //    GameEvents.onKerbalTypeChanged.Remove(ChangedKerbalType);
                //}

                //if (editorScene) {
                //    GameEvents.OnVesselRollout.Remove(RolloutVessel);
                //    GameEvents.Contract.onOffered.Remove(OfferContract);
                //} else {
                //    GameEvents.Contract.onParameterChange.Remove(UpdateContract);
                //    GameEvents.Contract.onCompleted.Remove(CompleteContract);
                //    GameEvents.Contract.onFailed.Remove(FailContract);
                //    GameEvents.Modifiers.OnCurrencyModifierQuery.Remove(QueryCurrency);
                //    GameEvents.OnScienceRecieved.Remove(ReceiveScience);
                //    GameEvents.onVesselRecoveryProcessing.Remove(RecoverVessel);
                //}

                // GameEvents.Modifiers.OnCurrencyModified.Remove(ModifyCurrency);
                //GameEvents.OnFundsChanged.Remove(ChangeFunds);
                //GameEvents.OnReputationChanged.Remove(ChangeRep);

            }
        }
        //private void ChangeTimeWarpRate() {
        //    if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready) {
        //        List<ModuleReactionWheelKW> rws = FlightGlobals.ActiveVessel.FindPartModulesImplementing<ModuleReactionWheelKW>();
        //        rws?.ForEach(r => r.warpTarget = FlightGlobals.ActiveVessel.transform.rotation); }
        //}
        // private void DismissPartUI(Part part) => part.FindModuleImplementing<ModuleKWIgnitors>()?.ToggleUI();

        //private void ChangedKerbalStatus(ProtoCrewMember crew, ProtoCrewMember.RosterStatus _, ProtoCrewMember.RosterStatus status) {
        //    if (crew.name == KWUtil.KerbalOpts().name && (status == ProtoCrewMember.RosterStatus.Dead ||
        //        (status == ProtoCrewMember.RosterStatus.Missing && !HighLogic.CurrentGame.Parameters.Difficulty.MissingCrewsRespawn)))
        //        KWUI.GameOverPopup();
        //}
        //private void ChangedKerbalType(ProtoCrewMember crew, ProtoCrewMember.KerbalType _, ProtoCrewMember.KerbalType type) {
        //    if (crew.name == KWUtil.KerbalOpts().name && type == ProtoCrewMember.KerbalType.Applicant)
        //        KWUI.GameOverPopup();
        //}

        private void DeployExperiment(ScienceData data) {
            if (FlightGlobals.ActiveVessel.isEVA && KWUtil.evaKitConsumerIDs.Contains(data.subjectID.Split('@')[0]))
                FlightGlobals.ActiveVessel.evaController.ModuleInventoryPartReference.RemoveNPartsFromInventory("evaScienceKit", 1, true);
        }
        private void AttemptEVA(ProtoCrewMember crew, Part part, Transform trans) {
            if (part.Resources.Contains("IntakeAir")) {
                if (Math.Abs(KWUtil.GetCabPrskPa(part) - part.vessel.staticPressurekPa) > 5) {
                    FlightEVA.fetch.overrideEVA = true;
                    ScreenMessages.PostScreenMessage($"{Localizer.Format("#autoLOC_252195")}");
                } else KWUtil.OpenCapsule(part); }
        }
        private void BoardVessel(GameEvents.FromToAction<Part, Part> action) {
            if (action.to.Resources.Contains("IntakeAir"))
                KWUtil.OpenCapsule(action.to);
        }
        
        private void ModifiedVesselCrew(Vessel vessel) {
            //List<Part> gooList = vessel.Parts.Where(p => p.partInfo.name == "GooExperiment").ToList();
            //gooList.ForEach(p => p.FindModuleImplementing<ModuleGenerator>().efficiency = Math.Max((float)vessel.GetCrewCount() / gooList.Count, 0.1f));
            
        }

        private void CrewEVA(GameEvents.FromToAction<Part, Part> action) {
            // Debug.Log("crewEVA: " + action.from.partInfo.name + " -> " + action.to.partInfo.name);


        }
        private void CrewTransferred(GameEvents.HostedFromToAction<ProtoCrewMember, Part> action) {
            // Debug.Log("crewtrans: " + action.from.partInfo.name + " -> " + action.to.partInfo.name);

        }

        private void LoadLevel(GameScenes scene) {
            if (scene == GameScenes.FLIGHT) {
                


            }
            StartCoroutine(CallbackUtil.DelayedCallback(1, delegate {
                // if (KWUtil.SCParams().GetType().GetFields().Select(f => (bool)f.GetValue(KWUtil.SCParams())).Any(v => v == false))
                foreach (DestructibleBuilding db in FindObjectsOfType<DestructibleBuilding>()?.Where(d =>
                    Enum.TryParse(d.id.Split('/')[1], out SpaceCenterFacility fc) && KWUtil.IsTechLocked(fc + ""))) {// && fc != SpaceCenterFacility.Administration
                                                                                                                     //!(bool)KWUtil.SCParams().GetType().GetField(KWUtil.fcSCParams[fc + ""]).GetValue(KWUtil.SCParams())))
                    if (KWAgencies.NewPlayer()) {
                        db.GetType().GetField("intact", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(db, false);
                        db.GetType().GetField("destroyed", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(db, true);
                    }
                    foreach (DestructibleBuilding.CollapsibleObject co in db.CollapsibleObjects) {
                        co.collapseObject.SetActive(false);
                        co.replacementObject.SetActive(false);
                    }
                    // co.SetDestroyed(true);
                }
                // 
                if (KWAgencies.NewPlayer()) GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
            }));

        }
        private void EmptyResource(PartResource res) {
            
            
        }
        private void NonEmptyResource(PartResource res) {
            
            
        }

        private void StartNewGame() {

            // new List<string> { "found", "FlagPole", "Runway" }.ForEach(f => KWUtil.UnlockTech(f));

            // KWAdmin.AddFunds(KWUtil.GameOpts().adminStartFunds);
            // foreach (FieldInfo field in KWUtil.SCParams().GetType().GetFields().Where(f => f.Name != "CanLeaveToMainMenu"))
            //      field.SetValue(KWUtil.SCParams(), false);

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
            //while (HighLogic.CurrentGame.CrewRoster.Count > 0)
            //    HighLogic.CurrentGame.CrewRoster.Remove(0);
            //KWKerbalOptions kOpts = HighLogic.CurrentGame.Parameters.CustomParams<KWKerbalOptions>();
            //// KerbalRoster.SetExperienceTrait(kerbal, KerbalRoster.touristTrait);
            //if (kOpts.useSaveName) kOpts.name = $"{HighLogic.SaveFolder} {CrewGenerator.GetLastName()}";
            //HighLogic.CurrentGame.CrewRoster.AddCrewMember(new ProtoCrewMember(ProtoCrewMember.KerbalType.Crew, kOpts.name) {
            //    gender = (ProtoCrewMember.Gender)kOpts.genders.IndexOf(kOpts.gender), courage = kOpts.courage, stupidity = kOpts.stupidity, isBadass = kOpts.badS,
            //    rosterStatus = ProtoCrewMember.RosterStatus.Available, trait = KerbalRoster.touristTrait });
            // if (!KWUtil.CareerOpts().takeoverBids)
                KWAgencies.Takeover(KWAgencies.AList.Find(a => a.Name == "Research & Development Department"));
        }
        //private void ResearchTech(GameEvents.HostTargetAction<RDTech, RDTech.OperationResult> action) {
        //    if (action.target == RDTech.OperationResult.Successful) {
        //        if (HighLogic.CurrentGame.Parameters.Difficulty.BypassEntryPurchaseAfterResearch) KWUtil.LockAllParts();
        //        KWUtil.UnlockParts();
        //    }
        //}

        private void ModifyCurrency(CurrencyModifierQuery query) { }
        private void ChangeFunds(double _, TransactionReasons __) => KWAgencies.UpdatePlayerCurrencies();
        private void ChangeRep(float _, TransactionReasons __) => KWAgencies.UpdatePlayerCurrencies();


        private void DeclineContract(Contract contract) => KWAgencies.AddStanding(contract.Agent, -HighLogic.CurrentGame.Parameters.Career.RepLossDeclined);
        private void QueryCurrency(CurrencyModifierQuery query) {
            if (query.reason == TransactionReasons.ContractDecline) query.AddDelta(Currency.Reputation, -query.GetTotal(Currency.Reputation));
            else if (query.reason == TransactionReasons.Contracts && query.GetTotal(Currency.Reputation) != 0) contractRep = query.GetTotal(Currency.Reputation);
            else if (query.reason == TransactionReasons.VesselRecovery && query.GetTotal(Currency.Funds) > 0) query.AddDelta(Currency.Funds, -query.GetTotal(Currency.Funds));
            else if (query.reason == TransactionReasons.Progression && KWAgencies.Player().StratActive(KWUI.Mode.FRaise)) {
                if (query.GetTotal(Currency.Funds) > 0) query.AddDelta(Currency.Funds, query.GetTotal(Currency.Funds) / 2);
                if (query.GetTotal(Currency.Reputation) > 0) query.AddDelta(Currency.Reputation, -query.GetTotal(Currency.Reputation) * 1.5f);
            } 
        }
        private void UpdateContract(Contract contract, ContractParameter param) => KWAgencies.AddStanding(contract.Agent, contractRep);
        private void CompleteContract(Contract contract) { 
            KWAgencies.AddStanding(contract.Agent, contract.ReputationCompletion);
            KWAgencies.GetAgency(contract.Agent).AddRep(contract.ReputationCompletion);
        }
        private void CancelContract(Contract contract) => KWAgencies.AddStanding(contract.Agent, contract.ReputationFailure);
        private void FailContract(Contract contract) => KWAgencies.AddStanding(contract.Agent, contract.ReputationFailure);

        private void OfferContract(Contract contract) {
            // contract.ScienceCompletion = 0;
            // for (int i = 0; i < contract.ParameterCount; i++) contract.GetParameter(i).ScienceCompletion = 0;
                
        }

        private void ReceiveScience(float amount, ScienceSubject subject, ProtoVessel _, bool __) {
            CelestialBody body = FlightGlobals.Bodies.Find(b => subject.id.Split('@')[1].StartsWith(b.name));
            SciType type = SciType.Misc;
            string id = subject.id.Split('@')[0];
            if (id == "recovery") type = SciType.Milestones;
            else if (id.Contains("Sample")) type = SciType.Samples;
            else if (id.StartsWith("deployed")) type = SciType.Deployed;
            else if (id == "crewReport" || id.StartsWith("eva")) type = SciType.Crew;
            else if (id == "mysteryGoo" || id == "mobileMaterialsLab") type = SciType.Materials;
            else if (id.StartsWith("ROCScience") || id == "atmosphereAnalysis" || id == "infraredTelescope") type = SciType.Scans;
            else if (id == "temperatureScan" || id == "barometerScan" || id == "seismicScan" || id == "gravityScan" || id == "magnetometer") type = SciType.Sensors;
            if (body != null && type != SciType.Misc) KWAgencies.Player().AddSci(body, type, amount);
        }

        private void RolloutVessel(ShipConstruct ship) {
            // KWUtil.CareerOpts().adminFunds += (int)Math.Round(ship.GetShipCosts(out float d, out float f) * 0.5f * (KWUtil.CareerOpts().subsidyMod + 1f));
            foreach (AvailablePart part in ship.Parts.Select(p => p.partInfo)) {
                if (!KWAgencies.Player().RequestPart(part)) KWAgencies.OrderPart(part, false);
                KWAgencies.TryPayAgency(KWAgencies.GetManufacturer(part), part.cost * 0.5f);
                // KWAdmin.AddFunds(part.cost * 0.5f);
            }
        }
        private void RecoverVessel(ProtoVessel vessel, MissionRecoveryDialog dialog, float factor) {
            
            // Funding.Instance.AddFunds(-dialog.totalFunds, TransactionReasons.None);
            float vesselMass = 0f;
            foreach (ProtoPartSnapshot part in vessel.protoPartSnapshots) vesselMass += part.mass;
            double amount = vesselMass * (1.05 - factor) * -10000;
            amount += vessel.GetVesselCrew().Count * (1.05 - factor) * -10000;
            if (vessel.situation == Vessel.Situations.SPLASHED) amount *= 1.25;
            Funding.Instance.AddFunds(amount, TransactionReasons.None);
            ScreenMessages.PostScreenMessage("recovery cost: " + amount);
            Debug.Log("recovery cost: " + amount);
        }
        
    }

    [KSPAddon(KSPAddon.Startup.AllGameScenes, false)]
    public class KWUI : MonoBehaviour {
        // private readonly string[] labConverterIDs = new string[3] { "surfaceSample", "mysteryGoo", "mobileMaterialsLab" };
        private ApplicationLauncherButton agencyBtn;
        private ApplicationLauncherButton editorBtn;
        private object expDialogObj;
        private FieldInfo labBtnField;
        private Button labBtn;

        //private Button recoveryBtn;
        //private object erdInstance;
        //private FieldInfo erdBtnField;
        //private object vmInstance;
        //private FieldInfo vmListField;
        // private int vmCount;
        private bool inRnD;
        // private bool inAC;
        // private ListMode listMode;
        [Flags]
        public enum Mode {
            [Description("#autoLOC_6002637")] Main = 1,
            [Description("#autoLOC_900702")] Desc = 2,
            [Description("#KWLOC_takeoverBid")] Take = 4,
            [Description("#autoLOC_501170")] Lead = 8,
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
            [Description("#KWLOCLoc_junkyardBuy")] JunkBuy = 16384,
            [Description("#KWLOCLoc_junkyardSell")] JunkSell = 32768,
            Parts = PartBrs | PartBuy,
            Junk = JunkBuy | JunkSell
        }
        
        public void Start() {
            if (KWUtil.EditorScene() || KWUtil.FlightScene()) {
                GameEvents.onPartActionUICreate.Add(CreatePAW);
            }
            //if (kscScene) {
            //    vmInstance = typeof(KSCVesselMarkers).GetField("fetch", BindingFlags.Static | BindingFlags.Public).GetValue(null);
            //    vmListField = typeof(KSCVesselMarkers).GetField("markers", BindingFlags.Instance | BindingFlags.NonPublic);
            //}
            if (KWUtil.CareerMode()) {
                if (KWUtil.KSCScene()) {
                    // GameEvents.onGUIAdministrationFacilitySpawn.Add(SpawnAdmin);
                    // GameEvents.onGUIAdministrationFacilityDespawn.Add(DespawnAdmin);
                    // GameEvents.onGUIRnDComplexSpawn.Add(SpawnRnD);
                    // GameEvents.onGUIRnDComplexDespawn.Add(DespawnRnD);
                    //GameEvents.onGUIAstronautComplexSpawn.Add(SpawnAC);
                    //GameEvents.onGUIAstronautComplexDespawn.Add(DespawnAC);
                }
                //GameEvents.onGUIApplicationLauncherReady.Add(AddAppBtns);
                //GameEvents.onGUIApplicationLauncherDestroyed.Add(RemoveAppBtns);
                //    if (editorScene || kscScene)
                //        GameEvents.onTooltipSpawned.Add(SpawnTooltip);
                //    if (flightScene) {
                //        FindObjectOfType<AltimeterSliderButtons>().vesselRecoveryButton.gameObject.SetActive(false);
                //    } else if (trackScene) {
                //        recoveryBtn = SpaceTracking.Instance.RecoverButton;
                //    }
            }

            if (KWUtil.FlightScene()) MusicLogic.SetVolume(GameSettings.MUSIC_VOLUME, GameSettings.AMBIENCE_VOLUME);
            // else if (KWUtil.KSCScene()) MusicLogic.SetVolume(KWUtil.GenOpts().volKSCMus, GameSettings.AMBIENCE_VOLUME);
            else if (KWUtil.TrackScene()) MusicLogic.SetVolume(KWUtil.GenOpts().volTrackMus, GameSettings.AMBIENCE_VOLUME);
            else if (KWUtil.EditorScene()) MusicLogic.SetVolume(KWUtil.GenOpts().volEditorMus, GameSettings.AMBIENCE_VOLUME);
        }
        public void OnDisable() {
            if (KWUtil.EditorScene() || KWUtil.FlightScene()) {
                GameEvents.onPartActionUICreate.Remove(CreatePAW);

            }
            if (KWUtil.CareerMode()) {
                if (KWUtil.KSCScene()) {
                    // GameEvents.onGUIAdministrationFacilitySpawn.Remove(SpawnAdmin);
                    // GameEvents.onGUIAdministrationFacilityDespawn.Remove(DespawnAdmin);
                    // GameEvents.onGUIRnDComplexSpawn.Remove(SpawnRnD);
                    // GameEvents.onGUIRnDComplexDespawn.Remove(DespawnRnD);
                    //GameEvents.onGUIAstronautComplexSpawn.Remove(SpawnAC);
                    //GameEvents.onGUIAstronautComplexDespawn.Remove(DespawnAC);
                }
                //GameEvents.onGUIApplicationLauncherReady.Remove(AddAppBtns);
                //GameEvents.onGUIApplicationLauncherDestroyed.Remove(RemoveAppBtns);
                //RemoveAppBtns();
                //if (editorScene || kscScene)
                //    GameEvents.onTooltipSpawned.Remove(SpawnTooltip);

            }
        }
        public void Update() {
            if (KWUtil.FlightScene()) {
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
            }// else if (KWUtil.KSCScene()) {
                //if (inAC) {
                //    FindObjectOfType<AstronautComplex>().ScrollListApplicants.GetUilistItemAt(4).GetComponent<CrewListItem>().MouseoverEnabled = false;
                //}
            // }

            //if (trackScene && SpaceTracking.Instance.SelectedVessel != null) {
            //    SpaceTracking.Instance.RecoverButton.gameObject.SetActive(SpaceTracking.Instance.SelectedVessel.LandedInStockLaunchSite); 
            //}
            //    //if (trackScene && SpaceTracking.Instance.RecoverButton.gameObject.activeSelf)
            //    //    SpaceTracking.Instance.RecoverButton.gameObject.SetActive(false);
            //if (kscScene) {
            //    List<KSCVesselMarker> vesselMarkers = (List<KSCVesselMarker>)vmListField.GetValue(vmInstance);
            //    //if (vesselMarkers.Count > 0 && vesselMarkers.Count != vmCount) {
            //    //    for (int i = vesselMarkers.Count - 1; i >= 0; i--) {
            //    //        Vessel vessel = (Vessel)vesselMarkers[i].GetType().GetField("v", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(vesselMarkers[i]);
            //    //        if (!vessel.LandedInStockLaunchSite) {
            //    //            vesselMarkers[i].Terminate();
            //    //            vesselMarkers.RemoveAt(i); }}
            //    //    vmListField.SetValue(vmInstance, vesselMarkers);
            //    //    vmCount = vesselMarkers.Count; }
            //    if (vesselMarkers.Count > 0) {
            //        vesselMarkers.ForEach(m => m.Terminate());
            //        vesselMarkers.Clear();
            //        vmListField.SetValue(vmInstance, vesselMarkers); }
            //}
        }
        private void SpawnAdmin() { }
        private void DespawnAdmin() { }
        private void SpawnRnD() { KWUtil.LockAllParts(); inRnD = true; }
        private void DespawnRnD() { KWUtil.UnlockParts(); inRnD = false; }
        // private void SpawnAC() { inAC = true; }
        // private void DespawnAC() { inAC = false; }

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

            //if (part.Modules.Contains<ModuleKWIgnitors>())
            //    part.FindModuleImplementing<ModuleKWIgnitors>().UpdateUI();
            if (KWUtil.FlightScene()) {
                if (part.Modules.Contains<ModuleEnginesKW>())
                    part.FindModuleImplementing<ModuleEnginesKW>().UpdateUI();
                if (part.Modules.Contains<ModuleEnginesFXKW>())
                    part.FindModuleImplementing<ModuleEnginesFXKW>().UpdateUI();
                if (part.Modules.Contains<ModuleScienceExperimentKW>())
                    StartCoroutine(CallbackUtil.DelayedCallback(1, delegate { part.FindModuleImplementing<ModuleScienceExperimentKW>().UpdateUI(); }));
                
            }
        }
        private void SpawnFacilityMenu(KSCFacilityContextMenu menu) {
            SpaceCenterFacility facility = Enum.GetValues(typeof(SpaceCenterFacility)).Cast<SpaceCenterFacility>().FirstOrDefault(f => f.Description() ==
                (string)menu.GetType().GetField("facilityName", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(menu));
            foreach (string btnName in new string[2] { "Repair", "Upgrade" }) {
                FieldInfo field = menu.GetType().GetField(btnName + "Button", BindingFlags.Instance | BindingFlags.NonPublic);
                if (field == null) continue;
                Button btn = (Button)field.GetValue(menu);
                if (facility == SpaceCenterFacility.Administration && btnName == "Repair")
                    StartCoroutine(CallbackUtil.DelayedCallback(1, delegate {
                        btn.interactable = true;
                        btn.gameObject.SetActive(true);
                        btn.onClick.AddListener(delegate { KWUI.AdminPopup(); });
                        field.SetValue(menu, btn);
                    }));
                else {
                    btn.gameObject.SetActive(false);
                    field.SetValue(menu, btn);
                }
            }
            if (KWUtil.IsTechLocked(facility + ""))
                StartCoroutine(CallbackUtil.DelayedCallback(1, delegate {
                    menu.statusText.text = Localizer.Format("#KWLOC_underConstruction");
                    menu.descriptionText.text = "";
                    menu.levelFieldText.text = "";
                    menu.levelStatsText.text = "";
                }));
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
                Tooltip_TitleAndText tooltip = (Tooltip_TitleAndText)tt;
                tooltip.label.text = "";
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
            if (ApplicationLauncher.Ready && agencyBtn == null) {
                agencyBtn = ApplicationLauncher.Instance.AddModApplication(delegate { Popup(); }, delegate { Popup(); }, null, null, null, null,
                    ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPH |
                    ApplicationLauncher.AppScenes.MAPVIEW | ApplicationLauncher.AppScenes.TRACKSTATION,
                    /*!KWUtil.HeroExists() ? KWUtil.adminIcons[ProgType.LifeSci] :*/ (KWAgencies.Player()?.Agent().Logo ?? KWUtil.uiIcons[Mode.Take]));
            }
            if (HighLogic.LoadedSceneIsEditor && ApplicationLauncher.Ready && editorBtn == null) {
                editorBtn = ApplicationLauncher.Instance.AddModApplication(delegate { EditorPopup(); }, delegate { EditorPopup(); }, null, null, null, null,
                    ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPH, GameDatabase.Instance.GetTexture("Squad/Contracts/Icons/report", false));
            }
        }
        private void RemoveAppBtns() {
            if (agencyBtn != null) { ApplicationLauncher.Instance.RemoveModApplication(agencyBtn); agencyBtn = null; }
            if (editorBtn != null) { ApplicationLauncher.Instance.RemoveModApplication(editorBtn); editorBtn = null; }
        }
        private DialogGUIHorizontalLayout TitleBar(Agency agency, bool mainMode) {
            List<DialogGUIBase> bar = new List<DialogGUIBase> {
                new DialogGUIToggleButton(true, $"<i>{agency.Agent().Title}</i>",
                    delegate { Dismiss(); Popup(agency, Mode.Desc); }, mainMode ? 292 : 256, 32), new DialogGUIButton("x", delegate { }, 32, 32, true)};
            if (!mainMode) bar.Insert(0, new DialogGUIButton("<", delegate { Popup(agency, Mode.Main); }, 32, 32, true));
            return new DialogGUIHorizontalLayout(bar.ToArray());
        }
        private DialogGUIHorizontalLayout NavBar(Agency agency, Mode mode) { // , ListMode lMode = ListMode.All
            List<DialogGUIBase> bar = new List<DialogGUIBase>();
            bool navActive = (Mode.Main | Mode.Desc | Mode.PartBrs | Mode.OutSrc | Mode.Take | Mode.Negs).HasFlag(mode);
            if (navActive) {
                //List<Agency> aList = PartLoader.LoadedPartsList.Where(p => ResearchAndDevelopment.PartTechAvailable(p)).Select(p =>
                //    KWAgencies.GetAgency(AgentList.Instance.GetAgentbyTitle(p.manufacturer))).Append(KWAgencies.Player()).Append(KWAgencies.Junkyard()).Distinct().OrderBy(a => a.Name).ToList();
                List<Agency> aList = KWAgencies.AList.Where(a => (Mode.Main | Mode.Desc).HasFlag(mode) || a.Name == agency.Name ||
                    (mode == Mode.PartBrs && KWAgencies.GetAvailableParts(a).Count > 0) ||
                    (mode == Mode.Take && Funding.Instance.Funds >= a.Value) ||
                    (mode == Mode.Negs && !KWAgencies.PlayerIsAgency(a)) ||
                    (mode == Mode.OutSrc && a.Labs.Count > 0)).ToList();
                int i = aList.FindIndex(a => a.Name == agency.Name);
                bar.AddRange(new DialogGUIBase[] {
                    new DialogGUIButton("<<", delegate { Popup(aList[i - 1 > -1 ? i - 1 : aList.Count - 1], mode); }, 32, 32, true),
                    new DialogGUIButton(">>", delegate { Popup(aList[i + 1 < aList.Count ? i + 1 : 0], mode); }, 32, 32, true) });
                if (mode == Mode.Main && !KWAgencies.NewPlayer()) // bar.Insert(1, new DialogGUIBox("mode selector", 256, 32));
                    bar.Insert(1, new DialogGUIBox(Localizer.Format(""), 256, 32)); } // #autoLOC_6001393
            if ((Mode.Desc | Mode.Take).HasFlag(mode) || KWAgencies.NewPlayer())
                //if (!navActive) bar.Add(new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.Lead]));
                bar.InsertRange(1, new DialogGUIBase[] {
                    new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.Take]),
                    new DialogGUIToggleButton(mode == Mode.Take, Localizer.Format(Mode.Take.Description()),
                        delegate { Dismiss(); Popup(agency, mode != Mode.Take ? Mode.Take : Mode.Main); }, 184, 32), // 257, 221, 
                    new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.Take]) });
            else if ((Mode.Negs | Mode.FRaise | Mode.Interns).HasFlag(mode))
                bar.InsertRange(bar.Count > 1 ? 1 : 0, new DialogGUIBase[] {
                    new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[mode]),
                    new DialogGUIToggleButton(agency.StratActive(mode), Localizer.Format(mode.Description()), 
                        delegate { agency.ToggleStrat(mode); }, bar.Count > 1 ? 184 : 256, 32),
                    new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[mode]) });
            else if ((Mode.IPSell | Mode.Bail | Mode.OutSrc | Mode.OpenSrc | Mode.Patents).HasFlag(mode))
                bar.InsertRange(bar.Count > 1 ? 1 : 0, new DialogGUIBase[] {
                    new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[mode]),
                    new DialogGUIToggleButton(true, Localizer.Format(mode.Description()),
                        delegate { Dismiss(); Popup(agency, Mode.Main); }, bar.Count > 1 ? 184 : 256, 32),
                    new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[mode]) });
            else if (mode == Mode.PartBrs && KWAgencies.PlayerIsAgency(agency))
                bar.InsertRange(1, new DialogGUIBase[] {
                    new DialogGUIBox($"{Localizer.Format("#autoLOC_6001017")} {Localizer.Format("#autoLOC_901054")}", 220, 32), // !navActive ? 271 : 202
                    new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.PartBrs]) });
            else if ((Mode.Parts | Mode.Junk).HasFlag(mode)) {
                bar.InsertRange(1, new DialogGUIBase[] {
                    new DialogGUIBox($"{Localizer.Format("#autoLOC_6001017")} {Localizer.Format("#autoLOC_901054")}", Mode.Junk.HasFlag(mode) ? 187 : 151, 32), // !navActive ? 187 : 118
                    new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, mode == Mode.JunkBuy ? KWUtil.uiIcons[Mode.JunkBuy] : KWUtil.uiIcons[Mode.PartBrs]) });
                if (Mode.Junk.HasFlag(mode))
                    bar.Insert(3, new DialogGUIBox(Localizer.Format("#autoLOC_223622"), 80, 32));
                else bar.Insert(3, new DialogGUIBox(Localizer.Format("#autoLOC_223622"), 65, 32));
                //bar.Insert(3, new DialogGUIToggleButton(mode == Mode.PartBuy, Localizer.Format("#autoLOC_223622"),
                //            delegate { Dismiss(); Popup(agency, mode == Mode.PartBrs ? Mode.PartBuy : Mode.PartBrs); }, 65, 32));
            }
            return new DialogGUIHorizontalLayout(bar.ToArray());
        }
        private DialogGUIScrollList ScrollBox(Agency agency, Mode mode) {
            bool isPlayer = KWAgencies.PlayerIsAgency(agency);
            List<DialogGUIBase> box = new List<DialogGUIBase> {
                new DialogGUIContentSizer(ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.PreferredSize, true) };
            //if ((Mode.Main | Mode.OutSrc).HasFlag(mode) && !KWAgencies.NewPlayer()) {
            //     // KWUtil.sciLabs.Where(p => p.Value.Contains(agency.Agent().Name))?.Select(p => p.Key).ToArray();
            //    if (agency.Labs.Count > 0) {
            //        List<DialogGUIBase> icons = new List<DialogGUIBase> { new DialogGUIBox(Localizer.Format("#KWLOC_labs"), 50, 32) };
            //        foreach (var pair in agency.Labs)
            //            icons.Add(new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), pair.Value ? Color.green : Color.red, KWUtil.sciIcons[pair.Key]));
            //        box.Add(new DialogGUIHorizontalLayout(icons.ToArray()));
            //        if (!isPlayer) box.Add(new DialogGUIHorizontalLayout(
            //            new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.OutSrc]),
            //            new DialogGUIToggleButton(mode == Mode.OutSrc, Localizer.Format(Mode.OutSrc.Description()),
            //                delegate { Popup(agency, mode == Mode.Main ? Mode.OutSrc : Mode.Main); }, 266, 32)));
            //        if (isPlayer || mode == Mode.OutSrc) {
            //            box.Add(new DialogGUIHorizontalLayout(
            //                new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.OpenSrc]),
            //                new DialogGUIButton(Localizer.Format(Mode.OpenSrc.Description()), delegate { Popup(agency, Mode.OpenSrc); }, 266, 32, true)));
            //            box.Add(new DialogGUIHorizontalLayout(
            //                new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.Patents]),
            //                new DialogGUIButton(Localizer.Format(Mode.Patents.Description()), delegate { Popup(agency, Mode.Patents); }, 266, 32, true))); } } }
            if (mode == Mode.Main) {

                box.Add(new DialogGUIHorizontalLayout(
                    new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, isPlayer ? KWUtil.uiIcons[Mode.PartBrs] : KWUtil.uiIcons[Mode.PartBuy]),
                    new DialogGUIButton(Localizer.Format(isPlayer ? "#KWLOC_partsStorage" : "#KWLOC_partsCatalogue"), delegate { Popup(agency, Mode.PartBrs); }, 229, 32, true),
                    new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, isPlayer ? KWUtil.uiIcons[Mode.PartBrs] : KWUtil.uiIcons[Mode.PartBuy])));
                // box.Add(new DialogGUIBox("", 303, 32));
                //if (!KWAgencies.NewPlayer()) 
                //    box.Add(new DialogGUIHorizontalLayout(KWAdmin.GetLobbyProgs(agency).Select(p => 
                //        new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.adminIcons[p])).ToArray()));
                //else box.AddRange(KWAdmin.GetLobbyProgs(agency).Select(p =>
                //        new DialogGUIHorizontalLayout(
                //            new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.adminIcons[p]),
                //            new DialogGUIBox(Localizer.Format(p.Description()), 266, 32))).ToArray());
                if (isPlayer) {
                    //box.Add(new DialogGUIHorizontalLayout(
                    //    new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.IPSell]),
                    //    new DialogGUIButton(Localizer.Format(Mode.IPSell.Description()), delegate { Popup(agency, Mode.IPSell); }, 229, 32, true),
                    //    new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.IPSell])));
                    //if (!KWAgencies.PlayerIsRnD())
                    //    box.Add(new DialogGUIHorizontalLayout(
                    //        new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.FRaise]),
                    //        new DialogGUIToggleButton(agency.StratActive(Mode.FRaise), Localizer.Format(Mode.FRaise.Description()),
                    //            delegate { agency.ToggleStrat(Mode.FRaise); }, 193, 32),
                    //        new DialogGUIButton("?", delegate { Popup(agency, Mode.FRaise); }, 32, 32, true),
                    //        new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.FRaise])));
                    //else {
                    //    if (Reputation.CurrentRep > 0)
                    //        box.Add(new DialogGUIHorizontalLayout(
                    //            new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.Interns]),
                    //            new DialogGUIToggleButton(agency.StratActive(Mode.Interns), Localizer.Format(Mode.Interns.Description()),
                    //                delegate { agency.ToggleStrat(Mode.Interns); }, 193, 32),
                    //            new DialogGUIButton("?", delegate { Popup(agency, Mode.Interns); }, 32, 32, true),
                    //            new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.Interns])));
                    //    if (Funding.Instance.Funds < 0)
                    //        box.Add(new DialogGUIHorizontalLayout(
                    //            new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.Bail]),
                    //            new DialogGUIButton(Localizer.Format(Mode.Bail.Description()), delegate { Popup(agency, Mode.Bail); }, 229, 32, true),
                    //            new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.Bail])));
                    //}
                } else if (!KWAgencies.NewPlayer()) {
                    //box.Add(new DialogGUIHorizontalLayout(
                    //    new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.Negs]),
                    //    new DialogGUIToggleButton(agency.StratActive(Mode.Negs), Localizer.Format(Mode.Negs.Description()),
                    //        delegate { agency.ToggleStrat(Mode.Negs); }, 193, 32),
                    //    new DialogGUIButton("?", delegate { Popup(agency, Mode.Negs); }, 32, 32, true),
                    //    new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.Negs])));
                    if (agency.Name == KWAgencies.Junkyard().Name) {
                        box.Add(new DialogGUIHorizontalLayout(
                            new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.JunkBuy]),
                            new DialogGUIButton(Localizer.Format(Mode.JunkBuy.Description()), delegate { Popup(agency, Mode.JunkBuy); }, 113, 32, true),
                            new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.uiIcons[Mode.JunkSell]),
                            new DialogGUIButton(Localizer.Format(Mode.JunkSell.Description()), delegate { Popup(agency, Mode.JunkSell); }, 113, 32, true)));
                    }
                }
            } else if (Mode.Parts.HasFlag(mode)) {
                if (isPlayer) foreach (var pair in KWAgencies.Player().StoredParts())
                        box.Add(new DialogGUIHorizontalLayout(
                            new DialogGUIBox($"<color=white>{pair.Key.title}</color>", 266, 32),
                            new DialogGUIBox(pair.Value + "", 32, 32)));
                else {
                    foreach (AvailablePart part in KWAgencies.GetAvailableParts(agency)) {
                        box.Add(new DialogGUIHorizontalLayout(
                        new DialogGUIBox($"<color=white>{part.title}</color>", 182, 32),
                        new DialogGUIBox(KWAgencies.Player().GetPartQty(part) + "", 32, 32),
                        new DialogGUIButton($"<sprite=\"CurrencySpriteAsset\" name=\"Funds\" color=#B4D455> <color=#B4D455>{KWAgencies.PricePart(part):N0}</color>",
                            delegate { KWAgencies.OrderPart(part); Popup(agency, mode); }, 80, 32, true)));
                    }
                }
            } else if (mode == Mode.JunkBuy) {
                foreach (var pair in KWAgencies.JunkParts())
                    box.Add(new DialogGUIHorizontalLayout(
                    new DialogGUIBox(pair.Key.title, 182, 32),
                    new DialogGUIBox(pair.Value + "", 32, 32),
                    new DialogGUIButton($"<sprite=\"CurrencySpriteAsset\" name=\"Funds\" color=#B4D455> <color=#B4D455>{KWAgencies.PriceJunk(pair.Key):N0}</color>",
                        delegate { KWAgencies.BuyJunk(pair.Key); Popup(agency, mode); }, 80, 32, true)));
            } else if (mode == Mode.JunkSell) {
                foreach (var pair in KWAgencies.Player().StoredParts())
                    box.Add(new DialogGUIHorizontalLayout(
                    new DialogGUIBox(pair.Key.title, 182, 32),
                    new DialogGUIBox(pair.Value + "", 32, 32),
                    new DialogGUIButton($"<sprite=\"CurrencySpriteAsset\" name=\"Funds\" color=#B4D455> <color=#B4D455>{KWAgencies.PriceJunk(pair.Key, false):N0}</color>",
                        delegate { KWAgencies.SellJunk(pair.Key); Popup(agency, mode); }, 80, 32, true)));
            } else if (mode == Mode.OutSrc) {
                box.Add(new DialogGUIBox($"<color=#D4F4FD>{Localizer.Format("#autoLOC_501159")}</color>", 303, 140));
            } else if (mode == Mode.IPSell) {
                box.Add(new DialogGUIBox($"<color=#D4F4FD>{Localizer.Format("#autoLOC_501169")}</color>", 303, 140));

            } else if (mode == Mode.Negs) {
                box.Add(new DialogGUIBox($"<color=#D4F4FD>{Localizer.Format("#autoLOC_501163")}</color>", 303, 100));
            } else if (mode == Mode.Desc) {
                box.Add(new DialogGUIBox($"<color=#D4F4FD>{agency.Agent().Description}</color>", 303, 317));
            } else if (mode == Mode.FRaise) {
                box.Add(new DialogGUIBox($"<color=#D4F4FD>{Localizer.Format("#autoLOC_501153")}</color>", 303, 140));
            } else if (mode == Mode.OpenSrc) {

            } else if (mode == Mode.Patents) {

            } else if (mode == Mode.Bail) {
                float repLoss = (HighLogic.CurrentGame.Parameters.Career.StartingFunds - (float)Funding.Instance.Funds) / 500;
                box.AddRange(new DialogGUIBase[] {
                    new DialogGUIBox($"<color=#D4F4FD>{Localizer.Format("#autoLOC_501167")}</color>", 303, 100),
                    new DialogGUIBox("<color=#B4D455>" + Localizer.Format("#autoLOC_303952", Currency.Funds.Description(), "#autoLOC_8005038",
                        $": <sprite=\"CurrencySpriteAsset\" name=\"Funds\" color=#B4D455> {HighLogic.CurrentGame.Parameters.Career.StartingFunds:N0}</color>\n\n<color=#E0D503>") +
                        Localizer.Format("#autoLOC_303660", $"<sprite=\"CurrencySpriteAsset\" name=\"Reputation\" color=#E0D503> {repLoss:N0}",
                        Currency.Reputation.Description(), "</color>"), 303, 140),
                    new DialogGUIButton($"{Localizer.Format("#autoLOC_900384")} {Localizer.Format("#autoLOC_501166")}", delegate { 
                        Funding.Instance.SetFunds(HighLogic.CurrentGame.Parameters.Career.StartingFunds, TransactionReasons.Strategies);
                        Reputation.Instance.SetReputation(Reputation.CurrentRep - repLoss, TransactionReasons.Strategies);
                        Popup(agency); }, 303, 32, true) });
            } else if (mode == Mode.Interns) {
                box.Add(new DialogGUIBox($"<color=#D4F4FD>{Localizer.Format("#autoLOC_501157")}</color>", 303, 100));

            } else if (mode == Mode.Take) {
                // $"{(Funding.Instance.Funds < agency.Value ? Localizer.Format("#autoLOC_7003246") : "")}"
                // Funding.Instance.Funds >= agency.Value
                bool storageEmpty = KWAgencies.NewPlayer() || (!KWAgencies.Player()?.StoredParts().Values.Any(i => i > 0) ?? true);
                bool canTakeover = Funding.Instance.Funds >= agency.Value && Reputation.CurrentRep >= agency.Rep && ResearchAndDevelopment.Instance.Science == 0 &&
                    storageEmpty && FlightGlobals.Vessels.Count == 0 && KWUtil.CareerOpts().takeoverBids;
                box.AddRange(new DialogGUIBase[] {
                    new DialogGUIBox($"<color=#D4F4FD>{Localizer.Format("#KWLOC_lead_desc", agency.Agent().Title)}</color>", 303, 70),
                    new DialogGUIBox($"<color=white>{agency.Agent().Title}</color>\n\n<color=#B4D455>" +
                        $"{Localizer.Format("#autoLOC_900729")}: <sprite=\"CurrencySpriteAsset\" name=\"Funds\" color=#B4D455> {agency.Value:N0}</color>\n<color=#E0D503>" +
                        $"{Localizer.Format("#autoLOC_464661", $"<sprite=\"CurrencySpriteAsset\" name=\"Reputation\" color=#E0D503> {agency.Rep:N0}").Replace("%","")}</color>\n\n<color=" +
                        $"{(canTakeover ? "green>" : "red>") + Localizer.Format("#autoLOC_211272")}</color>\n\n<color={(Funding.Instance.Funds >= agency.Value ? "#B4D455>" : "red>")}" +
                        $"{Localizer.Format("#autoLOC_419441", $"<sprite=\"CurrencySpriteAsset\" name=\"Funds\" color=#B4D455> {Funding.Instance.Funds:N0}")}</color>\n<color=" +
                        (Reputation.CurrentRep >= agency.Rep ? "#E0D503>" : "red>") + Localizer.Format("#autoLOC_464661", "<sprite=\"CurrencySpriteAsset\" name=\"Reputation\" color=#E0D503> " +
                        $"{Reputation.CurrentRep:N0}").Replace("%","") + $"</color>\n<color={(ResearchAndDevelopment.Instance.Science == 0 ? "#6DCFF6>" : "red>")}" +
                        Localizer.Format("#autoLOC_419420", $"<sprite=\"CurrencySpriteAsset\" name=\"Science\" color=#6DCFF6> {ResearchAndDevelopment.Instance.Science:N0}") +
                        $"</color>\n<color={(storageEmpty ? "#D4F4FD>" : "red>") + Localizer.Format("#KWLOC_partsStorage")}: " + (storageEmpty ? Localizer.Format("#autoLOC_6002404") :
                        Localizer.Format("#autoLOC_244028")) + "</color>\n<color=" + (FlightGlobals.Vessels.Count == 0 ? "#D4F4FD>" : "red>") + Localizer.Format("#autoLOC_8003014",
                        FlightGlobals.Vessels.Count == 0 ? Localizer.Format("#autoLOC_6003000") : Localizer.Format("#autoLOC_145786")) + "</color>", 303, 206) });
                // + (canTakeover ? $"\n\n<color=green>{Localizer.Format("#KWLOC_takeoverBid")} {Localizer.Format("#autoLOC_238176")}</color>" : "")
                if (canTakeover) box.Add(new DialogGUIButton($"{Localizer.Format("#autoLOC_900523")} {Localizer.Format("#KWLOC_takeoverBid")}",
                    delegate { KWAgencies.Takeover(agency); agencyBtn.SetTexture(KWAgencies.Player().Agent().Logo); Popup(agency); }, 303, 32, true));
                else box.Add(new DialogGUIBox($"<color=red>{Localizer.Format("#KWLOC_takeoverBid")} " +
                    Localizer.Format("#autoLOC_234994", Localizer.Format("#autoLOC_7003010").Replace(":", "")) + "</color>", 303, 32));
            }
            return new DialogGUIScrollList(Vector2.one, false, true, new DialogGUIVerticalLayout(10, 100, 4, new RectOffset(5, 24, 5, 5), TextAnchor.MiddleLeft, box.ToArray()));
        }
        private void Popup(Agency agency = null, Mode mode = Mode.Main) { // , ListMode lMode = ListMode.All
            if (agency == null) agency = KWAgencies.Player() ?? KWAgencies.RandomAgency();
            DialogGUIBase[] content = new DialogGUIBase[] {
                TitleBar(agency, mode == Mode.Main),
                new DialogGUIBox("", 330, 3),
                new DialogGUIImage(new Vector2(330, 165), new Vector2(0, 0), Color.white, agency.Agent().Logo),
                new DialogGUIBox("", 330, 3),
                new DialogGUIHorizontalLayout(
                    new DialogGUIBox($"<sprite=\"CurrencySpriteAsset\" name=\"Funds\" color=#B4D455> <color=#B4D455>{agency.Value:N0}</color>", 85, 32),
                    new DialogGUIBox($"<sprite=\"CurrencySpriteAsset\" name=\"Reputation\" color=#E0D503> <color=#E0D503>{agency.Rep:N0}</color>", 50, 32),
                    //new DialogGUIBox(KWAgencies.NewPlayer() || KWAgencies.PlayerIsAgency(agency) ? Localizer.Format("#KWLOC_leader", agency.Leader) : KWAgencies.GetStandingDesc(agency), 185, 32)),
                    new DialogGUIBox(KWAgencies.NewPlayer() || KWAgencies.PlayerIsAgency(agency) ? agency.Leader : KWAgencies.GetStandingDesc(agency), 185, 32)),
                NavBar(agency, mode), // lMode
                ScrollBox(agency, mode) };
            PopupDialog.SpawnPopupDialog(new MultiOptionDialog("KWPopup", "", "", HighLogic.UISkin, new Rect(0.8f, 0.35f, 340, 636), content), false, HighLogic.UISkin); // 0.5f - content.Count / 58f
        }
        private void Dismiss() => PopupDialog.DismissPopup("KWPopup");
        internal static void AdminPopup() {
            List<DialogGUIBase> content = new List<DialogGUIBase> {
            new DialogGUIHorizontalLayout(
                new DialogGUIToggleButton(true, "<i>Donors</i>", delegate { }, 392, 32),
                new DialogGUIButton("x", delegate { }, 32, 32, true)) };

            content.Add(new DialogGUIHorizontalLayout(
                new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.adminIcons[ProgType.PhysSci]),
                new DialogGUIBox(Localizer.Format(ProgType.PhysSci.Description()), 103, 32),
                new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.adminIcons[ProgType.Pilots]),
                new DialogGUIBox(Localizer.Format(ProgType.Crew.Description()), 103, 32),
                new DialogGUIImage(new Vector2(32, 32), new Vector2(0, 0), Color.white, KWUtil.adminIcons[ProgType.Robotics]),
                new DialogGUIBox(Localizer.Format(ProgType.Robotics.Description()), 103, 32)));
            
            content.Add(new DialogGUIHorizontalLayout(
                new DialogGUIVerticalLayout(KWAdmin.GetLobbyProgs(ProgType.PhysSci).Select(a => 
                    new DialogGUIHorizontalLayout(
                        new DialogGUIImage(new Vector2(64, 40), new Vector2(0, 0), Color.white, a.Agent().LogoScaled),
                        new DialogGUIBox("<sprite=\"CurrencySpriteAsset\" name=\"Funds\" color=#B4D455> <color=#B4D455>1,000,000</color>", 75, 40))).ToArray()),
                new DialogGUIVerticalLayout(KWAdmin.GetLobbyProgs(ProgType.Pilots).Select(a =>
                    new DialogGUIHorizontalLayout(
                        new DialogGUIImage(new Vector2(64, 40), new Vector2(0, 0), Color.white, a.Agent().LogoScaled),
                        new DialogGUIBox("0", 75, 40))).ToArray()),
                new DialogGUIVerticalLayout(KWAdmin.GetLobbyProgs(ProgType.Robotics).Select(a =>
                    new DialogGUIHorizontalLayout(
                        new DialogGUIImage(new Vector2(64, 40), new Vector2(0, 0), Color.white, a.Agent().LogoScaled),
                        new DialogGUIBox("0", 75, 40))).ToArray())));

            PopupDialog.SpawnPopupDialog(new MultiOptionDialog("KWAdminPopup", "", "", HighLogic.UISkin, new Rect(0.5f, 0.5f, 440, 340), content.ToArray()), false, HighLogic.UISkin);
        }
        private void EditorPopup() {
            List<DialogGUIBase> content = new List<DialogGUIBase> { new DialogGUIHorizontalLayout(
                new DialogGUIToggleButton(true, Localizer.Format("#KWLOC_purchaseOrder"), delegate { }, 262, 32),
                new DialogGUIButton("x", delegate { }, 32, 32, true)) };
            List<AvailablePart> parts = EditorLogic.fetch.ship.Parts.Select(p => p.partInfo).ToList();
            int totalCost = 0;
            foreach (AvailablePart part in parts.Distinct()) {
                int count = parts.Count(p => p == part) - KWAgencies.Player().GetPartQty(part);
                if (count <= 0) continue;
                totalCost += count * part.entryCost;
                content.Add(new DialogGUIHorizontalLayout(
                    new DialogGUIBox(part.title, 178, 32),
                    new DialogGUIBox(count + "", 32, 32),
                    new DialogGUIBox($"<sprite=\"CurrencySpriteAsset\" name=\"Funds\" color=#B4D455> <color=#B4D455>{count * part.entryCost:N0}</color>", 80, 32))); }
            content.Add(new DialogGUIHorizontalLayout(
                new DialogGUIButton("x", delegate { }, 32, 32, true),
                new DialogGUIBox(Localizer.Format("#KWLOC_total"), 178, 32),
                new DialogGUIBox($"<sprite=\"CurrencySpriteAsset\" name=\"Funds\" color=#B4D455> <color=#B4D455>{totalCost:N0}</color>", 80, 32)));
            PopupDialog.SpawnPopupDialog(new MultiOptionDialog("KWEditorPopup", "", "",
                HighLogic.UISkin, new Rect(0.8f, 0.5f - content.Count / 58f, 310, 1), content.ToArray()), false, HighLogic.UISkin);
        }
        //public static void GameOverPopup() => PopupDialog.SpawnPopupDialog(
        //    new MultiOptionDialog("GameOverPopup", "", Localizer.Format("#autoLOC_8014052"), HighLogic.UISkin, new Rect(0.5f, 0.5f, 250, 1),
        //        new DialogGUIBase[2] { // PauseMenu / QuickSaveLoadDialog
        //            new DialogGUIButton(Localizer.Format("#autoLOC_360553"), delegate {
        //                LoadGameDialog.Create(new LoadGameDialog.FinishedCallback(delegate { }), HighLogic.SaveFolder, false, UISkinManager.GetSkin("MainMenuSkin")); }, true),
        //            new DialogGUIButton(Localizer.Format("#autoLOC_360837"), delegate { HighLogic.LoadScene(GameScenes.MAINMENU); }, true)
        //        }), false, HighLogic.UISkin);

    }
    
}