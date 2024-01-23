using Newtonsoft.Json;
using SimeonCloudSoftware.Model;

namespace SimeonCloudSoftware.Writter
{
    public class JsonFileWriter
    {
        /// <summary>
        /// Writes a Group object to a JSON file in a specified directory.
        /// This method takes a Group object, serializes it to JSON format with an indented layout,
        /// and saves it to a file named after the group's display name in the "MSGraph/Groups" directory.
        /// If the directory does not exist, it will be created.
        /// </summary>
        /// <param name="group">The Group object to be written to the JSON file.</param>
        public void WriteDataToFile(Group group)
        {
            try
            {
                string directoryPath = Constant.filePath;
                Directory.CreateDirectory(directoryPath);

                string filePath = Path.Combine(directoryPath, $"{group.DisplayName}.json");
                File.WriteAllText(filePath, JsonConvert.SerializeObject(group, Formatting.Indented));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void WriteGroupsToJson(IEnumerable<Group> groups)
        {
            try
            {
                foreach (var group in groups)
                {
                    WriteDataToFile(group);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
		}
    }
}
