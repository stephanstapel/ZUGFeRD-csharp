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
    /// In case that the values that should be written into the xml document contains any
    /// illegal character according to the xml 1.0 specifiction, this exception is thrown
    /// </summary>
    public class IllegalCharacterException : Exception
    {
        /// <summary>
        /// Initializes a new IllegalStreamException object
        /// </summary>
        /// <param name="message"></param>
        public IllegalCharacterException(string message = "")
            : base(message)
        {
        }
    }
}
