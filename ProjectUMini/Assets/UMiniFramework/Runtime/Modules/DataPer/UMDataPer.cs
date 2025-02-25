using System.Collections;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Modules.Base;

namespace UMiniFramework.Runtime.Modules.UMDataPer
{
    public class UMDataPer : UMBaseModule
    {
        public override UMModuleType ModuleType
        {
            get => UMModuleType.UMDataPer;
        }

        protected override IEnumerator Init(UMModuleConfig config)
        {
            yield return null;
        }
    }
}