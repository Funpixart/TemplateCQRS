namespace TemplateCQRS.WebApp.Data.Pagination;

public class PagingLink
{
    public string Text { get; set; } = "";
    public int Page { get; set; } = 1;
    public bool Enabled { get; set; }
    public bool Active { get; set; }
    public bool Hidden { get; set; }
    public string Info { get; set; } = "";

    public PagingLink(int page, bool enabled, string text, string info)
    {
        Page = page;
        Enabled = enabled;
        Text = text;
        Info = info;
    }

    public PagingLink()
    {
    }
}