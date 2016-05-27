using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;
using System.Web.WebPages;
using JR.Common.Extends;
using JR.Configuration;

namespace SZ.Aisino.CMS {
    public class DisplayModelSetting {

        public static void Config() {

            var config = ConfigurationHelper.GetSection<CustomDomainsConfig>();
            if (config != null && config.Domains != null) {
                config.Domains.Cast<CustomDomainItem>().ToList().ForEach(c => {
                    DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode(c.View) {
                        ContextCondition = ctx => {
                            return string.Equals(ctx.Request.Url.Host, c.Domain, StringComparison.OrdinalIgnoreCase);
                        }
                    });
                });
            }
        }

    }
}