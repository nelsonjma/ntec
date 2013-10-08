using System;
using System.IO;

/// <summary>
/// class that permits log of errors
/// </summary>
public class loging
{
    public static void message(string location, string operation, string message, string filename)
    {
        try
        {
            if (!string.IsNullOrEmpty(filename))
            {
                File.AppendAllText(filename, "<message>\n");
                File.AppendAllText(filename, "  <location>" + location + "</location>\n");
                File.AppendAllText(filename, "  <title>" + operation + "</title>\n");
                File.AppendAllText(filename, "  <date>" + DateTime.Now.ToString() + "</date>\n");
                File.AppendAllText(filename, "  <content>\n");
                File.AppendAllText(filename, message + "\n");
                File.AppendAllText(filename, "  </content>\n");
                File.AppendAllText(filename, "</message>\n");
                File.AppendAllText(filename, "\n");
            }
        }
        catch {}
    }

    public static void Error(string location, string operation, string message, string filename)
    {
        try
        {
            if (!string.IsNullOrEmpty(filename))
            {
                File.AppendAllText(filename, "<error>\n");
                File.AppendAllText(filename, "  <location>" + location + "</location>\n");
                File.AppendAllText(filename, "  <title>" + operation + "</title>\n");
                File.AppendAllText(filename, "  <date>" + DateTime.Now.ToString() + "</date>\n");
                File.AppendAllText(filename, "  <content>\n");
                File.AppendAllText(filename, message + "\n");
                File.AppendAllText(filename, "  </content>\n");
                File.AppendAllText(filename, "</error>\n");
                File.AppendAllText(filename, "\n");
            }
        }
        catch { }
    }

}