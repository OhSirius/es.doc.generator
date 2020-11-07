using BG.DAL.Models;
using BG.Extensions;
using BG.Infrastructure.Common.Processes;
using Gat.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
using System.Configuration;
using BG.Infrastructure.Process.Process;
using BG.Infrastructure.UICommon.Services.Impl;
using BG.Application.DocumentsGenerator.Services;
using BG.Application.DocumentsGenerator.ServiceReference;

namespace BG.Application.DocumentsGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SynchronizationContext context = null;
        CancellationTokenSource tokenSource = null;

        public MainWindow()
        {
            InitializeComponent();

            var source = ConfigurationManager.AppSettings["source"];
            var template = ConfigurationManager.AppSettings["template"];
            var result = ConfigurationManager.AppSettings["result"];

            //var viewModel = new ViewModel(@"C:\Users\aspav\source\repos\BG\Applications\src\BG.Application.RunConsole\Организации.xlsx", @"C:\Users\aspav\source\repos\BG\Applications\src\BG.Application.RunConsole\Шаблон.docx", @"D:\Out");//
            var viewModel = new ViewModel(source, template, result);
            //var viewModel = new ViewModel();
            context = SynchronizationContext.Current;
            tokenSource = new CancellationTokenSource();
            viewModel.Progress = 0;
            this.DataContext = viewModel;
        }

        ViewModel ViewModel => this.DataContext as ViewModel;

        /// <summary>
        /// https://opendialog.codeplex.com/documentation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SourceButton_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.SourceFileName = OpenDialog(s => s.AddFileFilterExtension(".xlsx"));
        }

        private void TemplateButton_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.TemplateFileName = OpenDialog(s => s.AddFileFilterExtension(".docx"));
        }

        private void ResultButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ResultFolderName = OpenDialog(s => s.IsDirectoryChooser = true);
        }

        string OpenDialog(Action<OpenDialogViewModel> settings)
        {
            var openDialog = new OpenDialogView();
            var vm = (OpenDialogViewModel)openDialog.DataContext;
            vm.Owner = this;
            vm.StartupLocation = WindowStartupLocation.CenterOwner;

            // Setting date format by using predefined date format
            vm.DateFormat = OpenDialogViewModel.ISO8601_DateFormat;
            settings?.Invoke(vm);

            // Customize UI texts
            vm.CancelText = "Отмена";
            vm.Caption = "Выбор";
            vm.DateFormat = "yyyy-MM-dd HH:mm:ss";
            vm.DateText = "Дата";
            vm.FileFilterText = "Тип";
            vm.FileNameText = "Файл";
            vm.NameText = "Название";
            vm.SaveText = "Сохранить";
            vm.SizeText = "Размер";
            vm.TypeText = "Тип";
            vm.OpenText = "Выбрать";
            vm.SelectedFileFilterExtension = vm.FileFilterExtensions.Where(f=>!string.IsNullOrEmpty(f)).FirstOrDefault();

            bool? result = vm.Show();
            if (result == true)
            {
                // Get selected file path
                return vm.SelectedFilePath;
            }
            else
            {
                return string.Empty;
            }
        }

        private async void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            //var ss = sender as Button;

            //ViewModel.IsProcessed = false;
            //return;

            try
            {
                if(string.IsNullOrWhiteSpace(ViewModel.TemplateFileName) || string.IsNullOrWhiteSpace(ViewModel.SourceFileName) || string.IsNullOrWhiteSpace(ViewModel.ResultFolderName))
                {
                    MessageBox.Show("Не задан путь к источнику данных, шаблону или папке вывода результатов", "Генерация", MessageBoxButton.OK);
                    return;
                }

                //ViewModel.IsProcessed = false;
                var process = GetProcess();
                context.Send(d => ViewModel.IsProcessed = false, null);
                process.Processing += Process_Processing;
                await process.Execute(tokenSource.Token).ConfigureAwait(false);
                context.Send(d => ViewModel.IsProcessed = true, null);
                //Task.Run(() => {
                //    try
                //    {
                //        context.Send(d => ViewModel.IsProcessed = false, null);
                //        process.Processing += Process_Processing;
                //        process.Execute(tokenSource.Token);
                //    }
                //    finally
                //    {
                //        context.Send(d => ViewModel.IsProcessed = true, null);
                //    }
                //}, tokenSource.Token);
            }
            catch (AggregateException ag)
            {
                if (!ag.InnerExceptions.IsNullOrEmpty())
                {
                    ag.InnerExceptions.ForEach(ex => ViewModel.Display = ex.ToString());
                }
            }
            catch(Exception ex)
            {
                ViewModel.Display = ex.ToString();
            }
            finally
            {
                //ViewModel.IsProcessed = true;
            }
        }

        private void Process_Processing(ProcessEventsArgs obj)
        {
            //lock (this)
            context.Send(d =>
            {
                ViewModel.Progress = obj.Percent > 0 ? obj.Percent : ViewModel.Progress;
                ViewModel.Display += $"\r\n{obj.Display}";
                tbResult.Focus();
                tbResult.CaretIndex = tbResult.Text.Length;
                tbResult.ScrollToEnd();
            }, null);
        }

        IProcess GetProcess()
        {
            IProcess process = Domain.DocumentsGenerator.Bootstrap.Configuration.Configure<Person>(ViewModel.SourceFileName, ViewModel.TemplateFileName, ViewModel.ResultFolderName);
            return process;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            tokenSource.Cancel();
            tokenSource = new CancellationTokenSource();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!ViewModel.IsProcessed)
            {
                if (MessageBox.Show("Сервис не завершил выполнение! Вы уверены, что хотите выйти?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    tokenSource.Cancel();
                else
                    e.Cancel = true;
            }

        }
    }

    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    public class ViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {
        }

        public ViewModel(string source, string template, string result)
        {
            SourceFileName = source;
            TemplateFileName = template;
            ResultFolderName = result;
        }

        bool _isProcessed = true;
        public bool IsProcessed
        {
            set
            {
                if (_isProcessed != value)
                {
                    _isProcessed = value;
                    NotifyPropertyChanged();
                }
            }
            get
            {
                return _isProcessed;
            }
        }

        string _sourceFileName;
        public string SourceFileName
        {
            set
            {
                if (_sourceFileName != value)
                {
                    _sourceFileName = value;
                    NotifyPropertyChanged();
                }
            }
            get
            {
                return _sourceFileName;
            }
        }

        string _templateFileName;
        public string TemplateFileName
        {
            set
            {
                if (_templateFileName != value)
                {
                    _templateFileName = value;
                    NotifyPropertyChanged();
                }
            }
            get
            {
                return _templateFileName;
            }
        }

        string _resultFolderName;
        public string ResultFolderName
        {
            set
            {
                if (_resultFolderName != value)
                {
                    _resultFolderName = value;
                    NotifyPropertyChanged();
                }
            }
            get
            {
                return _resultFolderName;
            }
        }

        string _display = "";
        public string Display
        {
            set
            {
                if (_display != value)
                {
                    _display = value;
                    NotifyPropertyChanged();
                }
            }
            get
            {
                return _display;
            }
        }

        int _progress;
        public int Progress
        {
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    NotifyPropertyChanged();
                }
            }
            get
            {
                return _progress;
            }
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
