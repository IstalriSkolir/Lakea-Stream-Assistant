using Lakea_Stream_Assistant.Models.Resources.OBS;

namespace Lakea_Tester.Lakea
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
            for (int i = 1; i < 11; i++)
            {
                sourceIDs.Add("Item " + i, i);
                sourceNames.Add(i, "Item " + i);
            }
            resources = new OBSResources(scenes, sourceIDs, sourceNames, scenes);
        }

        [TestMethod]
        public void ResourcesGetItemIDTest()
        {
            Assert.AreEqual(1, resources.GetSourceId("Item 1"), "resources.GetSourceID(\"Item 1\"): " + resources.GetSourceId("Item 1") + ", Expected: 1");
            Assert.AreEqual(3, resources.GetSourceId("Item 3"), "resources.GetSourceId(\"Item 3\"): " + resources.GetSourceId("Item 3") + ", Expected: 3");
            Assert.AreEqual(5, resources.GetSourceId("Item 5"), "resources.GetSourceId(\"Item 5\"): " + resources.GetSourceId("Item 5") + ", Expected: 5");
            Assert.AreEqual(8, resources.GetSourceId("Item 8"), "resources.GetSourceId(\"Item 8\"): " + resources.GetSourceId("Item 8") + ", Expected: 8");
            Assert.AreEqual(10, resources.GetSourceId("Item 10"), "resources.GetSourceId(\"Item 10\"): " + resources.GetSourceId("Item 10") + ", Expected: 10");
        }

        [TestMethod]
        public void ResourcesGetItemNameTest()
        {
            Assert.AreEqual("Item 2", resources.GetSourceName(2), "resources.GetSourceName(2): " + resources.GetSourceName(2) + ", Expected: Item 2");
            Assert.AreEqual("Item 4", resources.GetSourceName(4), "resources.GetSourceName(4): " + resources.GetSourceName(4) + ", Expected: Item 4");
            Assert.AreEqual("Item 6", resources.GetSourceName(6), "resources.GetSourceName(6): " + resources.GetSourceName(6) + ", Expected: Item 6");
            Assert.AreEqual("Item 7", resources.GetSourceName(7), "resources.GetSourceName(7): " + resources.GetSourceName(7) + ", Expected: Item 7");
            Assert.AreEqual("Item 9", resources.GetSourceName(9), "resources.GetSourceName(9): " + resources.GetSourceName(9) + ", Expected: Item 9");
        }
    }
}