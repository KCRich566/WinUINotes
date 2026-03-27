# StorageFolder 學習筆記

## 什麼是 UWP？

UWP (Universal Windows Platform) 是一種由微軟開發的應用程式平台，允許開發者創建可以在多種 Windows 設備上運行的應用程式，包括桌面電腦、平板電腦、手機和 Xbox 等。UWP 應用程式使用 C#、XAML 和其他相關技術來構建用戶界面和實現功能。

## 沙箱（Sandbox）機制

因為使用沙箱的機制，UWP 應用程式只能存取特定的資料夾，包括 `LocalFolder`、`RoamingFolder` 和 `TemporaryFolder` 等。這些資料夾是由系統管理的，可以用來儲存應用程式的設定、暫存檔案和其他資料。相當於軟體隔離。

`System.IO.Directory` 屬於同步 API，且在沙箱之外無法使用。相反地，UWP 提供了 `Windows.Storage` 命名空間中的 `StorageFolder` 類別來存取和管理應用程式的資料夾。

## System.IO（傳統寫法，沙箱外）


```CSharp
using System.IO;

Directory.CreateDirectory(@"C:\MyFolder");
// 建立與寫入檔案
File.WriteAllText(filepath, "Hello, World!");

// 取得資料夾下的所有檔案(Recursive)
public void GetFilesInFolder(string folderPath)
{
	foreach( string file in System.IO.Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories))
	{
		Note note = new Note()
		{
			Filename = System.IO.Path.GetFileName(file),
			Text = System.IO.File.ReadAllText(file),
			Date = System.IO.File.GetCreationTime(file)
		};
		Notes.Add(note);

	}
}

// 讀取檔案
public string ReadFile(string filepath)
{
	if (System.IO.File.Exists(filepath) == true)
	{
		return System.IO.File.ReadAllText(filepath);
	}
	return string.Empty;
}

// 刪除檔案
public void DeleteFile(string filepath)
{
	if (System.IO.File.Exists(filepath) == true)
	{
		System.IO.File.Delete(filepath);
	}
}

```

## Windows.Storage（沙箱內）

```CSharp
using Windows.Storage;

// 取得LocalFolder的位置
StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

// 取得LocalFolder下的所有檔案(Recursive)
private async Task GetFilesInFolderAsync(StorageFolder folder)
{
	IReadOnlyList<IStorageItem> files = await folder.GetItemsAsync();
	foreach (IStorageItem item in files)
	{
		if (item.IsOfType(StorageItemTypes.Folder) == true)
		{
			await GetFilesInFolderAsync((StorageFolder) item);
		}
		else if (item.IsOfType(StorageItemTypes.File) == true)
		{
			StorageFile file = (StorageFile) item;
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

// 建立與寫入檔案
public async Task CreateAndWriteFileAsync(string filename)
{
	StorageFile? file = (StorageFile?)await storageFolder.TryGetItemAsync(filename);
	if (file == null)
	{
		file = await storageFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
	}
	await FileIO.WriteTextAsync(file, "Hello, World!");
}

// 讀取檔案
public async Task<string> ReadFileAsync(string filename)
{
	StorageFile? file = (StorageFile?)await storageFolder.TryGetItemAsync(filename);
	if (file != null)
	{
		return await FileIO.ReadTextAsync(file);
	}
	return string.Empty;
}

// 刪除檔案
public async Task DeleteFileAsync(string filename)
{
	StorageFile? file = (StorageFile?)await storageFolder.TryGetItemAsync(filename);
	if ( file != null)
	{
		await file.DeleteAsync();
	}
}

```
