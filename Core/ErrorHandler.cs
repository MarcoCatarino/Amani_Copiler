using System.Collections.Generic;

namespace Core
{
    public class ErrorHandlers
    {
        public List<string> Errors { get; private set; } = new List<string>();
        public List<string> Warnings { get; private set; } = new List<string>();

        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public void AddWarning(string warning)
        {
            Warnings.Add(warning);
        }

        public bool HasErrors => Errors.Count > 0;

        public void Clear()
        {
            Errors.Clear();
            Warnings.Clear();
        }
    }
}
