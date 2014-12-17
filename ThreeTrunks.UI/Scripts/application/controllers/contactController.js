angular
    .module('threeTrunkApp.ctrl.contact', [])
    .controller('contactCtrl', [
        '$scope',
        '$location',
        '$http',
        'Page',
        function ($scope) {

            $scope.Page.setTitle('Контакты');

            $scope.init = function () {

            };

            $scope.init();
        }]);




