using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Preferences {
  public partial class App : Application {
    public int fontSize { get; set; } = 12;
  }
}
