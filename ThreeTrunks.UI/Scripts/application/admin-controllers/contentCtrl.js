angular
    .module('adminApp.ctrl.content', [])
    .controller('contentCtrl', ['$scope', '$location', '$http', '$modal',
        function ($scope, $location, $http, $modal) {
            $scope.status = {
            };

            $scope.settings = [];

            $scope.saveSettings = function () {
                $http.post('/api/Settings/SaveSiteSettings/',
                    JSON.stringify($scope.settings),
                    {
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    })
                    .success(function (data, status, headers, config) {
                        alertify.success("Настройки сохранены");
                    })
                    .error(function (data, status, headers, config) {
                       alertify.error("Произошла ошибка при сохранении");
                    });
            };

            $scope.init = function () {
                $http.get('/api/Settings/GetSiteSettings/').
                success(function (data, status, headers, config) {
                    $scope.settings = data;
                }).
                error(function (data, status, headers, config) {
                    alertify.error("Произошла ошибка при загрузке данных");
                });
            };

            $scope.init();

        }])

.directive('editorFor', function ($compile) {

    var templateBool ="/Admin/BoolEditor";
    var templateText = "/Admin/TextEditor";


    var getTemplateUrl = function (type) {
        var template = '';

        switch (type) {
            case 1:
                template = templateBool;
                break;
            case 2:
                template = templateText;
                break;
        }

        return template;
    };

    var linker = function (scope, element, attrs) {
        scope.contentUrl = getTemplateUrl(scope.setting.type);
    };

    return {
        restrict: "E",
        replace: true,
        link: linker,
        template: '<div ng-include="contentUrl"></div>'
    };
});

