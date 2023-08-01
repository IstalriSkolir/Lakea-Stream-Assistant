﻿namespace Lakea_Stream_Assistant.Models.OBS
{
    public class OBSResources
    {
        private IDictionary<string, int> sourceIDs = new Dictionary<string, int>();
        private IDictionary<string, string> sourceScenes = new Dictionary<string, string>();
        private List<string> scenes = new List<string>();

        public OBSResources(List<string> scenes, IDictionary<string, int> sourceIDs)
        {
            this.sourceIDs = sourceIDs;
            this.scenes = scenes;
        }

        public int GetSourceId(string source)
        {
            return sourceIDs[source];
        }
    }
}
