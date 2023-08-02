using Lakea_Stream_Assistant.Models.OBS;

namespace Lakea_Tester
{
    [TestClass]
    public class OBS_Tests
    {
        OBSResources resources;

        public OBS_Tests() 
        {
            List<string> scenes = new List<string>() { "Scene 1", "Scene 2", "Scene 3" };
            IDictionary<string, int> sourceIDs = new Dictionary<string, int>();
            for(int i = 1; i < 11; i++)
            {
                sourceIDs.Add("Item " + i, i);
            }
            resources = new OBSResources(scenes, sourceIDs);
        }

        [TestMethod]
        public void TestResourcesGetItemID()
        {
            Assert.AreEqual(1, resources.GetSourceId("Item 1"));
            Assert.AreEqual(3, resources.GetSourceId("Item 3"));
            Assert.AreEqual(5, resources.GetSourceId("Item 5"));
            Assert.AreEqual(8, resources.GetSourceId("Item 8"));
            Assert.AreEqual(10, resources.GetSourceId("Item 10"));
        }
    }
}