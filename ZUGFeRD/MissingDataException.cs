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
using System.Text;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// This exception is thrown when data is missing that is mandatory for a certain ZUGFeRD version or profile.
    /// </summary>
    public class MissingDataException : Exception
    {
        /// <summary>
        /// Initializes a new MissingDataException object
        /// </summary>
        /// <param name="message">Cleartext message of the exception containing clear text information about the reason for the exception</param>
        public MissingDataException(string message)
            : base(message)
        {
        } // !MissingDataException()
    }
}
