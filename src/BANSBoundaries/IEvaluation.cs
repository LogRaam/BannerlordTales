// unset

#region

using System.Collections.Generic;
using TalesEnums;

#endregion

namespace TalesContract
{
    public interface IEvaluation
    {
        public IEquipments Equipments { get; set; }
        public INumbers Numbers { get; set; }
        public IOutcome Outcome { get; set; }
        public PartyType PartyType { get; set; }
        public IPersona Persona { get; set; }
        public List<string> Tags { get; set; }
        public GameTime Time { get; set; }
    }
}