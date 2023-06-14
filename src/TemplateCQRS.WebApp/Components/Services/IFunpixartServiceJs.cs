namespace TemplateCQRS.WebApp.Components.Services;

public interface IFunpixartServiceJs
{
    public ValueTask InvokeVoidAsync(string funcName);
    public ValueTask InvokeVoidAsync(string funcName, params object[] obj);
    public ValueTask<T> InvokeAsync<T>(string funcName, params object[] args);
    public ValueTask<T> InvokeAsyncWithoutLib<T>(string funcName, params object[] args);
}