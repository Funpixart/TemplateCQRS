using Microsoft.JSInterop;

namespace TemplateCQRS.WebApp.Components.Services;
public class FunpixartServiceJs : IFunpixartServiceJs
{
    private readonly IJSRuntime _jsRuntime;
    private const string LibraryName = "Funpixart.";

    public FunpixartServiceJs(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public ValueTask InvokeVoidAsync(string funcName)
    {
        return _jsRuntime.InvokeVoidAsync(LibraryName + funcName);
    }

    public ValueTask InvokeVoidAsync(string funcName, params object[] args)
    {
        return _jsRuntime.InvokeVoidAsync(LibraryName + funcName, args);
    }

    public ValueTask<T> InvokeAsync<T>(string funcName, params object[] args)
    {
        var result = _jsRuntime.InvokeAsync<T>(LibraryName + funcName, args);
        return result;
    }

    public ValueTask<T> InvokeAsyncWithoutLib<T>(string funcName, params object[] args)
    {
        var result = _jsRuntime.InvokeAsync<T>(funcName, args);
        return result;
    }
}