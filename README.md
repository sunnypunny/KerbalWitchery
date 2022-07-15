# Kerbal Witchery Backend Index

*([plugin starts here](#properties))*

## [GameData](GameData)

*Installation Files (mirrors distribution)*

* [Localisation file](GameData/KerbalWitchery/Languages.cfg)
* [Settings file](GameData/KerbalWitchery/Settings.cfg) (customisable)

### [Agencies](GameData/KerbalWitchery/Agencies)

* Logos & configs for 3 missing stock agencies +1 parachute agency
* Fixes for agency manufacturer values

### [AttitudeControl](GameData/KerbalWitchery/AttitudeControl)

* Added small gimbal range for engines without any
* Reaction wheel torque reduction

### [Career](GameData/KerbalWitchery/Career)

* Strategies overhaul [WIP]
* Fixes for missing contracts
* Engine costs increase + all parts entry cost standardisation
* Propellant costs reduction

### [Experiments](GameData/KerbalWitchery/Experiments)

* Added experiment fractions to crew reports (requires Breaking Ground)
* Added EVA materials study
* Removed experiment containers from capsules
* Reduced lab conversion rate
* Rescaled Materials Bay requires science kit for reset
* Mystery Goo requires Goo resource for experiment + can't be reset or transmitted
* Reduced Breaking Ground scanner transmission rates
* Increased scientist return rates
* Increased sensor transmission rates
* Reduced surface sample transmission rates + added experiments to more parts

### [LifeSciences](GameData/KerbalWitchery/LifeSciences)

* Mystery Goo grows & photosynethesises like algae

### [Parts](GameData/KerbalWitchery/Parts)

* Removed most internal modules from capsules
* Filled antenna gaps by rescaling orbital scanner + adding small relay & increasing range on first extendible antenna
* Fixes for part bulkhead profiles values
* Added more cargo tank variants for each size + resource toggle
* Rescaled drills
* Added gas tank resource toggle + reassigned larger RCS tanks to gas
* Rescaled ISRU parts
* Removed resource functions from smaller scanners + reassigned kerbnet functions from probes
* Added more resource extraction abilities to air scoop

### [Propellant](GameData/KerbalWitchery/Propellant)

* Added tanks to adapters + cones, standardised amounts & turned some duplicates into variants
* Reassigned radial engines to hypergolic fuel combination with simpler fuel ratio
* Reassigned EVA jetpack fuel to MonoPropellant
* Simplified LF + Lox fuel ratio
* Added resource toggle to all tanks + now hold a single resource only
* Added cryogenic fuel combination to some engines with simpler fuel ratio + cryo fuel option for propellant tanks

### [Propulsion](GameData/KerbalWitchery/Propulsion)

* Added ignitor modules for non-hypergolic chemical engines
* Simple thrust curve for SRBs modelled on circular grain geometry
* Rebalanced thrust for some SRBs
* Reduced throttle range for most chemical engines

### [Recipes](GameData/KerbalWitchery/Recipes)

*Resource conversion & extraction recipes for various parts*

### [Resources](GameData/KerbalWitchery/Resources)

*Resources + planetary distributions*

### [TechTree](GameData/KerbalWitchery/TechTree)

* Adjusted locations of some resource containers + antennae in stock tree with CTT integration*

## [Properties](Properties)

*Generic plugin info*

## [CustomParameterNodes.cs](CustomParameterNodes.cs)

*Classes adding custom settings to stock settings menu*

* **KWGeneralOptions:** *Settings for all game modes*
* **KWCareerOptions:** *Career settings*

## [CustomTypes.cs](CustomTypes.cs)

*Entirely custom classes not inheriting from others*

* **KWUtil:** *Static utilities class which stores icons and methods needed by multiple other classes*
* **Agency:** *Agency object with various additional values for each agency needed for career mode*

## [KSPAddons.cs](KSPAddons.cs)

*Classes with the KSPAddon attribute*

* **KWEvents:** *Events handler that adds various functions to stock events across all scenes and game modes*
* **KWUI:** *UI handler with events and methods used in both stock and custom UI*

## [PartModulesCustom.cs](PartModulesCustom.cs)

*Custom part module classes*

* **ModuleKWPartMaker:** *Converts resources into inventory parts*
* **ModuleKWCabin:** *Life sciences functions for capsules*
* **ModuleKWGrowthOrganism:** *Resource converter with growth function*
* **ModuleKWEVA:** *Life sciences functions for EVAs*
* **ModuleKWResourceToggle:** *Resource toggle needed for containers*
* **ModuleKWNonCryoContainer:** *Cryo Fuel boiloff*


## [PartModulesDerived.cs](PartModulesDerived.cs)

*Part module classes derived from stock modules*

* **ModuleEnginesKW:** *Added ignition functions + throttle range fix for engine modules*
* **ModuleEnginesFXKW:** *As above*
* **ModuleResourceIntakeKW:** *Adds resource switcher to intakes*
* **ModuleScienceExperimentKW:** *Adds science kit consumption and resource requirement to experiments*

## [ScenarioModules.cs](ScenarioModules.cs)

*Classes that inherit from ScenarioModule*

* **KWAgencies:** *Persistent data storage for agency objects with additional functions needed in career mode*
