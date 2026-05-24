// (c) Copyright 2011-2019 MiKeSoft, Michel Keijzers, All rights reserved

using System.Collections.Generic;

using PcgTools.Model.Common.Synth.Meta;
using PcgTools.Model.Common.Synth.OldParameters;
using PcgTools.Model.Common.Synth.PatchCombis;

namespace PcgTools.Model.MSpecific.Synth
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MTimbre : Timbre
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timbres"></param>
        /// <param name="index"></param>
        /// <param name="timbresSizeConstant"></param>
        protected MTimbre(ITimbres timbres, int index, int timbresSizeConstant)
            : base(timbres, index, timbresSizeConstant)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override IParameter GetParam(ParameterNames.TimbreParameterName name)
        {
            IParameter parameter;

            switch (name)
            {
                case ParameterNames.TimbreParameterName.Status:
                    parameter = new EnumParameter(
                        Root, Root.Content, TimbresOffset + 2, 7, 5,
                        new List<string> {"Off", "Int", "Ext", "Ex2"}, Parent as IPatch);
                    break;

                case ParameterNames.TimbreParameterName.Mute:
                    parameter = new BoolParameter(Root, Root.Content, TimbresOffset + 27, 7,
                        Parent as IPatch);
                    break;

                case ParameterNames.TimbreParameterName.TopKey:
                    parameter = new IntParameter(Root, Root.Content, TimbresOffset + 30, 7, 0, false,
                        Parent as IPatch);
                    break;

                case ParameterNames.TimbreParameterName.BottomKey:
                    parameter = new IntParameter(Root, Root.Content, TimbresOffset + 31, 7, 0, false,
                        Parent as IPatch);
                    break;

                case ParameterNames.TimbreParameterName.TopVelocity:
                    parameter = new IntParameter(Root, Root.Content, TimbresOffset + 33, 7, 0, false,
                        Parent as IPatch);
                    break;

                case ParameterNames.TimbreParameterName.BottomVelocity:
                    parameter = new IntParameter(Root, Root.Content, TimbresOffset + 34, 7, 0, false,
                        Parent as IPatch);
                    break;

                case ParameterNames.TimbreParameterName.OscMode:
                    parameter = new EnumParameter(
                        Root, Root.Content, TimbresOffset + 28, 1, 0,
                        new List<string> {"Prg", "Poly", "Mono", "Legato"}, Parent as IPatch);
                    break;

                case ParameterNames.TimbreParameterName.OscSelect:
                    parameter = new EnumParameter(
                        Root, Root.Content, TimbresOffset + 28, 3, 2,
                        new List<string> {"Both", "Osc1", "Osc2"}, Parent as IPatch);
                    break;

                case ParameterNames.TimbreParameterName.Portamento:
                    parameter = new IntParameter(Root, Root.Content, TimbresOffset + 29, 7, 0, true,
                        Parent as IPatch);
                    break;

                default:
                    parameter = base.GetParam(name);
                    break;
            }
            return parameter;
        }
    }
}
