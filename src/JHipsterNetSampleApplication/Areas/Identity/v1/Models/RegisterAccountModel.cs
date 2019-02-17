using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JHipsterNetSampleApplication.Areas.Identity.v1.Models {
    public class RegisterAccountModel {
        public string Email { get; internal set; }
        public string UserName { get; internal set; }
        public string Password { get; internal set; }
    }
}
