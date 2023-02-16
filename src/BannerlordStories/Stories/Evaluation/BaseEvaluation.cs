// Code written by Gabriel Mailhot, 02/12/2023.

#region

using System.Collections.Generic;
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
        public PartyType PartyType { get; set; } = PartyType.NotAssigned;
        public IPersona Persona { get; set; } = new Persona();
        public List<string> Tags { get; set; } = new List<string>();
        public GameTime Time { get; set; } = GameTime.Anytime;
    }
}