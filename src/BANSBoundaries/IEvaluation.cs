// Code written by Gabriel Mailhot, 02/12/2023.  Updated by  Gabriel Mailhot on 02/19/2023.

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
        bool IsEquivalentTo(IEvaluation evaluation);
    }
}