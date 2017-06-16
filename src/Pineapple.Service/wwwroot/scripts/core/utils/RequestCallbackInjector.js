angular
    .module("RequestCallbackInjectorModule")
    .factory("RequestCallbackInjector",
            [
                function () {
                    var injector = {};

                    injector.InjectCallbackToRequest = function (request, callback) {
                        request.success = function (data) {
                            callback(true, data);
                        };
                        request.error = function (data) {
                            callback(false, data);
                        };
                        return request;
                    };

                    return injector;
                }
            ]
        );