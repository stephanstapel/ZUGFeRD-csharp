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
using System.IO;

namespace s2industries.ZUGFeRD
{
    public abstract class IInvoiceDescriptorReader
    {
        public abstract InvoiceDescriptor Load(Stream stream);        
        public abstract bool IsReadableByThisReaderVersion(Stream stream);


        public InvoiceDescriptor Load(string filename)
        {
            if (!System.IO.File.Exists(filename))
            {
                throw new FileNotFoundException();
            }

            return Load(new FileStream(filename, FileMode.Open, FileAccess.Read));
        } // !Load()


        public bool IsReadableByThisReaderVersion(string filename)
        {
            if (!System.IO.File.Exists(filename))
            {
                throw new FileNotFoundException();
            }

            return IsReadableByThisReaderVersion(new FileStream(filename, FileMode.Open, FileAccess.Read));
        } // !IsReadableByThisReaderVersion()
    }
}