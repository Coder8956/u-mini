using System;
using System.Collections;
using UMiniFramework.Runtime.Common;
using UMiniFramework.Runtime.Modules.Base;

namespace UMiniFramework.Runtime.Modules.Resource
{
    /// <summary>
    /// UI模块, 同时也是UI的根Canvas
    /// </summary>
    public class UMResource : UMBaseModule
    {
        public override UMModuleType ModuleType
        {
            get => UMModuleType.UI;
        }

        protected override IEnumerator Init(UMModuleInitArgs initArgs)
        {
            throw new NotImplementedException();
        }
    }
}