using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClassificationAPI
{
    /// <summary>
    /// Handles the HTTP requests to LMStudio
    /// </summary>
    public class LMStudio
    {
        private static string GenerateClassificationRequestBody(string imageFile, string modelName, string classificationQuestion)
        {
            string codedImage = HttpUtils.ConvertImageToBase64(imageFile);
            string json = "{" +
                $"'model': '{modelName}'," +
                "'messages': [" +
                  "{ " +
                    "'role': 'user'," +
                    "'content': [" +
                      "{" +
                        "'type': 'text'," +
                        $"'text': '{classificationQuestion}'" +
                      "}," +
                      "{" +
                        "'type': 'image_url'," +
                        $"'image_url': {{ 'url': 'data:image/png;base64,{codedImage}' }}" +
                      "}" +
                    "]" +
                  "}" +
                "]," +
                "'temperature': 0.7," +
                "'max_tokens': -1," +
                "'stream': false" +
            "}";
            return json.Replace("'", "\"");
        }

        public static async Task<ClassificationEventArgs> ClassifyImage(string apiLocation, string modelName, string classificationQuestion, string imageFile, int timeoutMS)
        {
            PostResponse pr = await HttpUtils.PostRequest(imageFile, 
                $"{apiLocation}/v1/chat/completions",
                LMStudio.GenerateClassificationRequestBody(imageFile, modelName, classificationQuestion),
                timeoutMS);

            if (pr.status != HttpStatusCode.OK)
            {
                // Error occurred, don't try to parse anything, just return
                return new ClassificationEventArgs(imageFile, pr?.status ?? HttpStatusCode.InternalServerError, $"ERROR: {pr?.content ?? ""}");
            }
            try
            {                
                ClassificationResult? cl = JsonSerializer.Deserialize<ClassificationResult>(pr?.content ?? "");
                if (cl?.choices != null && cl?.choices.Length > 0 && cl.choices[0]?.message?.content != null)
                {
                    return new ClassificationEventArgs(imageFile, pr?.status ?? HttpStatusCode.InternalServerError, cl?.choices[0]?.message?.content ?? "");
                }
                else
                {
                    return new ClassificationEventArgs(imageFile, pr?.status ?? HttpStatusCode.InternalServerError, "Classification Failed");
                }
            }
            catch (Exception ex)
            {
                return new ClassificationEventArgs(imageFile, pr?.status ?? HttpStatusCode.InternalServerError, $"ERROR: {ex.ToString()}");
            }

        }

        public static async Task<ModelsEventArgs> GetModelList(string apiLocation)
        {
            GetResponse gr = await HttpUtils.GetRequest($"{apiLocation}/v1/models/");

            if (gr == null) return new ModelsEventArgs(HttpStatusCode.InternalServerError, []); ;

            ModelList? ml = JsonSerializer.Deserialize<ModelList>(gr?.content ?? "");

            if (ml == null) return new ModelsEventArgs(HttpStatusCode.InternalServerError, []);

            string[] modelNames = [];
            if (ml?.data != null)
            {
                modelNames = ml.data.Select(d => d.modelId ?? "").ToArray();
            }
            return new ModelsEventArgs(gr?.status ?? HttpStatusCode.InternalServerError, modelNames);
        }

    }
    public class ClassificationEventArgs : EventArgs
    {
        public HttpStatusCode resultCode { get; set; }
        public string description { get; set; }
        public string imageName { get; set; }

        public ClassificationEventArgs(string image, HttpStatusCode res, string desc)
        {
            resultCode = res;
            description = desc;
            imageName = image;
        }
    }
    public class ModelsEventArgs : EventArgs
    {
        public HttpStatusCode resultCode { get; set; }
        public string[] models { get; set; }
        public ModelsEventArgs(HttpStatusCode res, string[] mods)
        {
            resultCode = res;
            models = mods;
        }
    }
    public class ModelList
    {
        public ModelData[]? data { get; set; }
    }
    public class ModelData
    {
        [JsonPropertyName("id")] 
        public string? modelId { get; set; }
        [JsonPropertyName("object")]
        public string? modelObject { get; set; }
        public string? owned_by { get; set; }
    }
    public class Usage
    {
        public int? prompt_tokens { get; set; }
        public int? completion_tokens { get; set; }
        public int? total_tokens { get; set; }
    }
    public class Message
    {
        public string? assistant { get; set; }
        public string? content { get; set; }
    }
    public class Choice
    {
        public int? index { get; set; }
        public string? logprobs { get; set; }
        public string? finish_reason { get; set; }
        public Message? message { get; set; }
    }
    public class ClassificationResult
    {
        [JsonPropertyName("id")]
        public string? requestId { get; set; }
        [JsonPropertyName("object")]
        public string? created { get; set; }
        public string? model { get; set; }
        public Choice[]? choices { get; set; }
        public Usage? usage { get; set; }
        public string? system_fingerprint { get; set; }
    }
}
