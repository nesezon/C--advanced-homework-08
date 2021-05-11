using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ShowMyContacts {
  static class Program {
    static void Main() {
      string file2search = "TelephoneBook.xml";
      // ищу файл в папке 1-го упражнения
      DirectoryInfo directoryInfo = new DirectoryInfo("../../../1 CreateMyContacts");
      FileInfo[] fileNames = directoryInfo.GetFiles(file2search, SearchOption.AllDirectories);
      if (fileNames.Length > 0) {
        // найден
        XDocument xdoc = XDocument.Load(fileNames[0].FullName);
        IEnumerable<Contact> contacts = from row in xdoc.Element("MyContacts")?.Elements("Contact")
                                        select new Contact {
                                          Name = row.Value,
                                          TelephoneNumber = row.Attribute("TelephoneNumber")?.Value
                                        };
        foreach (Contact contact in contacts) {
          Console.WriteLine(contact);
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

  /// <summary>
  /// Класс для представления контакта
  /// </summary>
  class Contact {
    public string Name { get; set; }
    public string TelephoneNumber { get; set; }
    public override string ToString() {
      return string.Format(format: "{0, -20} {1}",arg0: Name, TelephoneNumber);
    }
  }
}
