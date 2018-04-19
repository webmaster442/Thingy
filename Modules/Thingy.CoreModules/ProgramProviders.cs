using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Thingy.CoreModules.Models;
using Thingy.XAML.Converters;

namespace Thingy.CoreModules
{
    public static class ProgramProviders
    {
        public static IEnumerable<SystemProgram> GetStartMenu()
        {
            var userstartmenu = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
            var commonstartmenu = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);

            var user = Directory.GetFiles(userstartmenu, "*.lnk", SearchOption.AllDirectories);
            var common = Directory.GetFiles(commonstartmenu, "*.lnk", SearchOption.AllDirectories);

            var programs = new List<SystemProgram>(user.Length + common.Length);

            foreach (var item in common)
            {
                var program = new SystemProgram
                {
                    Name = Path.GetFileNameWithoutExtension(item),
                    Path = item,
                    Icon = ExeIconExtractor.GetIcon(item)
                };
                programs.Add(program);
            }

            return programs.OrderBy(p => p.Name);
        }
    }
}
