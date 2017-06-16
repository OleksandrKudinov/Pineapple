﻿angular
    .module("AuthorizationServiceModule")
    .factory("AuthorizedServiceProvider",
    [
        function () {
            var service = {};

            service._authorizeRequest = function (request) {
                if (!request.headers) {
                    request.headers = {};
                }

                var token = localStorage.getItem("access_token");

                if (token === null) {
                    console.log("NON AUTHORIZED!");
                    return;
                }

                request.headers["Authorization"] = "Bearer " + token;

                if (request.method == "post" || request.method == "POST") {
                    request.headers["Content-type"] = "application/json";
                }
            };

            service._injectCallbackToRequest = function (request, callback) {
                request.success = function (data) {
                    callback(true, data);
                };
                request.error = function (data) {
                    callback(false, data);
                };
                return request;
            };

            service._attachEntrypoint = function (request) {
                var entrypoint = "http://localhost:5000/";
                request.url = entrypoint + request.url;
            };

            service.Send = function (request, callback) {
                if (!!callback) {
                    service._injectCallbackToRequest(request, callback);
                }
                $.ajax(request);
            };

            service.SendAuth = function (request, callback) {
                service._authorizeRequest(request);
                service.Send(request, callback);
            };

            return service;
        }]
    );