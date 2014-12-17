angular
    .module('threeTrunkApp', [
        'ngRoute',
        'ui.bootstrap',
        'threeTrunkApp.ctrl.home',
        'threeTrunkApp.ctrl.about',
        'threeTrunkApp.ctrl.gallery',
        'threeTrunkApp.ctrl.contact'
    ])
    .config(['$routeProvider', '$locationProvider', function ($routeProvider) {

        $routeProvider.when('/', {
            templateUrl: '/Home/Home',
            controller: 'homeCtrl',
        });

        $routeProvider.when('/gallery/:type', {
            templateUrl: '/Home/Gallery',
            controller: 'galleryCtrl',
        });

        $routeProvider.when('/about', {
            templateUrl: '/Home/AboutUs',
            controller: 'aboutCtrl',
        });

        $routeProvider.when('/contact', {
            templateUrl: '/Home/Contacts',
            controller: 'contactCtrl',
        });

        $routeProvider.otherwise({
            redirectTo: '/'
        });
    }])
.factory('Page', function () {
    var title = 'default';
    return {
        title: function () { return title; },
        setTitle: function (newTitle) { title = newTitle; }
    };
})
 .controller('mainCtrl', [
        '$scope',
        '$location',
        'Page',
        '$http',
    function ($scope, $location, Page, $http) {
        $scope.Page = Page;

        $scope.isActive = function (href) {
            var path = $location.path();
            if (href == '/')
                return href == path;

            var regex = new RegExp(href);
            return path.match(regex);
        };

        $scope.galleryCategories = [];

        $scope.init = function () {
            $http.get('/api/Image/GetGalleryCategories/').
              success(function (data) {
                  $scope.galleryCategories = data;
              }).
              error(function () {
                  //if error
              });
        };

    }]);
