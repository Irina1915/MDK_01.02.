using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestPracticheskai
{
    [TestClass]
    public class TestBD
    {
        // метод мок для имитации работы с файлом
        Mock<IFileImport> mockFile = new Mock<IFileImport>();
        // метод мок для имитации бд
        Mock<IUserRepository> mockRepo = new Mock<IUserRepository>();

        // создаем двух тестовых пользователей, которых будем "импортировать"
        var user1 = new User { Login = "user1", Password = "pass1" };
        var user2 = new User { Login = "user2", Password = "pass2" };
        var listOfUsers = new List<User> { user1, user2 };

        // если попросят прочитать файл — отдаем наш список пользователей
        mockFile.Setup(f => f.ReadDataFromFile(It.IsAny<string>()))
        .ReturnsAsync(listOfUsers);

        // если будут проверять пользователя — говорим, что все хорошие (валидация прошла)
        mockFile.Setup(f => f.ValidateUser(It.IsAny<string>(), It.IsAny<string>()))
        .Returns(true);

        // создаем объект класса для теста, передаем туда наши имитации
        var importer = new ImportBD(mockFile.Object, mockRepo.Object);

        // запускаем метод импорта
        await importer.ImportData("test_file.txt");

        // проверяем: метод добавления в базу был вызван ровно 2 раза (по числу пользователей)
        mockRepo.Verify(r => r.AddUser(It.IsAny<User>()), Times.Exactly(2));

    // проверяем: сохранение изменений было вызвано тоже 2 раза 
    // (потому что в твоем коде SaveChanges стоит внутри цикла foreach)
    mockRepo.Verify(r => r.SaveChanges(), Times.Exactly(2));
    }
}
