namespace Lakea_Stream_Assistant.Models.Resources.OBS
{
    //This class stores scene and source information for the OBS Singleton to refer back to
    public class OBSResources
    {
        private Dictionary<string, int> sourceIDs = new Dictionary<string, int>();
        private Dictionary<int, string> sourceNames = new Dictionary<int, string>();
        private Dictionary<string, string> sourceScenes = new Dictionary<string, string>();
        private List<string> scenes = new List<string>();
        private List<string> sceneTransitions = new List<string>();

        public OBSResources(List<string> scenes, Dictionary<string, int> sourceIDs, Dictionary<int, string> sourceNames,List<string> sceneTransitions)
        {
            this.sourceIDs = sourceIDs;
            this.sourceNames = sourceNames;
            this.scenes = scenes;
            this.sceneTransitions = sceneTransitions;
        }

        public int GetSourceId(string source)
        {
            return sourceIDs[source];
        }

        public string GetSourceName(int source)
        {
            return sourceNames[source];
        }
    }
}
