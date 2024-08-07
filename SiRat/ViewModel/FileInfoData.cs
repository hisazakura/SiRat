using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SiRat.ViewModel
{
    public class FileInfoData : INotifyPropertyChanged
    {
        public string Key { get; set; }
        public string Separator { get; set; }
        public string Value { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public FileInfoData(string key, string separator, string value)
        {
            Key = key;
            Separator = separator;
            Value = value;
        }
    }
}
