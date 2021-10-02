using Maxi.Models.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace MaxiApi.Controllers
{
    public abstract class MaxiBaseController
    {
        public readonly CurrentProcess _process;
        public MaxiBaseController(CurrentProcess process)
        {
            _process = process;
        }
        private JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        protected IActionResult ToResponse()
        {
            var model = new ResponseJsonModel();
            bool hasError = _checkHasError(model);
            return new JsonResult(model, _jsonSerializerSettings);
        }

        protected IActionResult ToResponse(long data)
        {
            var model = new ResponseJsonModel<long>();
            bool hasError = _checkHasError(model);
            if (!hasError)
                model.data = data;
            return new JsonResult(model, _jsonSerializerSettings);
        }

        protected IActionResult ToResponse<T>(T data) where T : class
        {
            var model = new ResponseJsonModel<T>();
            if (!_checkHasError(model))
                model.data = data;

            return new JsonResult(model, _jsonSerializerSettings);
        }
        protected IActionResult ToResponse(bool isSuccess)
        {
            var model = new ResponseJsonModel();

            if (!_checkHasError(model))
                model.success = isSuccess;

            return new JsonResult(model, _jsonSerializerSettings);
        }
        private bool _checkHasError(ResponseJsonModel model)
        {
            var hasError = _process.HasError;
            if (hasError)
            {
                var errorMessage = _process.ToError();

                model.error = new ErrorJsonModel()
                {
                    code = errorMessage.Message,
                    message = "error",
                    trace_keys = errorMessage.TraceKeys
                };
            }
            model.success = !hasError;
            return hasError;
        }
    }
}
