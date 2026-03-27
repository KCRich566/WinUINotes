using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.WebUI;
using WinRT.Interop;
using WinUINotes.Models;
using Windows.Storage;
using Windows.Devices.AllJoyn;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUINotes.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AllNotesPage : Page
    {
        private AllNotes notesModel = new AllNotes();
        public AllNotesPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            notesModel.LoadNotes();
        }

        private void NewNoteButton_Click(object sender, RoutedEventArgs e)
        {
            // 每個 Page 都有一個 Frame 屬性
            Frame.Navigate(typeof(NotePage));
        }

        private void ItemsView_ItemInvoked(ItemsView sender, ItemsViewItemInvokedEventArgs args)
        {
            Frame.Navigate(typeof(NotePage), args.InvokedItem);
        }

        // 這種menu flyout的操作是每個item會個別觸發
        private async void DeleteNote_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem && menuItem.Tag is Note note)
            {
                if (await ShowDeleteConfirmationAsync(1))
                {
                    await note.DeleteAsync();
                    notesModel.Notes.Remove(note);
                }
            }
        }


        private async void ExportNote_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem && menuItem.Tag is Note note) 
            {
                await ExportNotesAsync(new List<Note> { note });
            }
        }

        private async void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            List<Note>? selectedNotes = NotesItemsView.SelectedItems?.OfType<Note>().ToList();
            if (selectedNotes == null || selectedNotes.Count == 0)
            {
                await ShowInfoDialogAsync("No notes selected.");
                return;
            }
            if(await ShowDeleteConfirmationAsync(selectedNotes.Count))
            {
                await notesModel.DeleteNotesAsync(selectedNotes);
            }

        }

        private async void ExportSelected_Click(object sender, RoutedEventArgs e)
        {
            List<Note>? selectedNotes = NotesItemsView.SelectedItems?.OfType<Note>().ToList();
            if (selectedNotes == null || selectedNotes.Count == 0)
            {
                await ShowInfoDialogAsync("No notes selected.");
                return;
            }

            await ExportNotesAsync(selectedNotes);
        }

        // 匯入筆記
        private async void ImportNotes_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".txt");

            InitializeWithWindow.Initialize(picker, GetWindowHandle());

            IReadOnlyCollection<StorageFile> files = await picker.PickMultipleFilesAsync();

            if (files == null || files.Count == 0) return;

            foreach(StorageFile file in files)
            {
                string text = await FileIO.ReadTextAsync(file);
                Note note = new Note()
                {
                    Text = text,
                };
                await note.SaveAsync();
            }
            notesModel.LoadNotes();
        }

        private async Task ExportNotesAsync(IList<Note> notes)
        {
            // 是使用Windows.Storage.Pickers而不是Microsoft.Windows.Storage.Pickers.
            // 因為 Windows.Storage.Pickers 是 Windows Runtime API 的一部分，而 Microsoft.Windows.Storage.Pickers 是 Windows Forms API 的一部分。
            FolderPicker picker = new FolderPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            // 至少要有個FileTypeFilter,不然會丟出例外
            picker.FileTypeFilter.Add("*");

            InitializeWithWindow.Initialize(picker, GetWindowHandle());

            StorageFolder folder = await picker.PickSingleFolderAsync();
            if(folder == null) { return; }
            foreach(Note note in notes)
            {
                StorageFile file = await folder.CreateFileAsync(note.Filename, CreationCollisionOption.GenerateUniqueName);
                await FileIO.WriteTextAsync(file, note.Text);
            }
            await ShowInfoDialogAsync("Export completed!");
        }

        // 顯示刪除確認對話框
        private async Task<bool> ShowDeleteConfirmationAsync(int count)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "Confirm Deletion",
                Content = $"Are you sure you want to delete {count} note(s)?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.XamlRoot
            };
            ContentDialogResult result = await dialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }


        // 顯示訊息對話框(我想要有多視窗)
        private async Task ShowInfoDialogAsync(string message)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "Hint",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }

        // 取得視窗控制代碼（Window Handle），供 FilePicker / FolderPicker 使用??? 為什麼要取得視窗控制代碼？
        // 因為 FilePicker / FolderPicker 需要知道在哪個視窗上顯示，
        // 才能正確地顯示在使用者的螢幕上。這樣可以確保使用者在選擇檔案或資料夾時，能夠看到對話框並與之互動。
        // OpenFileDialog為什麼不用？因為 OpenFileDialog 是 Windows Forms 的一部分，而不是 WinUI 的一部分。WinUI 使用的是 Windows Runtime API，
        // 而不是 Windows Forms API。因此，在 WinUI 中使用 FilePicker 或 FolderPicker 時，需要提供視窗控制代碼來確保對話框能夠正確顯示在使用者的螢幕上。
        private static IntPtr GetWindowHandle()
        {
            return WindowNative.GetWindowHandle(App.MainWindow ?? throw new InvalidOperationException("MainWindow is not initialized."));
        }
    }
    
}
