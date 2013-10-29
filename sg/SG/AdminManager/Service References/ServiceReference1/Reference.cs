﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.17929
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdminManager.ServiceReference1 {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Desguace", Namespace="http://schemas.datacontract.org/2004/07/ManagerSystem")]
    [System.SerializableAttribute()]
    public partial class Desguace : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool activeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int idField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string nameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool active {
            get {
                return this.activeField;
            }
            set {
                if ((this.activeField.Equals(value) != true)) {
                    this.activeField = value;
                    this.RaisePropertyChanged("active");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int id {
            get {
                return this.idField;
            }
            set {
                if ((this.idField.Equals(value) != true)) {
                    this.idField = value;
                    this.RaisePropertyChanged("id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                if ((object.ReferenceEquals(this.nameField, value) != true)) {
                    this.nameField = value;
                    this.RaisePropertyChanged("name");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IGestionDesguace")]
    public interface IGestionDesguace {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGestionDesguace/addNewDesguace", ReplyAction="http://tempuri.org/IGestionDesguace/addNewDesguaceResponse")]
        int addNewDesguace(string nombre);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGestionDesguace/addNewDesguace", ReplyAction="http://tempuri.org/IGestionDesguace/addNewDesguaceResponse")]
        System.Threading.Tasks.Task<int> addNewDesguaceAsync(string nombre);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGestionDesguace/getById", ReplyAction="http://tempuri.org/IGestionDesguace/getByIdResponse")]
        AdminManager.ServiceReference1.Desguace getById(int desguaceId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGestionDesguace/getById", ReplyAction="http://tempuri.org/IGestionDesguace/getByIdResponse")]
        System.Threading.Tasks.Task<AdminManager.ServiceReference1.Desguace> getByIdAsync(int desguaceId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGestionDesguace/getAll", ReplyAction="http://tempuri.org/IGestionDesguace/getAllResponse")]
        AdminManager.ServiceReference1.Desguace[] getAll();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGestionDesguace/getAll", ReplyAction="http://tempuri.org/IGestionDesguace/getAllResponse")]
        System.Threading.Tasks.Task<AdminManager.ServiceReference1.Desguace[]> getAllAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGestionDesguace/activateDesguace", ReplyAction="http://tempuri.org/IGestionDesguace/activateDesguaceResponse")]
        int activateDesguace(int desguaceId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGestionDesguace/activateDesguace", ReplyAction="http://tempuri.org/IGestionDesguace/activateDesguaceResponse")]
        System.Threading.Tasks.Task<int> activateDesguaceAsync(int desguaceId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGestionDesguaceChannel : AdminManager.ServiceReference1.IGestionDesguace, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GestionDesguaceClient : System.ServiceModel.ClientBase<AdminManager.ServiceReference1.IGestionDesguace>, AdminManager.ServiceReference1.IGestionDesguace {
        
        public GestionDesguaceClient() {
        }
        
        public GestionDesguaceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public GestionDesguaceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GestionDesguaceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GestionDesguaceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int addNewDesguace(string nombre) {
            return base.Channel.addNewDesguace(nombre);
        }
        
        public System.Threading.Tasks.Task<int> addNewDesguaceAsync(string nombre) {
            return base.Channel.addNewDesguaceAsync(nombre);
        }
        
        public AdminManager.ServiceReference1.Desguace getById(int desguaceId) {
            return base.Channel.getById(desguaceId);
        }
        
        public System.Threading.Tasks.Task<AdminManager.ServiceReference1.Desguace> getByIdAsync(int desguaceId) {
            return base.Channel.getByIdAsync(desguaceId);
        }
        
        public AdminManager.ServiceReference1.Desguace[] getAll() {
            return base.Channel.getAll();
        }
        
        public System.Threading.Tasks.Task<AdminManager.ServiceReference1.Desguace[]> getAllAsync() {
            return base.Channel.getAllAsync();
        }
        
        public int activateDesguace(int desguaceId) {
            return base.Channel.activateDesguace(desguaceId);
        }
        
        public System.Threading.Tasks.Task<int> activateDesguaceAsync(int desguaceId) {
            return base.Channel.activateDesguaceAsync(desguaceId);
        }
    }
}
