using System.ComponentModel;
using System.Runtime.CompilerServices;

public class ViewModelBase : INotifyPropertyChanged
{
#pragma warning disable CS8612 // Nullability of reference types in type doesn't match implicitly implemented member.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning restore CS8612 // Nullability of reference types in type doesn't match implicitly implemented member.

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(name);
        return true;
    }

    // Backwards-compatible alias used by some view models in the project
    // Some classes call SetProperty(...) while this base previously exposed SetField(...)
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        return SetField(ref field, value, name);
    }
}