using System;
using System.Windows;
using System.Windows.Media;

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

    private void Window_Deactivated(object sender, EventArgs e) {
      // В зависимости от выбранного режима
      if (Mode0.IsChecked == true) {
        // сбрасываем настройки в файл конфигурации
        settings.Save2File(this);
        // чистим реестр
        settings.clearRegistry();
      } else {
        // сбрасываем настройки в реестр
        settings.Save2Registry(this);
        // и удаляем файл
        settings.removeFile();
      }
    }

    private void Background_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
      Point position = new Point {
        X = e.GetPosition(BackgroundColorPanel).X,
        Y = e.GetPosition(BackgroundColorPanel).Y
      };

      SolidColorBrush solidcolor = ColorPicker.GetPixelColor(BackgroundColorPanel.PointToScreen(position));

      Color color = Color.FromArgb(solidcolor.Color.A,
                                   solidcolor.Color.R,
                                   solidcolor.Color.G,
                                   solidcolor.Color.B);

      SolidColorBrush brush = new SolidColorBrush(color);
      txtArea.Background = brush;
    }

    private void Foreground_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
      Point position = new Point {
        X = e.GetPosition(BackgroundColorPanel).X,
        Y = e.GetPosition(BackgroundColorPanel).Y
      };

      SolidColorBrush solidcolor = ColorPicker.GetPixelColor(BackgroundColorPanel.PointToScreen(position));

      Color color = Color.FromArgb(solidcolor.Color.A,
                                   solidcolor.Color.R,
                                   solidcolor.Color.G,
                                   solidcolor.Color.B);

      SolidColorBrush brush = new SolidColorBrush(color);
      txtArea.Foreground = brush;
    }
  }
}
