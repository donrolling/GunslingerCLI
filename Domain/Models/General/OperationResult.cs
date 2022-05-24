using Models.Enums;

namespace Models
{
    public class OperationResult<T> : OperationResult
    {
        /// <summary>
        /// Contains the results of a successful call.  Can be null
        /// </summary>
        public T Result { get; set; }

        public OperationResult() { }

        public OperationResult(OperationResult copyFrom, T result)
        {
            foreach (var propInf in copyFrom.GetType().GetProperties())
            {
                propInf.SetValue(this, propInf.GetValue(copyFrom));
            }
            Result = result;
        }

        public OperationResult(OperationResult copyFrom)
        {
            foreach (var propInf in copyFrom.GetType().GetProperties())
            {
                propInf.SetValue(this, propInf.GetValue(copyFrom));
            }
        }
    }

    public class OperationResult
    {
        public bool Failed
        {
            get => _failure;
            set
            {
                _failure = value;
                //this is here to make the success and failure values agree with one another
                _success = !_failure;
                //this is here to prevent the status to disagree with success and failure settings
                if (_failure && _status == Status.Success)
                {
                    _status = Status.Failed;
                }
            }
        }

        public string Message { get; set; }

        public Status Status
        {
            get => _status;
            set => _status = value;
        }

        public bool Success
        {
            get => _success;
            set
            {
                _success = value;
                //this is here to make the success and failure values agree with one another
                _failure = !_success;
                //this is here to prevent the status to disagree with success and failure settings
                if (!_success && _status == Status.Success)
                {
                    _status = Status.Failed;
                }
            }
        }

        private bool _failure;
        private Status _status = Status.Success;
        private bool _success = true;

        public OperationResult()
        {
        }

        public OperationResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public OperationResult(bool success, string message, Status status)
        {
            Success = success;
            Message = message;
            Status = status;
        }

        public static OperationResult Ok(Status status = Status.Success, string message = "")
        {
            return new OperationResult(true, message, status);
        }

        public static OperationResult<T> Ok<T>(T result, Status status = Status.Success, string message = "")
        {
            var x = new OperationResult<T>(Ok());
            x.Result = result;
            return x;
        }

        public static OperationResult Fail(string message, Status status = Status.Failed)
        {
            return new OperationResult(false, message, status);
        }

        public static OperationResult<T> Fail<T>(string message, Status status = Status.Failed)
        {
            return new OperationResult<T>(OperationResult.Fail(message));
        }
    }
}