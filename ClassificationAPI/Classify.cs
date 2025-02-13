﻿using ClassificationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace ClassificationAPI
{
    /// <summary>
    /// Coordinates image classification tasks (e.g. classify all images in a folder)
    /// </summary>
    public class Classify
    {

        #pragma warning disable CA1416 // Validate platform compatibility
        /// <summary>
        /// Returns the date taken property of the image path provided or the last write time if that can't be obtained
        /// </summary>
        private static DateTime GetDataTaken(string imagePath)
        {
            if (!File.Exists(imagePath)) return DateTime.MinValue;

            using (Image i = System.Drawing.Image.FromFile(imagePath))
            {
                {
                    DateTime? dateTaken = null;

                    PropertyItem? prop = null;
                    try
                    {
                        prop = i.GetPropertyItem(36867); // The magic number that returns the date taken field
                    }
                    catch
                    {
                        prop = null;
                    }

                    if (prop != null)
                    {
                        // Convert the value to a readable DateTime format
                        string dateTakenStr = System.Text.Encoding.ASCII.GetString(prop?.Value ?? []).Trim();

                        if (dateTakenStr.Length > 19)
                        {
                            dateTakenStr = dateTakenStr.Substring(0, 19);
                        }

                        if (DateTime.TryParseExact(dateTakenStr, "yyyy:MM:dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDT))
                        {
                            dateTaken = parsedDT;
                        }
                    }
                    if (dateTaken == null)
                    {
                        // No Date Taken property found, just find the last write time instead
                        FileInfo imageInfo = new FileInfo(imagePath);
                        dateTaken = imageInfo.LastWriteTime;
                    }

                    return dateTaken ?? DateTime.MinValue;
                }
            }
        }

        /// <summary>
        /// Classify all images in the given folder with the provided model name
        /// </summary>
        /// <param name="folderPath">Path to classify.</param>
        /// <param name="apiAddress">URL of AI API (e.g. LM Studio).</param>
        /// <param name="modelName">LLM to use for classification.</param>
        /// <param name="recursive">Whether to search subdirectories.</param>
        /// <param name="replace">If true, existing image descriptions are overwritted, otherwise images with an existing description will be skipped.</param>
        /// <param name="progress">Callback to inform progress (decimal progress, string fileName, string status), return false to stop processing. If progress is negative, status is the last classification returned and the value of process is the amount of ms elapsed to classify it.</param>
        /// <returns>Error string or null if classification run was successful</returns>
        public static async Task ClassifyFolderImages(string folderPath, string apiAddress, string modelName, string classificationQuestion, bool recursive, bool replace, int timeoutMS, Func<ClassifyProgress, bool> progress)
        {
            if (!progress(new ClassifyProgress() { 
                Status = null, 
                Description = "Searching for image files",
                ElapsedMS = 0, 
                ImagePath = "",
                Progress = 0 })) return;

            string[] fileTypes = ["*.jpg", "*.jpeg", "*.tif", "*.tiff", "*.png"];
            string[] fileList = fileTypes.SelectMany(ft => Directory.GetFiles(folderPath, ft, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)).ToArray();

            int curFileIndex = 0;
            foreach (string file in fileList)
            {
                string descriptorFile = Images.ImageFileDescriptorPath(file);
                if (replace || !File.Exists(descriptorFile))
                {
                    if (!progress(new ClassifyProgress() {
                        Status = null,
                        Description = "Classifying file",
                        ElapsedMS = 0,
                        ImagePath = file,
                        Progress = (double)curFileIndex / (double)fileList.Length })) return;

                    DateTime startClass = DateTime.Now;
                    ClassificationEventArgs classRes = await LMStudio.ClassifyImage(apiAddress, modelName, classificationQuestion, file, timeoutMS);
                    TimeSpan elapsed = DateTime.Now - startClass;
                    if (classRes.resultCode == System.Net.HttpStatusCode.OK)
                    {
                        File.WriteAllText(descriptorFile, classRes.description);
                        ClassificationData.Images.UpsertImage(file, classRes.description, GetDataTaken(file), DateTime.Now, modelName, (int)elapsed.TotalMilliseconds);
                        if (!progress(new ClassifyProgress() { 
                            Status = classRes.resultCode, 
                            Description = classRes.description, 
                            ElapsedMS = elapsed.TotalMilliseconds, 
                            ImagePath = file,
                            Progress = 1 })) return;
                    }
                    else 
                    {
                        // Error occurred during classification, so fire progress with error info to see if caller wants to abort
                        if (!progress(new ClassifyProgress() { 
                            Status = classRes.resultCode, 
                            Description = classRes.description, 
                            ElapsedMS = elapsed.TotalMilliseconds, 
                            ImagePath = file,
                            Progress = 1 })) return;
                    }
                }
                curFileIndex++;
            }
        }

        public class ClassifyProgress
        {
            public System.Net.HttpStatusCode? Status { get; set; } = System.Net.HttpStatusCode.OK;
            public double Progress { get; set; } = 0;
            public double ElapsedMS { get; set; } = 0;
            public string ImagePath { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;

        }
    }
}
