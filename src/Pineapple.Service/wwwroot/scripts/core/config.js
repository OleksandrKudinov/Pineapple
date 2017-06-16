angular.module("AuthorizationServiceModule", []);
angular.module("ChatServiceModule", ["AuthorizationServiceModule"]);
angular.module("PineappleModule", ["AuthorizationServiceModule", "ChatServiceModule"]);