﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Alayaz.SOA.IService.TestClient.RemoteServiceHostByWinServiceRef {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="RemoteServiceHostByWinServiceRef.ISyncImportInvoiceService")]
    public interface ISyncImportInvoiceService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISyncImportInvoiceService/InjectList", ReplyAction="http://tempuri.org/ISyncImportInvoiceService/InjectListResponse")]
        Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO InjectList(Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO soap);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ISyncImportInvoiceService/InjectList", ReplyAction="http://tempuri.org/ISyncImportInvoiceService/InjectListResponse")]
        System.IAsyncResult BeginInjectList(Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO soap, System.AsyncCallback callback, object asyncState);
        
        Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO EndInjectList(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISyncImportInvoiceService/FetchList", ReplyAction="http://tempuri.org/ISyncImportInvoiceService/FetchListResponse")]
        Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO FetchList(Alayaz.SOA.Service.ViewModel.ImportInvoiceDTO soap);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ISyncImportInvoiceService/FetchList", ReplyAction="http://tempuri.org/ISyncImportInvoiceService/FetchListResponse")]
        System.IAsyncResult BeginFetchList(Alayaz.SOA.Service.ViewModel.ImportInvoiceDTO soap, System.AsyncCallback callback, object asyncState);
        
        Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO EndFetchList(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISyncImportInvoiceServiceChannel : Alayaz.SOA.IService.TestClient.RemoteServiceHostByWinServiceRef.ISyncImportInvoiceService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class InjectListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public InjectListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FetchListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public FetchListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SyncImportInvoiceServiceClient : System.ServiceModel.ClientBase<Alayaz.SOA.IService.TestClient.RemoteServiceHostByWinServiceRef.ISyncImportInvoiceService>, Alayaz.SOA.IService.TestClient.RemoteServiceHostByWinServiceRef.ISyncImportInvoiceService {
        
        private BeginOperationDelegate onBeginInjectListDelegate;
        
        private EndOperationDelegate onEndInjectListDelegate;
        
        private System.Threading.SendOrPostCallback onInjectListCompletedDelegate;
        
        private BeginOperationDelegate onBeginFetchListDelegate;
        
        private EndOperationDelegate onEndFetchListDelegate;
        
        private System.Threading.SendOrPostCallback onFetchListCompletedDelegate;
        
        public SyncImportInvoiceServiceClient() {
        }
        
        public SyncImportInvoiceServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SyncImportInvoiceServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SyncImportInvoiceServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SyncImportInvoiceServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public event System.EventHandler<InjectListCompletedEventArgs> InjectListCompleted;
        
        public event System.EventHandler<FetchListCompletedEventArgs> FetchListCompleted;
        
        public Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO InjectList(Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO soap) {
            return base.Channel.InjectList(soap);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginInjectList(Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO soap, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginInjectList(soap, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO EndInjectList(System.IAsyncResult result) {
            return base.Channel.EndInjectList(result);
        }
        
        private System.IAsyncResult OnBeginInjectList(object[] inValues, System.AsyncCallback callback, object asyncState) {
            Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO soap = ((Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO)(inValues[0]));
            return this.BeginInjectList(soap, callback, asyncState);
        }
        
        private object[] OnEndInjectList(System.IAsyncResult result) {
            Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO retVal = this.EndInjectList(result);
            return new object[] {
                    retVal};
        }
        
        private void OnInjectListCompleted(object state) {
            if ((this.InjectListCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.InjectListCompleted(this, new InjectListCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void InjectListAsync(Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO soap) {
            this.InjectListAsync(soap, null);
        }
        
        public void InjectListAsync(Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO soap, object userState) {
            if ((this.onBeginInjectListDelegate == null)) {
                this.onBeginInjectListDelegate = new BeginOperationDelegate(this.OnBeginInjectList);
            }
            if ((this.onEndInjectListDelegate == null)) {
                this.onEndInjectListDelegate = new EndOperationDelegate(this.OnEndInjectList);
            }
            if ((this.onInjectListCompletedDelegate == null)) {
                this.onInjectListCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnInjectListCompleted);
            }
            base.InvokeAsync(this.onBeginInjectListDelegate, new object[] {
                        soap}, this.onEndInjectListDelegate, this.onInjectListCompletedDelegate, userState);
        }
        
        public Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO FetchList(Alayaz.SOA.Service.ViewModel.ImportInvoiceDTO soap) {
            return base.Channel.FetchList(soap);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginFetchList(Alayaz.SOA.Service.ViewModel.ImportInvoiceDTO soap, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginFetchList(soap, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO EndFetchList(System.IAsyncResult result) {
            return base.Channel.EndFetchList(result);
        }
        
        private System.IAsyncResult OnBeginFetchList(object[] inValues, System.AsyncCallback callback, object asyncState) {
            Alayaz.SOA.Service.ViewModel.ImportInvoiceDTO soap = ((Alayaz.SOA.Service.ViewModel.ImportInvoiceDTO)(inValues[0]));
            return this.BeginFetchList(soap, callback, asyncState);
        }
        
        private object[] OnEndFetchList(System.IAsyncResult result) {
            Alayaz.SOA.Service.ViewModel.ImportInvoiceListDTO retVal = this.EndFetchList(result);
            return new object[] {
                    retVal};
        }
        
        private void OnFetchListCompleted(object state) {
            if ((this.FetchListCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.FetchListCompleted(this, new FetchListCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void FetchListAsync(Alayaz.SOA.Service.ViewModel.ImportInvoiceDTO soap) {
            this.FetchListAsync(soap, null);
        }
        
        public void FetchListAsync(Alayaz.SOA.Service.ViewModel.ImportInvoiceDTO soap, object userState) {
            if ((this.onBeginFetchListDelegate == null)) {
                this.onBeginFetchListDelegate = new BeginOperationDelegate(this.OnBeginFetchList);
            }
            if ((this.onEndFetchListDelegate == null)) {
                this.onEndFetchListDelegate = new EndOperationDelegate(this.OnEndFetchList);
            }
            if ((this.onFetchListCompletedDelegate == null)) {
                this.onFetchListCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnFetchListCompleted);
            }
            base.InvokeAsync(this.onBeginFetchListDelegate, new object[] {
                        soap}, this.onEndFetchListDelegate, this.onFetchListCompletedDelegate, userState);
        }
    }
}
