using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleJavaWpfLibrary.ViewModel;
public class MainWindowViewModel : BaseViewModel
{
    private BaseViewModel viewModel;
    public BaseViewModel ViewModel
    {
        get { return viewModel; }
        set
        {
            viewModel = value;
            OnPropertyChanged();
        }
    }

    public MainWindowViewModel()
    {
        ViewModel = new WeatherViewModel();
    }
}
