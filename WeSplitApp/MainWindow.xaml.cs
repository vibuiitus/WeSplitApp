﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;

namespace WeSplitApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		//---------------------------------------- Khai báo các biến toàn cục --------------------------------------------//

		public event PropertyChangedEventHandler PropertyChanged;
		private Button clickedControlButton, clickedTypeButton;
		private List<Trip> TripInfoList = new List<Trip>();     //Danh sách thông tin tất cả các chuyến đi
		private CollectionView view;
		//private BindingList<Trip> TripOnScreen;						//Danh sách chuyến đi để hiện trên màn hình
		private BindingList<ColorSetting> ListColor;
		private Condition FilterCondition = new Condition { Type = "" };
		public Trip trip = new Trip();
		private bool isMinimizeMenu, isEditMode, IsDetailTrip;
		int selectedTripIndex = 0;
		/*private int TripPerPage = 12;           //Số chuyến đi mỗi trang
		private int _totalPage = 0;             //Tổng số trang
		public int TotalPage
		{
			get
			{
				return _totalPage;
			}
			set
			{
				_totalPage = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("TotalPage"));
				}
			}
		}
		private int _currentPage = 1;           //Trang hiện tại
		public int CurrentPage
		{
			get
			{
				return _currentPage;
			}
			set
			{
				_currentPage = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("CurrentPage"));
				}
			}
		}
		private string _totalItem = "0 item";   //Tổng số món ăn theo filter hiện tại
		public string TotalItem
		{
			get
			{
				return _totalItem;
			}
			set
			{
				_totalItem = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("TotalItem"));
				}
			}
		}*/

		private string _colorScheme = "";           //Màu nền hiện tại
		public string ColorScheme
		{
			get
			{
				return _colorScheme;
			}
			set
			{
				_colorScheme = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("ColorScheme"));
				}
			}
		}

		//---------------------------------------- Khai báo các class --------------------------------------------//

		//Class lưu trữ màu trong Color setting
		public class ColorSetting
		{
			public string Color { get; set; }
		}

		//Class điều kiện để filter
		class Condition
		{
			public string Type;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			// Đọc dữ liệu các món ăn từ data

			XmlSerializer xsFood = new XmlSerializer(typeof(List<Trip>));
			try
			{
				using (var reader = new StreamReader(@"Data\Trip.xml"))
				{
					TripInfoList = (List<Trip>)xsFood.Deserialize(reader);
				}
			}
			catch
			{
				TripInfoList = new List<Trip>();
			}
			/*TripOnScreen = TripInfoList;

			//Khởi tạo phân trang
			TotalPage = (TripInfoList.Count - 1) / TripPerPage + 1;
			TotalItem = TripInfoList.Count.ToString();
			if (TripInfoList.Count > 1)
			{
				TotalItem += " items";
			}
			else
			{
				TotalItem += " item";
			}*/
			//UpdatePageButtonStatus();

			//         TripInfoList = new List<Trip>
			//         {
			//             new Trip
			//             {
			//                 TripName = "abc",
			//                 Location = "Quang Nam",
			//                 Stage = "abc",
			//                 PrimaryImagePath = "Images\\1.jpg",
			//                 IsFavorite = true,
			//                // ImagesList = new List<string> { "Images\\2.jpg", "Images\\3.jpg" },
			//                 MembersList = new List<Member>
			//                 {
			//                     new Member
			//                     {
			//                         MemberName = "Bui Van Vi",
			//                         CostsList = new List<Cost>
			//                         {
			//                             new Cost
			//                             {
			//                                 PaymentName = "Com D2",
			//                                 Charge = 20000
			//                             },
			//                             new Cost
			//                             {
			//                                 PaymentName = "Sua chua nha dam",
			//                                 Charge = 7000
			//                             }
			//                         }
			//                     },
			//                     new Member
			//                     {
			//                         MemberName = "Pham Tan",
			//                         CostsList = new List<Cost>
			//                         {
			//                             new Cost
			//                             {
			//                                 PaymentName = "Pho B5",
			//                                 Charge = 22000
			//                             },
			//                             new Cost
			//                             {
			//                                 PaymentName = "Sua chua Long Thanh",
			//                                 Charge = 6000
			//                             }
			//                         }
			//                     }
			//                 }
			//             }
			//         };
			//DetailTripGrid.DataContext = TripInfoList[0];


			this.DataContext = this;

			//Tạo dữ liệu màu cho ListColor
			ListColor = new BindingList<ColorSetting>
			{
				new ColorSetting { Color = "#FFCA5010"}, new ColorSetting { Color = "#FFFF8C00"}, new ColorSetting { Color = "#FFE81123"}, new ColorSetting { Color = "#FFD13438"}, new ColorSetting { Color = "#FFFF4081"},
				new ColorSetting { Color = "#FFC30052"}, new ColorSetting { Color = "#FFBF0077"}, new ColorSetting { Color = "#FF9A0089"}, new ColorSetting { Color = "#FF881798"}, new ColorSetting { Color = "#FF744DA9"},
				new ColorSetting { Color = "#FF4CAF50"}, new ColorSetting { Color = "#FF10893E"}, new ColorSetting { Color = "#FF018574"}, new ColorSetting { Color = "#FF03A9F4"}, new ColorSetting { Color = "#FF304FFE"},
				new ColorSetting { Color = "#FF0063B1"}, new ColorSetting { Color = "#FF6B69D6"}, new ColorSetting { Color = "#FF8E8CD8"}, new ColorSetting { Color = "#FF8764B8"}, new ColorSetting { Color = "#FF038387"},
				new ColorSetting { Color = "#FF525E54"}, new ColorSetting { Color = "#FF7E735F"}, new ColorSetting { Color = "#FF9E9E9E"}, new ColorSetting { Color = "#FF515C6B"}, new ColorSetting { Color = "#FF000000"}
			};
			ColorScheme = ListColor[11].Color;

			//Danh sách các giai đoạn của chuyến đi
			//StageList = new BindingList<string>
			//{
			//	"Bắt đầu", "Đang đi", "Đến nơi", "Đang về", "Kết thúc"
			//};

			//Mặc định khi mở ứng dụng thị hiển thị menu ở dạng mở rộng
			isMinimizeMenu = false;
			//Mặc định không ở màn hình chi tiết
			IsDetailTrip = false;

			//Mặc định không ở chế độ chỉnh sửa chuyến đi
			isEditMode = false;
			
			TripButtonItemsControl.ItemsSource = TripInfoList;
			view = (CollectionView)CollectionViewSource.GetDefaultView(TripInfoList);
			TripListAppearAnimation();

			//Default buttons
			clickedTypeButton = AllButton;
			clickedTypeButton.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorScheme);
			clickedControlButton = HomeButton;
		}

		public MainWindow()
		{
			InitializeComponent();
		}

		//---------------------------------------- Các hàm xử lý cập nhật giao diện --------------------------------------------//

		//Cập nhật lại thay đổi từ dữ liệu lên màn hình
		private void UpdateUIFromData()
		{
			view.Filter = Filter;
			TripButtonItemsControl.ItemsSource = TripInfoList;
			TripListAppearAnimation();
		}

		//---------------------------------------- Các hàm Get --------------------------------------------//

		//Get current app domain
		public static string GetAppDomain()
		{
			string absolutePath;
			absolutePath = AppDomain.CurrentDomain.BaseDirectory;
			return absolutePath;
		}

		//Lấy danh sách món ăn của view
		//private void GetFilterList()
		//{
		//	TripOnScreen = new List<Trip>();
		//	foreach (var trip in view)
		//	{
		//		TripOnScreen.Add((Trip)trip);
		//	}
		//}

		//Lấy chỉ số phần tử của chuyến đi trong mảng
		private int GetElementIndexInArray(Button button)
		{
			var curTrip = new Trip();
			//Nếu nhấn hình ảnh món ăn ở màn hình Home
			if (button.Content.GetType().Name == "WrapPanel")
			{
				var wrapPanel = (WrapPanel)button.Content;
				curTrip = (Trip)wrapPanel.DataContext;
			}
			else //Nếu nhấn món ăn ở trong nút Search
			{
				curTrip = (Trip)button.DataContext;
			}

			var result = 0;
			for (int i = 0; i < TripInfoList.Count; i++)
			{
				if (curTrip == TripInfoList[i])
				{
					result = i;
					break;
				}
				else
				{
					//Do nothing
				}
			}
			return result;
		}

		//---------------------------------------- Các hàm lưu trữ dữ liệu --------------------------------------------//

		//Lưu lại danh sách món ăn
		private void SaveListFood()
		{
			XmlSerializer xs = new XmlSerializer(typeof(List<Trip>));
			TextWriter writer = new StreamWriter(@"Data\Trip.xml");
			xs.Serialize(writer, TripInfoList);
			writer.Close();
		}


		//---------------------------------------- Xử lý cửa sổ --------------------------------------------//

		//Cài đặt nút đóng cửa sổ
		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			SaveListFood();
			Application.Current.Shutdown();

		}
		//Cài đặt nút phóng to/ thu nhỏ cửa sổ
		private void MaximizeButton_Click(object sender, RoutedEventArgs e)
		{
			AdjustWindowSize();
		}

		//Cài đặt nút ẩn cửa sổ
		private void MinimizeButton_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}

		//Thay đổi kích thước cửa sổ
		//Nếu đang ở trạng thái phóng to thì thu nhỏ và ngược lại
		private void AdjustWindowSize()
		{
			var imgName = "";

			if (WindowState == WindowState.Maximized)
			{
				WindowState = WindowState.Normal;
				imgName = "Images/maximize.png";
			}
			else
			{
				WindowState = WindowState.Maximized;
				imgName = "Images/restoreDown.png";
			}

			//Lấy nguồn ảnh
			var img = new BitmapImage(new Uri(
						imgName,
						UriKind.Relative)
				);

			//Thiết lập ảnh chất lượng cao
			RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);

			//Thay đổi icon
			(MaxButton.Content as Image).Source = img;
		}



		//---------------------------------------- Các hàm sắp xếp --------------------------------------------//

		private bool Filter(object item)
		{
			bool result = true;
			var tripInfo = (Trip)item;
			if (FilterCondition.Type != "" && FilterCondition.Type != tripInfo.Stage)
			{
				result = false;
			}
			return result;
		}



		//---------------------------------------- Xử lý các nút bấm --------------------------------------------//

		private void ChangeClickedTypeButton_Click(object sender, RoutedEventArgs e)
		{
			clickedTypeButton.Foreground = Brushes.Gray;

			var button = (Button)sender;
			clickedTypeButton = button;
			button.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorScheme);

			//Hiển thị các món ăn thuộc loại thức ăn được chọn
			if (button == AllButton)
			{
				FilterCondition.Type = "";
			}
			else if (button == ProcessingButton)
			{
				FilterCondition.Type = "Đang đi";
			}
			else if (button == AccomplishedButton)
			{
				FilterCondition.Type = "Đã hoàn thành";
			}
			else
			{
				//Do nothing
			}

			//Cập nhật lại giao diện
			UpdateUIFromData();
		}

		private void ChangeClickedControlButton_Click(object sender, RoutedEventArgs e)
		{
			//Tắt màu của nút hiện tại
			var wrapPanel = (WrapPanel)clickedControlButton.Content;
			var collection = wrapPanel.Children;
			var block = (TextBlock)collection[0];
			var text = (TextBlock)collection[2];
			block.Background = Brushes.Transparent;
			text.Foreground = Brushes.Black;

			if (IsDetailTrip == true)
			{
				//Đóng giao diện màn hình chi tiết chuyến đi
				DetailTripGrid.Visibility = Visibility.Collapsed;
				IsDetailTrip = false;
			}
			else if (clickedControlButton == HomeButton)
			{
				//Đóng giao diện màn hình trang chủ
				TripListGrid.Visibility = Visibility.Collapsed;
			}
			else if (clickedControlButton == AddTripButton)
			{
				//Đóng giao diện màn hình thêm chuyến đi mới
				AddTripGrid.Visibility = Visibility.Collapsed;
			}

			//Hiển thị màu cho nút vừa được nhấn
			var button = (Button)sender;
			wrapPanel = (WrapPanel)button.Content;
			collection = wrapPanel.Children;
			block = (TextBlock)collection[0];
			text = (TextBlock)collection[2];
			block.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorScheme);
			text.Foreground = block.Background;

			//Cập nhật nút mới
			clickedControlButton = button;

			//Mở giao diện mới sau khi nhấn nút
			if (button == HomeButton)
			{
				TripListGrid.Visibility = Visibility.Visible;
				TypeBarDockPanel.Visibility = Visibility.Visible;
				ControlStackPanel.Visibility = Visibility.Visible;
			}
			else if (button == AddTripButton)
			{
				AddTripGrid.Visibility = Visibility.Visible;
				TypeBarDockPanel.Visibility = Visibility.Collapsed;
				ControlStackPanel.Visibility = Visibility.Collapsed;
				if (isEditMode == false)
				{
					trip = new Trip();
				}
				AddTripGrid.DataContext = trip;
			}

			//Cập nhật lại giao diện
			UpdateUIFromData();
			/*if (button != clickedControlButton)
			{
				//Đóng giao diện cũ trước khi nhấn nút
				if (!isEditMode && (clickedControlButton == HomeButton || clickedControlButton == FavoriteButton))
				{
					var listStack = windowsStack.Pop();
					var condition = new Condition { Favorite = FilterCondition.Favorite, Type = FilterCondition.Type };
					listStack.Insert(listStack.Count - 1, condition);
					windowsStack.Push(listStack);
				}
				else if (clickedControlButton == AddDishButton)
				{
					AddFood.DataContext = null;
					DefaultLevelComboxBoxItem.IsSelected = true;
					DefaultTypeComboxBoxItem.IsSelected = true;
					SaveOrDiscardBorder.Visibility = Visibility.Collapsed;
					EnterFoodNameTextBlock.Visibility = Visibility.Collapsed;
					ControlStackPanel.Visibility = Visibility.Visible;
					AddFoodAnhDishScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
				}
				else
				{
					//Do nothing
				}

				//Đóng giao diện Panel hiện tại
				ProcessPanelVisible(Visibility.Collapsed);

				//Nếu nhấn sang cửa sổ thứ 2 thì hiển thị nút Back
				if (windowsStack.Count == 1)
				{
					BackButton.Visibility = Visibility.Visible;
				}
				else
				{
					//Do nothing
				}

				List<object> list = new List<object>();

				//Mở giao diện mới sau khi nhấn nút
				if (button == HomeButton)
				{
					FilterCondition.Favorite = false;

					//Xóa hết lịch sử các cửa sổ khác khi nhấn nút Home
					while (windowsStack.Count > 0)
					{
						windowsStack.Pop();
					}
					//Thêm màn hình Favorite vào stack
					list.Add(PaginationBar);
					list.Add(TypeBar);
					list.Add(foodButtonItemsControl);
					list.Add(FilterCondition);

					//Nếu nhấn sang nút Home thì không còn trang nào phía trước
					BackButton.Visibility = Visibility.Collapsed;
				}
				else if (button == FavoriteButton)
				{
					FilterCondition.Favorite = true;

					//Thêm màn hình Favorite vào stack
					list.Add(PaginationBar);
					list.Add(TypeBar);
					list.Add(foodButtonItemsControl);
					list.Add(FilterCondition);
				}
				else if (button == AddDishButton)
				{
					AddFoodAnhDishScrollViewer.ScrollToHome();
					ControlStackPanel.Visibility = Visibility.Collapsed;
					AddFoodAnhDishScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
					SortFoodList();
					ListStep = new BindingList<Step>();
					if (isEditMode == false)
					{
						//AddFood.DataContext = null;
						//DefaultLevelComboxBoxItem.IsSelected = true;
						//DefaultTypeComboxBoxItem.IsSelected = true;
						//SaveOrDiscardBorder.Visibility = Visibility.Collapsed;
						//EnterFoodNameTextBlock.Visibility = Visibility.Collapsed;

						var index = GetMinID();
						newFood = new FoodInfomation() { ID = index, VideoLink = "", Steps = new BindingList<Step>() };
					}
					else
					{
						var food = ListFoodInfo[CurrentElementIndex];
						newFood = food;
						editFood = new FoodInfomation()
						{
							PrimaryImagePath = food.PrimaryImagePath,
							DateAdd = food.DateAdd,
							Discription = food.Discription,
							ID = food.ID,
							Ingredients = food.Ingredients,
							IsFavorite = food.IsFavorite,
							Level = food.Level,
							Name = food.Name,
							Steps = new BindingList<Step>(),
							Type = food.Type,
							VideoLink = food.VideoLink
						};
						//AddFood.DataContext = ListFoodInfo[CurrentElementIndex];
						//ImageStepItemsControl.ItemsSource = ListFoodInfo[CurrentElementIndex].Steps;
						foreach (var step in food.Steps)
						{
							editFood.Steps.Add(step);
						}
						for (int i = 0; i < LevelComboBox.Items.Count; i++)
						{
							var comboboxItem = LevelComboBox.Items[i] as ComboBoxItem;
							if (newFood.Level == (string)comboboxItem.Content)
							{
								LevelComboBox.SelectedIndex = i;
								break;
							}
						}

						for (int i = 0; i < LevelComboBox.Items.Count; i++)
						{
							var comboboxItem = TypeComboBox.Items[i] as ComboBoxItem;
							if (newFood.Type == (string)comboboxItem.Content)
							{
								TypeComboBox.SelectedIndex = i;
								break;
							}
						}
					}

					AddFood.DataContext = newFood;
					ImageStepItemsControl.ItemsSource = newFood.Steps;


					//ListStep = newFood.Steps;

					//Thêm màn hình Add vào stack
					list.Add(AddFood);

					//Thay đổi màu chữ cho các tiêu đề trong món ăn
					AddFood_TitleTextBlock.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorScheme);
					AddFood_LinkVideoTextBlock.Foreground = AddFood_TitleTextBlock.Foreground;
					AddFood_LevelTextBlock.Foreground = AddFood_TitleTextBlock.Foreground;
					AddFood_TypeTextBlock.Foreground = AddFood_TitleTextBlock.Foreground;
					AddFood_PhotosTextBlock.Foreground = AddFood_TitleTextBlock.Foreground;
					AddFood_DescriptionTextBlock.Foreground = AddFood_TitleTextBlock.Foreground;
					AddFood_IngredientsTextBlock.Foreground = AddFood_TitleTextBlock.Foreground;
					AddFood_DirectionsTextBlock.Foreground = AddFood_TitleTextBlock.Foreground;
				}
				else if (button == DishButton)
				{
					//Thêm màn hình Note vào stack
					list.Add(DishList);
				}
				else if (button == SettingButton)
				{
					var value = ConfigurationManager.AppSettings["ShowSplashScreen"];
					bool showSplashStatus = bool.Parse(value);
					if (showSplashStatus == true)
					{
						ShowSplashScreenCheckBox.IsChecked = true;
					}
					list.Add(SettingStackPanel);
				}
				else if (button == AboutButton)
				{
					list.Add(AboutStackPanel);
				}
				else
				{
					//Do nothing
				}

				//Cập nhật lại nút được chọn
				clickedControlButton = button;

				//Mở giao diện Panel vừa được chọn
				list.Add(clickedControlButton);
				windowsStack.Push(list);
				ProcessPanelVisible(Visibility.Visible);

				//Cập nhật lại giao diện
				UpdateUIFromData();
			}
			else
			{
				//Do nothing
			}*/
		}

		private void AddChargeButton_Click(object sender, RoutedEventArgs e)
		{
			var member = ((Button)sender).DataContext as Member;
			member.CostsList.Add(new Cost());
		}

		private void AddMemeberButton_Click(object sender, RoutedEventArgs e)
		{
			var selectedTrip = ((Button)sender).DataContext as Trip;
			selectedTrip.MembersList.Add(new Member());

		}

		private void DeleteChargeButton_Click(object sender, RoutedEventArgs e)
		{
			var member = ((Button)sender).DataContext as Member;
			if (member.CostsList.Count >= 1)
			{
				member.CostsList.Remove(member.CostsList[member.CostsList.Count - 1]);
			}
			else
			{
				MessageBox.Show($"{member.MemberName} không còn khoản chi nào để xoá!", "Warning!!", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

		private void DeleteMemeberButton_Click(object sender, RoutedEventArgs e)
		{
			var selectedTrip = ((Button)sender).DataContext as Trip;
			if (selectedTrip.MembersList.Count >= 1)
			{
				selectedTrip.MembersList.Remove(selectedTrip.MembersList[selectedTrip.MembersList.Count - 1]);
			}
			else
			{
				MessageBox.Show("Không còn thành viên nào để xoá!", "Warning!!", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

		private void DeleteImageButton_Click(object sender, RoutedEventArgs e)
		{
			trip.ImagesList.Remove(ImagesListView.SelectedItem as TripImage);
		}

		private void AddImageButton_Click(object sender, RoutedEventArgs e)
		{

			var fileDialog = new OpenFileDialog();
			fileDialog.Multiselect = true;
			fileDialog.Filter = "Image Files(*.JPG*;*.JPEG*;*.PNG*)|*.JPG;*.JPEG*;*.PNG*";
			fileDialog.Title = "Select Image";

			if (fileDialog.ShowDialog() == true)
			{
				var fileNames = fileDialog.FileNames;
				foreach (var filename in fileNames)
				{
					trip.ImagesList.Add(new TripImage(filename));
				}
			}
		}

		private void SaveTripButton_Click(object sender, RoutedEventArgs e)
		{
			if (isEditMode == false)
			{
				string appFolder = GetAppDomain();
				for (int i = 0; i < trip.ImagesList.Count; i++)
				{
					var imageExtension = System.IO.Path.GetExtension(trip.ImagesList[i].ImagePath);
					var newImageName = $"Images/{trip.TripID}_{i}.{imageExtension}";
					var newPath = appFolder + newImageName;
					File.Copy(trip.ImagesList[i].ImagePath, newPath, true);
					trip.ImagesList[i].ImagePath = newImageName;
				}
				trip.PrimaryImagePath = trip.ImagesList[0].ImagePath;
				TripInfoList.Add(trip);
			}
			else
			{
				string appFolder = GetAppDomain();
				for (int i = 0; i < trip.ImagesList.Count; i++)
				{
					TripImage currentImage = trip.ImagesList[i];
					var imageExtension = System.IO.Path.GetExtension(currentImage.ImagePath);
					var newImageName = $"Images/{trip.TripID}_{i}{imageExtension}";
					var newPath = appFolder + newImageName;
					if (System.IO.Path.IsPathRooted(currentImage.ImagePath))
					{
						File.Copy(currentImage.ImagePath, newPath, true);
						trip.ImagesList[i].ImagePath = newImageName;
					}
					else
					{
						if (currentImage.ImagePath != TripInfoList[selectedTripIndex].ImagesList[i].ImagePath)
						{
							File.Delete(appFolder + TripInfoList[selectedTripIndex].ImagesList[i].ImagePath);
							File.Move(appFolder + currentImage.ImagePath, newPath);
							currentImage.ImagePath = newImageName;
						}
					}
				}
				if (trip.ImagesList.Count > 0)
				{
					trip.PrimaryImagePath = trip.ImagesList[0].ImagePath;
				}
				else
				{
					trip.PrimaryImagePath = "";
				}
				TripInfoList[selectedTripIndex] = trip;
			}

			//Đóng giao diện thêm/chỉnh sửa và mở giao diện trang chủ
			CancelTripButton_Click(null, null);
		}

		private void CancelTripButton_Click(object sender, RoutedEventArgs e)
		{
			//Đóng màn hình thêm chuyến đi
			AddTripGrid.Visibility = Visibility.Collapsed;
			//Tắt màu của nút Add
			var wrapPanel = (WrapPanel)AddTripButton.Content;
			var collection = wrapPanel.Children;
			var block = (TextBlock)collection[0];
			var text = (TextBlock)collection[2];
			block.Background = Brushes.Transparent;
			text.Foreground = Brushes.Black;

			if (isEditMode == true)
			{
				//Quay ve man hinh chi tiet
				DetailTripGrid.DataContext = TripInfoList[selectedTripIndex];
				DetailTripGrid.Visibility = Visibility.Visible;
				ControlStackPanel.Visibility = Visibility.Visible;

				//Tắt chế độ chỉnh sửa
				isEditMode = false;
				IsDetailTrip = true;
			}
			else
			{
				//Quay về màn hình Home
				clickedControlButton = HomeButton;
				TripListGrid.Visibility = Visibility.Visible;
				TypeBarDockPanel.Visibility = Visibility.Visible;
				ControlStackPanel.Visibility = Visibility.Visible;
			}

			//Hiển thị màu cho nút Home
			wrapPanel = (WrapPanel)HomeButton.Content;
			collection = wrapPanel.Children;
			block = (TextBlock)collection[0];
			text = (TextBlock)collection[2];
			block.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorScheme);
			text.Foreground = block.Background;

			//Cập nhật lại giao diện
			UpdateUIFromData();
		}

		private void EditTripButton_Click(object sender, RoutedEventArgs e)
		{
			isEditMode = true;
			trip = new Trip(TripInfoList[selectedTripIndex]);
			//Bật màn hình chỉnh sửa
			ChangeClickedControlButton_Click(AddTripButton, null);
		}

		private void MenuButton_Click(object sender, RoutedEventArgs e)
		{
			if (isMinimizeMenu == false)
			{
				col0.Width = new GridLength(46);
				//TripPerPage = 15;
				//UpdateFoodStatus();
				isMinimizeMenu = true;
			}
			else
			{
				col0.Width = new GridLength(250);
				//TripPerPage = 12;
				//UpdateFoodStatus();
				isMinimizeMenu = false;
			}
		}

		private void DeleteTripButton_Click(object sender, RoutedEventArgs e)
		{
			TripInfoList.Remove(TripInfoList[selectedTripIndex]);

			//
			DetailTripGrid.Visibility = Visibility.Collapsed;
			//Tắt màu của nút Add
			var wrapPanel = (WrapPanel)AddTripButton.Content;
			var collection = wrapPanel.Children;
			var block = (TextBlock)collection[0];
			var text = (TextBlock)collection[2];
			block.Background = Brushes.Transparent;
			text.Foreground = Brushes.Black;

			//Quay về màn hình Home
			clickedControlButton = HomeButton;
			TripListGrid.Visibility = Visibility.Visible;
			TypeBarDockPanel.Visibility = Visibility.Visible;
			ControlStackPanel.Visibility = Visibility.Visible;
			//Hiển thị màu cho nút Home
			wrapPanel = (WrapPanel)HomeButton.Content;
			collection = wrapPanel.Children;
			block = (TextBlock)collection[0];
			text = (TextBlock)collection[2];
			block.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorScheme);
			text.Foreground = block.Background;

			//Tắt chế độ chỉnh sửa
			isEditMode = false;

			//Cập nhật lại giao diện
			UpdateUIFromData();
		}

		private void Trip_Click(object sender, RoutedEventArgs e)
		{
			//Đóng giao diện màn hình danh sách các chuyến đi
			TripListGrid.Visibility = Visibility.Collapsed;
			//Đóng giao diện thanh chọn loại chuyến đi
			TypeBarDockPanel.Visibility = Visibility.Collapsed;

			//Lấy chỉ số của hình ảnh món ăn được nhấn
			selectedTripIndex = GetElementIndexInArray((Button)sender);
			DetailTripGrid.DataContext = TripInfoList[selectedTripIndex];
			trip = new Trip(TripInfoList[selectedTripIndex]);

			//Mở giao diện màn hình chi tiết chuyến đi
			DetailTripGrid.Visibility = Visibility.Visible;

			//Bật chế độ đang ở màn hình chi tiết
			IsDetailTrip = true;
			//Đóng giao diện Panel hiện tại
			/*ProcessPanelVisible(Visibility.Collapsed);

			//Lấy chỉ số của hình ảnh món ăn được nhấn
			if (sender != null)
			{
				CurrentElementIndex = GetElementIndexInArray((Button)sender);
			}
			else
			{
				if (e == null)
				{
					CurrentElementIndex = (int)windowsStack.Peek()[0];
				}
				else
				{
					if (isEditMode == false)
					{
						CurrentElementIndex = ListFoodInfo.Count - 1;
					}
					else
					{
						//Do nothing
					}
				}
			}

			//Binding dữ liệu để hiển thị chi tiết món ăn
			FoodDetailGrid.DataContext = ListFoodInfo[CurrentElementIndex];

			//Thay đổi màu chữ cho tiêu đề thông tin chi tiết món ăn
			FoodInfo_NameTextBlock.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorScheme);
			FoodInfo_IngredientsTextBlock.Foreground = FoodInfo_NameTextBlock.Foreground;
			FoodInfo_DirectionsTextBlock.Foreground = FoodInfo_NameTextBlock.Foreground;
			FoodInfo_VideoTextBlock.Foreground = FoodInfo_NameTextBlock.Foreground;

			UpdatePaginationForDetailFoodUI();

			if (windowsStack.Count == 1)
			{
				var listStack = windowsStack.Pop();
				var condition = new Condition { Favorite = FilterCondition.Favorite, Type = FilterCondition.Type };
				listStack.Insert(listStack.Count - 1, condition);
				windowsStack.Push(listStack);
			}
			else
			{
				//Do nothing
			}

			if (sender != null || e != null)
			{
				//Mở giao diện chi tiết món ăn
				windowsStack.Push(new List<object> { CurrentElementIndex, FoodDetailScrollViewer, clickedControlButton });
				ProcessPanelVisible(Visibility.Visible);

				//Hiển thị nút quay lại
				if (BackButton.Visibility == Visibility.Collapsed)
				{
					BackButton.Visibility = Visibility.Visible;
				}
				else
				{
					//Do nothing
				}
			}
			else
			{
				//Do nothing
			}*/
		}





        //---------------------------------------- Các hàm xử lý khác --------------------------------------------//

        private void TripListAppearAnimation()
		{
			ThicknessAnimation animation = new ThicknessAnimation();
			animation.AccelerationRatio = 0.9;
			animation.From = new Thickness(15, 60, 0, 0);
			animation.To = new Thickness(15, 6, 0, 0);
			animation.Duration = TimeSpan.FromSeconds(0.5);
			TripListGrid.BeginAnimation(Grid.MarginProperty, animation);
		}



        private void ChargePie_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			ChargePie.Series = new SeriesCollection();
			((DefaultTooltip)ChargePie.DataTooltip).SelectionMode = TooltipSelectionMode.OnlySender;
			foreach (var member in trip.MembersList)
			{
				ChargePie.Series.Add(
						new PieSeries()
						{
							Values = new ChartValues<decimal> { member.CostsList.Sum(value => value.Charge) },
							Title = member.MemberName,
						}
					); ;
			}
		}

		private void ChargeChart_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			ChargeChart.Series = new SeriesCollection();
			((DefaultTooltip)ChargeChart.DataTooltip).SelectionMode = TooltipSelectionMode.OnlySender;
			ChargeChart.AxisY = new AxesCollection();
			foreach (var member in trip.MembersList)
			{
				foreach(var cost in member.CostsList)
                {
					ChargeChart.Series.Add(new ColumnSeries()
					{
						Values = new ChartValues<decimal> { cost.Charge },
						Title = cost.PaymentName
					}); ;
                }
			}

		}
		private void memberSummaryTextBlock_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var index = AverageChargeTextBlock.Text.IndexOf(" ");
			if (index != -1)
			{
				double averageCharge = double.Parse(AverageChargeTextBlock.Text.Substring(0, index));
				var member = ((TextBlock)sender).DataContext as Member;
				averageCharge *= ConvertUnitStringIntoInt(AverageChargeTextBlock.Text.Substring(index + 1));
				double res = member.Deposits - averageCharge;
				((TextBlock)sender).Text = ConvertMoneyUnit(res);
				if (res < 0)
				{
					((TextBlock)sender).Foreground = Brushes.Red;
				}
				else
				{
					((TextBlock)sender).Foreground = Brushes.ForestGreen;
				}
			}
			
		}

		private void AverageChargeTextBlock_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			int sum = 0;
			int count = TripInfoList[selectedTripIndex].MembersList.Count;
			foreach (var member in TripInfoList[selectedTripIndex].MembersList)
			{
				foreach (var cost in member.CostsList)
				{
					sum += cost.Charge;

				}
			}
			var res = (double)sum / count;
			((TextBlock)sender).Text = ConvertMoneyUnit(res);
		}

		private void SumChargeTextBlock_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			int sum = 0;
			foreach (var member in TripInfoList[selectedTripIndex].MembersList)
			{
				foreach (var cost in member.CostsList)
				{
					sum += cost.Charge;

				}
			}
			((TextBlock)sender).Text = ConvertMoneyUnit(sum);
		}

		private void memberSummaryTextBlock_Loaded(object sender, RoutedEventArgs e)
		{
			memberSummaryTextBlock_IsVisibleChanged(sender, new DependencyPropertyChangedEventArgs());
		}

		private void AverageChargeTextBlock_Loaded(object sender, RoutedEventArgs e)
		{
			AverageChargeTextBlock_IsVisibleChanged(sender, new DependencyPropertyChangedEventArgs());
		}

		private void SumChargeTextBlock_Loaded(object sender, RoutedEventArgs e)
		{
			SumChargeTextBlock_IsVisibleChanged(sender, new DependencyPropertyChangedEventArgs());
		}

		/*tim kiem*/
		private string ConvertToUnSign(string input)
		{
			if (input != null)
			{
				input = input.Trim();
				for (int i = 0x20; i < 0x30; i++)
				{
					input = input.Replace(((char)i).ToString(), " ");
				}
				Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
				string str = input.Normalize(NormalizationForm.FormD);
				string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
				while (str2.IndexOf("?") >= 0)
				{
					str2 = str2.Remove(str2.IndexOf("?"), 1);
				}
				return str2;
			}
			else
			{
				var res = "";
				return res;
			}
		}

		private void searchTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (e.Text != "\u001b")  //khác escapes
			{
				searchComboBox.IsDropDownOpen = true;
			}
			if (SearchFilter.IsChecked == false)
			{
				if (!string.IsNullOrEmpty(searchTextBox.Text))
				{
					string fullText = ConvertToUnSign(searchTextBox.Text.Insert(searchTextBox.CaretIndex, (e.Text)));
					searchComboBox.ItemsSource = TripInfoList.Where(s => ConvertToUnSign(s.Location).IndexOf(fullText, StringComparison.InvariantCultureIgnoreCase) != -1).ToList();
					if (searchComboBox.Items.Count == 0)
					{
						SearchNotificationComboBox.IsDropDownOpen = true;
						searchComboBox.IsDropDownOpen = false;
					}
				}
				else if (!string.IsNullOrEmpty(e.Text))
				{
					searchComboBox.ItemsSource = TripInfoList.Where(s => ConvertToUnSign(s.Location).IndexOf(ConvertToUnSign(e.Text), StringComparison.InvariantCultureIgnoreCase) != -1).ToList();
				}
				else
				{
					searchComboBox.ItemsSource = TripInfoList;
				}
			}
            else
            {
				if (!string.IsNullOrEmpty(searchTextBox.Text))
				{
					string fullText = ConvertToUnSign(searchTextBox.Text.Insert(searchTextBox.CaretIndex, (e.Text)));
					searchComboBox.ItemsSource = TripInfoList.Where(s => s.MembersList.Any(p => ConvertToUnSign(p.MemberName).IndexOf(fullText, StringComparison.InvariantCultureIgnoreCase) != -1)).ToList();
					if (searchComboBox.Items.Count == 0)
					{
						SearchNotificationComboBox.IsDropDownOpen = true;
						searchComboBox.IsDropDownOpen = false;
					}
				}
				else if (!string.IsNullOrEmpty(e.Text))
				{
					searchComboBox.ItemsSource = TripInfoList.Where(s => s.MembersList.Any(p => ConvertToUnSign(p.MemberName).IndexOf(ConvertToUnSign(e.Text), StringComparison.InvariantCultureIgnoreCase) != -1)).ToList();
				}
				else
				{
					searchComboBox.ItemsSource = TripInfoList;
				}
			}
		}

		private void PreviewKeyUp_EnhanceTextBoxSearch(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Back || e.Key == Key.Delete)
			{


				searchComboBox.IsDropDownOpen = true;

				if (SearchFilter.IsChecked == false)
				{
					if (!string.IsNullOrEmpty(searchTextBox.Text))
					{
						searchComboBox.ItemsSource = TripInfoList.Where(s => ConvertToUnSign(s.Location).IndexOf(ConvertToUnSign(searchTextBox.Text), StringComparison.InvariantCultureIgnoreCase) != -1).ToList();
						if (searchComboBox.Items.Count == 0)
						{
							SearchNotificationComboBox.IsDropDownOpen = true;
							searchComboBox.IsDropDownOpen = false;
						}

					}
					else
					{
						searchComboBox.ItemsSource = TripInfoList;
					}
				}
                else
                {
					if (!string.IsNullOrEmpty(searchTextBox.Text))
					{
						searchComboBox.ItemsSource = TripInfoList.Where(s => s.MembersList.Any(p => ConvertToUnSign(p.MemberName).IndexOf(ConvertToUnSign(searchTextBox.Text), StringComparison.InvariantCultureIgnoreCase) != -1)).ToList();
						if (searchComboBox.Items.Count == 0)
						{
							SearchNotificationComboBox.IsDropDownOpen = true;
							searchComboBox.IsDropDownOpen = false;
						}

					}
					else
					{
						searchComboBox.ItemsSource = TripInfoList;
					}
				}
			}
		}
		private void searchTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Down)
			{
				searchComboBox.Focus();
				searchComboBox.SelectedIndex = 0;
				searchComboBox.IsDropDownOpen = true;
			}
			if (e.Key == Key.Escape)
			{
				searchComboBox.IsDropDownOpen = false;

			}
		}

		private void Pasting_EnhanceTextSearch(object sender, DataObjectPastingEventArgs e)
		{
			searchComboBox.IsDropDownOpen = true;

			string pastedText = (string)e.DataObject.GetData(typeof(string));
			string fullText = searchTextBox.Text.Insert(searchTextBox.CaretIndex, (pastedText));

			if (!string.IsNullOrEmpty(fullText))
			{
				searchComboBox.ItemsSource = TripInfoList.Where(s => ConvertToUnSign(s.Location).IndexOf(ConvertToUnSign(fullText), StringComparison.InvariantCultureIgnoreCase) != -1).ToList();
				if (searchComboBox.Items.Count == 0)
				{
					SearchNotificationComboBox.IsDropDownOpen = true;
					searchComboBox.IsDropDownOpen = false;
				}
			}
			else
			{
				searchComboBox.ItemsSource = TripInfoList;
			}
		}

		private string ConvertMoneyUnit(double value)
		{
			var unit = "";
			if (Math.Abs(value) < 1000)
			{
				unit = " Đồng";
			}
			else if (Math.Abs(value) < 1000000)
			{
				value /= 1000;
				unit = " Nghìn";
			}
			else if (Math.Abs(value) < 1000000000)
			{
				value /= 1000000;
				unit = " Triệu";
			}
			value = Math.Abs(Math.Round(value, 2));
			var res = value.ToString() + unit;
			return res;
		}

		private int ConvertUnitStringIntoInt(string unit)
		{
			var res = 1;
			if (unit == "Nghìn")
			{
				res = 1000;
			}
			else if (unit == "Triệu")
			{
				res = 1000000;
			}
			return res;
		}
	}
}
