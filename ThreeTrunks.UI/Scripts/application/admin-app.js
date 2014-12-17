angular
    .module("adminApp", [
        'ngRoute',
        'ui.bootstrap',
        'adminApp.ctrl.images',
        'adminApp.ctrl.content'
    ])
    .config(['$routeProvider', '$locationProvider', '$httpProvider',
        function ($routeProvider, $locationProvider, $httpProvider) {

            $routeProvider.when('/images', {
                templateUrl: '/Admin/Images',
                controller: 'imgCtrl',
            });

            $routeProvider.when('/content', {
                templateUrl: '/Admin/Content',
                controller: 'contentCtrl',
            });

            $routeProvider.otherwise({
                redirectTo: '/images'
            });


            var $http,
       interceptor = ['$q', '$injector', function ($q, $injector) {
           var notificationChannel;

           function success(response) {
               // get $http via $injector because of circular dependency problem
               $http = $http || $injector.get('$http');
               // don't send notification until all requests are complete
               if ($http.pendingRequests.length < 1) {
                   // get requestNotificationChannel via $injector because of circular dependency problem
                   notificationChannel = notificationChannel || $injector.get('requestNotificationChannel');
                   // send a notification requests are complete
                   notificationChannel.requestEnded();
               }
               return response;
           }

           function error(response) {
               // get $http via $injector because of circular dependency problem
               $http = $http || $injector.get('$http');
               // don't send notification until all requests are complete
               if ($http.pendingRequests.length < 1) {
                   // get requestNotificationChannel via $injector because of circular dependency problem
                   notificationChannel = notificationChannel || $injector.get('requestNotificationChannel');
                   // send a notification requests are complete
                   notificationChannel.requestEnded();
               }
               return $q.reject(response);
           }

           return function (promise) {
               // get requestNotificationChannel via $injector because of circular dependency problem
               notificationChannel = notificationChannel || $injector.get('requestNotificationChannel');
               // send a notification requests are complete
               notificationChannel.requestStarted();
               return promise.then(success, error);
           };
       }];

            $httpProvider.responseInterceptors.push(interceptor);

        }])

    // register the interceptor as a service, intercepts ALL angular ajax http calls
.factory('requestNotificationChannel', ['$rootScope', function ($rootScope) {
    // private notification messages
    var _START_REQUEST_ = '_START_REQUEST_';
    var _END_REQUEST_ = '_END_REQUEST_';

    // publish start request notification
    var requestStarted = function () {
        $rootScope.$broadcast(_START_REQUEST_);
    };
    // publish end request notification
    var requestEnded = function () {
        $rootScope.$broadcast(_END_REQUEST_);
    };
    // subscribe to start request notification
    var onRequestStarted = function ($scope, handler) {
        $scope.$on(_START_REQUEST_, function (event) {
            handler();
        });
    };
    // subscribe to end request notification
    var onRequestEnded = function ($scope, handler) {
        $scope.$on(_END_REQUEST_, function (event) {
            handler();
        });
    };

    return {
        requestStarted: requestStarted,
        requestEnded: requestEnded,
        onRequestStarted: onRequestStarted,
        onRequestEnded: onRequestEnded
    };
}])

    .directive('loadingWidget', ['requestNotificationChannel', function (requestNotificationChannel) {
        return {
            restrict: "A",
            link: function (scope, element) {
                // hide the element initially
                element.hide();

                var startRequestHandler = function () {
                    // got the request start notification, show the element
                    element.show();
                };

                var endRequestHandler = function () {
                    // got the request start notification, show the element
                    element.hide();
                };

                requestNotificationChannel.onRequestStarted(scope, startRequestHandler);
                requestNotificationChannel.onRequestEnded(scope, endRequestHandler);
            }
        };
    }])
    .controller('mainCtrl', [
        '$scope',
        '$location',
        function ($scope, $location) {

            //$scope.pageConfig = pageConfig;

            $scope.isActive = function (href) {
                var path = $location.path();
                if (href == '/')
                    return href == path;

                var regex = new RegExp(href);
                return path.match(regex);
            };

        }]);