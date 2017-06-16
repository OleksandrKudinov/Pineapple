angular
    .module("EntryPointProviderModule")
    .factory("EntryPointProvider",
        [
            function () {
                var provider = {};

                provider.GetEntryPoint = function () {
                    return "http://localhost:5000/";
                };

                return provider;
            }
        ]);