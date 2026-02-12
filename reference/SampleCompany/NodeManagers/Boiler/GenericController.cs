#region Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using Opc.Ua;
using Range = Opc.Ua.Range;
#endregion Using Directives

namespace SampleCompany.NodeManagers.Boiler
{
    /// <summary>
    /// An object representing a generic controller.
    /// </summary>
    public partial class GenericControllerState
    {
        /// <summary>
        /// Updates the measurement and calculates the new control output.
        /// </summary>
        public double UpdateMeasurement(AnalogItemState<double> source)
        {
            Range range = source.EURange.Value;
            m_measurement.Value = source.Value;

            // clamp the setpoint.
            if (range != null)
            {
                if (m_setPoint.Value > range.High)
                {
                    m_setPoint.Value = range.High;
                }

                if (m_setPoint.Value < range.Low)
                {
                    m_setPoint.Value = range.Low;
                }
            }

            // calculate error.
            m_controlOut.Value = m_setPoint.Value - m_measurement.Value;

            if (range != null)
            {
                m_controlOut.Value /= range.Magnitude;

                if (Math.Abs(m_controlOut.Value) > 1.0)
                {
                    m_controlOut.Value = m_controlOut.Value < 0 ? -1.0 : +1.0;
                }
            }

            // return the new output.
            return m_controlOut.Value;
        }
    }
}
