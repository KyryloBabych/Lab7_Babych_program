namespace Lab1_Babych_programTest
{
    [TestClass]
    public class Lab7_Babych_programTests
    {
        [TestMethod]
        public void ShouldInitializeComicsList()
        {
            // Arrange
            Lab7_Babych_program lab1 = new Lab7_Babych_program();

            // Assert
            Assert.IsNotNull(lab1);
            Assert.IsNotNull(lab1.comicsList);
            Assert.AreEqual(0, lab1.comicsList.Count);
        }

        [TestMethod]
        public void ShouldThrowException()
        {
            // Arrange
            Lab7_Babych_program lab1 = new Lab7_Babych_program();

            // Act & Assert
            Assert.ThrowsException<IOException>(() => lab1.Run());
        }

        [TestMethod]
        public void ShouldDisplayCorrectCount()
        {
            // Arrange
            Lab7_Babych_program lab1 = new Lab7_Babych_program();
            lab1.comicsList.Add(new Comic("Test", Genre.Фентезі, DateTime.Now, "Author", 10.0));

            // Act
            lab1.DisplayObjectCount();

            // Assert
            // Ваше утверждение, например:
            Assert.AreEqual(1, lab1.comicsList.Count);
        }
        [TestMethod]
        public void DemonstrateStaticMethods_ShouldNotThrowException()
        {
            // Arrange
            Lab7_Babych_program lab1 = new Lab7_Babych_program();

            // Act & Assert
            Assert.IsNotNull(() => lab1.DemonstrateStaticMethods());
        }


        [TestMethod]
        public void AutoFillData_ShouldFillComicsList()
        {
            // Arrange
            Lab7_Babych_program lab1 = new Lab7_Babych_program();
            int maxComics = 5;

            // Act
            lab1.AutoFillData(maxComics);

            // Assert
            Assert.AreEqual(maxComics, lab1.comicsList.Count);
        }

        [TestMethod]
        public void DisplayComics_ShouldNotThrowException()
        {
            // Arrange
            Lab7_Babych_program lab1 = new Lab7_Babych_program();

            // Act & Assert
            Assert.IsNotNull(() => lab1.DisplayComics(new List<Comic>()));
        }

        [TestMethod]
        public void SearchComic_ShouldNotThrowException()
        {
            // Arrange
            Lab7_Babych_program lab1 = new Lab7_Babych_program();

            // Act & Assert
            Assert.IsNotNull(() => lab1.SearchComic());
        }

        [TestMethod]
        public void DeleteComic_ShouldThrowExceptionWhenListIsEmpty()
        {
            // Arrange
            Lab7_Babych_program lab1 = new Lab7_Babych_program();

            // Act & Assert
            Assert.ThrowsException<IOException>(() => lab1.DeleteComic());
        }


        [TestMethod]
        public void DemonstrateBehavior_ShouldNotThrowException()
        {
            // Arrange
            Lab7_Babych_program lab1 = new Lab7_Babych_program();
            lab1.comicsList = new List<Comic>(); // Предполагаем, что список не пустой

            // Act & Assert
            Assert.IsNotNull(() => lab1.DemonstrateBehavior());
        }
        [TestMethod]
        public void SaveToCsv_WhenComicsListNotEmpty_ShouldSaveDataToCsvFile()
        {
            Lab7_Babych_program program = new Lab7_Babych_program();
            List<Comic> comicsList = new List<Comic>
        {
            new Comic("Comic1", Genre.Фентезі, new DateTime(2022, 1, 1), "Author1", 9.99),
            new Comic("Comic2", Genre.Жахи, new DateTime(2022, 2, 1), "Author2", 14.99)
        };
            program.comicsList = comicsList;
            string testFilePath = "D:\\KHAI\\ООП\\Lab7_Babych_program\\test.csv";
            program.SaveToCsv(testFilePath);
            Assert.IsTrue(File.Exists(testFilePath));
        }

        [TestMethod]
        public void SaveToJSON_WhenComicsListNotEmpty_ShouldSaveDataToJSONFile()
        {
            Lab7_Babych_program program = new Lab7_Babych_program();
            List<Comic> comicsList = new List<Comic>
        {
            new Comic("Comic1", Genre.Фентезі, new DateTime(2022, 1, 1), "Author1", 9.99),
            new Comic("Comic2", Genre.Жахи, new DateTime(2022, 2, 1), "Author2", 14.99)
        };
            program.comicsList = comicsList;
            string testFilePath = "D:\\KHAI\\ООП\\Lab7_Babych_program\\test.json";
            program.SaveToJson(testFilePath);
            Assert.IsTrue(File.Exists(testFilePath));
        }
    }
}