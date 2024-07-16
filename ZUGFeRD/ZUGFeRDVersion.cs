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
    /// Enumeration of the different ZUGFeRD versions supported by ZUGFeRD-csharp
    /// </summary>
    public enum ZUGFeRDVersion
    {
        /// <summary>
        /// Version 1.x - first public ZUGFeRD version
        /// </summary>
        Version1 = 100,
        
        /// <summary>
        /// Version 2.0 - second major ZUGFeRD version 
        /// </summary>
        Version20 = 200,

        /// <summary>
        /// Version 2.1 - unified with french factur-x 1.0, supports XRechnung
        /// </summary>
        [Obsolete("Will be removed in the next version")]
        Version21 = 210,

        /// <summary>
        /// Version 2.2 - unified with french factur-x 1.0, supports XRechnung
        /// </summary>
        Version22 = 220
    }
}
