https://learn.microsoft.com/zh-tw/windows/apps/get-started/start-here?tabs=wingetconfig

# WinUI Notes 學習筆記

> 📖 參考：[WinUI 3 快速入門](https://learn.microsoft.com/zh-tw/windows/apps/get-started/start-here?tabs=wingetconfig)
>
> 💡 持續留意：WinUI 3 的 XAML 若要移植到 MAUI 需要注意什麼？


---

## 目錄

1. [快速入門](#快速入門)
2. [專案結構](#專案結構)
3. [XAML 檔案與 Partial Class](#xaml-檔案與-partial-class)
4. [更新主視窗 MainWindow](#更新主視窗-mainwindow)
5. [建立筆記頁面 NotePage](#建立筆記頁面-notepage)
6. [新增檢視與模型](#新增檢視與模型)
7. [XAML 命名空間與 local 前綴](#xaml-命名空間與-local-前綴)
8. [資料繫結 Data Binding](#資料繫結-data-binding)
9. [MVVM 模式](#mvvm-模式)
10. [所有筆記頁面 AllNotesPage](#所有筆記頁面-allnotespage)

---


## 快速入門

開發 WinUI 3 的前置需求：

1. 安裝最新的 **Visual Studio 2026**
2. 開啟 [開發人員模式](ms-settings:developers)
3. 建立專案時選擇 **WinUI Blank App (Packaged)**

> **封裝（Packaged） vs 未封裝（Unpackaged）**
>
> 新應用程式建議使用**封裝**模板。封裝應用程式使用 MSIX（Microsoft Installer for XML），
> 提供乾淨的安裝/卸載體驗，並可啟用通知、背景任務及 Microsoft Store 發行。
>
> 詳情：[打包、部署和流程](https://learn.microsoft.com/zh-tw/windows/apps/get-started/intro-pack-dep-proc)

---

## 建立第一個 WinUI 3 應用程式

使用 **C#** 和 **XAML**（eXtensible Application Markup Language）來建立 WinUI 應用程式。

### 學習目標

- 使用 XAML 標記定義 UI
- 透過 C# 程式碼與 XAML 元素互動
- 從本機檔案系統儲存和載入檔案
- 建立檢視（View）並繫結至資料
- 使用導覽功能瀏覽頁面

### 應用程式頁面

| 頁面 | 用途 |
|------|------|
| `NotePage` | 編輯單一筆記 |
| `AllNotesPage` | 顯示所有已儲存的筆記 |

---

## 專案結構

| 檔案 | 說明 |
|------|------|
| `Assets/` | 包含 app 標誌的佔位符檔案，用於開始選單、任務列、Microsoft Store |
| `App.xaml` | 全應用程式的 XAML 資源（色彩、樣式、範本） |
| `App.xaml.cs` | 實例化並啟動 `MainWindow` |
| `MainWindow.xaml` / `.cs` | 應用程式的主視窗 |
| `Package.appxmanifest` | 設定 publisher、標誌、處理器架構，決定在 Store 的呈現方式 |

---


## XAML 檔案與 Partial Class

XAML 是一種**宣告式語言**，用來初始化物件及設定屬性。每個 `.xaml` 檔案搭配一個 `.xaml.cs` 程式碼後置（code-behind）檔案，兩者透過 **partial class** 組成完整類別。

### 運作流程

- `.xaml` 定義 UI，透過 `x:Class` 指定對應的 C# 類別
- `.xaml.cs` 撰寫事件處理邏輯，呼叫 `InitializeComponent()` 初始化 XAML 元素
- **編譯時**，編譯器會呼叫 InitializeComponent 方法來解析 `.xaml`，產生另一半 partial class，與 `.cs` 合併

### `x:` 常用屬性

| 屬性 | 用途 |
|------|------|
| `x:Class` | 指定此 XAML 對應的 C# partial class（完全限定名稱） |
| `x:Name` | 給 UI 元素命名，讓 code-behind 可透過變數存取 |
| `x:Key` | 給資源（Resource）命名，方便用 `{StaticResource}` 引用 |
| `x:Type` | 指定類型（通常用於 Resource / Style） |

**範例：**

```
<StackPanel>
    <Button x:Name="btnOK" Content="OK" Click="Button_Click"/>
</StackPanel>
```

> 📚 延伸閱讀：
> - [XAML 概述](https://learn.microsoft.com/zh-tw/windows/apps/develop/platform/xaml/xaml-overview)
> - [部分類別和方法](https://learn.microsoft.com/zh-tw/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods)（C# 程式設計手冊）
> - [x:Class 屬性](https://learn.microsoft.com/zh-tw/windows/apps/develop/platform/xaml/x-class-attribute)

---


## 更新主視窗 MainWindow

`MainWindow` 繼承自 XAML `Window`，用來定義應用程式的外殼。

### 視窗組成

- **用戶端區域（Client Area）**：應用程式內容所在位置
- **非用戶端區域（Non-Client Area）**：由 Windows 控制，包含標題列（最小化/最大化/關閉按鈕）、app 圖示、標題、拖曳區域及視窗框架

### Fluent Design 調整

1. **Mica 材質** — 設定 `Window.SystemBackdrop` 為 `MicaBackdrop`，取得不透明的動態桌面融合背景
2. **自訂標題列** — `ExtendsContentIntoTitleBar = true`，用 XAML `TitleBar` 控制項取代系統標題列
3. **Frame 導覽** — 在視窗中放一個 `Frame`，搭配 `Page` 實現頁面切換

可以參考 MainWindow.xaml與MainWindow.xaml.cs 



---

## 建立筆記頁面 NotePage

### 新增方式

右鍵點擊 WinUINotes 專案 → **新增 > 新項目...** → 選取 **WinUI > 空白頁（Blank Page）**

### 有效圖元（epx）

> 設計 XAML 時使用**有效圖元（effective pixels, epx）** 而非實體圖元。
> epx 是與螢幕密度無關的虛擬單位，確保不同 DPI 裝置上呈現一致。

---

## 新增檢視與模型

### 為什麼要分層？

目前 `NotePage` 的資料只存在於 `TextBox.Text` 屬性中，沒有獨立的資料層。
這樣無法在不同頁面重複使用或以不同方式呈現資料。

### 分隔步驟

1. 在專案中新增 **`Models/`** 資料夾 → 放資料模型
2. 新增 **`Views/`** 資料夾 → 放 UI 頁面

### Note 模型 可參考`Models/Note.cs`
### AllNotes 模型 可參考 `Models/AllNotes.cs`

---

## XAML 命名空間與 `local` 前綴

> 💡 **原本的困惑：「我不太懂 local」— 以下解說。**

在 `MainWindow.xaml` 中你會看到：

```
xmlns:local="using:WinUINotes"
xmlns:views="using:WinUINotes.Views"
```
### 這是什麼意思？

`xmlns:前綴="using:C#命名空間"` 是把 C# namespace 引入 XAML，讓你用 `前綴:類別名` 存取該 namespace 裡的型別。

| XAML 前綴 | 對應 C# 命名空間 | 能存取的類別 |
|-----------|-----------------|-------------|
| `local:` | `WinUINotes` | `MainWindow`、`App` 等根命名空間下的類別 |
| `views:` | `WinUINotes.Views` | `NotePage`、`AllNotesPage` |
| `models:` | `WinUINotes.Models` | `Note`、`AllNotes` |

### 頁面搬到 Views 後為什麼要改？

❌ 頁面搬到 WinUINotes.Views 後，local 只對應根命名空間，找不到 NotePage --> <Frame SourcePageType="local:NotePage"/>
✅ 要用 views 前綴才能找到 --> <Frame SourcePageType="views:AllNotesPage"/>

> **一句話記住**：`local` 只是一個自訂名稱（可以叫任何名字），重點是 `using:` 後面對應哪個 C# namespace。類別搬了 namespace，XAML 前綴就要跟著改。


??? 為什麼要加views的原因是local沒辦法又只到Views裏頭的類別了 對嗎? 
---

## 資料繫結 Data Binding

WinUI 中有兩種繫結方式：

| 繫結方式 | 處理時機 | 特點 | 建議 |
|----------|----------|------|------|
| `{x:Bind}` | **編譯時** | 效能佳、有編譯時驗證、型別安全 | ✅ WinUI 優先使用 |
| `{Binding}` | **執行時** | 使用 `DataContext`，較彈性 | MVVM / 動態場景 |

### 本專案使用 `{x:Bind}` 的範例

可以參考 NotePage.xaml與NotePage.xaml.cs 

 `x:Bind` 預設：繫結**屬性** → `OneWay`（Model→View），繫結**方法/表達式** → `OneTime`（只讀一次）

### `{Binding}` 範例（搭配 ViewModel）

**ViewModel：**
```
public class NoteViewModel : INotifyPropertyChanged
{
    private string _text;
    public string Text
    {
        get => _text;
        set { _text = value; OnPropertyChanged(); }
    }

    private DateTime _date;
    public DateTime Date
    {
        get => _date;
        set { _date = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
```

Page 初始化 DataContext
```
public sealed partial class NotePage : Page
{
    public NoteViewModel VM { get; set; }

    public NotePage()
    {
        this.InitializeComponent();
        VM = new NoteViewModel { Text = "Hello", Date = DateTime.Now };
        this.DataContext = VM;
    }
}
```
XAML 綁定
```
<StackPanel>
    <!-- 雙向綁定 TextBox -->
    <TextBox Text="{Binding Text, Mode=TwoWay}" />

    <!-- 顯示日期 -->
    <TextBlock Text="{Binding Date, StringFormat='yyyy-MM-dd'}" />
</StackPanel>
```

--

## MVVM 模式

**Model-View-ViewModel（MVVM）** 是將 UI 與非 UI 程式碼解耦的架構設計模式。

```mermaid
flowchart LR V["View<br>（XAML 頁面）"] <-->|"資料繫結"| VM["ViewModel<br>（C#35; 邏輯 + INotifyPropertyChanged）"] VM <-->|"存取"| M["Model<br>（資料 + 商業邏輯）"]
```

- **Model** — 資料與商業邏輯（`Note.cs`、`AllNotes.cs`）
- **View** — UI 頁面（`NotePage.xaml`、`AllNotesPage.xaml`）
- **ViewModel** — 連接 View 與 Model，實作 `INotifyPropertyChanged` 通知 UI 更新

---

## 所有筆記頁面 AllNotesPage

### 新增步驟

1. 在 `Views/` 新增 **空白頁（WinUI）** → `AllNotesPage.xaml`
2. 在 `Models/` 新增 **類別** → `AllNotes.cs`

### 頁面導覽流程

```sequenceDiagram
participant U as 使用者 participant AP as AllNotesPage participant F as Frame participant NP as NotePage participant N as Note participant FS as LocalFolder
AP->>F: 初始頁面（SourcePageType）

U->>AP: 點擊「New Note」
AP->>F: Frame.Navigate(typeof(NotePage))
F->>NP: OnNavigatedTo → new Note()

U->>NP: 輸入文字 → 點擊「Save」
NP->>N: SaveAsync()
N->>FS: CreateFileAsync + WriteTextAsync

U->>NP: 點擊「Delete」
NP->>N: DeleteAsync()
NP->>F: Frame.GoBack()
F->>AP: 返回 AllNotesPage

U->>AP: 點擊既有筆記
AP->>F: Frame.Navigate(typeof(NotePage), note)
F->>NP: OnNavigatedTo → e.Parameter is Note

```
## DataTemplate（資料範本）

用 `DataTemplate` 告訴 `ItemsView` 每筆資料要長什麼樣

```
<Page.Resources> <DataTemplate x:Key="NoteItemTemplate" x:DataType="models:Note"> <ItemContainer> <Grid> <TextBlock Text="{x:Bind Text}" /> <TextBlock Text="{x:Bind Date}" /> </Grid> </ItemContainer> </DataTemplate> </Page.Resources>
<ItemsView ItemsSource="{x:Bind notesModel.Notes}" ItemTemplate="{StaticResource NoteItemTemplate}" />

```

### XAML 資源擷取方式

| 標記 | 說明 | 適用場景 |
|------|------|----------|
| `{StaticResource}` | 載入後固定，不隨主題變更 | Font、Style、DataTemplate |
| `{ThemeResource}` | 隨系統色彩主題動態切換 | Foreground、Background 等色彩屬性 |

> 在 `ResourceDictionary` 中定義資源時，必須用 `x:Key` 識別，再透過 `{StaticResource KeyName}` 或 `{ThemeResource KeyName}` 引用。