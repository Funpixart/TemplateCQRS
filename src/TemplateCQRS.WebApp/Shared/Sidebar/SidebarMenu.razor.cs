using Microsoft.AspNetCore.Components;
using TemplateCQRS.WebApp.Components;

namespace TemplateCQRS.WebApp.Shared.Sidebar;

public class SidebarComponent : FunpixartComponent
{
    [Parameter] public string UserName { get; set; } = "Jhon Doe";
    [Parameter] public string UserRole { get; set; } = "Visitor";

    protected string LogoutEndPoint { get; set; } = "/account/logout";
}
