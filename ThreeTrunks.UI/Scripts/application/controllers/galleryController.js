angular
    .module('threeTrunkApp.ctrl.gallery', [])
    .controller('galleryCtrl', ['$scope', '$location', '$http', 'Page', '$routeParams', '$modal',
        function ($scope, $location, $http, Page, $routeParams, $modal) {
            Page.setTitle("Галерея");
            $scope.galleryType = $routeParams.type;

            $scope.imgs = [];

            $scope.open = function (img, size) {
                var modalInstance = $modal.open({
                    templateUrl: 'viewImage',
                    controller: ModalInstanceCtrl,
                    size: size,
                    resolve: {
                        img: function () {
                            return img;
                        }
                    }
                });
            };

            $scope.init = function () {
                $http.post('/api/Image/GetCategoryImages', '"' + $scope.galleryType + '"').
                success(function (data, status, headers, config) {
                    $scope.imgs = data;
                }).
                error(function (data, status, headers, config) {

                });
            };

            $scope.init();

        }]);


var ModalInstanceCtrl = function ($scope, $modalInstance, img) {
    $scope.img = img;
    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
};


