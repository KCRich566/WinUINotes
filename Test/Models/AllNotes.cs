using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.Storage;

namespace Test.Models
{
    public class AllNotes
    {
        public ObservableCollection<Note> Notes { get; set; } = new ObservableCollection<Note>();

        public AllNotes()
        {
            LoadNotes();
        }

        public async void LoadNotes()
        {
            Notes.Clear();
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            await GetFilesInFolderAsync(storageFolder);
        }

        private async Task GetFilesInFolderAsync(StorageFolder folder)
        {
            IReadOnlyCollection<IStorageItem> storageItems = await folder.GetItemsAsync();

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
                        FileName = file.Name,
                        Text = await FileIO.ReadTextAsync(file),
                        Date = file.DateCreated.DateTime
                    };
                    Notes.Add(note);
                }
            }
        }
    }
}
