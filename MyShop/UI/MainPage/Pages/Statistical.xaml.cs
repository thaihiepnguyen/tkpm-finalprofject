using MyShop.BUS;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using DocumentFormat.OpenXml.Bibliography;

namespace MyShop.UI.MainPage.Pages
{
    /// <summary>
    /// Interaction logic for Statistical.xaml
    /// </summary>
    public partial class Statistical : Page
    {
        private ReportBUS _reportBUS;
        private CartesianChart _chart;
        private int _currentYear;
        private Frame _pageNavigation;
        private ProgressBar _loadingProgressBar;
        public Statistical(Frame pageNavigation, ProgressBar loadingProgressBar)
        {
            _reportBUS = new ReportBUS();
            _pageNavigation = pageNavigation;
            _loadingProgressBar = loadingProgressBar;
            InitializeComponent();

            _chart = chart;

            _chart.AxisY.Add(new Axis
            {
                Foreground = Brushes.Black,
                Title = "Doanh thu/ lợi nhuận",
                MinValue = 0
            });
            Title.Text = "Đang hiển thị chế độ xem theo năm";
        }

        private async void displayYearMode()
        {
            _loadingProgressBar.IsIndeterminate = true;
            var pricesByYear = await _reportBUS.groupPriceTotalByYear();
            var profitsByYear = await _reportBUS.groupProfitTotalByYear();
            _loadingProgressBar.IsIndeterminate = false;
            var valuesLineChart = new ChartValues<double>();
            var valuesColChart = new ChartValues<double>();

            foreach (var item in pricesByYear)
            {
                valuesColChart.Add((double)item);
            }

            foreach (var item in profitsByYear)
            {
                valuesLineChart.Add((double)item);
            }

            _chart.Series = new SeriesCollection();
            _chart.AxisX = new AxesCollection();

            _chart.Series.Add(new LineSeries()
            {
                Stroke = Brushes.DeepSkyBlue,
                Title = "Lợi nhuận theo năm",
                Values = valuesLineChart
            });

            _chart.Series.Add(new ColumnSeries()
            {
                Fill = Brushes.Chocolate,
                Title = "Doanh thu theo năm",
                Values = valuesColChart
            });

            chart.AxisX.Add(
                new Axis()
                {
                    Foreground = Brushes.Black,
                    Title = "Year",
                    Labels = new string[] {
                        "Năm 2021",
                        "Năm 2022",
                        "Năm 2023",
                    }
                });
            Title.Text = "Đang hiển thị chế độ xem theo năm";
        }

        private async void displayMonthMode(int year)
        {
            _loadingProgressBar.IsIndeterminate = true;
            var pricesByMonth = await _reportBUS.groupPriceTotalByMonth(year);
            var profitsByMonth = await _reportBUS.groupProfitTotalByMonth(year);
            _loadingProgressBar.IsIndeterminate = false;
            var valuesLineChart = new ChartValues<double>();
            var valuesColChart = new ChartValues<double>();

            foreach (var item in pricesByMonth)
            {
                valuesColChart.Add((double)item);
            }

            foreach (var item in profitsByMonth)
            {
                valuesLineChart.Add((double)item);
            }

            _chart.Series = new SeriesCollection();
            _chart.AxisX = new AxesCollection();

            _chart.Series.Add(new LineSeries()
            {
                Stroke = Brushes.DeepSkyBlue,
                Title = "Lợi nhuận theo tháng",
                Values = valuesLineChart
            });

            _chart.Series.Add(new ColumnSeries()
            {
                Fill = Brushes.Chocolate,
                Title = "Doanh thu theo tháng",
                Values = valuesColChart
            });

            chart.AxisX.Add(
                new Axis()
                {
                    Foreground = Brushes.Black,
                    Title = "Month",
                    Labels = new string[] {
                        "Tháng 1",
                        "Tháng 2",
                        "Tháng 3",
                        "Tháng 4",
                        "Tháng 5",
                        "Tháng 6",
                        "Tháng 7",
                        "Tháng 8",
                        "Tháng 9",
                        "Tháng 10",
                        "Tháng 11",
                        "Tháng 12",
                    }
                });
            Title.Text = "Đang hiển thị chế độ xem theo tháng";
            MonthCombobox.IsEnabled = true;
            MonthCombobox.SelectedIndex = 0;
        }

