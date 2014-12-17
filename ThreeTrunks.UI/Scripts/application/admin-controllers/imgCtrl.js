angular
.module('adminApp.ctrl.images', ['angularFileUpload'])
.controller('imgCtrl', ['$scope', '$location', '$http', '$modal', 'requestNotificationChannel',
    function ($scope, $location, $http, $modal, requestNotificationChannel) {
        $scope.status = {
            isFirstOpen: true,
            isFirstDisabled: false,
            isNew: false,
        };

        $scope.categories = [];
        $scope.targetCategory = {};
        $scope.selectedCategory = {};
        $scope.isEdit = false;

        $scope.init = function () {
            $http.get('/api/Image/GetCategories/').
            success(function (data, status, headers, config) {
                $scope.categories = data;
                if ($scope.categories && $scope.categories.length > 0) {
                    $scope.selectCategory($scope.categories[0]);
                } else {
                    $scope.addNewCategory();
                }

            }).
            error(function () {
                alertify.success("Произошла ошибка при загрузке данных");
            }).then(function () {
                //$scope.pageConfig.isLoading = false;
            });
        };

        $scope.init();

        $scope.editImage = function (img) {
            var modalInstance = $modal.open({
                templateUrl: '/Admin/EditImage',
                controller: ModalInstanceCtrl,
                resolve: {
                    img: function () {
                        return img;
                    },
                    categories: function () {
                        return $scope.categories;
                    }
                }
            });

            modalInstance.result.then(function (data) {
                $scope.saveImage(data.image, data.originalImg);
            }, function () {

            });
        };

        $scope.deleteImage = function (img) {
            var modalInstance = $modal.open({
                templateUrl: 'deleteImage',
                controller: ModalInstanceCtrl,  
                resolve: {
                    img: function () {
                        return img;
                    }
                }
            });

            modalInstance.result.then(function (data) {
                $scope.removeImage(data.image);
            }, function () {

            });
        };

        $scope.saveImage = function (image, originalImg) {
            $http.post('/api/Image/SaveImage/',
             JSON.stringify(image),
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
        .success(function (data, status, headers, config) {
            alertify.success("Изображение сохранено");
            if (data && data.id > 0) {
                if (originalImg && originalImg.categoryId != data.categoryId) {

                    var newCategory = $.grep($scope.categories, function (e) { return e.id == data.categoryId; });

                    if (newCategory.length != 0) {
                        newCategory[0].images.push(image);
                    }

                    var oldCategory = $.grep($scope.categories, function (e) { return e.id == originalImg.categoryId; });

                    if (oldCategory.length != 0) {
                        oldCategory[0].images.forEach(function (item, index) {
                            if (item.id == originalImg.id)
                                oldCategory[0].images.splice(index, 1);
                        });

                        //Refresh category
                        if ($scope.targetCategory.id == oldCategory[0].id) {
                            $scope.selectCategory(oldCategory[0]);
                        }
                    }
                }
            }
        })
        .error(function (data, status, headers, config) {
            alertify.error("Произошла ошибка при сохранении изображания");
        });
        };

        $scope.removeImage = function (image) {
            $http.post('/api/Image/DeleteImage/',
             JSON.stringify(image),
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
        .success(function (data, status, headers, config) {
            if (data) {
                if (image.id == data.id) {
                    var category = $.grep($scope.categories, function (e) { return e.id == data.categoryId; });
                
                    if (category.length != 0) {
                        category[0].images.forEach(function (item, index) {
                            if (item.id == image.id)
                                category[0].images.splice(index, 1);
                        });

                        //Refresh target category
                        if ($scope.targetCategory.id == data.categoryId) {
                            $scope.selectCategory(category[0]);
                        }
                    }
                }
            }

            alertify.success("Изображение удалено");
        })
        .error(function (data, status, headers, config) {
            alertify.error("Произошла ошибка при удалении изображания");
        });
        };

        $scope.selectCategory = function (item) {
            $scope.selectedCategory = item;
            angular.copy($scope.selectedCategory, $scope.targetCategory);
            $scope.status.isNew = false;
        };

        $scope.addNewCategory = function () {
            $scope.selectedCategory = null;
            angular.copy({ id: 0, name: '' }, $scope.targetCategory);
            $scope.status.isFirstOpen = true;
            $scope.status.isNew = true;
            $scope.isEdit = true;
        };

        $scope.saveCategory = function (category) {
            $http.post('/api/Image/SaveCategory/',
                 JSON.stringify(category),
            {
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .success(function (data, status, headers, config) {
                    if (data && data.id > 0) {
                        if ($scope.targetCategory.id == data.id) {
                            $scope.categories.forEach(
                                function (element, index, array) {
                                if (element.id == data.id)
                                    array[index] = data;
                            });
                        } else {
                            $scope.categories.push(data);
                        }
                    }

                    alertify.success("Категория сохранена");
                })
                .error(function (data, status, headers, config) {
                    alertify.success("Произошла ошибка при сохранении категории");
                });
        };
}])

.controller('uploadCtrl', ['$scope', 'FileUploader', 'requestNotificationChannel',
    function ($scope, FileUploader, requestNotificationChannel) {
        
        var uploader = $scope.uploader = new FileUploader({
            url: '/api/Image/UploadImage'
        });

        uploader.filters.push({
            name: 'imageFilter',
            fn: function (item /*{File|FileLikeObject}*/, options) {
                var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
                return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
            }
        });

        uploader.onBeforeUploadItem = function (item) {
            requestNotificationChannel.requestStarted();
            item.url = uploader.url + '/' + $scope.targetCategory.id;
            item.formData.push({ categoryId: $scope.targetCategory.id });
        };
        
        uploader.onCompleteItem = function (item, response, status, headers) {
            if (status == 200 && response && response.UploadedImages) {
                var result = $.grep($scope.categories, function (e) { return e.id == response.CategoryId; });
                if (result.length != 0) {
                    response.UploadedImages.forEach(function logArrayElements(element, index, array) {
                        result[0].images.push(element);
                        //Refresh target category
                        if ($scope.targetCategory.id == result[0].id) {
                            $scope.selectCategory(result[0]);
                        }
                    });
                }
            }

            $scope.status.isDirtyImageData = true;
        };

        uploader.onCompleteAll = function () {
            requestNotificationChannel.requestEnded();
            alertify.success("Изображения загружены");
        };
    }])

.directive('fileInput', function () {
    return {
        restrict: 'C',
        transclude: true,
        link: function (scope, element, attrs) {
            $(element).bootstrapFileInput();
        }
    };
})

.directive('editCategory', function () {
    return {
        restrict: 'EA',
        templateUrl: 'Admin/EditCategory',
        transclude: true,
        scope: {
            save: '=saveCall',
            category: '='
        },
        controller: function ($scope) {

        },
    };
});

var ModalInstanceCtrl = function ($scope, $modalInstance, img, categories) {

    $scope.originalImg = {};
    angular.copy(img, $scope.originalImg);

    $scope.img = img;
    $scope.categories = categories;

    $scope.ok = function () {
        $modalInstance.close({ image: $scope.img, originalImg: $scope.originalImg });
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
};

