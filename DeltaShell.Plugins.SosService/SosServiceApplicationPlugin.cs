using DelftTools.Shell.Core;
using Mono.Addins;

namespace DeltaShell.Plugins.SosService
{
    [Extension(typeof(IPlugin))]
    public class SosServiceApplicationPlugin : ApplicationPlugin
    {
        public override string Name
        {
            get { return "SosServiceApplication"; }
        }
        public override string DisplayName
        {
            get { return "Sos Service application"; }
        }
        public override string Description
        {
            get { return "Application plugin to query SOS NIWA service"; }
        }
        public override string Version
        {
            get { return "1.0"; }
        }
        public override string FileFormatVersion
        {
            get { return "1.0"; }
        }
    }
}