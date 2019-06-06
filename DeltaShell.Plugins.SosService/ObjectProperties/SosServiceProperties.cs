using System.ComponentModel;
using DelftTools.Shell.Gui;

namespace DeltaShell.Plugins.SosService.ObjectProperties
{
    [DisplayName("SOS Query model information")]
    public class VolumeModelProperties : ObjectProperties<Models.SosService>
    {
        [Category("General")]
        [DisplayName("Name")]
        [Description("Name of this Service")]
        public string Name
        {
            get { return data.Name; }
            set { data.Name = value; }
        }

        [Category("Input")]
        [DisplayName("Property")]
        [Description("Selected property (Needs to be QR or HG)")]
        public string Property
        {
            get { return data.Property; }
            set { data.Property = value; }
        }

        [Category("Input")]
        [DisplayName("Station ID")]
        [Description("Selected station ID")]
        public string Station
        {
            get { return data.Station; }
            set { data.Station = value; }
        }
    }
}