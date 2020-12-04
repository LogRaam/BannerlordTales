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
        public IEquipments Equipments { get; set; } = new Equipments();
        public INumbers Numbers { get; set; } = new Numbers();
        public IOutcome Outcome { get; set; } = new Outcome();
        public PartyType PartyType { get; set; } = PartyType.UNKNOWN;
        public IPersona Persona { get; set; } = new Persona();
        public GameTime Time { get; set; } = GameTime.UNKNOWN;
    }
}