using FiguresLib;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Task_3_WPF
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TypeViewModel> _availableTypes;
        private TypeViewModel _selectedType;
        private ObservableCollection<MethodViewModel> _methods;
        private MethodViewModel _selectedMethod;
        private string _parameters;
        private string _assemblyPath;

        public ObservableCollection<TypeViewModel> AvailableTypes
        {
            get { return _availableTypes; }
            set
            {
                _availableTypes = value;
                OnPropertyChanged();
            }
        }

        public TypeViewModel SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
                LoadMethods();
            }
        }

        public ObservableCollection<MethodViewModel> Methods
        {
            get { return _methods; }
            set
            {
                _methods = value;
                OnPropertyChanged();
            }
        }

        public MethodViewModel SelectedMethod
        {
            get { return _selectedMethod; }
            set
            {
                _selectedMethod = value;
                OnPropertyChanged();
            }
        }

        public string Parameters
        {
            get { return _parameters; }
            set
            {
                _parameters = value;
                OnPropertyChanged();
            }
        }

        public ICommand BrowseCommand { get; private set; }
        public ICommand ExecuteCommand { get; private set; }

        public MainViewModel()
        {
            BrowseCommand = new RelayCommand(Browse);
            ExecuteCommand = new RelayCommand(Execute);
        }

        private void Browse(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Assemblies (*.dll)|*.dll|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                _assemblyPath = openFileDialog.FileName;
                LoadTypes();
            }
        }

        private void LoadTypes()
        {
            AvailableTypes = new ObservableCollection<TypeViewModel>();
            Methods = null;

            try
            {
                Assembly assembly = Assembly.LoadFrom(_assemblyPath);
                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(GeometricFigure).IsAssignableFrom(type))
                    {
                        ConstructorInfo defaultConstructor = type.GetConstructor(Type.EmptyTypes);
                        if (defaultConstructor != null)
                        {
                            AvailableTypes.Add(new TypeViewModel(type, type.Name));
                        }
                        else
                        {
                            AvailableTypes.Add(new TypeViewModel(type, type.Name + " (No default constructor)"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading types: " + ex.Message);
            }
        }



        private void LoadMethods()
        {
            Methods = new ObservableCollection<MethodViewModel>();

            if (SelectedType != null)
            {
                foreach (MethodInfo methodInfo in SelectedType.Type.GetMethods())
                {
                    Methods.Add(new MethodViewModel(methodInfo));
                }
            }
        }

        private void Execute(object parameter)
        {
            if (SelectedMethod == null)
            {
                MessageBox.Show("Please select a method.");
                return;
            }

            try
            {
                object instance = Activator.CreateInstance(SelectedType.Type);
                object[] parameters = ParseParameters();

                // Проверяем, является ли выбранный метод методом "get"
                if (SelectedMethod.MethodInfo.Name.StartsWith("get_"))
                {
                    // Вызываем метод "get"
                    object result = SelectedMethod.MethodInfo.Invoke(instance, parameters);
                    // Отображаем результат в интерфейсе
                    Result = result.ToString();
                }
                // Проверяем, является ли выбранный метод методом "set"
                else if (SelectedMethod.MethodInfo.Name.StartsWith("set_"))
                {
                    // Вызываем метод "set"
                    SelectedMethod.MethodInfo.Invoke(instance, parameters);
                    // Обновляем интерфейс для отображения сообщения об успешной установке значения
                    Result = "Value has been set successfully";
                }
                else
                {
                    // Если выбранный метод не является ни методом "get", ни методом "set",
                    // выводим сообщение об ошибке
                    MessageBox.Show("Selected method is neither a getter nor a setter.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error executing method: " + ex.Message);
            }
        }






        private object[] ParseParameters()
        {
            string[] parameterStrings = Parameters.Split(',');
            ParameterInfo[] parameterInfos = SelectedMethod.MethodInfo.GetParameters();
            object[] parameters = new object[parameterInfos.Length];

            for (int i = 0; i < parameterInfos.Length; i++)
            {
                Type parameterType = parameterInfos[i].ParameterType;
                parameters[i] = Convert.ChangeType(parameterStrings[i], parameterType);
            }

            return parameters;
        
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _result;
        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged();
            }
        }

    }

}
