
using KSP.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KerbalWitchery {

    //public class ModuleReactionWheelKW : ModuleReactionWheel {
    //    [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_315358")]
    //    public void HoldAttitude() {
    //        Debug.Log(part.vessel.angularMomentum.magnitude);
    //        if (part.vessel.angularMomentum.magnitude < 0.01f)
    //    }
    //}

    public class ModuleKWPartMaker : PartModule {
        [KSPField]
        public string displayName = "#KWLOC_3dPrinter";
        [KSPField]
        public string actionText = "#autoLOC_900341";
        [KSPField]
        public string partName;
        [KSPField]
        public string partTitle;
        [KSPField]
        public string resourceName = "Ore";
        [KSPField]
        public float resourceCost;
        public override void OnStart(StartState state) => Events[nameof(MakePart)].guiName = $"{Localizer.Format(actionText)} {Localizer.Format(partTitle)}";
        public override string GetModuleDisplayName() => Localizer.Format(displayName);
        public override string GetInfo() => Localizer.Format("#autoLOC_8006061", partTitle) +
            $"\n{Localizer.Format("#autoLOC_244332")} {resourceCost:N1} {PartResourceLibrary.Instance.GetDefinition(resourceName)?.displayName}";
        [KSPEvent(guiActive = true)]
        public void MakePart() {
            int resID = PartResourceLibrary.Instance.GetDefinition(resourceName).id;
            ModuleInventoryPart inv = part.FindModuleImplementing<ModuleInventoryPart>();
            int slot = inv.FirstEmptySlot();
            if (slot == -1)
                ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6005091"));
            else if ((part.Resources.Get(resID)?.amount ?? 0) >= resourceCost) {
                part.RequestResource(resID, resourceCost, ResourceFlowMode.NO_FLOW);
                inv.StoreCargoPartAtSlot(partName, slot);
            } else ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_247970"));
        }

    }

    public class ModuleEnginesFX2KW : ModuleEnginesFX {
        [KSPField(isPersistant = true)]
        public int ignitions;
        [KSPField(isPersistant = true)]
        public bool canRepair;
        [KSPField(guiActive = true, guiName = "#KWLOC_ignitionChance")]
        public string ignitionChance;
        [KSPField]
        public int maxRestarts;
        private bool ignited;
        private float minThrottle;
        public override void OnStart(StartState state) {
            base.OnStart(state);
            if (HighLogic.LoadedSceneIsFlight) {
                minThrottle = throttleMin;
                ToggleRepairUI(canRepair);
                if (maxRestarts == -1) Fields[nameof(ignitionChance)].guiActive = false;
            }
        }
        public new void FixedUpdate() {
            base.FixedUpdate();
            if (KWUtil.GenOpts().stockThrottle && HighLogic.LoadedSceneIsFlight && FlightGlobals.ready) {
                if (part.vessel.ctrlState.mainThrottle > 0f)
                    throttleMin = minThrottle;
                else throttleMin = 0f;
                if (maxRestarts > -1)
                    if (GetCurrentThrust() > 0f) {
                        if (!ignited) RunIgnitionCheck();
                    } else ignited = false; }
        }
        public override string GetInfo() => (propellants.Count > 1 ? $"{propellants[0].displayName} + {propellants[1].displayName} ({propellants[0].ratio}:{propellants[1].ratio})\n" : "") +
            (maxRestarts > -1 ? Localizer.Format("#KWLOC_avgIgntions", (maxRestarts / 2) + "\n\n") : "\n") + base.GetInfo();
        [KSPEvent(guiActive = true, guiName = "#autoLOC_6001382")]
        public override void Activate() {
            base.Activate();
            if (!KWUtil.GenOpts().stockThrottle && finalThrust == 0 && maxRestarts > -1)
                RunIgnitionCheck();
        }
        public void RunIgnitionCheck() {
            if (ignitions > UnityEngine.Random.Range(maxRestarts / 2, maxRestarts))
                StartCoroutine(CallbackUtil.DelayedCallback(UnityEngine.Random.Range(1, maxRestarts * 10), delegate { part.explode(); }));
            else if (ignitions > UnityEngine.Random.Range(0, maxRestarts) - (FlightGlobals.ship_geeForce > 0.025 ? 0 : maxRestarts / 2))
                StartCoroutine(CallbackUtil.DelayedCallback(UnityEngine.Random.Range(1, maxRestarts * 5), delegate { FailIgnition(); }));
            ignited = true;
            ignitions++;
        }
        private void FailIgnition() {
            ToggleRepairUI(true);
            Shutdown();
            ScreenMessages.PostScreenMessage(
                $"{Localizer.Format("#autoLOC_236416")} {part.partInfo.title} {Localizer.Format("#autoLOC_7001053")} {Localizer.Format("#autoLOC_8003101")}");
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
            if (ignitions == 0)
                ToggleRepairUI(false);
        }
        private void ToggleRepairUI(bool toggle) {
            canRepair = toggle;
            Events["Repair"].guiActiveUnfocused = toggle;
        }
        public void UpdateUI() => ignitionChance = $"{Mathf.Clamp(((FlightGlobals.ship_geeForce > 0.025 ? maxRestarts : maxRestarts * 0.5f) - ignitions) * (1f / maxRestarts), 0, 1):P0}";

    }

    public class ModuleKWIgnitors : PartModule {
        [KSPField(isPersistant = true)]
        public int ignitions;
        [KSPField(isPersistant = true)]
        public bool canRepair;
        [KSPField(guiActive = true)]
        public string ignitionChance;
        [KSPField]
        public int maxLife;

        private ModuleEngines engine;
        private float minThrottle;
        private bool ignited;

        // separate out throttle fix
        // how to know which engine

        // extend moduleengines

        public override void OnStart(StartState state) {
            if (HighLogic.LoadedSceneIsFlight) {
                engine = part.FindModulesImplementing<ModuleEngines>().First(e =>
                    new EngineType[3] { EngineType.Electric, EngineType.LiquidFuel, EngineType.Nuclear }.Contains(e.engineType));
                minThrottle = engine.throttleMin;
                GameEvents.onEngineActiveChange.Add(ActivateEngine);
                // GameEvents.onPartActionUICreate.Add(CreatePAW);
                Events["Repair"].guiActiveUnfocused = canRepair;
            }
            // Events["EstimateIgnitionChance"].guiName = $"{Localizer.Format("#autoLOC_7001053")} {Localizer.Format("#autoLOC_8000199")} {Localizer.Format("#autoLOC_8004209")}";
            // Fields["ignitionChance"].guiName = $"{Localizer.Format("#autoLOC_7001053")} {Localizer.Format("#autoLOC_8000199")}";
        }
        public void OnDestroy() {
            if (HighLogic.LoadedSceneIsFlight) {
                GameEvents.onEngineActiveChange.Remove(ActivateEngine);
                // GameEvents.onPartActionUICreate.Remove(CreatePAW);
            }
        }
        public override string GetModuleDisplayName() => Localizer.Format("Ignitors");
        public override string GetInfo() => "average life span: " + maxLife / 2;

        //private void ActivateEngine(ModuleEngines engine) {
        //    if (engine == part.FindModuleImplementing<ModuleEngines>() && engine.finalThrust == 0) {
        //        if (ignitions > UnityEngine.Random.Range(maxLife / 2, maxLife))
        //            StartCoroutine(CallbackUtil.DelayedCallback(UnityEngine.Random.Range(1, maxLife * 10), delegate { engine.part.explode(); }));
        //        else if (ignitions > UnityEngine.Random.Range(0, maxLife) - (FlightGlobals.ship_geeForce > 0.025 ? 0 : maxLife / 2))
        //            StartCoroutine(CallbackUtil.DelayedCallback(UnityEngine.Random.Range(1, maxLife * 5), delegate { FailIgnition(engine); }));
        //        ignitions++;
        //        // if (kwIgn.Fields["ignitionChance"].guiActive) kwIgn.ToggleUI();
        //    }
        //}

        //private void CreatePAW(Part part) {
        //    if (part == this.part)
        //        ignitionChance = $"{Mathf.Clamp(((FlightGlobals.ship_geeForce > 0.025 ? maxLife : maxLife * 0.5f) - ignitions) * (1f / maxLife), 0, 1):P0}";
        //}
        private void ActivateEngine(ModuleEngines engine) {
            if (!KWUtil.GenOpts().stockThrottle && engine == this.engine && engine.finalThrust == 0)
                RunIgnitionCheck();
        }

        public void RunIgnitionCheck() {
            if (ignitions > UnityEngine.Random.Range(maxLife / 2, maxLife))
                StartCoroutine(CallbackUtil.DelayedCallback(UnityEngine.Random.Range(1, maxLife * 10), delegate { engine.part.explode(); }));
            else if (ignitions > UnityEngine.Random.Range(0, maxLife) - (FlightGlobals.ship_geeForce > 0.025 ? 0 : maxLife / 2))
                StartCoroutine(CallbackUtil.DelayedCallback(UnityEngine.Random.Range(1, maxLife * 5), delegate { FailIgnition(engine); }));
            ignited = true;
            ignitions++;
        }
        public void FixedUpdate() {
            if (KWUtil.GenOpts().stockThrottle && HighLogic.LoadedSceneIsFlight && FlightGlobals.ready) {
                if (part.vessel.ctrlState.mainThrottle > 0f)
                    engine.throttleMin = minThrottle;
                else engine.throttleMin = 0f;
                if (engine.GetCurrentThrust() > 0f) {
                    if (ignited == false)
                        RunIgnitionCheck();
                } else ignited = false;
            }
        }
        private void FailIgnition(ModuleEngines engine) {
            ToggleRepairUI(true);
            engine.Shutdown();
            ScreenMessages.PostScreenMessage(
                $"{Localizer.Format("#autoLOC_236416")} {engine.part.partInfo.title} {Localizer.Format("#autoLOC_7001053")} {Localizer.Format("#autoLOC_8003101")}");
        }

        [KSPEvent(guiName = "#autoLOC_8000063")]
        public void Repair() {
            if (!FlightGlobals.ActiveVessel.isEVA || (HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().KerbalExperienceEnabled(HighLogic.CurrentGame.Mode)
                && FlightGlobals.ActiveVessel.VesselValues.RepairSkill.value < 1)) {
                    ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_246904", "1"));
                    return; }
            else if (FlightGlobals.ActiveVessel.evaController.ModuleInventoryPartReference.TotalAmountOfPartStored("evaRepairKit") < 1) {
                ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6006097", new string[2] { "1", "#autoLOC_6005094" }));
                return; }
            FlightGlobals.ActiveVessel.evaController.ModuleInventoryPartReference.RemoveNPartsFromInventory("evaRepairKit", 1, true);
            ignitions--;
            if (ignitions == 0)
                ToggleRepairUI(false);
        }
        private void ToggleRepairUI(bool toggle) {
            canRepair = toggle;
            Events["Repair"].guiActiveUnfocused = toggle;
        }
        public void UpdateUI() => ignitionChance = $"{Mathf.Clamp(((FlightGlobals.ship_geeForce > 0.025 ? maxLife : maxLife * 0.5f) - ignitions) * (1f / maxLife), 0, 1):P0}";

        // public void EstimateIgnitionChance() =>
        // ignitionChance = $"{Mathf.Clamp(((FlightGlobals.ship_geeForce > 0.025 ? maxLife : maxLife * 0.5f) - ignitions) * (1f / maxLife), 0, 1):P0}";
        // ToggleUI();
        // + UnityEngine.Random.Range(-1f, 1f))

        // public void ToggleUI() {
        // Fields["ignitionChance"].guiActive = !Fields["ignitionChance"].guiActive;
        // Events["EstimateIgnitionChance"].guiActive = !Events["EstimateIgnitionChance"].guiActive;
        // }

    }

    public class ModuleKWCabin : PartModule {

        [KSPField(guiActive = true, guiName = "#autoLOC_6001352")]
        public string status;

        // [KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6100039")]
        // public string cabPrs;
        [UI_FloatRange(affectSymCounterparts = UI_Scene.Editor, maxValue = 1f, stepIncrement = 0.01f)]
        [KSPField(guiActive = true, guiActiveEditor = true, guiFormat = "0.00", guiName = "", guiUnits = "#autoLOC_7001419")]
        public float cabPrsAtm = 1f;
        [UI_MinMaxRange(affectSymCounterparts = UI_Scene.Editor, minValueX = 0f, minValueY = 0f, maxValueX = 100f, maxValueY = 100f, stepIncrement = 1f)]
        [KSPField(guiActive = true, guiActiveEditor = true, guiFormat = "0", guiName = "", guiUnits = "%")]
        public Vector2 cabComp = new Vector2(79f, 100f);
        public bool showStockControls;
        // [KSPField(guiActive = false, guiActiveEditor = true, guiName = "")]
        // public string capCompDesc = "21% Ox | 1% Tox";

        public override void OnStart(StartState state) {
            Fields["cabComp"].OnValueModified += new Callback<object>(UpdateCabComp);
            Fields["cabPrsAtm"].OnValueModified += new Callback<object>(UpdateCabComp);

            UpdateUIText();
        }

        public void OnDestroy() {
            
            Fields["cabComp"].OnValueModified -= new Callback<object>(UpdateCabComp);
            Fields["cabPrsAtm"].OnValueModified -= new Callback<object>(UpdateCabComp);
            
        }

        public override string GetInfo() {
            return "Max Prs: 20 atm / 2026.5 kPa \n\n<color=#99FF00>Composition:</color>\n-Inert Gas\n-Ox\n-Toxic Gas";
            // return resHandler.PrintModuleResources();
        }

        public override string GetModuleDisplayName() => Localizer.Format("#KWLOC_pressurisedCabin");

        public void FixedUpdate() {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready) {

                //double fixedDeltaTime = TimeWarp.fixedDeltaTime;
                //List<ModuleResource> resList = resHandler.inputResources;
                //// pull oxygen
                //// if none, pull air
                //// if prs < kerbaleva cutoff, reduce hp down to 50%
                //// if prs < armstrong limit, reduce hp to 0%
                //// define a dict(?) determining what to do with shortages
                //foreach (ModuleResource res in resHandler.inputResources) {
                //    double amt = res.rate * part.protoModuleCrew.Count * fixedDeltaTime;
                //    if (part.RequestResource(res.id, amt) < amt) { }
                //}

                resHandler.UpdateModuleResourceInputs(ref status, part.protoModuleCrew.Count, 0, false);

                resHandler.UpdateModuleResourceOutputs();


                // get shortage types
                // subtract hp based on types
                // when hp hits zero, die
            }
        }

        private void UpdateCabComp(object field) {

            // in editor -> set res amounts based on slider values
            // in flight -> lock values to amounts based on resources
            // add button to unlock values for editing
            double oneAtmCab = part.Resources.Get("Oxidizer").maxAmount * 0.2;
            if (HighLogic.LoadedSceneIsEditor) {
                part.Resources.Get("InertGas").amount = cabPrsAtm * oneAtmCab * cabComp.x * 0.01;
                part.Resources.Get("Oxidizer").amount = cabPrsAtm * oneAtmCab * (cabComp.y - cabComp.x) * 0.01;
                part.Resources.Get("ToxicGas").amount = cabPrsAtm * oneAtmCab * (100 - cabComp.y) * 0.01;
            } else UpdateUIAir();
            UpdateUIText();
        }

        [KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "")]
        public void ToggleControls() {
            showStockControls = !showStockControls;
            foreach (string fName in new string[2] { "cabPrsAtm", "cabComp" }) {
                Fields[fName].guiActive = !showStockControls;
                Fields[fName].guiActiveEditor = !showStockControls; }
            if (!showStockControls) HideStockControls();
            else UpdateUIAir();
        }
        public void HideStockControls() {
            foreach (string res in new string[4] { "InertGas", "Oxidizer", "ToxicGas", "HydroFuel" })
                part.PartActionWindow.RemoveResourceControlEditor(part.Resources.Get(res));
        }
        private void UpdateUIAir() {
            double total = part.Resources.Get("InertGas").amount + part.Resources.Get("Oxidizer").amount + part.Resources.Get("ToxicGas").amount;
            cabPrsAtm = (float)(total / (part.Resources.Get("Oxidizer").maxAmount * 0.2));
            cabComp.x = (float)(part.Resources.Get("InertGas").amount / total) * 100f;
            cabComp.y = (float)((total - part.Resources.Get("ToxicGas").amount) / total) * 100f;
        }

        private void UpdateUIText() {
            // StartCoroutine(CallbackUtil.DelayedCallback(1, delegate {
            Events["ToggleControls"].guiName = $"{Localizer.Format("#autoLOC_6001329")} {Localizer.Format("#KWLOC_cabinPressure")} {Localizer.Format("#autoLOC_6001628")}"; 
            Fields["cabPrsAtm"].guiName = $"{Localizer.Format("#KWLOC_cabinPressure")} ({cabPrsAtm * 101.325:N0} {Localizer.Format("#autoLOC_7001410")})";
            Fields["cabComp"].guiName = $"{(cabComp.y - cabComp.x) * 0.01:P0} {Localizer.Format("#autoLOC_6002096")} / " +
                $"{(100 - cabComp.y) * 0.01:P0} {Localizer.Format("#KWLOC_toxicGasShort")}";
            // }));
        }


        // public void UpdateCabPrs() => cabPrs = Localizer.Format("#autoLOC_5050036", (KWUtil.GetCabPrskPa(part) / 101.325).ToString("N2"));

    }

    public class ModuleKWGrowthOrganism : PartModule {
        [KSPField(guiActive = true, guiFormat = "0.0", guiName = "growth rate", guiUnits = "%")]
        public float growthRate;
        // [KSPField(guiActive = true, guiName = "feedback test")]
        // public string feedback;

        [KSPField]
        public string growthResourceName;

        private PartResource growthRes;

        public override void OnStart(StartState state) {
            growthRes = part.Resources.Get(growthResourceName);
        }

        public void FixedUpdate() {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready) {
                double fixedDeltaTime = TimeWarp.fixedDeltaTime;
                growthRate = (float)(growthRes.amount / growthRes.maxAmount);
                // feedback = "";
                // double inputRateTotal = 1.0;
                if (growthRate > 0f)
                    foreach (ModuleResource inputRes in resHandler.inputResources) {
                        double maxAmt = inputRes.rate * fixedDeltaTime;
                        //double inputConsumed = part.RequestResource(inputRes.id, maxAmt * growthRate);
                        //feedback += inputRes.name.Substring(0, 1) + "=" + inputConsumed.ToString("N4") + "|";
                        double consumedPct = part.RequestResource(inputRes.id, maxAmt * growthRate) / maxAmt;
                        if (consumedPct < growthRate) growthRate = (float)consumedPct;
                    }
                if (growthRate > 0f)
                    foreach (ModuleResource outputRes in resHandler.outputResources)
                        // double amt = outputRes.rate * growthRate * fixedDeltaTime;
                        part.RequestResource(outputRes.id, -outputRes.rate * growthRate * fixedDeltaTime);
                        //part.TransferResource(outputRes.id, outputRes.rate * growthRate * fixedDeltaTime);
                        // feedback += outputRes.name.Substring(0, 1) + "=" + amt.ToString("N4") + "|";
                    
                    //if (outputRes.name == growthResourceName) {
                    //    double amt = outputRes.rate * growthRate * fixedDeltaTime;
                    //    part.TransferResource(outputRes.id, amt);
                    //    feedback += growthResourceName.Substring(0, 1) + "=" + amt.ToString("N2") + "|";
                    //} else {
                    //    //part.TransferResource(outputRes.id, outputRes.rate * outputPct * fixedDeltaTime);
                    //    //feedback += outputRes.name.Substring(0, 1) + "=" + (outputRes.rate * outputPct * fixedDeltaTime).ToString("N2");
                    //}
                growthRate *= 100;
            }
        }
        public override string GetModuleDisplayName() => Localizer.Format("#KWLOC_livingOrganism");
        public override string GetInfo() {
            string content = $"<color=#99FF00>{Localizer.Format("#autoLOC_456381", "#autoLOC_900348")}</color>\n";
            foreach (ModuleResource inputRes in resHandler.inputResources)
                content += KWUtil.FormatResource(inputRes);
            content += $"\n<color=#99FF00>{Localizer.Format("#autoLOC_900348")} {Localizer.Format("#autoLOC_244333")}</color>\n";
            foreach (ModuleResource outputRes in resHandler.outputResources)
                content += KWUtil.FormatResource(outputRes);
            return content;
        }

    }

    public class ModuleKWEVA : PartModule {

        [KSPField(guiActive = true, guiName = "#autoLOC_6001352")]
        public string status;

        private bool isHelmetOn;
        private KerbalEVA eva;
        public override void OnStart(StartState state) {
            GameEvents.OnHelmetChanged.Add(HelmetChange);
            eva = part.FindModuleImplementing<KerbalEVA>();
            status = "Nominal";
        }
        public void OnDestroy() => GameEvents.OnHelmetChanged.Remove(HelmetChange);


        public override string GetInfo() {

            return resHandler.PrintModuleResources();
        }

        public void FixedUpdate() {
            if (isHelmetOn) {
                string err = string.Empty;
                resHandler.UpdateModuleResourceInputs(ref err, part.protoModuleCrew.Count, 0, false);
            }
            PartResource suitAir = part.Resources.Get("IntakeAir"); // PartResourceLibrary.Instance.GetDefinition("IntakeAir").id
            float suitAirPrsAtm = (float)(suitAir.amount / suitAir.maxAmount);
            if (suitAirPrsAtm < eva.helmetOffMinSafePressureAtm) {
                KWHealth.ModifyHP(part.vessel.vesselName, 0);
                status = "Not enough Air";
                // if prs below 0.0618 atm -> die in 90s max
                // else minor hp loss (die in no less than 60s from lower limit)
            }
            if (!isHelmetOn || suitAirPrsAtm > eva.helmetOffMinSafePressureAtm)
                KWHealth.ModifyHP(part.vessel.vesselName, 0);
        }
        private void HelmetChange(KerbalEVA kerbal, bool isHelmetOn, bool neckRingOn) => this.isHelmetOn = isHelmetOn;

    }

    // add auto empty on new resource fill
    // add option to auto-hide empty

    public class ModuleKWResourceToggle : PartModule {
        // private int hydroID;
        [KSPField]
        public string toggleName = "#autoLOC_502068";
        [KSPField]
        public string resourceNames;
        [KSPField]
        public bool hideInFlight;
        [KSPField]
        public bool hideAll;
        [KSPField(isPersistant = true)]
        public int currentIndex;

        // public PartResource[] resources;
        public override void OnStart(StartState state) {
            // GameEvents.onPartActionUICreate.Add(CreatePAW);
            PartResource[] resources = Resources();
            if (state == StartState.Editor && resources.All(r => r.flowState) && !hideAll)
                foreach (PartResource res in resources.Where(r => resources.IndexOf(r) != currentIndex)) // .Where(r => (tankIsEmpty && r.resourceName != "LiquidFuel") || (!tankIsEmpty && r.amount == 0))
                    res.flowState = false;
            // res.flowState = res.amount > 0 || (AllAreEmpty() && resources.IndexOf(res) == 0);
            // res.amount = 0;
            // hydroID = PartResourceLibrary.Instance.GetDefinition("HydroFuel").id;
            Events[nameof(ToggleResource)].guiName = toggleName;
        }
        // if amount == 0 hide if (all == 0 && name != liquidfuel) || (all != 0 && amount == 0)
        //public void OnDestroy() {
        //    GameEvents.onPartActionUICreate.Remove(CreatePAW);
        //}
        //public void FixedUpdate() {
        //    if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready && part.Resources.Contains(hydroID))
        //        if (part.Resources.Get(hydroID).amount > 0)
        //            part.RequestResource(hydroID, TimeWarp.fixedDeltaTime * 0.00000005787 * part.Resources.Get(hydroID).amount, ResourceFlowMode.NO_FLOW);
        //}
        // public override string GetModuleDisplayName() => $"{Localizer.Format("#autoLOC_6002649")} {Localizer.Format(displayName)}";
        // public override string GetInfo() => $"{Localizer.Format("#autoLOC_444646")}: {resources}";

        [KSPEvent(guiActiveEditor = true)]
        public void ToggleResource() {
            foreach (Part part in part.symmetryCounterparts.Append(part)) {
                PartResource[] resources = part.Resources.Where(r => resourceNames.Contains(r.resourceName)).ToArray();
                resources.ElementAt(currentIndex).flowState = false;
                resources.ElementAt(currentIndex).amount = 0;
                ModuleKWResourceToggle resToggle = part.FindModuleImplementing<ModuleKWResourceToggle>();
                resToggle.currentIndex++;
                if (resToggle.currentIndex == resources.Length)
                    resToggle.currentIndex = 0;

                if (HighLogic.LoadedSceneIsEditor && !resources.ElementAt(currentIndex).isVisible)
                    resToggle.currentIndex++;

                resources.ElementAt(currentIndex).flowState = true;
            }
            //PartResource[] resources = Resources();
            //// if (resources.First(r => r.flowState).amount == 0) { // IsTankEmpty()
            //foreach (Part part in part.symmetryCounterparts.Append(part)) {
            //    int index = Array.FindIndex(resources, r => r.flowState);
            //    resources.ElementAt(index).flowState = false;
            //    resources.ElementAt(index).amount = 0;
            //    if (index < resources.Length - 1)
            //        resources[index + 1].flowState = true;
            //    else resources[0].flowState = true; }
            //// } else ScreenMessages.PostScreenMessage("tank must be empty");
        }
        //private void CreatePAW(Part part) {
        //    // try using uipartactioncontroller onhideresource method
        //    if (part == this.part) {
        //        // Debug.Log(part.transform.name);
        //        if (HighLogic.LoadedSceneIsFlight)
        //            Events["ToggleResource"].guiActive = AreAllEmpty();
        //        StartCoroutine(CallbackUtil.DelayedCallback(1, delegate {
        //            foreach (PartResource res in Resources().Where(r => !r.flowState))
        //                if (HighLogic.LoadedSceneIsEditor)
        //                    part.PartActionWindow.RemoveResourceControlEditor(res);
        //                else {
        //                    part.PartActionWindow.RemoveResourceControlFlight(res);
        //                    part.PartActionWindow.RemoveResourceTransferControl(res);
        //                }
        //        }));
        //    }
        //}
        public PartResource[] Resources() => part.Resources.Where(r => resourceNames.Contains(r.resourceName)).ToArray();
        public bool AllAreEmpty() => Resources().All(r => r.amount == 0.0);
        public void UpdateUI() {
            bool flightScene = HighLogic.LoadedSceneIsFlight;
            PartResource[] resources = Resources();
            if (flightScene)
                Events[nameof(ToggleResource)].guiActive = !hideAll && AllAreEmpty() && !hideInFlight;
            else Events[nameof(ToggleResource)].guiActiveEditor = !hideAll;
            foreach (PartResource res in resources.Where(r => resources.IndexOf(r) != currentIndex || hideAll))
                if (flightScene) {
                    part.PartActionWindow.RemoveResourceControlFlight(res);
                    part.PartActionWindow.RemoveResourceTransferControl(res);
                } else part.PartActionWindow.RemoveResourceControlEditor(res);
        }
        
        //public void UpdateUI() {
        //    if (HighLogic.LoadedSceneIsFlight)
        //        Events["ToggleResource"].guiActive = AllAreEmpty();
        //    foreach (PartResource res in Resources().Where(r => !r.flowState && r.amount == 0))
        //        if (Resources().Where(r => r.flowState).Count() == 0 && Resources().IndexOf(res) == 0) continue;
        //        else if (HighLogic.LoadedSceneIsEditor)
        //            part.PartActionWindow.RemoveResourceControlEditor(res);
        //        else {
        //            part.PartActionWindow.RemoveResourceControlFlight(res);
        //            part.PartActionWindow.RemoveResourceTransferControl(res);
        //        }

        //}
    }

    public class ModuleKWNonCryoContainer : PartModule {
        private PartResource cryoRes;
        // private PartResource gasRes;
        // private int gasID;
        public override void OnStart(StartState state) {
            GameEvents.onPartResourceEmptyNonempty.Add(ResourceCheck);
            GameEvents.onPartResourceNonemptyEmpty.Add(ResourceCheck);
            cryoRes = part.Resources.Get("CryoFuel");
            // gasRes = part.Resources.Get("HydroGas");
            // gasID = PartResourceLibrary.Instance.GetDefinition("HydroGas").id;
            FuelCheck();
        }
        public void OnDestroy() {
            GameEvents.onPartResourceEmptyNonempty.Remove(ResourceCheck);
            GameEvents.onPartResourceNonemptyEmpty.Remove(ResourceCheck);
        }
        public override string GetModuleDisplayName() => Localizer.Format("#KWLOC_nonCryoContainer");
        public override string GetInfo() => Localizer.Format("#KWLOC_boiloffRates");

        public override void OnFixedUpdate() {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready && cryoRes.amount > 0) {
                double amount = part.RequestResource("CryoFuel", cryoRes.amount * 0.00000005788 * TimeWarp.fixedDeltaTime, ResourceFlowMode.NO_FLOW);
                part.RequestResource("HydroGas", -amount);
            }
        }
        private void ResourceCheck(PartResource res) {
            if (res == cryoRes)
                FuelCheck();

        }

        private void FuelCheck() {
            if (cryoRes.amount > 0f)
                part.force_activate(false);
            else part.deactivate();

        }
        //public void UpdateUI() {
        //    if (HighLogic.LoadedSceneIsFlight) {
        //        part.PartActionWindow.RemoveResourceControlFlight(gasRes);
        //        part.PartActionWindow.RemoveResourceTransferControl(gasRes);
        //    } else part.PartActionWindow.RemoveResourceControlEditor(gasRes);
        //}
    }

}