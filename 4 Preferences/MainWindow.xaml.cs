using System;

namespace Preferences {
  public partial class MainWindow {

    Settings settings = new Settings();

    public MainWindow() {

      InitializeComponent();

      // определяем режим
      if (settings.areInRegistry()) {
        Mode1.IsChecked = true;
        settings.LoadFromRegistry(this);
      } else {
        Mode0.IsChecked = true;
        settings.LoadFromFile(this);
      }
    }

    private void Window_Closed(object sender, EventArgs e) {
      // В зависимости от выбранного режима
      if (Mode0.IsChecked == true) {
        // сбрасываем настройки в файл конфигурации
        settings.Save2File(this);
        // чистим реестр
        settings.clearRegistry();
      }
      else {
        // сбрасываем настройки в реестр
        settings.Save2Registry(this);
        // и удаляем файл
        settings.removeFile();
      }
    }

    private void Window_Deactivated(object sender, EventArgs e) {
      Window_Closed(null, null);
    }
  }
}