        private async void displayWeekMode(int month, int year)
        {
            _loadingProgressBar.IsIndeterminate = true;
            var pricesByWeek = await _reportBUS.groupPriceTotalByWeek(month, year);
            var profitsByWeek = await _reportBUS.groupProfitTotalByWeek(month, year);
            _loadingProgressBar.IsIndeterminate = false;
            var valuesLineChart = new ChartValues<double>();
            var valuesColChart = new ChartValues<double>();

            foreach (var item in pricesByWeek)
            {
                valuesColChart.Add((double)item);
            }

            foreach (var item in profitsByWeek)
            {
                valuesLineChart.Add((double)item);
            }

            _chart.Series = new SeriesCollection();
            _chart.AxisX = new AxesCollection();

            _chart.Series.Add(new LineSeries()
            {
                Stroke = Brushes.DeepSkyBlue,
                Title = "Lợi nhuận theo tuần",
                Values = valuesLineChart
            });

            _chart.Series.Add(new ColumnSeries()
            {
                Fill = Brushes.Chocolate,
                Title = "Doanh thu theo tuần",
                Values = valuesColChart
            });

            chart.AxisX.Add(
                new Axis()
                {
                    Foreground = Brushes.Black,
                    Title = "Week",
                    Labels = new string[] {
                        "Tuần 1",
                        "Tuần 2",
                        "Tuần 3",
                        "Tuần 4",
                        "Tuần 5",
                    }
                });
            Title.Text = "Đang hiển thị chế độ xem theo tuần";
        }

        private async void displayDateMode(DateTime startDate, DateTime endDate)
        {
            _loadingProgressBar.IsIndeterminate = true;
            var pricesByDate = await _reportBUS.groupPriceTotalByDate(startDate, endDate);
            var profitsByDate = await _reportBUS.groupProfitTotalByDate(startDate, endDate);
            _loadingProgressBar.IsIndeterminate = false;
            var valuesLineChart = new ChartValues<double>();
            var valuesColChart = new ChartValues<double>();

            foreach (var item in pricesByDate)
            {
                valuesColChart.Add((double)item);
            }

            foreach (var item in profitsByDate)
            {
                valuesLineChart.Add((double)item);
            }

            _chart.Series = new SeriesCollection();
            _chart.AxisX = new AxesCollection();

            _chart.Series.Add(new LineSeries()
            {
                Stroke = Brushes.DeepSkyBlue,
                Title = "Lợi nhuận theo ngày",
                Values = valuesLineChart
            });

            _chart.Series.Add(new ColumnSeries()
            {
                Fill = Brushes.Chocolate,
                Title = "Doanh thu theo ngày",
                Values = valuesColChart
            });

            chart.AxisX.Add(
                new Axis()
                {
                    Foreground = Brushes.Black,
                    Title = "Date",
                    Labels = _reportBUS.EachDayConverter(startDate, endDate)
                });
            Title.Text = "Đang hiển thị chế độ xem theo ngày";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            displayYearMode();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            var startDate = StartDate.SelectedDate;
            var endDate = EndDate.SelectedDate;

            displayDateMode((DateTime)startDate, (DateTime)endDate);
        }

        private void NextProductReport_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.Navigate(new StatisticalProduct(_pageNavigation, _loadingProgressBar));
        }

        private void MonthCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = MonthCombobox.SelectedIndex;
            if (i == -1 || i == 0)
            {
                return;
            } else
            {
                displayWeekMode(i, _currentYear);
            }
        }

        private void YearCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = YearCombobox.SelectedIndex; 
            if (i == -1)
            {
                return;
            }
            else
            {
                if (i == 1)
                {
                    displayMonthMode(2021);
                    _currentYear = 2021;
                }
                if (i == 2)
                {
                    displayMonthMode(2022);
                    _currentYear = 2022;
                }
                if (i == 3)
                {
                    displayMonthMode(2023);
                    _currentYear = 2023;
                }
            }
        }
    }
}
