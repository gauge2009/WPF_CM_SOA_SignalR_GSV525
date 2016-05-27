using JR.Common.Extends;
using Alayaz.SOA.IService;
using Alayaz.SOA.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using SZ.Aisino.CMS.DbContext.Tax;
using SZ.Aisino.CMS.DbEntity.Tax;


//using log4net;
//using UN = Microsoft.Practices.Unity;

namespace Alayaz.SOA.Service.HostByIIS
{
    public static class DbContextHelper
    {

        public static decimal GetPkIndentity(this System.Data.Entity.DbContext ctx, string pkName, string tableName)
        {
            try
            {
                return ctx.Database.SqlQuery<Int32>(string.Format("select MAX({0}) from {1}", pkName, tableName)).First() + 1;
            }
            catch (Exception e)
            {
                //从具体化“System.Int32”类型到“System.Decimal”类型的指定强制转换无效。
                return 0;
            }
        }
    }
    public class SyncImportInvoiceService : ISyncImportInvoiceService
    {
        #region SERVICES

        public ImportInvoiceListDTO InjectList(ImportInvoiceListDTO soap)
        {
            int probe = 0;
            string exceptionMsg = "";
            using (TaxEntities db = new TaxEntities())
            {
                foreach (var model in soap.List)
                {
                    VIMS_BIZ_INVOICE entity = null;
                    if (db.VIMS_BIZ_INVOICE.Any(o => o.FPDM == model.InvoiceCode && o.FPHM == model.InvoiceNumber))
                    {//修改
                        entity = db.VIMS_BIZ_INVOICE.First(o => o.FPDM == model.InvoiceCode && o.FPHM == model.InvoiceNumber);
                        //db.VIMS_BIZ_INVOICE.Remove(old);
                        entity.DKZT = model.DeductionStatus;
                        entity.SHRQ = DateTime.Now;
                    }
                    else
                    {//新增
                        entity = new VIMS_BIZ_INVOICE();
                        entity.ID = Guid.NewGuid().ToString();

                        #region 非空冗余
                        entity.FPLB = "0";
                        entity.GFSH = model.TaxCode;
                        entity.FPMW = "N/A";
                        // entity.FPSL = 0.0M;//发票税率（只有机动车销售发票显示税率）
                        entity.QDBZ = "N";//清单标识（当清单标识为Y时发票为汉字防伪发票）
                        entity.SHRQ = DateTime.Now;
                        #endregion
                        #region Mapping
                        entity.DKZT = model.DeductionStatus;
                        entity.RZJG = "0";//认证结果（0：认证通过，1：认证未通过，255：待认证）
                        entity.RZZT = "0";//!string.IsNullOrEmpty(model.CertificateStatus) && string.Compare(model.CertificateStatus, "已确认", StringComparison.InvariantCultureIgnoreCase) == 0 ? "0" : "1"; //认证结果（0：认证通过，1：认证未通过，255：待认证）//认证状态 0：可以认证 1：不能认证
                        entity.FPDM = model.InvoiceCode;
                        entity.FPHM = model.InvoiceNumber;
                        DateTime d_createDate = DateTime.Now;
                        entity.KPRQ = DateTime.TryParse(model.CreateDate, out d_createDate) ? d_createDate : d_createDate;

                        entity.XFSH = model.SalesTaxNumber;//销方税号（货运为承运人，机动车为纳税人识别号）
                        entity.FPJE = model.Amount;
                        entity.FPSE = model.Tax;
                        //??????
                        entity.FPLY = "0";//??????发票来源 FPLY（0：手工录入；1：扫描采集；2：接口传入；3：导入；4：录入）  /  来源 RZLY（0：系统认证；1：其他认证）

                        //entity.??? = model.SelectTag;
                        //entity.RZSJ = model.OperationTime;

                        #endregion
                        entity = db.VIMS_BIZ_INVOICE.Add(entity);
                    }
                }
                this.Errors = GetErrors(db);
                if (!this.HasError)
                {
                    try
                    {
                        //db.SaveChanges();
                        probe = db.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                        exceptionMsg = ex.Message;
                    }
                }
                else
                {
                    exceptionMsg = this.Errors != null && this.Errors.Count > 0 ? this.Errors.Values.FirstOrDefault() : "DTO校验异常";
                }

            }
            soap.Result = new ImportInvoiceResultDTO
            {
                Message = !string.IsNullOrEmpty(exceptionMsg) ? exceptionMsg : "SUCCESS",
                Status = probe
            };
            return soap;
        }



        /* public List<VIMS_BIZ_INVOICE> FetchList(Condition condition)
         {

             using (TaxEntities db = new TaxEntities())
             {

                 var datas = condition.Filter(db.VIMS_BIZ_INVOICE);

                 //if (!string.IsNullOrEmpty(condition.Location))
                 //{
                 //    datas = datas.Where(o => o.Address.Contains(condition.Location));

                 //}
                 //var data = datas.OrderByDescending(o => o.KPRQ).DoPage(condition.Pager).ToList();
                 var data = datas.OrderByDescending(o => o.KPRQ).ToList();
                 return data;

             }

         }*/

