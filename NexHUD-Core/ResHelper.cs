using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace NexHUDCore
{
    public class ResHelper
    {
        /* from https://stackoverflow.com/questions/7758766/how-to-get-a-stream-object-from-a-resource-file-console-app-windows-service-pro */

        /// <summary>
        /// Enum to indicate what type of file a resource is.
        /// </summary>
        public enum FileType
        {
            /// <summary>
            /// The resource is an image.
            /// </summary>
            Image,

            /// <summary>
            /// The resource is something other than an image or text file.
            /// </summary>
            Other,

            /// <summary>
            /// The resource is a text file.
            /// </summary>
            Text,
        }

        internal static Bitmap GetResourceImage(string dotFilePath)
        {
            return GetResource(Assembly.GetExecutingAssembly(), dotFilePath, FileType.Image) as Bitmap;
        }
        internal static string GetResourceText(string dotFilePath)
        {
            return GetResource(Assembly.GetExecutingAssembly(), dotFilePath, FileType.Text) as string;
        }
        public static Bitmap GetResourceImage(Assembly _assembly, string dotFilePath)
        {
            return GetResource(_assembly, dotFilePath, FileType.Image) as Bitmap;
        }
        public static string GetResourceText(Assembly _assembly, string dotFilePath)
        {
            return GetResource(_assembly, dotFilePath, FileType.Text) as string;
        }
        /// <summary>
        /// This method allows you to specify the dot file path and type of the resource file and return it in its native format.
        /// </summary>
        /// <param name="dotFilePath">The file path with dots instead of backslashes. e.g. Images.zombie.gif instead of Images\zombie.gif</param>
        /// <param name="fileType">The type of file the resource is.</param>
        /// <returns>Returns the resource in its native format.</returns>
        public static dynamic GetResource(Assembly _assembly, string dotFilePath, FileType fileType)
        {
            try
            {
                //var assembly = Assembly.GetExecutingAssembly();
                var assemblyName = _assembly.GetName().Name;
                var stream = _assembly.GetManifestResourceStream(assemblyName + "." + dotFilePath);
                switch (fileType)
                {
                    case FileType.Image:
                        return new Bitmap(stream);
                    case FileType.Text:
                        return new StreamReader(stream).ReadToEnd();
                    default:
                        return stream;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// This method allows you to specify the dot file path and type of the resource file and return it in its native format.
        /// </summary>
        /// <param name="dotFilePath">The file path with dots instead of backslashes. e.g. Images.zombie.gif instead of Images\zombie.gif</param>
        /// <param name="fileType">The type of file the resource is.</param>
        /// <param name="useNativeFormat">Indicates that the resource is to be returned as resource's native format or as a stream.</param>
        /// <returns>When "useNativeFormat" is true, returns the resource in its native format. Otherwise it returns the resource as a stream.</returns>
        public static dynamic GetResource(Assembly _assembly, string dotFilePath, FileType fileType, bool useNativeFormat)
        {
            try
            {
                if (useNativeFormat)
                {
                    return GetResource(_assembly, dotFilePath, fileType);
                }

                var assembly = Assembly.GetExecutingAssembly();
                var assemblyName = assembly.GetName().Name;
                return assembly.GetManifestResourceStream(assemblyName + "." + dotFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
