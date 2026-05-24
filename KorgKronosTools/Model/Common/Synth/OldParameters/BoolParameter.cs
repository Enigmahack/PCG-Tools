// (c) Copyright 2011-2019 MiKeSoft, Michel Keijzers, All rights reserved

using System.Diagnostics;
using Common.Utils;
using PcgTools.Model.Common.Synth.MemoryAndFactory;
using PcgTools.Model.Common.Synth.Meta;

namespace PcgTools.Model.Common.Synth.OldParameters
{
    /// <summary>
    ///
    /// </summary>
    public class BoolParameter: Parameter
    {
        /// <summary>
        ///
        /// </summary>
        int _bit;


        /// <summary>
        ///
        /// </summary>
        public BoolParameter(IMemory memory, byte[] pcgData, int pcgOffset, int bit, IPatch patch)
        {
            Set(memory, pcgData, pcgOffset, patch);
            _bit = bit;
        }

        /// <summary>
        ///
        /// </summary>
        public override dynamic Value
        {
            get
            {
                return BitsUtil.GetBit(PcgData, PcgOffset, _bit);
            }

            set
            {
                Debug.Assert(PcgData != null);
                PcgMemory.IsDirty |= BitsUtil.SetBit(PcgData, PcgOffset, _bit, value);
                if (Patch != null)
                {
                    Patch.RaisePropertyChanged(string.Empty, false);
                }
            }
        }
    }
}
