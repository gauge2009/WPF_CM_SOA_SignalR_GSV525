using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace SZ.Aisino.WebCrawl.ViewModels {
    public class EditorViewModel : Screen {

        public string ResFilePath {
            get;
            set;
        }

        private Dictionary<string, string> ResFiles {
            get;
            set;
        }

        public string DllFilePath {
            get;
            set;
        }

        public Type TmpType {
            get;
            set;
        }

        public BindableCollection<dynamic> Datas {
            get;
            set;
        }

        public List<Type> EntityTypes {
            get;
            set;
        }

        public Type SelectedEntity {
            get;
            set;
        }


        #region resx
        public void ChoiceResxFile() {
            var dialog = new OpenFileDialog() {
                Filter = "Resx|*.resx"
            };
            dialog.FileName = this.ResFilePath;
            if (dialog.ShowDialog() == true) {
                this.ResFilePath = dialog.FileName;
                this.NotifyOfPropertyChange(() => this.ResFilePath);

                this.ResFiles = this.GetLangFiles(dialog.FileName);
                this.DefineType(this.ResFiles.Keys);
                this.ReadResx(this.ResFiles);
            }
        }

        private void DefineType(IEnumerable<string> langs) {
            var tb = TypeBuilderHelper.Define("TTT", typeof(Record));
            langs.ToList().ForEach(l => {
                tb.DefineProperty(l.Replace("-", "_"), typeof(string));
            });
            this.TmpType = tb.CreateType();
        }

        private string GetResFileName(string path) {
            var onlyName = Path.GetFileNameWithoutExtension(path);
            var tmp = onlyName.Split(new char[] { '.' });
            if (tmp.Length > 1 && this.IsLang(tmp.Last())) {
                onlyName = string.Join(".", tmp.Take(tmp.Length - 1));
            }
            return onlyName;
        }

        private Dictionary<string, string> GetLangFiles(string path) {
            var name = this.GetResFileName(path);
            var dir = Path.GetDirectoryName(path);
            var files = Directory.GetFiles(dir, string.Format("{0}.*.resx", name));

            var dic = new Dictionary<string, string>() { 
                {"Default", Path.Combine(dir, string.Format("{0}.resx", name)) }
            };

            foreach (var f in files) {
                var lang = Regex.Match(f, @".(?<lang>[^. ]*?).resx").Groups["lang"].Value;
                dic.Add(lang, f);
            }

            return dic;
        }

        private bool IsLang(string lang) {
            try {
                CultureInfo.GetCultureInfo(lang);
                return true;
            } catch {
                return false;
            }
        }

        private void ReadResx(Dictionary<string, string> dic) {

            List<dynamic> results = new List<dynamic>();

            foreach (var d in dic) {
                try {

                    using (var reader = new ResXResourceReader(d.Value)) {
                        foreach (DictionaryEntry entry in reader) {
                            try {

                                if (entry.Key == null || entry.Value == null || entry.Key.GetType() != typeof(string) || entry.Value.GetType() != typeof(string))
                                    continue;

                                var key = (string)entry.Key;

                                var o = results.FirstOrDefault(r => r.Key.Equals(key));
                                if (o == null) {
                                    o = Activator.CreateInstance(this.TmpType);
                                    o.Key = (string)entry.Key;
                                    o.IsExists = true;
                                    results.Add(o);
                                }

                                this.TmpType.GetProperty(d.Key.Replace("-", "_"))
                                    .SetValue(o, (string)entry.Value);
                            } catch {
                            }
                        }
                    }
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }

            if (results.Count == 0)
                results.Add(Activator.CreateInstance(this.TmpType));

            this.Datas = new BindableCollection<dynamic>(results);
            this.NotifyOfPropertyChange(() => this.Datas);
        }
        #endregion

        #region dll
        public void ChoiceDllFile() {
            var dialog = new OpenFileDialog() {
                Filter = "DLL|*.dll",
                FileName = this.DllFilePath
            };

            if (dialog.ShowDialog() == true) {
                this.DllFilePath = dialog.FileName;
                this.NotifyOfPropertyChange(() => this.DllFilePath);
                this.LoadDll(dialog.FileName);
            }
        }

        private void LoadDll(string file) {
            try {
                var asm = Assembly.LoadFrom(file);
                this.EntityTypes = asm.GetTypes().ToList();
                this.NotifyOfPropertyChange(() => this.EntityTypes);
            } catch {
            }
        }

        public void LoadKeysFromDll() {

            this.Datas.RemoveRange(this.Datas.Where(d => !d.IsExists).ToList());

            this.SelectedEntity.GetProperties().ToList()
                .ForEach(p => {
                    var key = string.Format("{0}_{1}_DisplayName", this.SelectedEntity.FullName.Replace(".", ""), p.Name);
                    var exists = this.Datas.Any(d => d.Key.Equals(key));
                    if (!exists) {
                        dynamic o = Activator.CreateInstance(this.TmpType);
                        o.Key = key;
                        o.EntityProperty = p.Name;
                        this.Datas.Add(o);
                    }
                });

            this.NotifyOfPropertyChange(() => this.Datas);
        }
        #endregion


        #region save
        public void Save() {

            if (this.ResFiles == null || this.Datas == null)
                return;

            Dictionary<string, ResXResourceWriter> writers = new Dictionary<string, ResXResourceWriter>();
            foreach (var f in this.ResFiles) {
                writers.Add(f.Key, new ResXResourceWriter(f.Value));
            }

            foreach (var d in this.Datas) {

                if (string.IsNullOrEmpty(d.Key))
                    continue;

                var writer = writers.First(w => w.Key == "Default");
                writer.Value.AddResource(d.Key, (string)d.Default);

                foreach (var w in writers.Where(ww => !ww.Key.Equals("Default"))) {
                    var k = w.Key.Replace("-", "_");
                    PropertyInfo p = d.GetType().GetProperty(k);
                    if (p != null) {
                        var value = (string)p.GetValue(d);
                        w.Value.AddResource(d.Key, value);
                    }
                }

                d.IsExists = true;
            }

            foreach (var w in writers) {
                w.Value.Dispose();
            }
        }
        #endregion
    }
}
