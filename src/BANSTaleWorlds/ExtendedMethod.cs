// Code written by Gabriel Mailhot, 06/10/2020.

#region

using System.Collections.Generic;
using TalesContract;
using TalesPersistence.Stories;

#endregion

namespace TalesRuntime
{
    public static class ExtendedMethod
    {
        public static List<Act> ToAct(this List<IAct> acts)
        {
            var result = new List<Act>();
            foreach (var act in acts)
            {
                result.Add(new Act(act));
            }

            return result;
        }
    }
}