using DSM.Controller.AppServices.IIS;
using DSM.Core.Interfaces.AppServices;
using DSM.Core.Ops;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace DSM.Agents.MonitorService
{
    public class SiteService : DSMAgent
    {
        public SiteService() : base("DSM.Agents.MonitorService") { }

        public override void ActionMain(object authObj)
        {

            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            if (isWindows)
            {
                if (!WebServerOperations.IsIISServerInstalled())
                {
                    Stop();
                    return;
                }
            }

            string apiKey = (string)authObj;
            if (authToken != null && authToken.Length > 1)
            {
                apiKey = authToken;
            }

            SiteController sController = new SiteController(apiKey);
            sController.PostSites();
            loggingSvc.Write("[Post] SiteController ran.");

            IEnumerable<ISite> sites = sController.Sites;
            loggingSvc.Write($"[Get] SiteController item count -> {sites.Count()}.");

            SiteBindingController bindingController = new SiteBindingController(apiKey);
            bindingController.Post(sites);
            loggingSvc.Write("[Post] SiteBindingController ran.");

            WebConfigController wCfgController = new WebConfigController(apiKey);
            wCfgController.Post(sites);
            loggingSvc.Write("[Post] WebConfigController ran.");

            SitePackageController sPackController = new SitePackageController(apiKey);
            sPackController.Post(sites);
            loggingSvc.Write("[Post] SitePackageController ran.");

            SiteEndpointController endpointController = new SiteEndpointController(apiKey);
            endpointController.Post(sites);

            SiteConnectionStringController connectionStringController = new SiteConnectionStringController(apiKey);
            connectionStringController.Post(sites);
        }
    }
}
