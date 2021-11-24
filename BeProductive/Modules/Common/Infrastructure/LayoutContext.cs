namespace BeProductive.Modules.Common.Infrastructure;

public class LayoutContext
{
    public string? Title
    {
        get => _title;
        set => SetField(ref _title, value);
    }

    public string? Subtitle
    {
        get => _subtitle;
        set => SetField(ref _subtitle, value);
    }

    private string? _subtitle = "";
    private string _title = "";

    public event Action OnUpdate;

    private bool SetField<T>(ref T field, T value)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnUpdate?.Invoke();
        return true;
    }
}