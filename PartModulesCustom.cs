
using KSP.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

    public class ModuleKWCabin : PartModule {

        //[KSPField(guiActive = true, guiName = "#autoLOC_6001352")]
        //public string status;

        // [KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6100039")]
        // public string cabPrs;
        [UI_FloatRange(affectSymCounterparts = UI_Scene.Editor, maxValue = 1f, stepIncrement = 0.01f)]
        [KSPField(guiActive = true, guiActiveEditor = true, guiFormat = "0.00", guiName = "", guiUnits = "#autoLOC_7001419")]
        public float cabPrsAtm = 1f;
        [UI_MinMaxRange(affectSymCounterparts = UI_Scene.Editor, minValueX = 0f, minValueY = 0f, maxValueX = 100f, maxValueY = 100f, stepIncrement = 1f)]
        [KSPField(guiActive = true, guiActiveEditor = true, guiFormat = "0", guiName = "", guiUnits = "%")]
        public Vector2 cabComp = new Vector2(79f, 99f);
        //[KSPField(guiActive = true, guiFormat = "0.0", guiName = "#KWLOC_cabinTemp", guiUnits = "#autoLOC_7001419")]
        //public float cabTemp;

        private bool showStockControls;
        private Dictionary<string, ProtoPartResourceSnapshot[]> invOx;
        private Dictionary<string, Kerb> kerbs;
        private ModuleResource oxInput;
        // private ModuleResource edInput;
        // private ModuleResource aqInput;
        //private bool airIntake;
        // private Dictionary<ProtoCrewMember, double> healthLoss;
        // [KSPField(guiActive = false, guiActiveEditor = true, guiName = "")]
        // public string capCompDesc = "21% Ox | 1% Tox";

        public override void OnStart(StartState state) {
            Fields["cabComp"].OnValueModified += new Callback<object>(UpdateCabComp);
            Fields["cabPrsAtm"].OnValueModified += new Callback<object>(UpdateCabComp);
            oxInput = resHandler.inputResources[0];
            invOx = new Dictionary<string, ProtoPartResourceSnapshot[]>();
            kerbs = new Dictionary<string, Kerb>();
            UpdateCabComp(null);
            // healthLoss = part.protoModuleCrew.ToDictionary(c => c, c => 0.0);
            // oxInput = resHandler.inputResources[0];
            // edInput = resHandler.inputResources[1];
            // aqInput = resHandler.inputResources[2];
        }
        public void OnDestroy() {
            Fields["cabComp"].OnValueModified -= new Callback<object>(UpdateCabComp);
            Fields["cabPrsAtm"].OnValueModified -= new Callback<object>(UpdateCabComp);
        }
        public override string GetInfo() => Localizer.Format("#KWLOC_pressurisedCabin_info");
        public override string GetModuleDisplayName() => Localizer.Format("#KWLOC_pressurisedCabin");

        public void FixedUpdate() {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready) {
                float deltaT = TimeWarp.fixedDeltaTime;
                foreach (ProtoCrewMember crew in part.protoModuleCrew) {
                    if (!kerbs.ContainsKey(crew.name)) kerbs[crew.name] = KWKerbs.GetKerb(crew);
                    if (crew.hasHelmetOn) {
                        if (!invOx.ContainsKey(crew.name)) UpdateInvOx(crew);
                        if (invOx[crew.name].Length > 0) {
                            invOx[crew.name][0].amount -= oxInput.rate * deltaT;
                            if (invOx[crew.name][0].amount < 0) {
                                invOx[crew.name][0].amount = 0;
                                UpdateInvOx(crew); }
                            if (kerbs[crew.name].OxLoss > 0) kerbs[crew.name].AddOxLoss((0.1f + crew.courage) * -75 * deltaT);
                        } else {
                            kerbs[crew.name].AddOxLoss((1.1f - crew.courage) * 50 * deltaT);
                        }
                    } else if (!CabCompSafe())
                        kerbs[crew.name].AddOxLoss((1.1f - crew.courage) * 50 * deltaT);
                    else {
                        part.RequestResource(oxInput.id, oxInput.rate * deltaT, ResourceFlowMode.NO_FLOW);
                        part.RequestResource(resHandler.outputResources[0].id, -resHandler.outputResources[0].rate * deltaT, ResourceFlowMode.NO_FLOW);
                        if (kerbs[crew.name].OxLoss > 0) kerbs[crew.name].AddOxLoss((0.1f + crew.courage) * -75 * deltaT);
                    }
                    crew.gExperienced += kerbs[crew.name].OxLoss;
                    if (crew.GExperiencedNormalized > 1.5 && HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().GKerbalLimits)
                        crew.Die();
                    
                }
            }
        }

        private void UpdateInvOx(ProtoCrewMember crew) {
            invOx[crew.name] = KWUtil.GetInvResources("Oxygen", crew.KerbalInventoryModule).Where(r => r.amount > 0).ToArray();

        }

        //public void FixedUpdate() {
        //    float deltaT = TimeWarp.fixedDeltaTime;
        //    // Debug.Log(deltaT);
        //    if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready && part.protoModuleCrew.Count > 0) {
        //        foreach (ProtoCrewMember crew in part.protoModuleCrew) {
        //            Kerb kerb = KWKerbs.GetKerb(crew);
        //            if (crew.hasHelmetOn) {
        //                double reqOx = oxInput.rate * deltaT;
        //                double consumedOxPct = part.RequestResource(oxInput.id, reqOx) / reqOx;
        //                if (consumedOxPct < 0.999) kerb.AddOxLoss((1.1f - crew.courage) * (1f - consumedOxPct) * 50 * deltaT);
        //                else if (kerb.OxLoss > 0) kerb.AddOxLoss((0.1f + crew.courage) * -50 * deltaT);
        //            } else if (!CabCompSafe()) kerb.AddOxLoss((1.1f - crew.courage) * 50 * deltaT);
        //            else {
        //                part.RequestResource(oxInput.id, oxInput.rate * deltaT, ResourceFlowMode.NO_FLOW);
        //                part.RequestResource(resHandler.outputResources[0].id, -resHandler.outputResources[0].rate * deltaT, ResourceFlowMode.NO_FLOW);
        //                if (kerb.OxLoss > 0) kerb.AddOxLoss((0.1f + crew.courage) * -50 * deltaT); }
        //            crew.gExperienced += kerb.OxLoss;
        //            if (crew.GExperiencedNormalized > 1.5 && HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().GKerbalLimits) crew.Die();

        //            //double reqAmt = aqInput.rate * deltaT;
        //            //double consumedPct = part.RequestResource(aqInput.id, reqAmt) / reqAmt;
        //            //if (consumedPct < 0.999) kerb.AddAqLoss((1f - consumedPct) * 0.01f * deltaT);
        //            //else if (kerb.EdLoss > 0) kerb.AddAqLoss(-0.01f * deltaT);
        //            //crew.gExperienced += kerb.AqLoss;

        //            //reqAmt = edInput.rate * deltaT;
        //            //consumedPct = part.RequestResource(edInput.id, reqAmt) / reqAmt;
        //            //if (consumedPct < 0.999) kerb.AddEdLoss((1f - consumedPct) * 0.001f * deltaT);
        //            //else if (kerb.EdLoss > 0) kerb.AddEdLoss(-0.001f * deltaT);
        //            //crew.gExperienced += kerb.EdLoss; 

        //        }
        //    }
        //}

        //[KSPEvent(guiActive = true, guiName = "#autoLOC_8003231")]
        //public void ToggleHelmets() {
        //    bool setHelmets = !part.protoModuleCrew[0].hasHelmetOn;
        //    foreach (ProtoCrewMember crew in part.protoModuleCrew) {
        //        //if (crew.KerbalInventoryModule.TotalAmountOfPartStored("KWhelmet") == 0) {
        //        //    ScreenMessages.PostScreenMessage($"{crew.displayName}: {Localizer.Format("#autoLOC_261263", "KWLOC_helmet")}");
        //        //    continue; }
        //        crew.KerbalRef.helmetTransform.gameObject.SetActive(setHelmets);
        //        crew.KerbalRef.showHelmet = setHelmets;
        //        crew.hasHelmetOn = setHelmets;
        //        // GameEvents.OnHelmetChanged.Fire(null, setHelmets, true);
        //    }
        //}
        //[KSPEvent(guiActive = true, guiName = "#autoLOC_6001425")]
        //public void ToggleAirIntake() {
        //    airIntake = !airIntake;
        //    Events[nameof(ToggleAirIntake)].guiName = $"{Localizer.Format(airIntake ? "#autoLOC_6001426" : "#autoLOC_6001427")} ({Localizer.Format("#autoLOC_6002101")})";
        //}
        public void UpdateCabComp(object field) {
            if (HighLogic.LoadedSceneIsEditor) {
                PartResource[] resources = Resources();
                double oneAtmCab = resources[0].maxAmount;
                resources[0].amount = Mathf.Clamp((float)(cabPrsAtm * oneAtmCab) * cabComp.x * 0.01f, 0f, (float)resources[0].maxAmount);
                resources[1].amount = Mathf.Clamp((float)(cabPrsAtm * oneAtmCab) * (cabComp.y - cabComp.x) * 0.01f, 0f, (float)resources[1].maxAmount);
                resources[2].amount = Mathf.Clamp((float)(cabPrsAtm * oneAtmCab) * (100 - cabComp.y) * 0.01f, 0f, (float)resources[2].maxAmount);
                if (Math.Abs(resources[0].maxAmount - resources[0].amount) < 0.001 || Math.Abs(resources[1].maxAmount - resources[1].amount) < 0.001) UpdateValues();
            } else if (!showStockControls) UpdateValues();
            UpdateUIText();
        }
        [KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001431")]
        public void ToggleStockControls() {
            showStockControls = !showStockControls;
            foreach (string fName in new string[2] { "cabPrsAtm", "cabComp" }) {
                Fields[fName].guiActive = !showStockControls;
                Fields[fName].guiActiveEditor = !showStockControls; }
            if (!showStockControls) {
                UpdateValues();
                UpdateUIText(); }
        }
        public void UpdateUI() {
            if (!showStockControls) {
                foreach (PartResource res in Resources())
                    if (HighLogic.LoadedSceneIsFlight) {
                        part.PartActionWindow.RemoveResourceControlFlight(res);
                        part.PartActionWindow.RemoveResourceTransferControl(res);
                    } else part.PartActionWindow.RemoveResourceControlEditor(res);
                if (HighLogic.LoadedSceneIsFlight) {
                    UpdateValues();
                    UpdateUIText(); }}
            //if (HighLogic.LoadedSceneIsFlight)
            //    Events[nameof(ToggleHelmets)].guiActive = part.protoModuleCrew.Count > 0;
        }
        public PartResource[] Resources() => part.Resources.Where(r => new string[3] { "InertGas", "Oxygen", "ToxicGas" }.Contains(r.resourceName)).ToArray();
        public void UpdateValues() {
            PartResource[] resources = Resources();
            double total = resources.Select(r => r.amount).Sum();
            cabPrsAtm = (float)(total / resources[0].maxAmount);
            cabComp.x = total > 0 ? (float)(resources[0].amount / total) * 100f : 0f;
            cabComp.y = total > 0 ? (float)((total - resources[2].amount) / total) * 100f : 0f;
        }
        public void UpdateUIText() {
            // StartCoroutine(CallbackUtil.DelayedCallback(1, delegate {
            Fields[nameof(cabPrsAtm)].guiName = $"{Localizer.Format("#KWLOC_cabinPressure")} ({cabPrsAtm * 101.325:N1} {Localizer.Format("#autoLOC_7001410")})";
            Fields[nameof(cabComp)].guiName = $"{cabComp.y - cabComp.x:N0}% {Localizer.Format("#autoLOC_6002096")} / " +
                $"{100 - cabComp.y:N0}% {Localizer.Format("#KWLOC_toxicGasShort")}";
            // }));
        }
        public bool CabCompSafe() {
            PartResource[] resources = Resources();
            UpdateValues();
            return cabPrsAtm > 0.177f && resources[1].amount > resources[0].maxAmount * 0.1f && cabComp.y > 92f; }
        // [KSPEvent(guiActive = true, guiName = "#KWLOC_ventCabin")]
        public void OpenCabin() {
            PartResource[] resources = Resources();
            string atmoRes = KWUtil.bodyResources[vessel.mainBody.name].GetValue("atmo");
            if (vessel.staticPressurekPa > 0 && (atmoRes == "IntakeAir" || atmoRes == "ToxicGas")) {
                double total = resources[0].maxAmount * (vessel.staticPressurekPa / 101.325);
                if (atmoRes == "ToxicGas") {
                    resources[0].amount = 0;
                    resources[1].amount = 0;
                    resources[2].amount = total;
                } else {
                    resources[0].amount = total * 0.7901;
                    resources[1].amount = total * 0.2094;
                    resources[2].amount = total * 0.0004; }
            } else resources.ToList().ForEach(r => r.amount = 0);
        }

    }

    public class ModuleKWEVA : PartModule {

        [KSPField(guiActive = true, guiName = "#KWLOC_oxygen")] // , guiUnits = "#autoLOC_6002340", guiFormat = "0 "
        public string timeLeft;
        //private bool isHelmetOn;
        //private KerbalEVA eva;

        private ModuleInventoryPart inv;
        private ModuleResource input;
        private ProtoCrewMember crew;
        private KerbalEVA eva;
        private Kerb kerb;
        private ProtoPartResourceSnapshot[] ox;
        // private PartResource oxTank;
        // public double healthLoss;

        public override void OnStart(StartState state) {
            inv = part.FindModuleImplementing<ModuleInventoryPart>();
            input = resHandler.inputResources[0];
            crew = part.protoModuleCrew[0];
            kerb = KWKerbs.GetKerb(crew);
            eva = part.FindModuleImplementing<KerbalEVA>();
            Fields[nameof(timeLeft)].guiName = $"{Localizer.Format("#KWLOC_oxygen")} {Localizer.Format("#autoLOC_266526")}";
            GameEvents.onModuleInventoryChanged.Add(ChangeInventory);
            GameEvents.onPartActionUICreate.Add(UpdateUI);
            ChangeInventory(inv);
            eva.Events["ChangeHelmet"].guiActive = false;
            // oxTank = part.Resources.Get(oxInput.id);
            //GameEvents.OnHelmetChanged.Add(HelmetChange);
            //eva = part.FindModuleImplementing<KerbalEVA>();
            //status = "Nominal";
        }
        // public void OnDestroy() => GameEvents.OnHelmetChanged.Remove(HelmetChange);

        //public override string GetInfo() {
        //    return resHandler.PrintModuleResources();
        //}

        public void OnDestroy() {
            GameEvents.onModuleInventoryChanged.Remove(ChangeInventory);
            GameEvents.onPartActionUICreate.Remove(UpdateUI);
        }

        public void FixedUpdate() {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready) {
                float deltaT = TimeWarp.fixedDeltaTime;
                if (crew.hasHelmetOn) {
                    //double reqAmt = oxInput.rate * deltaT;
                    //double consumedPct = part.RequestResource(oxInput.id, reqAmt) / reqAmt;
                    //if (consumedPct < 0.999) kerb.AddOxLoss((1.1f - crew.courage) * (1f - consumedPct) * 50 * deltaT);
                    //else if (kerb.OxLoss > 0) kerb.AddOxLoss((0.1f + crew.courage) * -80 * deltaT);
                    
                    //ProtoPartResourceSnapshot[] oxRes = inv.storedParts.Values.Select(p => p.snapshot).Where(s =>
                    //    s.resources[0]?.resourceName == "Oxygen" && s.resources[0]?.amount >= amt).OrderByDescending(s => s.resourcePriorityOffset).Select(s => s.resources[0]).ToArray();
                    if (ox.Length > 0) {
                        ox[0].amount -= input.rate * deltaT;
                        if (ox[0].amount < 0) {
                            ox[0].amount = 0;
                            UpdateOx(); }
                        // timeLeft = KSPUtil.PrintTimeCompact(oxRes.Select(r => r.amount).Sum() / resHandler.inputResources[0].rate, true);
                        if (kerb.OxLoss > 0) kerb.AddOxLoss((0.1f + crew.courage) * -75 * deltaT);
                    } else {
                        // timeLeft = KSPUtil.PrintTimeCompact(0, true);
                        kerb.AddOxLoss((1.1f - crew.courage) * 50 * deltaT);
                        if (crew.GExperiencedNormalized > 1.5 && HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().GKerbalLimits) {
                            crew.Die();
                            part.explode(); }}
                } else if (kerb.OxLoss > 0) kerb.AddOxLoss((0.1f + crew.courage) * -75 * deltaT);
                
                crew.gExperienced += kerb.OxLoss;
            }
            // courage = 1 -> 1477s ; 0 -> 77s

            //if (consumedPct < 0.999) healthLoss += (1.1f - crew.courage) * (1f - consumedPct) * 50 * deltaT;
            //else if (healthLoss > 0) healthLoss -= (0.1f + crew.courage) * 50 * deltaT;
            //if (oxTank.amount > 0) {
            //    if (health > 0) health -= (0.1 + part.protoModuleCrew[0].courage) * 1000 * deltaTime;
            //} else {
            //    health += (1.1 - part.protoModuleCrew[0].courage) * 1000 * deltaTime;
            //    part.protoModuleCrew[0].gExperienced += health;
            //    if (part.protoModuleCrew[0].GExperiencedNormalized > 1.5) {
            //        part.protoModuleCrew[0].Die();
            //        part.explode(); }}}}
            //if (isHelmetOn) {
            //    string err = string.Empty;
            //    resHandler.UpdateModuleResourceInputs(ref err, part.protoModuleCrew.Count, 0, false);
            //}
            //PartResource suitAir = part.Resources.Get("IntakeAir"); // PartResourceLibrary.Instance.GetDefinition("IntakeAir").id
            //float suitAirPrsAtm = (float)(suitAir.amount / suitAir.maxAmount);
            //if (suitAirPrsAtm < eva.helmetOffMinSafePressureAtm) {
            //    // KWHealth.ModifyHP(part.vessel.vesselName, 0);
            //    status = "Not enough Air";
            //    // if prs below 0.0618 atm -> die in 90s max
            //    // else minor hp loss (die in no less than 60s from lower limit)
            //}
            // if (!isHelmetOn || suitAirPrsAtm > eva.helmetOffMinSafePressureAtm)
            // KWHealth.ModifyHP(part.vessel.vesselName, 0);

            // private void HelmetChange(KerbalEVA kerbal, bool isHelmetOn, bool neckRingOn) => this.isHelmetOn = isHelmetOn;

        }

        public void UpdateOx() {
            ox = KWUtil.GetInvResources("Oxygen", inv).Where(r => r.amount > 0).ToArray();
            //ox = inv.storedParts.Values.Where(p => p.snapshot.resources[0]?.resourceName == input.name && p.snapshot.resources[0]?.amount > 0).OrderByDescending(p => 
            //    p.snapshot.resourcePriorityOffset).Select(p => p.snapshot.resources[0]).ToArray();
        }

        public void UpdateUI(Part p) {
            if (p != part) return;
            eva.Events["ChangeNeckRing"].guiActive = false;
            Fields[nameof(timeLeft)].guiActive = crew.hasHelmetOn;
            timeLeft = KSPUtil.PrintTime(ox.Length > 0 ? ox.Select(r => r.amount).Sum() / input.rate : 0, 4, false);
        }

        private void ChangeInventory(ModuleInventoryPart invP) {
            if (invP == inv) {
                UpdateOx();
                //if (crew.hasHelmetOn && !inv.ContainsPart("KWhelmet")) {
                //    crew.hasHelmetOn = false;
                //    // KerbalEVA eva = inv.part.FindModuleImplementing<KerbalEVA>();
                //    typeof(KerbalEVA).GetField("isHelmetEnabled", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(eva, false);
                //    eva.helmetTransform.gameObject.SetActive(false);
                //    GameEvents.OnHelmetChanged.Fire(eva, false, (bool)typeof(KerbalEVA).GetField("isNeckRingEnabled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(eva));
                //    typeof(KerbalEVA).GetMethod("UpdateVisorEventStates", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(eva, null);

                //}
            }
        }

    }

    public class ModuleKWEVARefill : PartModule {

        [KSPField]
        public string resourceName = "MonoPropellant";

        public override void OnStart(StartState state) {
            Events[nameof(RefillTanks)].guiName = $"{Localizer.Format("#autoLOC_313053")}: {PartResourceLibrary.Instance.GetDefinition(resourceName)?.displayName}";
        }

        [KSPEvent(guiActiveUnfocused = true, guiName = "#autoLOC_313053", unfocusedRange = 1f)]
        public void RefillTanks() {
            if (FlightGlobals.ActiveVessel.isEVA) {
                ProtoPartResourceSnapshot[] evaTanks = KWUtil.GetInvResources(resourceName, FlightGlobals.ActiveVessel.FindPartModuleImplementing<KerbalEVA>().ModuleInventoryPartReference);
                //ProtoPartResourceSnapshot[] evaTanks = FlightGlobals.ActiveVessel.FindPartModuleImplementing<KerbalEVA>().ModuleInventoryPartReference.storedParts.Values.Where(p =>
                //    p.snapshot.resources[0]?.resourceName == resourceName).Select(p => p.snapshot.resources[0]).ToArray();
                if (evaTanks.Length == 0) 
                    ScreenMessages.PostScreenMessage($"{Localizer.Format("#autoLOC_6002290")}: {Localizer.Format("#autoLOC_261263", PartResourceLibrary.Instance.GetDefinition(resourceName)?.displayName)}");
                else foreach (ProtoPartResourceSnapshot res in evaTanks) {
                    double reqAmt = res.maxAmount - res.amount;
                    if (reqAmt > 0) res.amount += part.RequestResource(resourceName, reqAmt);
                    if (res.amount > res.maxAmount) res.amount = res.maxAmount;
                }
            }
        }
    }

    public class ModuleKWGrowthOrganism : PartModule {
        [KSPField(guiActive = true, guiFormat = "0.0", guiName = "#KWLOC_growthRate", guiUnits = "%")]
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
                double deltaTime = TimeWarp.fixedDeltaTime;
                growthRate = (float)(growthRes.amount / growthRes.maxAmount);
                // feedback = "";
                // double inputRateTotal = 1.0;
                if (growthRate > 0f)
                    foreach (ModuleResource inputRes in resHandler.inputResources) {
                        double maxAmt = inputRes.rate * deltaTime;
                        //double inputConsumed = part.RequestResource(inputRes.id, maxAmt * growthRate);
                        //feedback += inputRes.name.Substring(0, 1) + "=" + inputConsumed.ToString("N4") + "|";
                        double consumedPct = part.RequestResource(inputRes.id, maxAmt * growthRate) / maxAmt;
                        if (consumedPct < growthRate) growthRate = (float)consumedPct;
                    }
                if (growthRate > 0f)
                    foreach (ModuleResource outputRes in resHandler.outputResources)
                        // double amt = outputRes.rate * growthRate * fixedDeltaTime;
                        part.RequestResource(outputRes.id, -outputRes.rate * growthRate * deltaTime);
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

    public class ModuleKWResourceToggle : PartModule {

        // add auto empty on new resource fill
        // add option to auto-hide empty

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
            PartResource[] resources = Resources();
            if (HighLogic.LoadedSceneIsFlight)
                Events[nameof(ToggleResource)].guiActive = !hideAll && AllAreEmpty() && !hideInFlight;
            else Events[nameof(ToggleResource)].guiActiveEditor = !hideAll;
            foreach (PartResource res in resources.Where(r => resources.IndexOf(r) != currentIndex || hideAll))
                if (HighLogic.LoadedSceneIsFlight) {
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