using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Test.Models
{
    public class Note
    {
        private StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        public string FileName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;

        public Note()
        {
            FileName = "notes" + Date.ToBinary().ToString() + ".txt";
        }

        public async Task SaveAsync()
        {
            StorageFile noteFile = (StorageFile)await storageFolder.TryGetItemAsync(FileName);
            if (noteFile is null)
            {
                noteFile = await storageFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            }
            await FileIO.WriteTextAsync(noteFile, Text);
        }

        public async Task DeleteAsync()
        {
            StorageFile noteFile = (StorageFile)await storageFolder.TryGetItemAsync(FileName);
            if(noteFile is not null)
            {
                await noteFile.DeleteAsync();
            }
        }
    }
}
