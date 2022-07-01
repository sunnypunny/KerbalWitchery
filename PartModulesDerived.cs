using Experience.Effects;
using KSP.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KerbalWitchery {

    public class ModuleEnginesKW : ModuleEngines {
        [KSPField(isPersistant = true)]
        public int ignitions;
        [KSPField(isPersistant = true)]
        public bool canRepair;
        [KSPField(guiActive = true, guiName = "#KWLOC_ignitionChance")]
        public string ignitionChance;
        [KSPField]
        public int maxIgnitions;
        private bool ignited;
        private float minThrottle;
        public override void OnStart(StartState state) {
            base.OnStart(state);
            if (HighLogic.LoadedSceneIsFlight) {
                minThrottle = throttleMin;
                ToggleRepairUI(canRepair);
                if (maxIgnitions == 0) Fields[nameof(ignitionChance)].guiActive = false; }
        }
        public new void FixedUpdate() {
            base.FixedUpdate();
            if (KWUtil.GenOpts().stockThrottle && HighLogic.LoadedSceneIsFlight && FlightGlobals.ready) {
                if (part.vessel.ctrlState.mainThrottle > 0f)
                    throttleMin = minThrottle;
                else throttleMin = 0f;
                if (maxIgnitions > 0)
                    if (GetCurrentThrust() > 0f) {
                        if (!ignited) RunIgnitionCheck();
                    } else ignited = false; }
        }
        public override string GetInfo() => (propellants.Count > 1 ? $"{propellants[0].displayName} + {propellants[1].displayName} ({propellants[0].ratio}:{propellants[1].ratio})\n" : "") +
            (maxIgnitions > 0 ? Localizer.Format("#KWLOC_avgIgnitions", (maxIgnitions / 2) + "\n\n") : "\n") + base.GetInfo();
        [KSPEvent(guiActive = true, guiName = "#autoLOC_6001382")]
        public override void Activate() {
            base.Activate();
            if (!KWUtil.GenOpts().stockThrottle && finalThrust == 0 && maxIgnitions > -1)
                RunIgnitionCheck();
        }
        public void RunIgnitionCheck() {
            if (ignitions > UnityEngine.Random.Range(maxIgnitions / 2, maxIgnitions))
                StartCoroutine(CallbackUtil.DelayedCallback(UnityEngine.Random.Range(1, maxIgnitions * 10), delegate { part.explode(); }));
            else if (ignitions > UnityEngine.Random.Range(0, maxIgnitions) - (FlightGlobals.ship_geeForce > 0.025 ? 0 : maxIgnitions / 2))
                StartCoroutine(CallbackUtil.DelayedCallback(UnityEngine.Random.Range(1, maxIgnitions * 5), delegate { FailIgnition(); }));
            ignited = true;
            ignitions++;
        }
        private void FailIgnition() {
            ToggleRepairUI(true);
            Shutdown();
            ScreenMessages.PostScreenMessage($"{Localizer.Format("#autoLOC_236416")} {part.partInfo.title} {Localizer.Format("#autoLOC_7001053")} {Localizer.Format("#autoLOC_8003101")}");
        }
        [KSPEvent(guiName = "#autoLOC_8000063")]
        public void Repair() {
            if (!FlightGlobals.ActiveVessel.isEVA || (HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().KerbalExperienceEnabled(HighLogic.CurrentGame.Mode)
                && FlightGlobals.ActiveVessel.VesselValues.RepairSkill.value < 1)) {
                ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_246904", "1"));
                return;
            } else if (FlightGlobals.ActiveVessel.evaController.ModuleInventoryPartReference.TotalAmountOfPartStored("evaRepairKit") < 1) {
                ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6006097", new string[2] { "1", "#autoLOC_6005094" }));
                return;
            }
            FlightGlobals.ActiveVessel.evaController.ModuleInventoryPartReference.RemoveNPartsFromInventory("evaRepairKit", 1, true);
            ignitions--;
            if (ignitions == 0) ToggleRepairUI(false);
        }
        private void ToggleRepairUI(bool toggle) {
            canRepair = toggle;
            Events["Repair"].guiActiveUnfocused = toggle;
        }
        public void UpdateUI() => ignitionChance = $"{Mathf.Clamp(((FlightGlobals.ship_geeForce > 0.025 ? maxIgnitions : maxIgnitions * 0.5f) - ignitions) * (1f / maxIgnitions), 0, 1):P0}";

    }

    public class ModuleEnginesFXKW : ModuleEnginesFX {
        [KSPField(isPersistant = true)]
        public int ignitions;
        [KSPField(isPersistant = true)]
        public bool canRepair;
        [KSPField(guiActive = true, guiName = "#KWLOC_ignitionChance")]
        public string ignitionChance;
        [KSPField]
        public int maxIgnitions;
        private bool ignited;
        private float minThrottle;
        public override void OnStart(StartState state) {
            base.OnStart(state);
            if (HighLogic.LoadedSceneIsFlight) {
                minThrottle = throttleMin;
                ToggleRepairUI(canRepair);
                if (maxIgnitions == 0) Fields[nameof(ignitionChance)].guiActive = false; }
        }
        public new void FixedUpdate() {
            base.FixedUpdate();
            if (KWUtil.GenOpts().stockThrottle && HighLogic.LoadedSceneIsFlight && FlightGlobals.ready) {
                if (part.vessel.ctrlState.mainThrottle > 0f)
                    throttleMin = minThrottle;
                else throttleMin = 0f;
                if (maxIgnitions > 0)
                    if (GetCurrentThrust() > 0f) {
                        if (!ignited) RunIgnitionCheck();
                    } else ignited = false; }
        }
        public override string GetInfo() => (propellants.Count > 1 ? $"{propellants[0].displayName} + {propellants[1].displayName} ({propellants[0].ratio}:{propellants[1].ratio})\n" : "") +
            (maxIgnitions > 0 ? Localizer.Format("#KWLOC_avgIgnitions", (maxIgnitions / 2) + "\n\n") : "\n") + base.GetInfo();
        [KSPEvent(guiActive = true, guiName = "#autoLOC_6001382")]
        public override void Activate() {
            base.Activate();
            if (!KWUtil.GenOpts().stockThrottle && finalThrust == 0 && maxIgnitions > -1)
                RunIgnitionCheck();
        }
        public void RunIgnitionCheck() {
            if (ignitions > UnityEngine.Random.Range(maxIgnitions / 2, maxIgnitions))
                StartCoroutine(CallbackUtil.DelayedCallback(UnityEngine.Random.Range(1, maxIgnitions * 10), delegate { part.explode(); }));
            else if (ignitions > UnityEngine.Random.Range(0, maxIgnitions) - (FlightGlobals.ship_geeForce > 0.025 ? 0 : maxIgnitions / 2))
                StartCoroutine(CallbackUtil.DelayedCallback(UnityEngine.Random.Range(1, maxIgnitions * 5), delegate { FailIgnition(); }));
            ignited = true;
            ignitions++;
        }
        private void FailIgnition() {
            ToggleRepairUI(true);
            Shutdown();
            ScreenMessages.PostScreenMessage($"{Localizer.Format("#autoLOC_236416")} {part.partInfo.title} {Localizer.Format("#autoLOC_7001053")} {Localizer.Format("#autoLOC_8003101")}");
        }
        [KSPEvent(guiName = "#autoLOC_8000063")]
        public void Repair() {
            if (!FlightGlobals.ActiveVessel.isEVA || (HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().KerbalExperienceEnabled(HighLogic.CurrentGame.Mode)
                && FlightGlobals.ActiveVessel.VesselValues.RepairSkill.value < 1)) {
                ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_246904", "1"));
                return;
            } else if (FlightGlobals.ActiveVessel.evaController.ModuleInventoryPartReference.TotalAmountOfPartStored("evaRepairKit") < 1) {
                ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6006097", new string[2] { "1", "#autoLOC_6005094" }));
                return;
            }
            FlightGlobals.ActiveVessel.evaController.ModuleInventoryPartReference.RemoveNPartsFromInventory("evaRepairKit", 1, true);
            ignitions--;
            if (ignitions == 0) ToggleRepairUI(false);
        }
        private void ToggleRepairUI(bool toggle) {
            canRepair = toggle;
            Events["Repair"].guiActiveUnfocused = toggle;
        }
        public void UpdateUI() => ignitionChance = $"{Mathf.Clamp(((FlightGlobals.ship_geeForce > 0.025 ? maxIgnitions : maxIgnitions * 0.5f) - ignitions) * (1f / maxIgnitions), 0, 1):P0}";

    }

    public class ModuleReactionWheelKW : ModuleReactionWheel {
        [KSPField(guiActive = true, guiName = "#autoLOC_6002700")]
        public bool locked;
        [KSPField(guiActive = true, guiName = "#KWLOC_angularMomentum", guiFormat = "0.000")]
        public float momentum;
        private Quaternion targetRot;
        public override void OnStart(StartState state) {
            base.OnStart(state);
            if ((int)state > 1) part.force_activate(false);
        }
        public override void OnFixedUpdate() {
            if (FlightGlobals.ready) {
                momentum = TimeWarp.CurrentRateIndex > 0 && locked ? 0f : part.vessel.angularMomentum.magnitude;
                locked = momentum < 0.01f && vessel.geeForce_immediate < 0.001 && State == WheelState.Active && vessel.Autopilot.Enabled;
                if (locked)
                    if (TimeWarp.CurrentRateIndex == 0)
                        targetRot = FlightGlobals.ActiveVessel.transform.rotation;
                    else vessel.SetRotation(targetRot); }
        }

    }

    public class ModuleResourceIntakeKW : ModuleResourceIntake {
        //public override void OnStart(StartState state) {
        //    base.OnStart(state);
        //    if (HighLogic.LoadedSceneIsFlight) {
        //        CheckSituationResource();
        //        GameEvents.onVesselSituationChange.Add(ChangeVesselSituation); }
        //}
        //public void OnDestroy() { if (HighLogic.LoadedSceneIsFlight) GameEvents.onVesselSituationChange.Remove(ChangeVesselSituation); }
        public new void FixedUpdate() {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready) {
                string atmoRes = KWUtil.bodyResources.ContainsKey(vessel.mainBody.name) ? KWUtil.bodyResources[vessel.mainBody.name].GetValue("atmo") : null;
                string oceanRes = KWUtil.bodyResources.ContainsKey(vessel.mainBody.name) ? KWUtil.bodyResources[vessel.mainBody.name].GetValue("ocean") : null;
                if (FlightGlobals.getAltitudeAtPos(intakeTransform.position, vessel.mainBody) < 0 && resourceName != oceanRes && oceanRes != null)
                    UpdateResource(oceanRes);
                else if (vessel.mainBody.atmosphere && resourceName != atmoRes && atmoRes != null)
                    UpdateResource(atmoRes);
            }
            base.FixedUpdate();
        }
        //private void CheckSituationResource() {

        //    if (vessel.situation == Vessel.Situations.SPLASHED && res.resourceName != "Aqua")
        //        UpdateResource("Aqua");
        //    else if (KWUtil.atmoComps.ContainsKey(part.vessel.mainBody.name))
        //        if (KWUtil.atmoComps[part.vessel.mainBody.name] != res.resourceName)
        //            UpdateResource(KWUtil.atmoComps[part.vessel.mainBody.name]);
        //    //if (vessel.mainBody.atmosphereContainsOxygen && res.resourceName != "IntakeAir") UpdateResource("IntakeAir");
        //    //else if ("Eve|Duna".Contains(part.vessel.mainBody.name) && res.resourceName != "ToxicGas") UpdateResource("ToxicGas");
        //    //else if ("Sun|Jool".Contains(part.vessel.mainBody.name) && res.resourceName != "HydroFuel") UpdateResource("HydroFuel");
        //}
        public void UpdateResource(string resName) {
            resourceName = resName;
            resourceId = resourceName.GetHashCode();
            resourceDef = PartResourceLibrary.Instance.GetDefinition(resourceId);
            densityRecip = 1.0 / (double)resourceDef.density;
            res = part.Resources[resourceName];
        }
        // private void ChangeVesselSituation(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> action) { if (action.host == part.vessel) CheckSituationResource(); }

    }

    public class ModuleScienceExperimentKW : ModuleScienceExperiment {
        [KSPField]
        public bool deployRequiresResource;
        [KSPField]
        public string deployResourceName;
        [KSPField]
        public bool resetRequiresKit;
        public override void OnStart(StartState state) {
            base.OnStart(state);
            if (resetRequiresKit)
                Events[nameof(ResetExperiment)].guiActive = false;
        }
        [KSPAction("Deploy")]
        public new void DeployAction(KSPActionParam param) {
            if (!deployRequiresResource) base.DeployAction(param);
            else if (part.Resources.Get(deployResourceName).amount > 0) base.DeployAction(param);
            else ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_244419", PartResourceLibrary.Instance.GetDefinition(deployResourceName).displayName));
        }
        [KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_502050")]
        public new void DeployExperiment() {
            if (!deployRequiresResource) base.DeployExperiment();
            else if (part.Resources.Get(deployResourceName).amount > 0) base.DeployExperiment();
            else ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_244419", PartResourceLibrary.Instance.GetDefinition(deployResourceName).displayName));
        }
        [KSPEvent(active = true, externalToEVAOnly = true, guiActive = false, guiActiveUnfocused = true, guiName = "#autoLOC_6002397")]
        public new void DeployExperimentExternal() {
            if (!deployRequiresResource) base.DeployExperimentExternal();
            else if (part.Resources.Get(deployResourceName).amount > 0) base.DeployExperimentExternal();
            else ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_244419", PartResourceLibrary.Instance.GetDefinition(deployResourceName).displayName));
        }
        [KSPEvent(active = true, externalToEVAOnly = true, guiActive = false, guiActiveUnfocused = true, guiName = "#autoLOC_6001862")]
        public new void CleanUpExperimentExternal() {
            if (!resetRequiresKit) base.CleanUpExperimentExternal();
            else if (KitCheck()) base.CleanUpExperimentExternal(); }
        //[KSPEvent(guiActiveUnfocused = true, guiName = "#autoLOC_900305")] // active = true, externalToEVAOnly = true, guiActive = false, 
        //public new void ResetExperimentExternal() { if (!resetRequiresKit || KitCheck()) base.ResetExperimentExternal(); }

        private bool KitCheck() {
            if (!FlightGlobals.ActiveVessel.isEVA || !FlightGlobals.ActiveVessel.parts[0].protoModuleCrew[0].HasEffect<ScienceResetSkill>()) {
                ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238776"));
                return false;
            } else if (FlightGlobals.ActiveVessel.evaController.ModuleInventoryPartReference.TotalAmountOfPartStored("evaScienceKit") < 1) {
                ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6006097", new string[2] { "1", "#autoLOC_6002538" }));
                return false; }
            FlightGlobals.ActiveVessel.evaController.ModuleInventoryPartReference.RemoveNPartsFromInventory("evaScienceKit", 1, true);
            return true;
        }

        public void UpdateUI() {
            if (Deployed && deployRequiresResource) {
                PartResource res = part.Resources.Get(deployResourceName);
                if (res.flowState) res.flowState = false;
                part.PartActionWindow.RemoveResourceControlFlight(res);
                part.PartActionWindow.RemoveResourceTransferControl(res);
            }
            //if (deployRequiresResource && part.Resources.Get(deployResourceName).amount == 0 && Events[nameof(DeployExperiment)].active) {
            //    Events[nameof(DeployExperiment)].active = false;
            //    Events[nameof(DeployExperimentExternal)].active = false;
            //} else if (deployRequiresResource && part.Resources.Get(deployResourceName).amount == 0 && !Events[nameof(DeployExperiment)].active) {
            //    Events[nameof(DeployExperiment)].active = true;
            //    Events[nameof(DeployExperimentExternal)].active = true;
            //}
        }

    }

}
