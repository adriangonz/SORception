module.controller("editOrderCtrl", ['$scope','$location', 'Taller', 'Auth', function ($scope, $location, Taller, Auth) {
    $scope.$on('tmp_order.update', function (event) {
        console.log(Taller.tmp_order);
        $scope.tmp_order = Taller.tmp_order;
        $scope.tmp_line = { "criterio": $scope.criterios[0] };
    });

    $scope.criterios = Taller.criterios;

    Auth.isLoggedIn();
    $scope.tmp_order = { "data": [] };
    $scope.tmp_line = { "criterio": $scope.criterios[0] };
    if (Taller.actual_order) {
        Taller.loadActualOrder();
    } else {
        $location.path("/orders");
    }

}]);
