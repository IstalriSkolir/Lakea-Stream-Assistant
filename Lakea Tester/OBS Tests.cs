using Lakea_Stream_Assistant.Models.Resources.OBS;

namespace Lakea_Tester
{
    [TestClass]
    public class OBS_Tests
    {
        OBSResources resources;

        public OBS_Tests() 
        {
            List<string> scenes = new List<string>() { "Scene 1", "Scene 2", "Scene 3" };
            Dictionary<string, int> sourceIDs = new Dictionary<string, int>();
            Dictionary<int, string> sourceNames = new Dictionary<int, string>();
            for(int i = 1; i < 11; i++)
            {
                sourceIDs.Add("Item " + i, i);
                sourceNames.Add(i, "Item " + i);
            }
            resources = new OBSResources(scenes, sourceIDs, sourceNames, scenes);
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

        [TestMethod]
        public void TestResourcesGetItemName()
        {
            Assert.AreEqual("Item 2", resources.GetSourceName(2));
            Assert.AreEqual("Item 4", resources.GetSourceName(4));
            Assert.AreEqual("Item 6", resources.GetSourceName(6));
            Assert.AreEqual("Item 7", resources.GetSourceName(7));
            Assert.AreEqual("Item 9", resources.GetSourceName(9));
        }
    }
}