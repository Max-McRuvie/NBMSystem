using System.Collections.Generic;
using System.IO;
using NBMSystem.Input;
using Newtonsoft.Json;

namespace NBMSystem.FileSave
{
    public class JsonFileSave
    {
        public void saveToJson(List<MessageInput> message)
        {
            using (StreamWriter file = File.CreateText(@"../../../savedMessages.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, message);
            }
        }
    }
}
