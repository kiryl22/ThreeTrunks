angular
    .module('threeTrunkApp.ctrl.home', [])
    .controller('homeCtrl', [
        '$scope',
        '$location',
        '$http',
        'Page',
        function ($scope, $location, $http, Page) {

            Page.setTitle("Главная");

            $scope.myInterval = 5000;

            $scope.slides = [];

            $scope.init = function () {
                $http.get('/api/Image/GetCarouselImages/').
                success(function (data, status, headers, config) {
                    $scope.slides = data;
                }).
                error(function (data, status, headers, config) {

                });
            };

            $scope.init();
        }]);




