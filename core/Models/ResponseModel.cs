using System.Collections.Generic;

namespace core.Models
{
    public class ResponseModel<TResult>
    {
        public ResponseModel()
        {
            Errors = new Dictionary<string, List<string>>();
        }
        public ResponseModel(bool success, int statusCode, TResult result)
        {
            Success = success;
            StatusCode = statusCode;
            Result = result;
            Errors = new Dictionary<string, List<string>>();
        }

        public ResponseModel(bool success, int statusCode, TResult result, Dictionary<string, List<string>> notifications)
        {
            Success = success;
            StatusCode = statusCode;
            Result = result;
            Errors = notifications;

            if (Errors is null)
                Errors = new Dictionary<string, List<string>>();
        }

        public int StatusCode { get; private set; } = 200;
        public TResult Result { get; private set; }
        public bool Success { get; private set; }
        public Dictionary<string, List<string>> Errors { get; private set; }

        public ResponseModel<TResult> SetSuccess(bool success, TResult result, int statusCode)
        {
            StatusCode = statusCode;
            Result = result;
            Success = success;
            return this;
        }
        public ResponseModel<TResult> AddErrors(string key, List<string> messages)
        {
            if (messages is null || string.IsNullOrWhiteSpace(key)) return this;

            if (Errors.ContainsKey(key))
            {
                foreach (var message in messages)
                    Errors[key].Add(message);

                return this;
            }

            Errors.Add(key, messages);

            return this;
        }
        public ResponseModel<TResult> AddErrors(Dictionary<string, string> notifications)
        {
            if (notifications is null) return this;

            foreach (var notification in notifications)
            {
                if (Errors.ContainsKey(notification.Key))
                {
                    Errors[notification.Key].Add(notification.Value);
                    continue;
                }

                Errors.Add(notification.Key, new List<string>() { notification.Value });
            }

            return this;
        }
    }

}
