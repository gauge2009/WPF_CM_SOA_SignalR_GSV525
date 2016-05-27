using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JR.Common.Attributes;

namespace SZ.Aisino.CMS.DbEntity.Annotation {
    public class AnnotationHelper {

        public static void AutoMap() {

            var types = typeof(AnnotationHelper).Assembly.GetTypes();
            foreach (var t in types) {
                var attr = (AnnoationForAttribute)t.GetCustomAttributes(typeof(AnnoationForAttribute), false).FirstOrDefault();
                if (attr != null)
                    TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(attr.ForType, t), attr.ForType);
            }

        }

    }
}
