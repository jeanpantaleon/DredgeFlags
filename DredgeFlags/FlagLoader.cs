using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Winch.Core;
using Winch.Serialization;
using Winch.Util;

namespace DredgeFlags
{
    internal class FlagLoader
    {
        internal static void LoadAssets()
        {
            WinchCore.Log.Debug("Loading flags...");

            string[] modDirs = Directory.GetDirectories("Mods");
            foreach (string modDir in modDirs)
            {
                string assetFolderPath = Path.Combine(modDir, "Assets");
                if (!Directory.Exists(assetFolderPath))
                    continue;
                string itemFolderPath = Path.Combine(assetFolderPath, "Flags");

                if (Directory.Exists(itemFolderPath)) LoadFlags(itemFolderPath);
            }
        }

        private static void LoadFlags(string flagFolderPath)
        {
            string[] itemFiles = Directory.GetFiles(flagFolderPath);
            foreach (string file in itemFiles)
            {
                try
                {
                    WinchCore.Log.Debug($"Loading flag {file}");
                    AddFlagFromMeta(file);
                }
                catch (Exception ex)
                {
                    WinchCore.Log.Error($"Failed to load flag from {file}: {ex}");
                }
            }
        }

        internal static void AddFlagFromMeta(string metaPath)
        {
            var meta = UtilHelpers.ParseMeta(metaPath);
            if (meta == null)
            {
                WinchCore.Log.Error($"Meta file {metaPath} is empty");
                return;
            }

            Flag flag = UtilHelpers.GetScriptableObjectFromMeta<Flag>(meta, metaPath);
            WinchCore.Log.Info(flag);
            if (PopulateObjectFromMeta(flag, meta))
            {
                DredgeFlags.Instance.flagList.Add(flag);
            }

            WinchCore.Log.Debug("Flag loaded !");
        }

        public static bool PopulateObjectFromMeta(Flag item, Dictionary<string, object> meta)
        {
            if (item == null) throw new ArgumentNullException($"{nameof(item)} is null");

            try
            {
                new FlagDataConverter().PopulateFields(item, meta);
            }
            catch (Exception e)
            {
                WinchCore.Log.Error(e);
                return false;
            }
            return true;
        }

        public class FlagDataConverter : DredgeTypeConverter<Flag>
        {
            private readonly Dictionary<string, FieldDefinition> _definitions = new()
            {
                { "author", new("Unknown", o=> o.ToString()) },
                { "name", new("Flag", o=> o.ToString()) },
                { "texture", new(null, o=> TextureUtil.GetTexture(o.ToString())) }
            };

            public FlagDataConverter()
            {
                AddDefinitions(_definitions);
            }
        }
    }
}
