using System.ComponentModel;

namespace Models
{
    public class Enums
    {
        public enum MessageText
        {
            [Description("Error")] Error = 1,
            [Description("Unautorized")] Unautorized = 2
        }
    }
}