using System.Security.Cryptography;
using System.Text;

static class Config
{
    public static string ACCDirectory { get; private set; } = "";
    public static string BaseURL { get; private set; } = "www.roblox.com";
    public static string GSScript = "print('get a gameserver script nerd')";
    public static string RScript = "print('get a place render script nerd')";
    public static string RAScript = "print('get a avatar render script nerd')";
    public static string RMScript = "print('get a model render script nerd')";
    public static string RMMScript = "print('get a mesh render script nerd')";
    public static string GSScriptPath { get; private set; } = "";
    public static string RScriptPath { get; private set; } = "";
    public static string RAScriptPath { get; private set; } = "";
    public static string RMScriptPath { get; private set; } = "";
    public static string RMMScriptPath { get; private set; } = "";
    public static int port { get; private set; } = 7000;
    public static int cores { get; private set; } = 1;
    public static bool debug { get; private set; } = false;
    public static string SECRET = "my-mother-ate-fries-lol";
    public static string AccessKey = "my-mother-ate-fries-lol";
    public static string FakeSECRET = "";
    public static bool experimental { get; private set; } = false;
    public static bool removeACCLogs { get; private set; } = false;
    public static bool Ready { get; set; } = false; // DO NOT CHANGE THIS. THIS WILL BE AUTO SET IF ACCSERVICES ARE READY.
    public static bool realtime { get; private set; } = false;
    public static string name = "ACCService";
    public static bool poolgs { get; private set; } = false;

    public static void ReloadScripts()
    {
        try
        {
            void LoadScript(ref string scriptField, string path)
            {
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                    return;

                string content = File.ReadAllText(path);
                content = content.Replace("\r\n", "\n").Trim();

                scriptField = content;
            }

            LoadScript(ref GSScript, GSScriptPath);
            LoadScript(ref RScript, RScriptPath);
            LoadScript(ref RAScript, RAScriptPath);
            LoadScript(ref RMScript, RMScriptPath);
            LoadScript(ref RMMScript, RMMScriptPath);

            if (debug)
                Logger.Info("Scripts reloaded successfully.");
        }
        catch (Exception ex)
        {
            Logger.Error("Configuration reload failed: " + ex);
        }
    }

    public static void Parse(string[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--dir": // path for accservice
                    if (i + 1 >= args.Length)
                        throw new ArgumentException("--dir requires a value");

                    ACCDirectory = args[++i];
                    break;

                case "--skip-sysstats": // skip anti skid
                    Logger.Warn("SysStats is deprecated, strip this arg out of the start function");
                    break;

                // this is much better
                case "--gscript":
                case "--rscript":
                case "--rascript":
                case "--rmscript":
                case "--rmmscript":
                    if (i + 1 >= args.Length)
                        throw new ArgumentException($"{args[i]} requires a value");

                    string path = args[++i];
                    if (!File.Exists(path))
                        throw new FileNotFoundException($"Script not found for {args[i]}", path);

                    string scriptContent = File.ReadAllText(path);

                    switch (args[i - 1])
                    {
                        case "--gscript": GSScript = scriptContent; GSScriptPath = path; break;
                        case "--rscript": RScript = scriptContent; RScriptPath = path; break;
                        case "--rascript": RAScript = scriptContent; RAScriptPath = path; break;
                        case "--rmscript": RMScript = scriptContent; RMScriptPath = path; break;
                        case "--rmmscript": RMMScript = scriptContent; RMMScriptPath = path; break;
                    }
                    break;

                case "--baseurl": // baseURL for soap
                    if (i + 1 >= args.Length)
                        throw new ArgumentException("--baseurl requires a value");

                    BaseURL = args[++i];
                    break;

                case "--port": // what port to listen on
                    if (i + 1 >= args.Length)
                        throw new ArgumentException("--port requires a value");

                    port = int.Parse(args[++i]); // why are we parsing for this
                    break;

                case "--cores": // how much cpu cores should we use for ACCService
                    if (i + 1 >= args.Length)
                        throw new ArgumentException("--cores requires a value");

                    cores = int.Parse(args[++i]); // why are we parsing for this
                    break;

                case "--debug": // debug mode (more information)
                    debug = true;
                    break;

                case "--secret": // access key for apis
                    if (i + 1 >= args.Length)
                        throw new ArgumentException("--secret requires a value");

                    i++;
                    SECRET = args[i];
                    FakeSECRET = SECRET;
                    break;

                case "--accesskey": // access key for gameserver
                    if (i + 1 >= args.Length)
                        throw new ArgumentException("--accesskey requires a value");

                    AccessKey = args[++i];
                    break;

                case "--experimental": // experimental
                    experimental = true;
                    break;

                case "--removeacclogs": // experimental
                    removeACCLogs = true;
                    break;

                case "--realtime": // realtime priority
                    realtime = true;
                    break;

                case "--name": // accservice name (example: ACCService)
                    if (i + 1 >= args.Length)
                        throw new ArgumentException("--name requires a value");

                    name = args[++i];
                    break;

                case "--poolforgameservers": // use pooled accservices for gameservers
                    poolgs = true;
                    break;
            }
        }

        if (string.IsNullOrWhiteSpace(ACCDirectory) || !Directory.Exists(ACCDirectory))
        {
            // I GUESS WE'LL JUST SET OUR OWN
            ACCDirectory = AppContext.BaseDirectory;
        }
    }
}