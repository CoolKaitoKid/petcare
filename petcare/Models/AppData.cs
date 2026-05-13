using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petcare.Models
{

    public static class AppData
    {
        public static ObservableCollection<Appointment> Appointments { get; set; }
            = new ObservableCollection<Appointment>();
    }
}

