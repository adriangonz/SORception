module.controller("editOrderCtrl", ['$scope','$location', 'Taller', 'Auth', function ($scope, $location, Taller, Auth) {
    $scope.$on('tmp_order.update', function (event) {
        console.log(Taller.tmp_order);
        $scope.tmp_order = Taller.tmp_order;
    });

    Auth.isLoggedIn();
    $scope.tmp_order = { "data": [] };
    if (Taller.actual_order) {
        Taller.loadActualOrder();
    } else {
        $location.path("/orders");
    }

}]);
