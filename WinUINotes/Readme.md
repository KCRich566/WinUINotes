https://learn.microsoft.com/zh-tw/windows/apps/get-started/start-here?tabs=wingetconfig

要一直問 如果現在的WinUI3的xaml要移植到MAUI要注意什麼?

# 快速入門
要開發WinUI3, 需要使用最新的Visual Studio 2026, 並且將[開發人員模式](ms-settings:developers)開啟.

選擇WinUI Black App (Packaged): 表示自動使用 MSIX(Microsoft Installer for XML) 封裝應用程式，並且可以直接在 Microsoft Store 發行。

新應用程式建議使用 封裝 模板。 打包的應用程式（使用 MSIX）能讓使用者有乾淨的安裝與卸載體驗，並啟用 Windows 功能，如通知、背景任務及 Microsoft Store。 你可以了解更多關於 Windows 應用程式中打包與非打包的選擇 ：[打包、部署和流程](https://learn.microsoft.com/zh-tw/windows/apps/get-started/intro-pack-dep-proc)

# 建立第一個WinUI3應用程式

使用C#和XAML(eXtensible Application Markup Language) 來建立WinUI應用程式

## 將瞭解如何：

使用 XAML 標記來定義應用程式的使用者介面。
透過 C# 程式代碼與 XAML 元素互動。
從本機檔系統儲存和載入檔案。
建立檢視並將其系結至數據。
在應用程式中使用導覽功能以瀏覽各個頁面。
使用檔與範例應用程式等資源來建立您自己的應用程式。

## 應用程式會有兩個頁面：

NotePage - 用於編輯單一筆記的頁面。
AllNotesPage - 顯示所有已儲存筆記的頁面。

# 專案設置

- Assets:

包含應用程式的標誌、影像和其他媒體資產。
它一開始會填入您 app 標誌的佔位符檔案。
此標誌代表您的應用程式在 Windows 開始選單、Windows 任務列，以及當您的應用程式在該處發行時，Microsoft 市集中的應用程式。

- App.xaml 和 App.xaml.cs

檔案 App.xaml 包含全應用程式的 XAML 資源，例如色彩、樣式或範本。 該 App.xaml.cs 檔案通常包含實例化和啟動應用程式視窗的程式碼。 在此專案中，指向的是 MainWindow 類。

- MainWindow.xaml 和 MainWindow.xaml.cs

這些檔案代表您應用程式的視窗

- Package.appxmanifest

這個 package manifest 檔案 讓你能設定publisher資訊、標誌、處理器架構及其他細節，決定你的應用程式在 Microsoft 商店中的呈現方式。


## XAML 檔案和部分類別
可延伸的應用程式標記語言 （XAML） 是一種宣告式語言，可以初始化物件和設定物件的屬性。 
您可以在宣告式 XAML 標記中建立可見的 UI 元素。 然後您可以將每個 XAML 檔案與一個個別的程式代碼檔案關聯（稱為 程式碼後置 檔案）
這些檔案可以回應事件，並操作原本在 XAML 中宣告的物件。

通常任何 XAML 檔案都有兩個檔案，一個是它本身的 .xaml 檔案，另一個是相對應的程式碼檔案，位於 Solution Explorer 的子項目。

檔案 .xaml 包含定義應用程式 UI 的 XAML 標記。 類別名稱是使用 x:Class 屬性宣告的。
程式代碼檔案(.cs)包含您建立以與 XAML 標記互動的程式代碼，以及呼叫 InitializeComponent 方法。 類別定義為 partial class。
當你編譯 project 時，會呼叫 InitializeComponent 方法來解析 .xaml 檔案，並產生與 partial class 連接的程式碼，建立完整類別。


x:Class="...": 定義了使哪個Partial Class與XAML檔案關聯. x表示XML命名空間，Class是屬性名稱，值是類別的完全限定名稱。

| 屬性      | 用途                             |
| ------- | ------------------------------ |
| x:Class | 指定這個 XAML 對應的 C# partial class |
| x:Name  | 給 UI 元素命名，讓 code-behind 可以使用   |
| x:Key   | 給資源（Resource）命名，方便引用           |
| x:Type  | 指定類型（通常用於 Resource / Style）    |

```
<StackPanel>
    <Button x:Name="btnOK" Content="OK" Click="Button_Click"/>
</StackPanel>
```

當你編譯 project 時，會呼叫 InitializeComponent 方法來解析 .xaml 檔案，並產生與 partial class 連接的程式碼

在文件中了解更多：

[XAML 概述](https://learn.microsoft.com/zh-tw/windows/apps/develop/platform/xaml/xaml-overview)
[部分類別和方法](https://learn.microsoft.com/zh-tw/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods)（C# 程式設計手冊）
[x:Class](https://learn.microsoft.com/zh-tw/windows/apps/develop/platform/xaml/x-class-attribute) 屬性、[x:Class](https://learn.microsoft.com/zh-tw/dotnet/desktop/xaml-services/xclass-directive) 指令


## 更新主視窗

project中包含的 MainWindow 類別是 XAML Window 類別的子類別，用於定義應用程式的外殼。 應用程式視窗有兩個部份：

- 視窗的 用戶端 部分是內容所在的位置。
- 非用戶端部分是由 Windows作系統所控制的部分。 它包含標題列，其中有標題控制項（最小化/最大化/關閉按鈕）、應用程式圖示、標題和拖曳區域。 它也包含視窗外側的框架。
若要讓 WinUI Notes 應用程式與 Fluent Design 指導方針一致，您將對 進行一些修改 MainWindow。 首先，您將套用 Mica 材質作為視窗背景。 Mica 是一種不透明的動態材質，其中包含主題和桌面桌布來繪製視窗的背景。 然後，您會將應用程式的內容延伸至 標題列 區域，並將系統標題列取代為 XAML TitleBar 控制件。 這可讓您更充分地使用空間，並讓您更充分掌控設計，同時提供標題欄所需的所有功能。


您也會將 Frame 新增為視窗的內容。 類別 Frame 可與 Page 類別搭配使用，讓您在應用程式中的內容頁面之間巡覽。 您將在稍後的步驟中新增頁面。


# 建立附注的(新)頁面\\滑鼠右鍵點擊 WinUINotes 專案 >[新增>項目...] > 選取視窗左側範本清單中的 WinUI -> 選擇 空白頁（WinUI）Blank Page 範本


由於縮放系統的運作方式，當您設計 XAML 應用程式時，您要以有效圖元而非實際實體圖元來設計。 有效圖元 （epx） 是一個虛擬度量單位，用來表示與螢幕密度無關的配置尺寸和間距

# 新增附注的檢視和模型

AllNotesPage: 此頁面允許使用者選擇在編輯器頁面開啟的筆記，以便查看、編輯或刪除。 它也應該讓使用者建立新的附註
若要達成此目的， AllNotesPage 必須有附注的集合，以及顯示集合的方法。

## 視圖和模型

通常 WinUI 3 應用程式至少有一個 檢視層 和 一個資料層。

目前， NotePage 代表數據檢視（附註文字）。 不過，數據從系統檔案讀入應用程式後，它只存在於 Text 的 TextBox 屬性中 NotePage。 它不會以可讓您以不同方式或不同位置呈現數據的方式在應用程式中表示;也就是說，應用程式沒有數據層。

## 分隔檢視和模型

在 Solution Explorer，右鍵點擊 WinUINotes project，選擇 Add>New Folder。 將資料夾命名為 Models, 另一個資料命名為Views

## 定義模型

在 Solution Explorer 面板中，按右鍵 Models 資料夾，選擇 新增>Class > 命名為Note.cs。







我不太懂local
在上一個步驟中，您已建立記事頁面，並更新 MainWindow.xaml 以導航至該頁面。 請記住，它已與 local: 命名空間對應。 通常會將名稱 local 映射到你project的根命名空間，而 Visual Studio project 範本已經幫你做到這點（xmlns:local="using:WinUINotes"）。 現在頁面已移至新的命名空間，XAML 中的類型對應現在無效。

## 更新附註頁面

在 WinUI 中，有兩種類型的系結可供您選擇：

標記 {x:Bind} 編譯時綁定. 延伸會在編譯階段處理。 它的一些好處是改善綁定表達式的效能和編譯時驗證。 建議在 WinUI 應用程式中系結。綁定屬性或方法呼叫，不需要複雜 DataContext 路徑
標記 {Binding} 執行時綁定. 延伸會在執行階段進行處理，並使用一般用途執行階段物件檢查。使用 MVVM，DataContext綁定

ViewModel
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

## 數據系結和MVVM

Model-View-ViewModel（MVVM）是一種用於將 UI 與非 UI 程式碼解耦的 UI 架構設計模式


# 為所有附註新增一個檢視與模型

## 多個附註和導覽

在 Solution Explorer 面板中，右鍵點擊 Views 資料夾，選擇 新增>新項目... > 空白頁（WinUI）Page > 命名為 AllNotesPage.xaml
在 Solution Explorer 面板中，右鍵按一下 資料夾，選擇 新增類別...為類別AllNotes.cs命名，然後按 [新增]。

## 新增數據範本

必須指定 DataTemplate ，以告知 ItemsView 應該如何顯示資料項. ItemsView.ItemTemplate 會根據宣告產生 XAML


當您在 XAML ResourceDictionary中定義資源時，您必須指派 x:Key 值來識別資源。 然後，您可以使用該 x:Key 屬性，透過 {StaticResource} 標記延伸或 {ThemeResource} 標記延伸，在 XAML 中擷取資源。

不論色彩主題為何，{StaticResource} 都相同，因此會用於如 Font 或 Style 等設置。
{ThemeResource}會根據選取的色彩主題進行變更，因此會用於Foreground、 Background和其他色彩相關屬性。

