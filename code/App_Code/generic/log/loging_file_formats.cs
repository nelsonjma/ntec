using System;

/// <summary>
/// Build personalised log files
/// </summary>
public class LogingFileFormats
{
    public static string ReplaceLogFrameFile(string oldPath, int pageId, int frameId)
    {
        // if no file selected leave
        if (oldPath == string.Empty) return oldPath;

        string newPath = oldPath.Remove(oldPath.LastIndexOf('\\'));

        return newPath + "\\pageid_" + pageId + "_frameid_" + frameId + "_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".log";
    }
}
