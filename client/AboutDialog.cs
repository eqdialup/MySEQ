using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace myseq
{
    public partial class AboutDialog : Form
    {
        private const string dockpanel = "https://dockpanelsuite.sourceforge.net";
        private const string showeqforum = "https://www.showeq.net/forums/forum.php";

        public AboutDialog()
        {
            InitializeComponent();
            Text = $"About {GetAssemblyTitle()}";
            labelProductName.Text = GetAssemblyProduct();
            labelVersion.Text = $"Version {AssemblyVersion}";
            labelCopyright.Text = GetAssemblyCopyright();
            labelCompanyName.Text = GetAssemblyCompany();
        }

        #region Assembly Attribute Accessors

        public static string GetAssemblyTitle()
        {
            var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (attributes.Length > 0)
            {
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                if (!string.IsNullOrEmpty(titleAttribute.Title))
                {
                    return titleAttribute.Title;
                }
            }
            return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
        }

        public static string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public static string GetAssemblyProduct()
        {
            var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            if (attributes.Length == 0)
            {
                return "";
            }
            return ((AssemblyProductAttribute)attributes[0]).Product;
        }

        public static string GetAssemblyCopyright()
        {
            var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (attributes.Length == 0)
            {
                return "";
            }
            return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
        }

        public static string GetAssemblyCompany()
        {
            var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            if (attributes.Length == 0)
            {
                return "";
            }
            return ((AssemblyCompanyAttribute)attributes[0]).Company;
        }

        #endregion Assembly Attribute Accessors

        private static void DockpanelClick(object sender, LinkLabelLinkClickedEventArgs e) =>
            Process.Start(dockpanel);

        private static void ForumlinkClick(object sender, LinkLabelLinkClickedEventArgs e) =>
            Process.Start(showeqforum);
    }
}