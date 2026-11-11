using System;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json.Linq;

public class AxpertHtmlAuthModule : IHttpModule
{
    private static readonly string[] ExcludedFolders =
    {
        "/ckeditor/", "/compiler/", "/packages/",
        "/bootstrap-session-timeout/", "/bootstrap-tokenfield/",
        "/gridstack", "/Linearicons/",
        "/CommonDir/", "/filemanager/", "/downloads/"
    };

    private static readonly string[] BypassFiles = BuildBypassFiles();

    private static string[] BuildBypassFiles()
    {
        string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppSettings.json");
        if (!System.IO.File.Exists(path))
        {
            return new string[0];
        }
        try
        {
            JObject settings = JObject.Parse(System.IO.File.ReadAllText(path));
            JToken configuredFiles = settings["HtmlAuthBypassFiles"];
            if (configuredFiles == null || configuredFiles.Type != JTokenType.Array)
            {
                return new string[0];
            }

            var files = new System.Collections.Generic.List<string>();
            foreach (JToken token in (JArray)configuredFiles)
            {
                if (token.Type != JTokenType.String)
                {
                    continue;
                }

                string name = token.Value<string>().Trim();
                if (name.Length == 0 || name.IndexOfAny(new[] { '/', '\\', ':' }) >= 0)
                {
                    continue;
                }

                if (!files.Contains(name, StringComparer.OrdinalIgnoreCase))
                {
                    files.Add(name);
                }
            }

            return files.ToArray();
        }
        catch (Newtonsoft.Json.JsonException)
        {
            return new string[0];
        }
        catch (System.IO.IOException)
        {
            return new string[0];
        }
        catch (System.UnauthorizedAccessException)
        {
            return new string[0];
        }
    }

    public void Init(HttpApplication application)
    {
        application.PostResolveRequestCache += OnPostMapRequestHandler;
    }

    private void OnPostMapRequestHandler(object sender, EventArgs e)
    {
        HttpApplication app = (HttpApplication)sender;
        HttpContext context = app.Context;

        string path = context.Request.Url.AbsolutePath;

        // Protect every .html/.htm file; third-party library folders are excluded
        if (!path.EndsWith(".html", StringComparison.OrdinalIgnoreCase) &&
            !path.EndsWith(".htm", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        // Environment-specific files (health checks etc.) are skipped by file name
        string fileName = System.IO.Path.GetFileName(path);
        foreach (string name in BypassFiles)
        {
            if (string.Equals(fileName, name, StringComparison.OrdinalIgnoreCase))
            {
                // Prevent browsers from keeping a copy, so removing a file from the
                // bypass list takes effect immediately.
                context.Response.Headers.Set("Cache-Control", "no-store, no-cache, must-revalidate");
                context.Response.Headers.Set("Pragma", "no-cache");
                return;
            }
        }

        foreach (string lib in ExcludedFolders)
        {
            if (path.IndexOf(lib, StringComparison.OrdinalIgnoreCase) >= 0)
                return;
        }

        // Replace IIS StaticFile handler with our handler.
        context.RemapHandler(new AxpertHtmlAuthHandler());
    }

    public void Dispose()
    {
    }
}


public class AxpertHtmlAuthHandler : IHttpHandler, IRequiresSessionState
{
    public bool IsReusable
    {
        get { return false; }
    }

    public void ProcessRequest(HttpContext context)
    {
        // Session should be available here because
        // this handler implements IRequiresSessionState.

        // Set before any early return so 401/403 responses are never cached either.
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        context.Response.Cache.SetNoStore();

        object project = context.Session["project"];
        object username = context.Session["username"];

        bool isLoggedIn =
            project != null &&
            !string.IsNullOrWhiteSpace(project.ToString()) &&
            username != null &&
            !string.IsNullOrWhiteSpace(username.ToString());

        if (!isLoggedIn)
        {
            context.Response.StatusCode = 401;
            context.Response.TrySkipIisCustomErrors = true;
            context.ApplicationInstance.CompleteRequest();
            return;
        }

        // Block direct browser navigation; iframe loads send Sec-Fetch-Dest: iframe.
        string dest = context.Request.Headers["Sec-Fetch-Dest"];
        bool direct = !string.IsNullOrEmpty(dest) && !string.Equals(dest, "iframe", StringComparison.OrdinalIgnoreCase);
        if (direct)
        {
            context.Response.StatusCode = 403;
            context.Response.TrySkipIisCustomErrors = true;
            context.ApplicationInstance.CompleteRequest();
            return;
        }

        string filePath = context.Request.PhysicalPath;

        if (!System.IO.File.Exists(filePath))
        {
            context.Response.StatusCode = 404;
            context.Response.TrySkipIisCustomErrors = true;
            context.ApplicationInstance.CompleteRequest();
            return;
        }

        context.Response.ContentType = "text/html";

        context.Response.WriteFile(filePath);
    }
}
