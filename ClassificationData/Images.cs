using ClassificationData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationData
{
    /// <summary>
    /// DB operations relating to the Images table
    /// </summary>
    public class Images
    {
        /// <summary>
        /// Find the name of the text file that should be used to hold the image description
        /// </summary>
        /// <param name="imageFile">Path to the image file.</param>
        /// <returns>the path to the text descriptor file</returns>
        public static string ImageFileDescriptorPath(string imageFile)
        {
            string folderName = Path.GetDirectoryName(imageFile) ?? "";
            string fileRoot = Path.GetFileNameWithoutExtension(imageFile);
            string newFileName = $"{fileRoot}_AIDescription.txt";
            return Path.Combine(folderName, newFileName);
        }

        public static void UpsertImage(string imagePath, string description, DateTime imageDateTime, DateTime descriptionDateTime, string modelUsed, int classificationMs)
        {
            using (var context = new PhotoClassificationContext())
            {
                Image? image = context?.Images?.Where(i => i.ImagePath == imagePath)?.FirstOrDefault();
                if (image == null)
                {
                    image = new Image
                    {
                        ImagePath = imagePath
                    };
                    context?.Images?.Add(image);
                }
                image.Description = description;
                image.SetImageDate(imageDateTime);
                image.SetDescriptionDate(descriptionDateTime);
                image.ModelUsed = modelUsed;
                image.ClassificationMs = classificationMs;

                context?.SaveChanges();
            }
        }
        /// <summary>
        /// Performs image search for images descriptions containing ALL of the provided terms, made async to ensure it is not called from the main UI thread
        /// </summary>
        public static async Task<Image[]> SearchImages(string[] searchTerms)
        {
            using (var context = new PhotoClassificationContext())
            {
                //Search for all images that contain all of our search terms
                // Need to add search for each word as 
                IQueryable<Models.Image>? query = context?.Images?.AsQueryable();

                foreach (string term in searchTerms)
                {
                    query = query?.Where(i => i.Description.ToLower().Contains(term.ToLower()));
                }

                if (query == null) return [];
                return await query!.ToArrayAsync();
            }
        }
        /// <summary>
        /// Returns the total number of images and the time it took to classify them
        /// </summary>
        public static async Task<ImagesSummary> GetTotalImagesAndTime()
        {
            using (var context = new PhotoClassificationContext())
            {
                var result = await context.Images!.GroupBy(i => 1)
                    .Select(g => new { Count = g.Count(), SumDuration = g.Sum(i => (long)i.ClassificationMs)})
                    .FirstOrDefaultAsync();

                return new ImagesSummary() { Count = result?.Count ?? 0, SumDuration = result?.SumDuration ?? 0};
            }
        }

        /// <summary>
        /// Removes all image classification files and deletes associated row form the images table in the database
        /// <param name="progress">Callback to inform progress (decimal progress), return false to stop processing.
        /// </summary>
        public static async Task ClearAllClassificaitons(Func<decimal, bool> progress)
        {
            using (var context = new PhotoClassificationContext())
            {
                var localImages = context.Images;
                if (localImages == null) return;
                int totalImages = localImages.Count();
                int curImage = 0;
                foreach (Image image in localImages)
                {
                    if (File.Exists(Images.ImageFileDescriptorPath(image.ImagePath)))
                    {
                        File.Delete(Images.ImageFileDescriptorPath(image.ImagePath));
                    }
                    context?.Images?.Remove(image);
                    await context!.SaveChangesAsync();
                    curImage++;
                    if (!progress((decimal)curImage / (decimal)totalImages)) return;
                }
            }
        }

    }
    public class ImagesSummary 
    {
        public int Count { get; set; } = 0;
        public long SumDuration { get; set; } = 0;
    }
}
