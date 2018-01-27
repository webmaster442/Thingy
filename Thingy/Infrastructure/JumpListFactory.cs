using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Shell;

namespace Thingy.Infrastructure
{
    internal static class JumpListFactory
    {
        private static IEnumerable<JumpTask> Tasks
        {
            get
            {
                yield return new JumpTask()
                {
                    Title = "Restart",
                    Arguments = "/restart",
                    Description = "Restarts the application",
                    CustomCategory = "Actions",
                    IconResourcePath = Assembly.GetEntryAssembly().CodeBase,
                    ApplicationPath = Assembly.GetEntryAssembly().CodeBase
                };
                yield return new JumpTask()
                {
                    Title = "Exit",
                    Arguments = "/exit",
                    Description = "Exit from application",
                    CustomCategory = "Actions",
                    IconResourcePath = Assembly.GetEntryAssembly().CodeBase,
                    ApplicationPath = Assembly.GetEntryAssembly().CodeBase
                };
            }
        }

        public static void CreateJumplist()
        {
            var appmenu = new JumpList();
            appmenu.JumpItems.AddRange(Tasks);
            appmenu.ShowFrequentCategory = false;
            appmenu.ShowRecentCategory = false;
            JumpList.SetJumpList(Application.Current, appmenu);
        }
    }
}
