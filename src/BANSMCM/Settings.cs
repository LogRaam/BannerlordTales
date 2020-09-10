// Code written by Gabriel Mailhot, on 30 of 07, 2020.

namespace BANSMCM
{
    #region

    using MCM.Abstractions.Settings.Base.Global;

    #endregion

    internal class Settings : AttributeGlobalSettings<Settings>
    {
        public override string DisplayName { get; }

        public override string FolderName { get; }

        public override string Id { get; }
    }
}