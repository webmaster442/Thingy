using System;
using System.Collections.Generic;

namespace Thingy.API.Jobs
{
    public class JobProgress : IEquatable<JobProgress>
    {
        public string StatusText { get; set; }
        public double Progress { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as JobProgress);
        }

        public bool Equals(JobProgress other)
        {
            return other != null &&
                   StatusText == other.StatusText &&
                   Progress == other.Progress;
        }

        public override int GetHashCode()
        {
            var hashCode = 244636670;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StatusText);
            hashCode = hashCode * -1521134295 + Progress.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(JobProgress progress1, JobProgress progress2)
        {
            return EqualityComparer<JobProgress>.Default.Equals(progress1, progress2);
        }

        public static bool operator !=(JobProgress progress1, JobProgress progress2)
        {
            return !(progress1 == progress2);
        }
    }
}
