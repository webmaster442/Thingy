using Setup.Internals;
using WixSharp;
using WixSharp.Forms;

namespace Setup
{
    class Program
    {
        static void Main()
        {
            var project = new ManagedProject("Thingy")
            {
                InstallPrivileges = InstallPrivileges.limited,
                OutDir = Constants.OutputDirectory,
                LicenceFile = Constants.LicenceFile,
                Platform = Platform.x64,
                Version = InstallerHelper.GetThingyVersion(),
                GUID = Constants.InstallerGUID,
                ManagedUI = ManagedUI.Empty,
                Dirs = new Dir[]
                {
                    new Dir(Constants.InstallDir,
                           InstallerHelper.GetFiles(InstallerHelper.ThingyReleaseDirectory,".dll", ".exe", ".config", ".vbs"))
                }
            };
            project.BackgroundImage = Constants.Banner;
            project.ManagedUI = ManagedUI.Default;

            //custom set of standard UI dialogs
            project.ManagedUI = new ManagedUI();

            project.ManagedUI.InstallDialogs.Add(Dialogs.Welcome)
                                            .Add(Dialogs.Licence)
                                            .Add(Dialogs.SetupType)
                                            .Add(Dialogs.InstallDir)
                                            .Add(Dialogs.Progress)
                                            .Add(Dialogs.Exit);

            project.ManagedUI.ModifyDialogs.Add(Dialogs.MaintenanceType)
                                           .Add(Dialogs.Progress)
                                           .Add(Dialogs.Exit);

            Compiler.AllowNonRtfLicense = true;
            Compiler.BuildMsi(project);
        }
    }
}