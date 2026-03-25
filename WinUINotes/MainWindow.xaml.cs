using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUINotes
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            // 讀取 XAML 標記，並初始化標記所定義的所有物件。
            // 物件會以其父子式關聯性連接，而且程式代碼中定義的事件處理程式會附加至 XAML 中設定的事件
            InitializeComponent();

            // 讓你的 App 內容「延伸進」原本系統的標題列區域,
            // 預設情況（false）: 上面一條是 Windows 原生標題列, 你不能動它的 UI（只能改 Title）
            // 設成 true 後, 整個視窗（包含最上面）都變成你的控制範圍
            // 你可以自己畫標題列 UI（Logo / 按鈕 / 搜尋欄等）
            ExtendsContentIntoTitleBar = true;

            // 設定視窗的titleBar指向在xaml中定義的AppTitleBar, 使他可以被拖曳移動
            SetTitleBar(AppTitleBar);
        }
    }
}
