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
    /// http://www.stylusstudio.com/edifact/D02A/4451.htm
    /// </summary>
    public enum SubjectCodes
    {
        /// <summary>
        /// Generelle Informationen
        /// </summary>
        /// Generelle Informationen zu diesem Kauf
        AAI,
        /// <summary>
        /// Zusätzliche Konditionen zu diesem Kauf
        /// 
        /// Angaben zum Eigentumsvorbehalt
        /// </summary>
        AAJ,
        
        /// <summary>
        /// Buchhaltungsinformationen
        /// </summary>
        /// Informationen für die Buchaltung zu diesem Kauf
        ABN,

        /// <summary>
        /// Preiskonditionen
        /// 
        /// Angaben zu Entgeltminderungen
        /// </summary>
        AAK,
        
        /// <summary>
        /// Zusätzliche Angaben
        /// </summary>
        /// Zusaätzliche Angaben zu diesem Kauf
        ACB,

        /// <summary>
        /// Text subject is note.
        /// </summary>
        ADU,

        /// <summary>
        /// Zahlungsinformation
        /// 
        /// Bekanntgabe der Abtretung der
        /// Forderung (Zession)
        /// </summary>
        PMT,

        /// <summary>
        /// Preiskalkulationsschema
        /// 
        /// Zum Beispiel Angabe Zählerstand,
        /// Zähler etc. oder andere Hinweise
        /// bezüglich Abrechnung.
        /// </summary>
        PRF,

        /// <summary>
        /// Regulatorische Informationen
        /// 
        /// Angaben zum leistenden Unternehmen
        /// (Angabe Geschäftsführer, HR-Nummer
        /// etc.)
        /// </summary>
        REG,

        /// <summary>
        /// Supplier remarks
        /// Remarks from or for a supplier of goods or services.
        /// </summary>
        SUR,

        /// <summary>
        /// Unknon/ invalid subject code
        /// </summary>
        Unknown
    }



    internal static class SubjectCodesExtensions
    {
        public static SubjectCodes FromString(this SubjectCodes _, string s)
        {
            try
            {
                return (SubjectCodes)Enum.Parse(typeof(SubjectCodes), s);
            }
            catch
            {
                return SubjectCodes.Unknown;
            }
        } // !FromString()


        public static string EnumToString(this SubjectCodes codes)
        {
            return codes.ToString("g");
        } // !ToString()
    }
}
