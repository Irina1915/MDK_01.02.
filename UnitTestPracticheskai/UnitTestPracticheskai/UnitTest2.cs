using System;
using Library;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTestPracticheskai
{
	[TestClass]
	public class UnitTest2
	{
		[TestMethod]
		public void TestMethodImpotrBD()
		{
            Mock<IFileImport> mockFile = new Mock<IFileImport>(); // мок для имитации работы с файлом
            Mock<IUserRepository> mockRepo = new Mock<IUserRepository>(); // метод мок для имитации бд

            var users = new List<User>
        {
            new User { Login = "user1", Password = "pass1" },
            new User { Login = "user2", Password = "pass2" }
        }; // создаем список из двух пользователей

            mockFile.Setup(f => f.ReadDataFromFile(It.IsAny<string>())) // если будет вызван метод чтения файла
                .ReturnsAsync(users); // в ответе я хочу чтобы выдавал наш список пользователей

            mockFile.Setup(f => f.ValidateUser(It.IsAny<string>(), It.IsAny<string>())) // если будут проверять пользователя
                .Returns(true); // в ответе я хочу чтобы валидация всегда проходила

            var importer = new ImportBD(mockFile.Object, mockRepo.Object); // используем имитацию

            importer.ImportData("test.txt").GetAwaiter().GetResult(); // запуск импорта, происходит проверка метода импорта

            mockRepo.Verify(r => r.AddUser(It.IsAny<User>()), Times.Exactly(2)); // проверяем что добавили 2 пользователей
            mockRepo.Verify(r => r.SaveChanges(), Times.Exactly(2)); // проверяем что сохранили изменения 2 раза
        }

        [TestMethod]
        public void TestMethodImportNo()
        {
            Mock<IFileImport> mockFile = new Mock<IFileImport>(); // мок для имитации работы с файлом
            Mock<IUserRepository> mockRepo = new Mock<IUserRepository>(); // метод мок для имитации бд

            var users = new List<User>
    {
        new User { Login = "bad_user", Password = "wrong_pass" }
    }; // создаем список с одним пользователем, который не пройдет валидацию

            mockFile.Setup(f => f.ReadDataFromFile(It.IsAny<string>())) // если будет вызван метод чтения файла
                .ReturnsAsync(users); // в ответе я хочу чтобы выдавал наш список пользователей

            mockFile.Setup(f => f.ValidateUser(It.IsAny<string>(), It.IsAny<string>())) // если будут проверять пользователя
                .Returns(false); // в ответе я хочу чтобы валидация не прошла

            var importer = new ImportBD(mockFile.Object, mockRepo.Object); // используем имитацию

            importer.ImportData("test.txt").GetAwaiter().GetResult(); // запуск импорта, происходит проверка метода импорта

            mockRepo.Verify(r => r.AddUser(It.IsAny<User>()), Times.Never); // проверяем что ни одного пользователя не добавили
            mockRepo.Verify(r => r.SaveChanges(), Times.Never); // проверяем что изменения не сохранялись
        }
	}
}
