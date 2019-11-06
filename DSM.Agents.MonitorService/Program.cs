using System.ServiceProcess;

namespace DSM.Agents.MonitorService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServiceBase.Run(new SiteService());
        }
    }
}
