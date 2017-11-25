using AppLib.WPF.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Thingy.Implementation.Models
{
    public class Drive : BindableBase, IEquatable<Drive>
    {
        private string _LabelText;
        private long _UsedSpace;
        private long _DiskSize;
        private ImageSource _Icon;

        public Drive() { }

        public Drive(DriveInfo driveInfo)
        {
            switch (driveInfo.DriveType)
            {
                case DriveType.CDRom:
                    Icon = new BitmapImage(new Uri("pack://application:,,,/Thingy.Images;component/Icons/icons8-cd.png"));
                    break;
                case DriveType.Fixed:
                    Icon = new BitmapImage(new Uri("pack://application:,,,/Thingy.Images;component/Icons/icons8-hdd.png"));
                    break;
                case DriveType.Network:
                    Icon = new BitmapImage(new Uri("pack://application:,,,/Thingy.Images;component/Icons/icons8-network-drive.png"));
                    break;
                case DriveType.Ram:
                    Icon = new BitmapImage(new Uri("pack://application:,,,/Thingy.Images;component/Icons/icons8-memory-slot.png"));
                    break;
                case DriveType.Removable:
                    Icon = new BitmapImage(new Uri("pack://application:,,,/Thingy.Images;component/Icons/icons8-usb-connector.png"));
                    break;
            }

            if (driveInfo.IsReady)
            {
                _LabelText = string.Format("{0} - {1}", driveInfo.Name, driveInfo.VolumeLabel);
                _DiskSize = driveInfo.TotalSize;
                _UsedSpace = driveInfo.TotalSize - driveInfo.TotalFreeSpace;
            }
            else
            {
                _LabelText = string.Format("{0} - {1}", driveInfo.Name, "( Not Ready )");
                _DiskSize = 1;
                _UsedSpace = 0;
            }
        }

        public string LabelText
        {
            get { return _LabelText; }
            set
            {
                SetValue(ref _LabelText, value);
            }
        }
        public long UsedSpace
        {
            get { return _UsedSpace; }
            set
            {
                SetValue(ref _UsedSpace, value);
            }
        }
        public long DiskSize
        {
            get { return _DiskSize; }
            set
            {
                SetValue(ref _DiskSize, value);
            }
        }
        public ImageSource Icon
        {
            get { return _Icon; }
            set
            {
                SetValue(ref _Icon, value);
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Drive);
        }

        public bool Equals(Drive other)
        {
            return other != null &&
                   _LabelText == other._LabelText &&
                   _UsedSpace == other._UsedSpace &&
                   _DiskSize == other._DiskSize &&
                   EqualityComparer<ImageSource>.Default.Equals(Icon, other.Icon);
        }

        public override int GetHashCode()
        {
            var hashCode = -1727172872;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_LabelText);
            hashCode = hashCode * -1521134295 + _UsedSpace.GetHashCode();
            hashCode = hashCode * -1521134295 + _DiskSize.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<ImageSource>.Default.GetHashCode(Icon);
            return hashCode;
        }

        public static bool operator ==(Drive drive1, Drive drive2)
        {
            return EqualityComparer<Drive>.Default.Equals(drive1, drive2);
        }

        public static bool operator !=(Drive drive1, Drive drive2)
        {
            return !(drive1 == drive2);
        }
    }
}
