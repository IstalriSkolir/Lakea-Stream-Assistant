namespace Lakea_Stream_Assistant.Models.Events
{
    //This class handles the outputs that are triggered from events
    public class EventOutputs
    {
        //Set OBS source active status, resets after duration expires if there is a duration
        public void SetActiveOBSSource(string source, int duration, bool active, bool invoked = false)
        {
            Singletons.OBS.SetSourceEnabled(source, active);
            if (!invoked && duration > 0)
            {
                Task.Delay(duration * 1000).ContinueWith(t => SetActiveOBSSource(source, duration, !active, true));
            }
        }

        //Changes OBS scene
        public void ChangeOBSScene(string scene)
        {
            Singletons.OBS.ChangeScene(scene);
        }
    }
}
