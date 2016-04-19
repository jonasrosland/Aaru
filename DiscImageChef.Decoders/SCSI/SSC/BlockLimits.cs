﻿// /***************************************************************************
// The Disc Image Chef
// ----------------------------------------------------------------------------
//
// Filename       : BlockLimits.cs
// Version        : 1.0
// Author(s)      : Natalia Portillo
//
// Component      : Component
//
// Revision       : $Revision$
// Last change by : $Author$
// Date           : $Date$
//
// --[ Description ] ----------------------------------------------------------
//
// Description
//
// --[ License ] --------------------------------------------------------------
//
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as
//     published by the Free Software Foundation, either version 3 of the
//     License, or (at your option) any later version.
//
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
//
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
// ----------------------------------------------------------------------------
// Copyright (C) 2011-2015 Claunia.com
// ****************************************************************************/
// //$Id$
using System;
using System.Text;

namespace DiscImageChef.Decoders.SCSI.SSC
{
    public static class BlockLimits
    {
        public struct BlockLimitsData
        {
            /// <summary>
            /// All blocks size must be multiple of 2^<cref name="granularity"/>
            /// </summary>
            public byte granularity;
            /// <summary>
            /// Maximum block length in bytes
            /// </summary>
            public uint maxBlockLen;
            /// <summary>
            /// Minimum block length in bytes
            /// </summary>
            public ushort minBlockLen;
        }

        public static BlockLimitsData? Decode(byte[] response)
        {
            if(response == null)
                return null;

            if(response.Length != 6)
                return null;

            BlockLimitsData dec = new BlockLimitsData();

            dec.granularity = (byte)(response[0] & 0x1F);
            dec.maxBlockLen = (uint)((response[1] << 16) + (response[2] << 8) + response[3]);
            dec.minBlockLen = (ushort)((response[4] << 8) + response[5]);

            return dec;
        }

        public static string Prettify(BlockLimitsData? decoded)
        {
            if(decoded == null)
                return null;

            StringBuilder sb = new StringBuilder();

            if(decoded.Value.maxBlockLen == decoded.Value.minBlockLen)
                sb.AppendFormat("Device's block size is fixed at {0} bytes", decoded.Value.minBlockLen).AppendLine();
            else
            {
                if(decoded.Value.maxBlockLen > 0)
                    sb.AppendFormat("Device's maximum block size is {0} bytes", decoded.Value.maxBlockLen).AppendLine();
                else
                    sb.AppendLine("Device does not specify a maximum block size");
                sb.AppendFormat("Device's minimum block size is {0} bytes", decoded.Value.minBlockLen).AppendLine();

                if(decoded.Value.granularity > 0)
                    sb.AppendFormat("Device's needs a block size granularity of 2^{0} ({1}) bytes", decoded.Value.granularity, Math.Pow(2, (double)decoded.Value.granularity)).AppendLine();
            }

            return sb.ToString();
        }

        public static string Prettify(byte[] response)
        {
            return Prettify(Decode(response));
        }
    }
}

