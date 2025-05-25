using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OracleJavaWpfLibrary.ViewModel;
public class CounterViewModel : BaseViewModel
{
    private int count;
    public int Count
    {
        get { return count; }
        set
        {
            count = value;
            OnPropertyChanged();
        }
    }
    public CounterViewModel()
    {
        Count = 0;
    }

    #region commands

    public ICommand IncrementCommand
    {
        get
        {
            return new ActionCommand( action =>
            {
                Increment();
            });
        }
    }

    public ICommand DecrementCommand
    {
        get
        {
            return new ActionCommand( action =>
            {
                Decrement();
            });
        }
    }

    public ICommand ResetCommand
    {
        get
        {
            return new ActionCommand( action =>
            {
                Reset();
            });
        }
    }

    #endregion


    public void Increment()
    {
        Count++;
    }
    public void Decrement()
    {
        Count--;
    }

    public void Reset()
    {
        Count = 0;
    }
}
