using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
        public interface IFileImport
        {
            // метод читающий пользователей из файла
            Task<List<User>> ReadDataFromFile(string filePath);
            // проверка правильности данных пользователя
            bool ValidateUser(string login, string password);

        }
    
}

