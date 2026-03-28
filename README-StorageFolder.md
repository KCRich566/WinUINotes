# StorageFolder 學習筆記

## 什麼是 UWP？

UWP (Universal Windows Platform) 是一種由微軟開發的應用程式平台，允許開發者創建可以在多種 Windows 設備上運行的應用程式，包括桌面電腦、平板電腦、手機和 Xbox 等。UWP 應用程式使用 C#、XAML 和其他相關技術來構建用戶界面和實現功能。

## 沙箱（Sandbox）機制

因為使用沙箱的機制，UWP 應用程式只能存取特定的資料夾，包括 `LocalFolder`、`RoamingFolder` 和 `TemporaryFolder` 等。這些資料夾是由系統管理的，可以用來儲存應用程式的設定、暫存檔案和其他資料。相當於軟體隔離。

AppContext.BaseDirectory 回傳目前執行組件的基底目錄（通常是應用程式的安裝資料夾 / executable 位置）。在 packaged WinUI/UWP 應用中，這通常是套件的安裝資料夾（Package 安裝路徑），那個資料夾是「可讀但不可寫」的（安裝資料夾為唯讀）。

因此你可以用 AppContext.BaseDirectory 或 Package.Current.InstalledLocation 去讀取放在安裝資料夾（例如 Assets）內的檔案，但不能在該目錄寫入或建立檔案。若要讀寫資料，應使用 ApplicationData.Current.LocalFolder（App 可在此自由讀寫）。

Package.appxmanifest 中的 `Capabilities` 標籤可以用來宣告應用程式需要存取的資源和功能，例如網路、相機、位置等。這些宣告會影響應用程式的權限和使用者體驗。
其中`broadFileSystemAccess` 是一個特殊的能力，允許應用程式存取整個檔案系統，但需要使用者明確授權，且只能在特定情況下使用。

file/folder picker 不需要特別定義Capability的內容, 因為picker的觸發是由使用者主動操作的, 系統會自動處理權限問題
```xml
<Capabilities>
	<!-- restricted capability: rescap-->
	<rescap:Capability Name="broadFileSystemAccess" />
</Capabilities>
```


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
