using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ShowContactPhonesOnly {
  class Program {
    static void Main(string[] args) {
      string file2search = "TelephoneBook.xml";
      // ищу файл в папке 1-го упражнения
      DirectoryInfo directoryInfo = new DirectoryInfo("../../../1 CreateMyContacts");
      FileInfo[] fileNames = directoryInfo.GetFiles(file2search, SearchOption.AllDirectories);
      if (fileNames.Length > 0) {
        XDocument xdoc = XDocument.Load(fileNames[0].FullName);
        
        // извлечение номеров телефонов
        IEnumerable<string> Phones = from num in xdoc.Element("MyContacts")?.Elements("Contact")
          select num.Attribute("TelephoneNumber")?.Value;

        // вывод на экран
        foreach (var phone in Phones) {
          Console.WriteLine(phone);
        }

        //задержка
        Console.ReadKey();
      } else {
        // не найден
        Console.WriteLine($"Файл {file2search} не найден в папке {directoryInfo.FullName}");
        Console.WriteLine("(Чтобы он появился, запустите исполняемый файл первого упражнения)");
      }
    }
  }
}
