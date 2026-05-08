using System.Text.Json;
using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.Models.Misc
{
    public class JSONIO
    {
        public T ReadJSONFile<T>(string path)
        {
            try
            {
                Terminal.Output($"Lakea: Reading JSON File -> {Path.GetFileName(path)}");
                Logs.Instance.NewLog(LogLevel.Info, $"Lakea: Reading JSON File -> {Path.GetFileName(path)}");
                string jsonString = File.ReadAllText(path);
                return (T)JsonSerializer.Deserialize<T>(jsonString);
            }
            catch (Exception ex)
            {
                Terminal.Output($"Lakea: Error Reading JSON File -> {ex.Message}");
                Logs.Instance.NewLog(LogLevel.Error, $"Lakea: Error Reading JSON File -> {ex.Message}");
                return default;
            }
        }

        public void WriteJSONFile(string path, object data)
        {
            try
            {
                Terminal.Output($"Lakea: Writing JSON File -> {Path.GetFileName(path)}");
                Logs.Instance.NewLog(LogLevel.Info, $"Lakea: Writing JSON File -> {Path.GetFileName(path)}");
                string jsonString = JsonSerializer.Serialize(data);
                File.WriteAllText(path, jsonString);
            }
            catch(Exception ex)
            {
                Terminal.Output($"Lakea: Error Writing JSON File -> {ex.Message}");
                Logs.Instance.NewLog(LogLevel.Error, $"Lakea: Error Writing JSON File -> {ex.Message}");
            }
        }
    }
}
