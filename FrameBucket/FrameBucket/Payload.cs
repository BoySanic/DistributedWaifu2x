using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FrameBucket
{
    class Payload
    {
        public string WorkstationName;
        public string Instruction;
        public byte[] Buffer;
        public int BufferSize;

        public Payload(string XMLString)
        {
            XmlSerializer xml = new XmlSerializer(typeof(Payload));
            using (TextReader reader = new StringReader(XMLString))
            {
                var p = (Payload)xml.Deserialize(reader);
                this.WorkstationName = p.WorkstationName;
                this.Instruction = p.Instruction;
                this.Buffer = p.Buffer;
                this.BufferSize = p.BufferSize;
            }
        }
    }
}
