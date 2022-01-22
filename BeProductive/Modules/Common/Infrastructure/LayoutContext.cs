using Microsoft.AspNetCore.Components;
using Serilog;

namespace BeProductive.Modules.Common.Infrastructure;

public class LayoutContext
{
    public string? Title
    {
        get => _title;
        set
        {
            SetField(ref _title, value);
            SetField(ref _showBackButton, false);
        }
    }

    public string? Subtitle
    {
        get => _subtitle;
        set => SetField(ref _subtitle, value);
    }
    
    public bool ShowBackButton
    {
        get => _showBackButton;
        set => SetField(ref _showBackButton, value);
    }
    
    public RenderFragment? ExtraContent
    {
        get => _extraContent;
        set => SetField(ref _extraContent, value);
    }

    private string? _subtitle = "";
    private string _title = "";
    private bool _showBackButton = false;
    private RenderFragment? _extraContent = null;

    public event Action OnUpdate;

    private bool SetField<T>(ref T field, T value)
    {
        // Log.Warning("Comparing {Field} with {Value}, equal: {Equal}", field, value, EqualityComparer<T>.Default.Equals(field, value));
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnUpdate?.Invoke();
        return true;
    }
}