        public ImportInvoiceListDTO FetchList(ImportInvoiceDTO condition)
        {
            ImportInvoiceListDTO rs = new ImportInvoiceListDTO
            {
                List = null,
                Result = new ImportInvoiceResultDTO
                {
                    Message = "",
                    Status = 0
                }
            };
            using (TaxEntities db = new TaxEntities())
            {

                var datas = db.VIMS_BIZ_INVOICE.Where(o =>
                 !string.IsNullOrEmpty(condition.InvoiceCode) ? o.FPDM == condition.InvoiceCode : true
                    &&
                       !string.IsNullOrEmpty(condition.InvoiceCode) ? o.FPDM == condition.InvoiceCode : true
                    );

                if (!string.IsNullOrEmpty(condition.InvoiceCode))
                {
                    datas = datas.Where(o => o.FPDM == condition.InvoiceCode);

                }
                // var data = datas.OrderByDescending(o => o.KPRQ).DoPage(condition.Pager).ToList();
                var data = datas.Select(o => new ImportInvoiceDTO
                {
                    InvoiceCode = o.FPDM,
                    InvoiceNumber = o.FPHM,
                    // CreateDate = o.KPRQ.ToString(),
                    Amount = o.FPJE,
                    Tax = o.FPSE,
                    SalesTaxNumber = o.XFSH,

                }).OrderByDescending(o => o.InvoiceCode).ToList();
                rs.List = data;

            }
            return rs;
        }

        #endregion


        #region  Assist Framwork
        internal bool HasError
        {
            get
            {
                return this.Errors.Count > 0;
            }
        }

        private Dictionary<string, string> errors = new Dictionary<string, string>();
        internal Dictionary<string, string> Errors
        {
            get
            {
                if (this.errors == null)
                    this.errors = new Dictionary<string, string>();
                return this.errors;
            }
            set
            {
                this.errors = value;
            }
        }

        internal static Dictionary<string, string> GetErrors(System.Data.Entity.DbContext db)
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            IEnumerable<DbEntityValidationResult> enumerable = from e in db.GetValidationErrors() select e;
            using (IEnumerator<DbEntityValidationResult> enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Action<DbValidationError> action = null;
                    DbEntityValidationResult e = enumerator.Current;
                    if (action == null)
                    {
                        action = delegate (DbValidationError ee)
                        {
                            datas.Set<string, string>(string.Format("{0} {1}", e.Entry.Entity.GetType(), ee.PropertyName), ee.ErrorMessage);
                        };
                    }
                    (from ee in e.ValidationErrors select ee).ToList<DbValidationError>().ForEach(action);
                }
            }
            return datas;
        }


        /// <summary>
        /// CopyP2P !
        /// </summary>
        /// <param name="source"></param>
        /// <param name="obj"></param>
        private void CopyP2P(object source, object obj)
        {
            // 用source属性写obj属性
            var propsObj = obj.GetType().GetProperties();
            var propsSource = source.GetType().GetProperties();
            (from po in propsObj
             from p in propsSource
             where po.Name == p.Name && po.CanWrite
             select new
             {
                 Do = new Action(() =>
                 {
                     var v = p.GetValue(source);
                     if (v == null)
                     {
                         if (po.PropertyType == typeof(String))
                             po.SetValue(obj, "N/A");
                         else if (po.PropertyType == typeof(DateTime))
                             po.SetValue(obj, DateTime.Now);
                         else if (po.PropertyType == typeof(Int32))
                             po.SetValue(obj, 0);
                     }
                     po.SetValue(obj, p.GetValue(source));
                 })
             }).ToList().ForEach(x =>
             {
                 x.Do();
             });
            // 用source属性写obj字段
            var fieldsObj = obj.GetType().GetFields();
            (from fo in fieldsObj
             from p in propsSource
             where fo.Name == p.Name
             select new
             {
                 Do = new Action(() =>
                 {
                     var v = p.GetValue(source);

                     if (fo.FieldType == typeof(bool) && (p.PropertyType == typeof(byte) || p.PropertyType == typeof(byte?)) && fo.Name.Contains("HideEmail"))
                     {  //  源中的byte要在目标中要转成bool  如："HideEmail" 
                         if (v != null && (byte)v > 0)
                             fo.SetValue(obj, true);
                         else
                             fo.SetValue(obj, false);
                     }
                     else if (fo.FieldType == typeof(bool) && (p.PropertyType == typeof(int) || p.PropertyType == typeof(int?)) && fo.Name.Contains("Sex"))
                     {  //  源中的byte要在目标中要转成bool  如："HideEmail" 
                         if (v != null && (int)v > 0)
                             fo.SetValue(obj, true);
                         else
                             fo.SetValue(obj, false);
                     }


                     else
                     {

                         if (v == null)
                         {
                             if (fo.FieldType == typeof(String))
                                 fo.SetValue(obj, "N/A");
                             else if (fo.FieldType == typeof(DateTime))
                                 fo.SetValue(obj, DateTime.Now);
                             else if (fo.FieldType == typeof(Int32))
                                 fo.SetValue(obj, 0);
                         }
                         fo.SetValue(obj, p.GetValue(source));
                     }
                 })
             }).ToList().ForEach(x =>
             {
                 x.Do();
             });
        }


        #endregion

    }
}
