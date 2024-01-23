using SimeonCloudSoftware.Writter;
using Newtonsoft.Json.Linq;
using SimeonCloudSoftware.Model;
using Xunit;
using System.Reflection.Metadata;

namespace UnitTests
{
    public class JsonFileWriterTests
	{
		/// <summary>
		/// This test case checks if the WriteGroupToFile method successfully creates a JSON file when provided with 
		/// a valid Group object.
		/// It creates an instance of JsonFileWriter and a sample Group object.
		/// Then, it calls the WriteGroupToFile method with the Group object.
		// Finally, it asserts that the file with the expected name exists in the specified directory.
		/// </summary>
		[Fact]
		public void WriteGroupToFile_CreatesFile_Successfully()
		{
			var jsonFileWriter = new JsonFileWriter();
			var group = new Group { DisplayName = "Test Group", Id = "1", Description = "Description" };
			string directoryPath = Constant.filePath;
			string filePath = Path.Combine(directoryPath, $"{group.DisplayName}.json");

			jsonFileWriter.WriteDataToFile(group);

			Assert.True(File.Exists(filePath));
		}

		/// <summary>
		/// This test case checks if the WriteGroupToFile method throws a NullReferenceException when provided with a null Group object.
		/// It creates an instance of JsonFileWriter and sets nullGroup to null.
		/// Then, it calls the WriteGroupToFile method with the nullGroup object.
		/// It asserts that a NullReferenceException is thrown.
		/// </summary>
		[Fact]
		public void WriteGroupToFile_ThrowsException_OnInvalidGroup()
		{
			var jsonFileWriter = new JsonFileWriter();
			Group nullGroup = null;

			// Act & Assert
			Assert.Throws<NullReferenceException>(() => jsonFileWriter.WriteDataToFile(nullGroup));
		}

		/// <summary>
		/// This test case checks if the WriteGroupToFile method creates a JSON file with the correct JSON format when provided with a valid Group object.
		/// It creates an instance of JsonFileWriter and a sample Group object.
		/// It also defines the expected file path based on the Group object's properties.
		/// The test then calls the WriteGroupToFile method with the Group object.
		/// It reads the file's content and parses it as a JSON object.
		/// Finally, it asserts that the JSON object contains the expected properties and values.
		/// </summary>
		[Fact]
		public void WriteGroupToFile_EnsuresCorrectJsonFormat()
		{
			// Arrange
			var jsonFileWriter = new JsonFileWriter();
			var group = new Group { DisplayName = "Test Group", Id = "1", Description = "Description" };
			string directoryPath = Constant.filePath;
			string fileName = $"{group.DisplayName}.json";
			string expectedFilePath = Path.Combine(directoryPath, fileName);

			// Act
			jsonFileWriter.WriteDataToFile(group);
			var fileContent = File.ReadAllText(expectedFilePath);
			var jsonObject = JObject.Parse(fileContent);

			// Assert
			Assert.Equal("Test Group", jsonObject["displayName"].ToString());
			Assert.Equal("1", jsonObject["id"].ToString());
			Assert.Equal("Description", jsonObject["description"].ToString());
		}
	}
}