using System;

namespace Puzzle.NAspect.Debug.Serialization.Elements
{
    /// <summary>
    /// DTO for the VS.NET 2005 debugger visualiser
    /// </summary>
    [Serializable]
    public class VizConstructor : VizMethodBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => string.Format(".ctor ({0})", GetParamTypes());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetCallSample() => string.Format("{0} My{2}Obj = engine.CreateProxy<{0}> ({1})", OwnerType.BaseName, GetParamTypes(),
                                                                OwnerType.BaseName);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetProxyText() => string.Format("Constructor {0} ({1})", OwnerType.Name, GetParamTypes());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetRealText() => string.Format("Constructor {0} ({1})", OwnerType.BaseName, GetParamTypes());
    }
}