using System;
using System.Xml.Linq;

namespace CreateMyContacts {
  class Program {
    static void Main(string[] args) {
      XDocument xdoc = new XDocument(
        new XElement("MyContacts",
          new XElement("Contact",
            new XAttribute("TelephoneNumber", "917-540-3375"),
            "Olivia Wilde"
          ),
          new XElement("Contact",
            new XAttribute("TelephoneNumber", "615-235-5390"),
            "Reese Witherspoon"
          ),
          new XElement("Contact",
            new XAttribute("TelephoneNumber", "(310) 421-0894"),
            "Cara Delevingne"
          ),
          new XElement("Contact",
            new XAttribute("TelephoneNumber", "305-690-0379"),
            "Jennifer Lopez"
          ),
          new XElement("Contact",
            new XAttribute("TelephoneNumber", "917-970-9333"),
            "Amy Schumer"
          )
        )
      );
      
      xdoc.Save("TelephoneBook.xml");
    }
  }
}
