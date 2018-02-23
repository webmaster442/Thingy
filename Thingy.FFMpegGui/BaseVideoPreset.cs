using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.FFMpegGui
{
    public abstract class BaseVideoPreset : BasePreset
    {
        public abstract string VideoCommandLine { get; }

        public override string CommandLine
        {
            get
            {
                return $"ffmpeg.exe -i \"{InputFile}\" -vn {VideoCommandLine} \"{OutputFile}\"";
            }
        }

        public BaseVideoPreset(): base()
        {

        }
    }
}
