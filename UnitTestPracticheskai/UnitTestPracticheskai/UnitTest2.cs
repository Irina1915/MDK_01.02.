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


//[TestMethod]
//public void TestMethodImpotrBD()
//{
//    Mock<IFileImport> mockFile = new Mock<IFileImport>(); // мок для имитации работы с файлом
//    Mock<IUserRepository> mockRepo = new Mock<IUserRepository>(); // мок для имитации бд

//    // Создаем двух конкретных пользователей — как будто они из файла
//    var user1 = new User { Login = "ivan", Password = "123" };
//    var user2 = new User { Login = "petr", Password = "456" };
//    var listOfUsers = new List<User> { user1, user2 };

//    // Если попросят прочитать файл — отдаем наш список
//    mockFile.Setup(f => f.ReadDataFromFile("test.txt"))
//        .ReturnsAsync(listOfUsers);

//    // ВАЖНО: Теперь мы не используем It.IsAny. Мы говорим: 
//    // "Если проверка придет именно для 'ivan' и '123' — пусть будет true"
//    mockFile.Setup(f => f.ValidateUser("ivan", "123"))
//        .Returns(true);

//    // И для второго пользователя тоже жестко прописываем
//    mockFile.Setup(f => f.ValidateUser("petr", "456"))
//        .Returns(true);

//    var importer = new ImportBD(mockFile.Object, mockRepo.Object); // создаем объект

//    importer.ImportData("test.txt").GetAwaiter().GetResult(); // запускаем импорт

//    // Проверяем: добавили ровно 2 раза
//    mockRepo.Verify(r => r.AddUser(It.IsAny<User>()), Times.Exactly(2));

//    // Проверяем: сохранили 2 раза (так как у тебя SaveChanges внутри цикла)
//    mockRepo.Verify(r => r.SaveChanges(), Times.Exactly(2));
//}


//[TestMethod]
//public void TestMethodImpotrBDFail()
//{
//    Mock<IFileImport> mockFile = new Mock<IFileImport>(); // мок для файла
//    Mock<IUserRepository> mockRepo = new Mock<IUserRepository>(); // мок для бд

//    // Один плохой пользователь
//    var badUser = new User { Login = "bad_user", Password = "wrong_pass" };
//    var listOfUsers = new List<User> { badUser };

//    // Читаем файл — отдаем плохого пользователя
//    mockFile.Setup(f => f.ReadDataFromFile("test.txt"))
//        .ReturnsAsync(listOfUsers);

//    // Жестко говорим: если спросят про этого конкретного пользователя — валидация не пройдет
//    mockFile.Setup(f => f.ValidateUser("bad_user", "wrong_pass"))
//        .Returns(false);

//    var importer = new ImportBD(mockFile.Object, mockRepo.Object);

//    importer.ImportData("test.txt").GetAwaiter().GetResult();

//    // Проверяем: в базу НЕ добавили ни одного (Times.Never)
//    mockRepo.Verify(r => r.AddUser(It.IsAny<User>()), Times.Never);

//    // Проверяем: не сохраняли изменения
//    mockRepo.Verify(r => r.SaveChanges(), Times.Never);
//}