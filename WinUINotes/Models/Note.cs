using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace WinUINotes.Models
{
    public class Note
    {
        // 存取應用程式的本機資料資料資料夾
        private StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        public string Filename { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;

        public Note()
        {
            Filename = "notes" + DateTime.Now.ToBinary().ToString() + ".txt";
        }
        
        public async Task SaveAsync()
        {
            //從資料夾擷取文字檔。 如果檔案不存在，則會傳 null回
            StorageFile noteFile = (StorageFile)await storageFolder.TryGetItemAsync(Filename);
            if (noteFile is null)
            {
                noteFile = await storageFolder.CreateFileAsync(Filename, CreationCollisionOption.ReplaceExisting);
            }
            await FileIO.WriteTextAsync(noteFile, Text);
        }
        public async Task DeleteAsync()
        {
            StorageFile noteFile = (StorageFile)await storageFolder.TryGetItemAsync(Filename);
            if ( noteFile is not null)
            {
                await noteFile.DeleteAsync();
            }
        }
    }
}
