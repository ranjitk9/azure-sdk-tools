﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Management.ServiceManagement.Helpers
{
    using System;
    using System.IO;
    using System.Xml.Serialization;
    using Microsoft.Samples.WindowsAzure.ServiceManagement;
    using Microsoft.WindowsAzure.Management.ServiceManagement.Model;

    public static class PersistentVMHelper
    {
        public static void SaveStateToFile(PersistentVM role, string filePath)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role", "Role cannot be null");
            }
            
            XmlAttributeOverrides overrides = new XmlAttributeOverrides();
            XmlAttributes ignoreAttrib = new XmlAttributes();
            ignoreAttrib.XmlIgnore = true;
            overrides.Add(typeof(Microsoft.Samples.WindowsAzure.ServiceManagement.DataVirtualHardDisk), "MediaLink", ignoreAttrib);
            overrides.Add(typeof(Microsoft.Samples.WindowsAzure.ServiceManagement.DataVirtualHardDisk), "SourceMediaLink", ignoreAttrib);
            overrides.Add(typeof(Microsoft.Samples.WindowsAzure.ServiceManagement.OSVirtualHardDisk), "MediaLink", ignoreAttrib);

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(PersistentVM), overrides, new Type[] { typeof(NetworkConfigurationSet) }, null, null);
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, role);
            }
        }

        public static PersistentVM LoadStateFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("The file to load the role does not exist", "filePath");
            }

            XmlAttributeOverrides overrides = new XmlAttributeOverrides();
            XmlAttributes ignoreAttrib = new XmlAttributes();
            ignoreAttrib.XmlIgnore = true;
            overrides.Add(typeof(Microsoft.Samples.WindowsAzure.ServiceManagement.DataVirtualHardDisk), "MediaLink", ignoreAttrib);
            overrides.Add(typeof(Microsoft.Samples.WindowsAzure.ServiceManagement.DataVirtualHardDisk), "SourceMediaLink", ignoreAttrib);
            overrides.Add(typeof(Microsoft.Samples.WindowsAzure.ServiceManagement.OSVirtualHardDisk), "MediaLink", ignoreAttrib);
            overrides.Add(typeof(Microsoft.Samples.WindowsAzure.ServiceManagement.OSVirtualHardDisk), "SourceImageName", ignoreAttrib);

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(PersistentVM), overrides, new Type[] { typeof(NetworkConfigurationSet) }, null, null);

            PersistentVM role = null;
            
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                role = serializer.Deserialize(stream) as PersistentVM;
            }

            return role;
        }
    }
}
