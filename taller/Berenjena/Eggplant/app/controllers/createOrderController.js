module.controller("createOrderCtrl", ['$scope', 'Taller', 'Auth', function ($scope, Taller, Auth) {
    $scope.$on('order.update', function (event) {
        console.log(Taller.tmp_order);
        $scope.tmp_order = Taller.tmp_order;
    });

    Auth.isLoggedIn();
    $scope.tmp_order = { "data": [] };

}]);
