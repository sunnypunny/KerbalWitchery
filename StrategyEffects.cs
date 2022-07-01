
using KSP.Localization;
using KSP.UI.Screens;
using Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KerbalWitchery {
    
    public class KWAdminStrat : StrategyEffect {
        private ProgType type;
        public KWAdminStrat(Strategy parent) : base(parent) { }
        public KWAdminStrat(Strategy parent, ProgType type) : base(parent) => this.type = type;
        protected override void OnLoadFromConfig(ConfigNode node) => type = (ProgType)Enum.Parse(typeof(ProgType), node.GetValue("type"));
        public override bool CanActivate(ref string reason) => ProgType.Lobbying.HasFlag(type);
        
        // protected override string GetDescription() => "";
    }

}
