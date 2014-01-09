module.controller("editOrderCtrl", ['$scope','$location', 'Taller', 'Auth', function ($scope, $location, Taller, Auth) {
    $scope.$on('tmp_order.update', function (event) {
        console.log(Taller.tmp_order);
        $scope.tmp_order = Taller.tmp_order;
    });

    Auth.isLoggedIn();
    $scope.tmp_order = { "data": [] };
    if (Taller.actual_order) {
        $scope.tmp_order.data = Taller.actual_order.lineaSolicitud;
        $scope.tmp_order.id = Taller.actual_order.id;
    } else {
        $location.path("/orders");
    }

}]);
