namespace TalesContract
{
   using TalesEnums;

   public interface IEventRequest
    {
       public StoryType StoryType { get; set; }

       public StoryAction Action { get; set; }
    }
}