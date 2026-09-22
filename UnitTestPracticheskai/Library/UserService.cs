using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class UserService
    {
        IUserRepository repository_;
        public UserService(IUserRepository repository)
        {
            repository_ = repository;
        }

        public string Autorization(string login, string password)
        {
            string result = "Ошибка";
            User user = repository_.GetUser(login);
            if(user.Password == password)
            {
                result = "true";
            }
            else
            {
                result = "Ошибка (проверьте введённые данные)";
            }
            return result;
        }

        public string Registrazia(string login, string password)
        {
            var exsisting = repository_.GetUser(login);
            if (exsisting != null)
            {
                return "пользователь уже существует";
            }
            User newUser = new User
            {
                Login = login,
                Password = password,
            };
            repository_.AddUser(newUser);
            return "успех";
        }
    }
}
