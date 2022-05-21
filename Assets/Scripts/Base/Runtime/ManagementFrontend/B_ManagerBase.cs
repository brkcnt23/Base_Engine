using System.Threading.Tasks;
using UnityEngine;
namespace Base {
    public abstract class B_ManagerBase  {
        
        public virtual Task ManagerStrapping() {
            return Task.CompletedTask;
        }
        public virtual Task ManagerDataFlush() {
            return Task.CompletedTask;
        }
        
    }
}