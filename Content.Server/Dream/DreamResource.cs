﻿using Robust.Shared.Log;
using System;
using System.IO;
using System.Text;

namespace Content.Server.Dream {
    public class DreamResource {
        public string ResourcePath;
        public byte[] ResourceData {
            get {
                //if (_resourceData == null && File.Exists(_filePath)) {
                //    _resourceData = File.ReadAllBytes(_filePath);
                //}

                return _resourceData;
            }
        }

        private byte[] _resourceData = null;

        public DreamResource(string resourcePath) {
            ResourcePath = resourcePath;
        }

        public virtual string ReadAsString() {
            //if (!File.Exists(_filePath)) return null;

            //string resourceString = Encoding.ASCII.GetString(ResourceData);

            //resourceString = resourceString.Replace("\r\n", "\n");
            return null;//return resourceString;
        }

        public virtual void Clear() {
            CreateDirectory();
            //File.WriteAllText(_filePath, String.Empty);
        }

        public virtual void Output(DreamValue value) {
            /*if (value.TryGetValueAsString(out string text)) {
                string filePath = Path.Combine(Runtime.ResourceManager.RootPath, ResourcePath);

                CreateDirectory();
                File.AppendAllText(filePath, text + "\r\n");
                _resourceData = null;
            } else {
                throw new Exception("Invalid output operation on '" + ResourcePath + "'");
            }*/
        }

        private void CreateDirectory() {
            //Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
        }
    }

    //A special resource that outputs to the console
    //world.log defaults to this
    class ConsoleOutputResource : DreamResource {
        public ConsoleOutputResource() : base(null) { }

        public override string ReadAsString() {
            return null;
        }

        public override void Output(DreamValue value) {
            Logger.Info(value.Stringify());
        }
    }
}
