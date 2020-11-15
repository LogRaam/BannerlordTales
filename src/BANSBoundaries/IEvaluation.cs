﻿// Code written by Gabriel Mailhot, 11/09/2020.

#region

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
        public GameTime Time { get; set; }
    }
}