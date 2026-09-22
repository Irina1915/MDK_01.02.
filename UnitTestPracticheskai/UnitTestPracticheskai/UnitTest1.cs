using System;
using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTestPracticheskai
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethodParolTrue()
        {
            Mock<IUserRepository> mock = new Mock<IUserRepository>(); // метод мок для имитации бд
            mock.Setup(repo => repo.GetUser("login")) // если будет вызван метод репозитория с аргументом логин, какие ожидают от этого результаты
                .Returns(new User { Login = "login", Password = "123" });   // в ответе я хочу чтобы выдавал пользователя с таким логиным и паролем

            var service = new UserService(mock.Object); // используем имитацию

            string result = service.Autorization("login", "123"); // проверка авторизации, происходит проверка метода авторизация

            Assert.AreEqual("true", result); // проверка результата
        }


        [TestMethod] 
        public void TestMethodParolFalse()
        {
            Mock<IUserRepository> mock = new Mock<IUserRepository>();

            mock.Setup(repo => repo.GetUser("login"))
                .Returns(new User { Login = "login", Password = "123" });

            var service = new UserService(mock.Object);

            string result = service.Autorization("login", "243");

            Assert.AreEqual("Ошибка (проверьте введённые данные)", result);
        }

        [TestMethod]
        public void TestMethodRegistraziaTrue()
        {
            Mock<IUserRepository> mock = new Mock<IUserRepository>();

            mock.Setup(repo => repo.GetUser("login")) // если программа попробует найти пользователя логином "login"
                .Returns((User)null); // говорим что пользователя нет (Если ищут пользователя "login", верни null)

            var service = new UserService(mock.Object); // создаем UserService, работающий с имитацией

            string result = service.Registrazia("login", "123"); // запускаем регистрацию

            Assert.AreEqual("успех", result); // для проверки результата теста
        }



    }
}
