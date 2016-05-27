using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SZ.Aisino.CMS.Models {
    public class ChangePwd {

        [Required, StringLength(50, MinimumLength = 8)]
        public string NewPwd {
            get;
            set;
        }

        [Required]
        public string OldPwd {
            get;
            set;
        }

        [Required, Compare("NewPwd")]
        public string Confirm {
            get;
            set;
        }

        //public string Captcha {
        //    get;
        //    set;
        //}

    }
}