using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JHipsterNetSampleApplication.Areas.Identity.v1.Models {
    public class ChangePasswordModel {
        public string NewPassword { get; set; }
        public string CurrentPassword { get; set; }
    }
}
