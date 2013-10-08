using System.Web;

namespace Views.Frames
{
    /// <summary>
    /// Get/Set frame info to store in session variavel for faster access;
    /// </summary>
    public class frame_sessions
    {
        /************** Frame Data **************/
        static public object GetFrame(string frameId)
        {
            return HttpContext.Current.Session[frameId];
        }

        static public void SetFrame(string frameId, DbConfig.Frame frame)
        {
            HttpContext.Current.Session[frameId] = frame;
        }
    }
}