using System;

namespace ConductorAPI.Business.Preferences
{
    [Serializable]
    public class PreferenceException : Exception
    {
        public PreferenceException(string message, Exception innerException) : base(message, innerException) {}

        public PreferenceException(string message) : base(message) { }
    }
}