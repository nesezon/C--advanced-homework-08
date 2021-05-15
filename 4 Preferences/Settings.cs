using System;
using Microsoft.Win32;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Xml;

namespace Preferences {
  class Settings {

    const string EMPTYCONF = "<?xml version='1.0' encoding='utf-8' ?><configuration><startup>" +
        "<supportedRuntime version='v4.0' sku='.NETFramework,Version=v4.7.2' /></startup>" +
        "<appSettings></appSettings></configuration>";

    /// <summary>
    /// Детектим режим хранения настроек (файл или реестр)
    /// </summary>
    public bool areInRegistry() {
      // ищем настройки в реестре
      RegistryKey regKey = Registry.CurrentUser;
      regKey = regKey.OpenSubKey("Software\\ITEA\\Lesson8");
      if (regKey != null) return true;
      // если в реестре нет, значит в файле конфигурации
      return false;
    }

    internal void clearRegistry() {
      RegistryKey key = Registry.CurrentUser;
      RegistryKey software = key.OpenSubKey("Software", true);
      RegistryKey regKey = software.OpenSubKey("ITEA");
      if (regKey != null) {
        software.DeleteSubKeyTree("ITEA");
        software.Close();
      }
    }

    internal void removeFile() {
      string confPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
      if (File.Exists(confPath)) {
        File.Delete(confPath);
      }
    }

    /// <summary>
    /// Загрузка настроек из реестра
    /// </summary>
    public void LoadFromRegistry(MainWindow mainWindow) {
      RegistryKey regKey = Registry.CurrentUser;
      regKey = regKey.OpenSubKey("Software\\ITEA\\Lesson8");
      if (regKey == null) return;
      setFromRegistry(regKey.GetValue("Window.Height"), mainWindow,
        (key, win) => { win.Height = Convert.ToDouble(key); });
      setFromRegistry(regKey.GetValue("Window.Width"), mainWindow,
        (key, win) => { win.Width = Convert.ToDouble(key); });
      setFromRegistry(regKey.GetValue("Window.State"), mainWindow,
        (key, win) => { win.WindowState = (WindowState)Enum.Parse(typeof(WindowState), key.ToString()); });
      if (regKey.GetValue("Window.Left") != null || regKey.GetValue("Window.Top") != null)
        mainWindow.WindowStartupLocation = WindowStartupLocation.Manual;
      setFromRegistry(regKey.GetValue("Window.Left"), mainWindow,
        (key, win) => { win.Left = Convert.ToDouble(key); });
      setFromRegistry(regKey.GetValue("Window.Top"), mainWindow,
        (key, win) => { win.Top = Convert.ToDouble(key); });
    }

    private void setFromRegistry(object key, MainWindow mainWindow, Action<object, MainWindow> doWork) {
      if (key != null) doWork(key, mainWindow);
    }

    /// <summary>
    /// Сохранение настроек в реестр
    /// </summary>
    public void Save2Registry(MainWindow mainWindow) {
      RegistryKey key = Registry.CurrentUser;
      // Создаю новый подраздел или открываю существующий для доступа на запись.
      RegistryKey wrkKey = key.CreateSubKey(@"Software\\ITEA\\Lesson8");
      if (wrkKey == null) return;
      wrkKey.SetValue("Window.Height", mainWindow.Height);
      wrkKey.SetValue("Window.Width", mainWindow.Width);
      wrkKey.SetValue("Window.State", mainWindow.WindowState);
      wrkKey.SetValue("Window.Left", mainWindow.Left);
      wrkKey.SetValue("Window.Top", mainWindow.Top);
    }

    /// <summary>
    /// Загрузка настроек из файла конфигурации
    /// </summary>
    public void LoadFromFile(MainWindow mainWindow) {
      NameValueCollection allAppSettings = ConfigurationManager.AppSettings;
      if (allAppSettings.AllKeys.Length > 0) {
        mainWindow.Height = Convert.ToDouble(allAppSettings["Window.Height"]);
        mainWindow.Width = Convert.ToDouble(allAppSettings["Window.Width"]);
        mainWindow.WindowState = (WindowState)Enum.Parse(typeof(WindowState), allAppSettings["Window.State"]);
        if (allAppSettings["Window.Left"] != null && allAppSettings["Window.Top"] != null) {
          mainWindow.WindowStartupLocation = WindowStartupLocation.Manual;
          mainWindow.Left = Convert.ToDouble(allAppSettings["Window.Left"]);
          mainWindow.Top = Convert.ToDouble(allAppSettings["Window.Top"]);
        }
      }
    }

    /// <summary>
    /// Сохранение настроек в файл конфигурации
    /// </summary>
    public void Save2File(MainWindow mainWindow) {
      // загрузка имеющегося конфига в doc
      var doc = new XmlDocument();
      string confPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

      if (File.Exists(confPath)) doc.Load(confPath);
      else doc.LoadXml(EMPTYCONF);

      // правка/добавление настроек
      Add2Config("Window.Height", mainWindow.Height.ToString(CultureInfo.CurrentCulture), doc);
      Add2Config("Window.Width", mainWindow.Width.ToString(CultureInfo.CurrentCulture), doc);
      Add2Config("Window.State", mainWindow.WindowState.ToString(), doc);
      Add2Config("Window.Left", mainWindow.Left.ToString(), doc);
      Add2Config("Window.Top", mainWindow.Top.ToString(), doc);

      // сохранение
      doc.Save(confPath);
    }

    /// <summary>
    /// Вставка / изменение параметра в конфигурационном файле
    /// </summary>
    private void Add2Config(string key, string value, XmlDocument doc) {
      XmlNode node = doc.SelectSingleNode("//appSettings");
      // если узла appSettings нет, добавляем
      // if (node == null) node = doc.CreateNode("element", "appSettings", "");
      XmlElement element = node.SelectSingleNode(string.Format("//add[@key='{0}']", key)) as XmlElement;
      // Если строка с таким ключем существует - записываем значение.
      if (element != null)
        element.SetAttribute("value", value);
      else {
        // Иначе: создаем строку и формируем в ней пару [ключ]-[значение].
        element = doc.CreateElement("add");
        element.SetAttribute("key", key);
        element.SetAttribute("value", value);
        node.AppendChild(element);
      }
    }
  }
}
