using UnityEngine;
using System.IO;
using System.Text;

public static class FileUtils {

    public static string SAVE_PATH = Application.streamingAssetsPath;

    public static MemoryStream ReadAllBytes(string path) {
        if (!File.Exists(path)) {
            Debug.LogWarning("File don't exist!!!");
            return null;
        }

        FileStream fileStream = File.OpenRead(path);
        if (fileStream == null) {
            Debug.LogWarning("Read Failed!!!");
            return null;
        }

        MemoryStream memoryStream = new MemoryStream();
        InternalCopyTo(fileStream, memoryStream, fileStream.Length);
        memoryStream.Position = 0;

        fileStream.Close();
        fileStream.Dispose();

        if (fileStream == null) {
            Debug.LogWarning("WTF!!!");
        }

        return memoryStream;
    }

    public static void WriteAllBytes(MemoryStream stream, string path) {
        string directoryPath = Path.GetDirectoryName(path);
        if (!Directory.Exists(directoryPath)) {
            Directory.CreateDirectory(directoryPath);
        }
        DeleteFile(path);
        byte[] bytes = stream.ToArray();
        stream.Close();
        stream.Dispose();
        File.WriteAllBytes(path, bytes);
    }

    public static void DeleteFile(string path) {
        if (!File.Exists(path)) {
            return;
        }
        File.Delete(path);
    }

    public static bool ContainsDataFile() {
        return File.Exists(CombinePath(Application.persistentDataPath, GameData.SAVE_DATA));
    }

    /// <summary>
    /// 拼接Path
    /// </summary>
    /// <param name="paths"></param>
    /// <returns></returns>
    public static string CombinePath(params string[] paths) {
        if (paths == null || paths.Length == 0) {
            return string.Empty;
        } else {
            string firstPath = paths[0].Replace("\\", "/");
            StringBuilder builder = new StringBuilder();
            string spliter = "/";
            if (!firstPath.EndsWith(spliter)) {
                firstPath = firstPath + spliter;
            }
            builder.Append(firstPath);
            for (int i = 1; i < paths.Length; i++) {
                string nextPath = paths[i].Replace("\\", "/");
                if (nextPath.StartsWith("/")) {
                    nextPath = nextPath.Substring(1);
                }
                if (i != paths.Length - 1)//not the last one
                {
                    if (nextPath.EndsWith("/")) {
                        nextPath = nextPath.Substring(0, nextPath.Length - 1) + spliter;
                    } else {
                        nextPath = nextPath + spliter;
                    }
                }
                builder.Append(nextPath);
            }
            return builder.ToString();
        }
    }

    /// <summary>
    /// 深度拷贝
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <param name="bufferSize"></param>
    public static void InternalCopyTo(Stream source, Stream destination, long bufferSize) {
        byte[] array = new byte[bufferSize];
        int count;
        while ((count = source.Read(array, 0, array.Length)) != 0) {
            destination.Write(array, 0, count);
        }
    }
}
