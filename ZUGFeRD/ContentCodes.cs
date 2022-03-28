/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 * 
 *   http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// Code for free text unstructured information about the invoice as a whole
    /// </summary>
    public enum ContentCodes
    {
        /// <summary>
        ///  Die Ware bleibt bis zur vollständigen Bezahlung
        ///  unser Eigentum. 
        /// </summary>
        EEV,

        /// <summary>
        /// Die Ware bleibt bis zur vollständigen Bezahlung
        /// aller Forderungen unser Eigentum. 
        /// </summary>
        WEV,

        /// <summary>
        /// Die Ware bleibt bis zur vollständigen Bezahlung
        /// unser Eigentum. Dies gilt auch im Falle der
        /// Weiterveräußerung oder -verarbeitung der Ware.
        /// </summary>
        VEV,

        /// <summary>
        /// Es ergeben sich Entgeltminderungen auf Grund von
        /// Rabatt- und Bonusvereinbarungen. 
        /// </summary>
        ST1,

        /// <summary>
        /// Entgeltminderungen ergeben sich aus unseren
        /// aktuellen Rahmen- und Konditionsvereinbarungen. 
        /// </summary>
        ST2,

        /// <summary>
        /// Es bestehen Rabatt- oder Bonusvereinbarungen.
        /// </summary>
        ST3,

        /// <summary>
        /// Unbekannter Wert
        /// </summary>
        Unknown
    }



    internal static class ContentCodesExtensions
    {
        public static ContentCodes FromString(this ContentCodes _, string s)
        {
            try
            {
                return (ContentCodes)Enum.Parse(typeof(ContentCodes), s);
            }
            catch
            {
                return ContentCodes.Unknown;
            }
        } // !FromString()


        public static string EnumToString(this ContentCodes c)
        {
            return c.ToString("g");
        } // !EnumToString()
    }
}
