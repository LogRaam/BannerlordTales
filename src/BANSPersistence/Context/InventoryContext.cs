// Code written by Gabriel Mailhot, 02/12/2023.

#region

using _45_TalesGameState;
using System.Collections.Generic;
using System.IO;
using TalesBase.Items;

#endregion

namespace TalesPersistence.Context
{
    public class InventoryContext
    {
        private FileInfo _bodyArmorsFolder;

        public List<BaseBodyArmor> BodyArmors { get; set; }

        public FileInfo BodyArmorsFile
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _bodyArmorsFolder = new FileInfo(GameData.Instance.StoryContext.ModuleFolder.FullName.Replace("LogRaamBannerlordTales", "SandBoxCore\\ModuleData\\items\\body_armors.xml"));

                return _bodyArmorsFolder;
            }
            set => _bodyArmorsFolder = value;
        }
    }
}