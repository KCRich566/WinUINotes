using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Storage;
using System.Threading.Tasks;
using WinUINotes.Models;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUINotes.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>

// sealed class → 類別不能被繼承
// sealed override → 方法不能被再次覆寫
public sealed partial class NotePage : Page
{
    //應用程式的本機資料資料資料夾
    //private StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
    //private StorageFile? noteFile = null;
    //private string fileName = "note.txt";
    private Note? noteModel;
    public NotePage()
    {
        
        // 讀取 XAML 標記，並初始化標記所定義的所有物件。
        // 物件會以其父子式關聯性連接，而且程式代碼中定義的事件處理程式會附加至 XAML 中設定的事件
        InitializeComponent();

        //Loaded += NotePage_Loaded;
    }

    //private async void NotePage_Loaded(object sender, RoutedEventArgs e)
    //{
    //    noteFile = (StorageFile)await storageFolder.TryGetItemAsync(fileName);
    //    if (noteFile is not null)
    //    {
    //        NoteEditor.Text = await FileIO.ReadTextAsync(noteFile);
    //    }
    //}

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        //if (noteFile is null)
        if (noteModel is not null)
        {
            await noteModel.SaveAsync();
            //noteFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
        }
        //await FileIO.WriteTextAsync(noteFile, NoteEditor.Text);
    }

    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        //if (noteFile is not null)
        if (noteModel is not null)
        {
            await noteModel.DeleteAsync();
            //    await noteFile.DeleteAsync();
            //    noteFile = null;
            //    NoteEditor.Text = string.Empty;
        }
        
    }


}
