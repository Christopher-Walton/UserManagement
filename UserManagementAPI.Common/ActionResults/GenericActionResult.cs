using System.Collections.Generic;

namespace UserManagementAPI.Common.ActionResults
{
    public interface IGenericActionResult
    {
        bool IsSuccess { get; set; }
        IEnumerable<string> Errors { get; set; }
    }

    public class GenericActionResult<T> : IGenericActionResult
    {
        public GenericActionResult()
        {
        }

        public GenericActionResult(bool isSuccess, IEnumerable<string> errors, T Result)
        {
            this.IsSuccess = isSuccess;
            this.Errors = errors;
            this.Result = Result;
        }

        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public T Result { get; set; }
    }
}