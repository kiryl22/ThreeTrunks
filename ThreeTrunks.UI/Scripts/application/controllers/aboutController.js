angular
    .module('threeTrunkApp.ctrl.about', [])
    .controller('aboutCtrl', [
        '$scope',
        '$location',
        '$http',
        'Page',
        function ($scope) {
            $scope.Page.setTitle("О нас");
        }]);




