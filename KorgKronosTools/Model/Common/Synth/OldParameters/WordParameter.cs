// (c) Copyright 2011-2019 MiKeSoft, Michel Keijzers, All rights reserved

using System.Diagnostics;
using PcgTools.Model.Common.Synth.MemoryAndFactory;
using PcgTools.Model.Common.Synth.Meta;

namespace PcgTools.Model.Common.Synth.OldParameters
{
    /// <summary>
    ///
    /// </summary>
    public class WordParameter : Parameter
    {
        /// <summary>
        /// True if Little endian, false if Big endian.
        /// </summary>
        bool _reverseOrder;


        /// <summary>
        ///
        /// </summary>
        private int _multiplication;


        /// <summary>
        ///
        /// </summary>
        public WordParameter(IMemory memory, byte[] pcgData, int pcgOffset, bool reverseOrder, int multiplication, IPatch patch)
        {
            Set(memory, pcgData, pcgOffset, patch);
            _multiplication = multiplication;
            _reverseOrder = reverseOrder;
        }


        /// <summary>
        /// Order is low byte - high byte.
        /// </summary>
        public override dynamic Value
        {
            get
            {
                int value = _reverseOrder
                    ? (PcgData[PcgOffset + 1] * 256 + PcgData[PcgOffset])
                    : (PcgData[PcgOffset] * 256 + PcgData[PcgOffset + 1]);

                return value / _multiplication;
            }

            set
            {
                Debug.Assert(PcgData != null);
                var val = (int) (value * _multiplication);

                if (_reverseOrder)
                {
                    PcgMemory.IsDirty |= (val != PcgData[PcgOffset] + PcgData[PcgOffset + 1] * 256);
                    PcgData[PcgOffset] = (byte) (val % 256);
                    PcgData[PcgOffset + 1] = (byte) (val / 256);
                }
                else
                {
                    PcgMemory.IsDirty |= (val != PcgData[PcgOffset] * 256 + PcgData[PcgOffset + 1]);
                    PcgData[PcgOffset] = (byte) (val / 256);
                    PcgData[PcgOffset + 1] = (byte) (val % 256);
                }
            }
        }
    }
}
