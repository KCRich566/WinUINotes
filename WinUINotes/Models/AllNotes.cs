using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace WinUINotes.Models
{
    public class AllNotes
    {
        // 這是一個特別的集合，適用於數據系結。當列出多個專案，
        // 例如 ItemsView 的控件系結至 ObservableCollection時，兩者會一起運作，以自動讓專案清單與集合保持同步

        public ObservableCollection<Note> Notes { get; set; } = new ObservableCollection<Note>();
        public AllNotes()
        {
            // 筆記將在頁面 OnNavigatedTo 時載入, 這是為了確保每次返回頁面時都會重新載入筆記, 以反映任何更改, 避免重複載入
            // LoadNotes();
        }

        public async void LoadNotes()
        {
            Notes.Clear();

            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            // 從資料夾讀取所有檔案(Recursive)
            await GetFilesInFolderAsync(storageFolder);
        }

        public async Task DeleteNotesAsync(IEnumerable<Note> notes)
        {
            foreach (Note note in notes)
            {
                await note.DeleteAsync();
                Notes.Remove(note);
            }
        }
        private async Task GetFilesInFolderAsync(StorageFolder folder)
        {
            // 先取得第一層的所有檔案和資料夾
            IReadOnlyList<IStorageItem> storageItems = await folder.GetItemsAsync();

            foreach (IStorageItem item in storageItems)
            {
                if (item.IsOfType(StorageItemTypes.Folder))
                {
                    await GetFilesInFolderAsync((StorageFolder)item);
                }
                else if (item.IsOfType(StorageItemTypes.File))
                {
                    StorageFile file = (StorageFile)item;
                    Note note = new Note()
                    {
                        Filename = file.Name,
                        Text = await FileIO.ReadTextAsync(file),
                        Date = file.DateCreated.DateTime
                    };
                    Notes.Add(note);
                }
            }
        }
    }
}
