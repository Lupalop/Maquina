﻿using System.Xml.Serialization;

namespace Maquina.Resources
{
    [XmlRoot("manifest")]
    public class StringManifest : IManifest
    {
        [XmlAttribute("id")]
        public string Id
        {
            get { return "strings"; }
            set { /* Ignore given ID */ }
        }
        private int _actualRevision;
        [XmlAttribute("revision")]
        public int Revision
        {
            get { return 0; }
            set { _actualRevision = value; }
        }

        [XmlElement("string")]
        public Property<string>[] StringPropertySet { get; set; }
    }
}
