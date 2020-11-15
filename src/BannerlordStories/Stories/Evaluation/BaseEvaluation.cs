// Code written by Gabriel Mailhot, 14/11/2020.

#region

using TalesContract;
using TalesEnums;

#endregion

namespace TalesBase.Stories.Evaluation
{
    #region

    #endregion

    public class BaseEvaluation : IEvaluation
    {
        public IEquipments Equipments { get; set; }
        public INumbers Numbers { get; set; }
        public IOutcome Outcome { get; set; }
        public PartyType PartyType { get; set; } = PartyType.UNKNOWN;
        public IPersona Persona { get; set; }
        public GameTime Time { get; set; }
    }
}