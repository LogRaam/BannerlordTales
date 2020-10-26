// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System.Collections.Generic;
using TalesContract;
using TalesPersistence.Context;
using TalesPersistence.Stories;
using TalesTaleWorlds.Menu;
using TaleWorlds.CampaignSystem;

#endregion

namespace TalesTaleWorlds
{
    public class StoryBroker
    {
        public void AddStoriesToGame(CampaignGameStarter gameStarter)
        {
            //this.TestEvent(gameStarter);
            var m = new MenuBroker();
            foreach (var story in GameData.Instance.StoryContext.Stories)
                m.CreateGameMenuFor(gameStarter, story);
        }

        public List<Act> RetrieveNonPlayedActsFrom(List<IStory> stories)
        {
            var result = new List<Act>();
            foreach (var story in stories)
                foreach (var act in story.Acts)
                {
                    var a = new Act(act);

                    if (a.AlreadyPlayed()) continue;

                    result.Add(a);
                }

            return result;
        }
    }
}